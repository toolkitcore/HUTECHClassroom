﻿using HUTECHClassroom.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace HUTECHClassroom.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Migration error");
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Seeding error");
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.Faculties.Any()
            || _context.Users.Any()
            || _context.Classrooms.Any()
            || _context.Exercises.Any()
            || _context.Answers.Any()
            || _context.Posts.Any()
            || _context.Comments.Any()
            || _context.Missions.Any()
            || _context.Projects.Any()
            || _context.Groups.Any()
            || _context.Roles.Any()) return;

        var faculties = new Faculty[]
        {
            new Faculty
            {
                Name = "Information Technology"
            },
            new Faculty
            {
                Name = "Marketing"
            }
        };

        var studentRole = new ApplicationRole("Student");
        var roles = new ApplicationRole[6]
        {
            new ApplicationRole("Administrator"),
            new ApplicationRole("TrainingOffice"),
            new ApplicationRole("Dean"),
            new ApplicationRole("Lecturer"),
            new ApplicationRole("Leader"),
            studentRole
        };


        foreach (var role in roles)
        {
            await _roleManager.CreateAsync(role);
        }

        var readMission = new Claim("mission", "read");
        await _roleManager.AddClaimAsync(studentRole, readMission);

        var users = new ApplicationUser[]
        {
            new ApplicationUser
            {
                UserName = "2080600914",
                Email = "thai@gmail.com",
                Faculty = faculties[0]
            },
            new ApplicationUser
            {
                UserName = "2080600803",
                Email = "mei@gmail.com",
                Faculty = faculties[0]
            },
            new ApplicationUser
            {
                UserName = "lecturer1",
                Email = "lecturer1@gmail.com",
                Faculty = faculties[0]
            },
            new ApplicationUser
            {
                UserName = "lecturer2",
                Email = "lecturer2@gmail.com",
                Faculty = faculties[0]
            }
        };

        foreach (var user in users)
        {
            await _userManager.CreateAsync(user, "P@ssw0rd").ConfigureAwait(false);
            await _userManager.AddToRoleAsync(user, roles[5].Name);
        }

        var classrooms = new Classroom[]
        {
            new Classroom
            {
                Title = "Linear algebra",
                Description = "A subject",
                Topic = "Mathemetics",
                Room = "101",
                Lecturer = users[2],
                Faculty = faculties[0]
            },
            new Classroom
            {
                Title = "English 1",
                Description = "A subject",
                Topic = "English",
                Room = "102",
                Lecturer = users[3],
                Faculty = faculties[0]
            }
        };

        var exercises = new Exercise[]
        {
            new Exercise
            {
                Title = "Solve the problem",
                Instruction = "Suppose that L1 and L2 are lines in the plane, that the x-intercepts of L1 and L2 are 5\r\nand −1, respectively, and that the respective y-intercepts are 5 and 1. Then L1 and L2\r\nintersect at the point ( , ) .",
                Link = "google.com",
                TotalScore = 10,
                Deadline = DateTime.UtcNow.AddDays(1),
                Topic = "Mathemetics",
                Criteria = "Good: 10, Bad: 5",
                Classroom = classrooms[0],
                ExerciseUsers = new ExerciseUser[]
                {
                    new ExerciseUser
                    {
                        User = users[0]
                    },
                    new ExerciseUser
                    {
                        User = users[1]
                    }
                }
            }
        };

        await _context.AddRangeAsync(exercises);

        var answers = new Answer[]
        {
            new Answer
            {
                Description = "Sorry, I don't know T_T",
                Link = "a.com",
                Score = 0,
                Exercise = exercises[0],
                User = users[0]
            },
            new Answer
            {
                Description = "Sorry, I don't know, too T_T",
                Link = "b.com",
                Score = 0,
                Exercise = exercises[0],
                User = users[0]
            }
        };

        await _context.AddRangeAsync(answers);

        var posts = new Post[]
        {
            new Post
            {
                Content = "Hello world",
                Link = "google.com",
                User = users[0],
                Classroom = classrooms[0]
            },
            new Post
            {
                Content = "Hello!",
                Link = "yahoo.com",
                User = users[1],
                Classroom = classrooms[1]
            }
        };

        await _context.AddRangeAsync(posts);

        var comments = new Comment[]
        {
            new Comment
            {
                Content = "Hello universe ._.",
                User = users[0],
                Post = posts[0]
            },
            new Comment
            {
                Content = "Hi",
                User = users[0],
                Post = posts[1]
            }
        };

        await _context.AddRangeAsync(comments);

        var groups = new Group[]
        {
            new Group
            {
                Name = "Owlvernyte",
                Description = "Owls group",
                Leader = users[0],
                GroupUsers = new GroupUser[]
                {
                    new GroupUser
                    {
                        User = users[0]
                    }
                },
                Classroom = classrooms[0]
            },
            new Group
            {
                Name = "Semibox",
                Description = "Half of a box",
                Leader = users[1],
                GroupUsers = new GroupUser[]
                {
                    new GroupUser
                    {
                        User = users[0]
                    },
                    new GroupUser
                    {
                        User = users[1]
                    }
                },
                Classroom = classrooms[1]
            }
        };

        await _context.AddRangeAsync(groups).ConfigureAwait(false);

        var projects = new Project[]
        {
            new Project
            {
                Name = "Plan together",
                Description = "Projects, Groups Management system",
                Group = groups[0]
            },
            new Project
            {
                Name = "HUTECH Classroom",
                Description = "Classroom, Students, Lecturers... Management system",
                Group = groups[1]
            }
        };

        await _context.AddRangeAsync(projects).ConfigureAwait(false);

        var missions = new Mission[]
        {
            new Mission
            {
                Title = "Let's read",
                Description = "Read 1 book",
                MissionUsers = new MissionUser[]
                {
                    new MissionUser
                    {
                        User = users[0],
                    }
                },
                Project = projects[0]
            },
            new Mission
            {
                Title = "Let's write",
                Description = "Write 1 note",
                MissionUsers = new MissionUser[]
                {
                    new MissionUser
                    {
                        User = users[1],
                    }
                },
                Project = projects[1]
            },
            new Mission
            {
                Title = "Let's listen",
                Description = "Listen 1 song",
                MissionUsers = new MissionUser[]
                {
                    new MissionUser
                    {
                        User = users[0],
                    },
                    new MissionUser
                    {
                        User = users[1],
                    }
                },
                Project = projects[1]
            },
        };

        await _context.AddRangeAsync(missions).ConfigureAwait(false);

        await _context.SaveChangesAsync();
    }

}
