using GanjAudio.Data;
using GanjAudio.Handler;
using GanjAudio.WebApi.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

const string allowedOrigin = "_allowedOrigin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigin, builder =>
        {
            builder.WithOrigins("https://ganjmp3.tk")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/getnotes", async (Guid id) =>
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApiDbContext>();

        return await new RequestHandler(dbContext).GetNotesAsync(id);
    }
});

app.MapGet("/getpublicnotes", async () =>
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApiDbContext>();

        return await new RequestHandler(dbContext).GetPublicNotes();
    }
});

app.MapPost("/addnote", [EnableCors(allowedOrigin)] async (Note note) =>
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApiDbContext>();

        await new RequestHandler(dbContext).AddNoteAsync(note.OwnerId, note.Content, note.IsPublic);
    }
});

app.MapPut("/updatenote", async Task<string> (Note note) =>
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApiDbContext>();

        return await new RequestHandler(dbContext).UpdateNoteAsync(note.Id, note.Content, note.IsPublic);
    }
});

app.MapDelete("/deletenote", async Task<string> (Guid id) =>
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApiDbContext>();

        return await new RequestHandler(dbContext).DeleteNoteAsync(id);
    }
});

app.UseCors(allowedOrigin);

app.Run();