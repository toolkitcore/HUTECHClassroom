﻿using HUTECHClassroom.Domain.Entities;
using HUTECHClassroom.Web.ViewModels.Groups;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HUTECHClassroom.Web.Controllers;

public class GroupsController : BaseEntityController<Group>
{
    // GET: Groups
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = DbContext.Groups.Include(g => g.Classroom).Include(g => g.Leader);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Groups/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || DbContext.Groups == null)
        {
            return NotFound();
        }

        var group = await DbContext.Groups
            .Include(g => g.Classroom)
            .Include(g => g.Leader)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    public async Task<IActionResult> ImportGroupUsers(Guid? id)
    {
        if (id == null)
            return View("Index");
        if (id == null || DbContext.Groups == null)
        {
            return NotFound();
        }
        var group = await DbContext.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }
        var viewModel = new ImportUsersToGroupViewModel
        {
            GroupId = group.Id,
            GroupName = group.Name
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportGroupUsers(ImportUsersToGroupViewModel viewModel)
    {
        if (viewModel.File == null || viewModel.File.Length == 0)
        {
            ViewBag.Error = "Please select a file to upload.";
            return View(viewModel);
        }

        if (!Path.GetExtension(viewModel.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.Error = "Please select an Excel file (.xlsx).";
            return View(viewModel);
        }

        var users = ExcelService.ReadExcelFileWithColumnNames<ApplicationUser>(viewModel.File.OpenReadStream(), null);
        // Do something with the imported people data, such as saving to a database
        var results = new List<IdentityResult>();
        foreach (var user in users)
        {
            results.Add(await UserManager.CreateAsync(user, user.UserName));
        }

        var group = await DbContext.Groups
            .Include(c => c.GroupUsers)
            .SingleOrDefaultAsync(c => c.Id == viewModel.GroupId);

        if (group == null)
        {
            return NotFound();
        }

        group.GroupUsers.AddRange(
            users.Select(user => new GroupUser { User = user })
        );

        await DbContext.SaveChangesAsync();

        ViewBag.Success = $"Successfully imported {results.Count(x => x.Succeeded)} rows.";
        return RedirectToAction("Index");
    }

    // GET: Groups/Create
    public IActionResult Create()
    {
        ViewData["ClassroomId"] = new SelectList(DbContext.Classrooms, "Id", "Title");
        ViewData["LeaderId"] = new SelectList(DbContext.Users, "Id", "UserName");
        return View();
    }

    // POST: Groups/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,LeaderId,ClassroomId,Id,CreateDate")] Group group)
    {
        if (ModelState.IsValid)
        {
            group.Id = Guid.NewGuid();
            DbContext.Add(group);
            await DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClassroomId"] = new SelectList(DbContext.Classrooms, "Id", "Title", group.ClassroomId);
        ViewData["LeaderId"] = new SelectList(DbContext.Users, "Id", "UserName", group.LeaderId);
        return View(group);
    }

    // GET: Groups/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || DbContext.Groups == null)
        {
            return NotFound();
        }

        var group = await DbContext.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }
        ViewData["ClassroomId"] = new SelectList(DbContext.Classrooms, "Id", "Title", group.ClassroomId);
        ViewData["LeaderId"] = new SelectList(DbContext.Users, "Id", "UserName", group.LeaderId);
        return View(group);
    }

    // POST: Groups/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Name,Description,LeaderId,ClassroomId,Id,CreateDate")] Group group)
    {
        if (id != group.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                DbContext.Update(group);
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(group.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClassroomId"] = new SelectList(DbContext.Classrooms, "Id", "Title", group.ClassroomId);
        ViewData["LeaderId"] = new SelectList(DbContext.Users, "Id", "UserName", group.LeaderId);
        return View(group);
    }

    // GET: Groups/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || DbContext.Groups == null)
        {
            return NotFound();
        }

        var group = await DbContext.Groups
            .Include(g => g.Classroom)
            .Include(g => g.Leader)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // POST: Groups/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (DbContext.Groups == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Groups'  is null.");
        }
        var group = await DbContext.Groups.FindAsync(id);
        if (group != null)
        {
            DbContext.Groups.Remove(group);
        }

        await DbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GroupExists(Guid id)
    {
        return DbContext.Groups.Any(e => e.Id == id);
    }
}
