using GanjAudio.Data;
using GanjAudio.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GanjAudio.Handler;

public class RequestHandler
{
    private readonly ApiDbContext _context;

    public RequestHandler(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Note>> GetNotesAsync(Guid id)
    {
        var userNotes = await _context.Notes.Where(n => n.OwnerId == id).ToListAsync();

        return userNotes;
    }

    public async Task<IEnumerable<Note>> GetPublicNotes()
    {
        var userNotes = await _context.Notes.Where(n => n.IsPublic == true).ToListAsync();

        return userNotes;
    }

    public async Task<IResult> AddNoteAsync(Guid id, string content, bool isPublic)
    {
        Note newNote = new()
        {
            OwnerId = id,
            Content = content,
            IsPublic = isPublic
        };

        await _context.Notes.AddAsync(newNote);

        await _context.SaveChangesAsync();

        return Results.Created("", new { theContent = content });
    }

    public async Task<string> UpdateNoteAsync(Guid id, string content, bool isPublic)
    {
        try
        {
            Note note = await _context.Notes.SingleAsync(n => n.Id == id);

            note.Content = content;

            note.IsPublic = isPublic;

            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return "succeed";

            return "faield";
        }
        catch
        {
            return "failed";
        }
    }

    public async Task<string> DeleteNoteAsync(Guid id)
    {
        try
        {
            Note note = await _context.Notes.SingleAsync(n => n.Id == id);

            _context.Remove(note);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return "succeed";

            return "faield";
        }
        catch
        {
            return "failed";
        }
    }
}