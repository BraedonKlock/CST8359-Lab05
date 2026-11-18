using Lab05WebApi.Data;
using Lab05WebApi.DTOs;
using Lab05WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab05WebApi.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
    {
        var events = await _context.Events
            .Include(e => e.Attendees)
            .Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Attendees = e.Attendees.Select(a => new AttendeeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email
                }).ToList()
            })
            .ToListAsync();

        return Ok(events);
    }

    // GET: api/events/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        var selectedEvent = await _context.Events
            .Where(e => e.Id == id)
            .Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Attendees = e.Attendees.Select(a => new AttendeeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (selectedEvent == null)
        {
            return NotFound();
        }

        return Ok(selectedEvent);
    }

    // POST: api/events
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(
        [FromBody] CreateEventDto dto)
    {
        var newEvent = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date!.Value,
            Location = dto.Location
        };

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        var eventDto = new EventDto
        {
            Id = newEvent.Id,
            Title = newEvent.Title,
            Description = newEvent.Description,
            Date = newEvent.Date,
            Location = newEvent.Location,
            Attendees = new List<AttendeeDto>()
        };

        return CreatedAtAction(
            nameof(GetEvent),
            new { id = newEvent.Id },
            eventDto);
    }

    // PUT: api/events/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEvent(
        int id,
        [FromBody] UpdateEventDto dto)
    {
        var selectedEvent = await _context.Events.FindAsync(id);

        if (selectedEvent == null)
        {
            return NotFound();
        }

        selectedEvent.Title = dto.Title;
        selectedEvent.Description = dto.Description;
        selectedEvent.Date = dto.Date!.Value;
        selectedEvent.Location = dto.Location;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/events/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var selectedEvent = await _context.Events.FindAsync(id);

        if (selectedEvent == null)
        {
            return NotFound();
        }

        _context.Events.Remove(selectedEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/events/1/attendees
    [HttpPost("{eventId:int}/attendees")]
    public async Task<ActionResult<AttendeeDto>> RegisterAttendee(
        int eventId,
        [FromBody] RegisterAttendeeDto dto)
    {
        var eventExists = await _context.Events
            .AnyAsync(e => e.Id == eventId);

        if (!eventExists)
        {
            return NotFound();
        }

        var attendee = new Attendee
        {
            Name = dto.Name,
            Email = dto.Email,
            EventId = eventId
        };

        _context.Attendees.Add(attendee);
        await _context.SaveChangesAsync();

        var attendeeDto = new AttendeeDto
        {
            Id = attendee.Id,
            Name = attendee.Name,
            Email = attendee.Email
        };

        return Created(
            $"/api/events/{eventId}/attendees/{attendee.Id}",
            attendeeDto);
    }

    // DELETE: api/events/1/attendees/{attendeeId}
    [HttpDelete("{eventId:int}/attendees/{attendeeId}")]
    public async Task<IActionResult> UnregisterAttendee(
        int eventId,
        string attendeeId)
    {
        var eventExists = await _context.Events
            .AnyAsync(e => e.Id == eventId);

        if (!eventExists)
        {
            return NotFound();
        }

        var attendee = await _context.Attendees
            .FirstOrDefaultAsync(a =>
                a.Id == attendeeId && a.EventId == eventId);

        if (attendee == null)
        {
            return NotFound();
        }

        _context.Attendees.Remove(attendee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
