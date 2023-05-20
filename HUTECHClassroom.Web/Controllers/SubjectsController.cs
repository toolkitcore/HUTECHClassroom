﻿using HUTECHClassroom.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HUTECHClassroom.Web.Controllers;

public class SubjectsController : BaseEntityController<Subject>
{
    public IActionResult Index(int? page, int? size)
    {
        int pageIndex = page ?? 1;
        int pageSize = size ?? 5;
        return View(DbContext.Subjects
            .OrderByDescending(x => x.CreateDate)
            .ToPagedList(pageIndex, pageSize));
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || DbContext.Subjects == null)
        {
            return NotFound();
        }

        var subject = await DbContext.Subjects
            .Include(s => s.Major)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (subject == null)
        {
            return NotFound();
        }

        return View(subject);
    }

    public IActionResult Create()
    {
        ViewData["MajorId"] = new SelectList(DbContext.Majors, "Id", "Code");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Code,Title,TotalCredits,MajorId,Id,CreateDate")] Subject subject)
    {
        if (ModelState.IsValid)
        {
            subject.Id = Guid.NewGuid();
            DbContext.Add(subject);
            await DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["MajorId"] = new SelectList(DbContext.Majors, "Id", "Code", subject.MajorId);
        return View(subject);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || DbContext.Subjects == null)
        {
            return NotFound();
        }

        var subject = await DbContext.Subjects.FindAsync(id);
        if (subject == null)
        {
            return NotFound();
        }
        ViewData["MajorId"] = new SelectList(DbContext.Majors, "Id", "Code", subject.MajorId);
        return View(subject);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Code,Title,TotalCredits,MajorId,Id,CreateDate")] Subject subject)
    {
        if (id != subject.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                DbContext.Update(subject);
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(subject.Id))
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
        ViewData["MajorId"] = new SelectList(DbContext.Majors, "Id", "Code", subject.MajorId);
        return View(subject);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || DbContext.Subjects == null)
        {
            return NotFound();
        }

        var subject = await DbContext.Subjects
            .Include(s => s.Major)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (subject == null)
        {
            return NotFound();
        }

        return View(subject);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (DbContext.Subjects == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Subjects'  is null.");
        }
        var subject = await DbContext.Subjects.FindAsync(id);
        if (subject != null)
        {
            DbContext.Subjects.Remove(subject);
        }

        await DbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubjectExists(Guid id)
    {
        return DbContext.Subjects.Any(e => e.Id == id);
    }
}