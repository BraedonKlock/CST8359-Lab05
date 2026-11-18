using Lab05WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab05WebApi.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // Don't seed again if events already exist.
        if (await context.Events.AnyAsync())
        {
            return;
        }

        var events = new List<Event>
        {
            new Event
            {
                Title = "Career Fair",
                Description = "Meet employers and explore career opportunities.",
                Date = new DateTime(2026, 12, 5, 10, 0, 0),
                Location = "Ottawa",
                Attendees = new List<Attendee>
                {
                    new Attendee
                    {
                        Name = "John Smith",
                        Email = "john@example.com"
                    },
                    new Attendee
                    {
                        Name = "Sarah Jones",
                        Email = "sarah@example.com"
                    }
                }
            },
            new Event
            {
                Title = "Tech Talk",
                Description = "Learn about modern software development.",
                Date = new DateTime(2026, 12, 10, 14, 0, 0),
                Location = "Algonquin College",
                Attendees = new List<Attendee>
                {
                    new Attendee
                    {
                        Name = "Michael Brown",
                        Email = "michael@example.com"
                    },
                    new Attendee
                    {
                        Name = "Emily Davis",
                        Email = "emily@example.com"
                    }
                }
            },
            new Event
            {
                Title = "Hack Night",
                Description = "Collaborate on programming projects.",
                Date = new DateTime(2026, 12, 15, 18, 0, 0),
                Location = "Ottawa",
                Attendees = new List<Attendee>
                {
                    new Attendee
                    {
                        Name = "Alex Wilson",
                        Email = "alex@example.com"
                    },
                    new Attendee
                    {
                        Name = "Jessica Taylor",
                        Email = "jessica@example.com"
                    }
                }
            }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();
    }
}
