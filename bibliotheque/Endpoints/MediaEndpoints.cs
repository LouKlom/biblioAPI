using System.Diagnostics;
using System.Runtime.InteropServices;
using bibliotheque.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class MediaEndpoints
{
    public static void MapMedia(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/medias", GetAllMedias)
            .WithTags("Medias");

        app.MapGet("/api/medias/{id}", GetMediaById)
            .WithTags("Medias");

        app.MapGet("/api/medias/auteur/{auteurId}", GetMediaByAuteur)
            .WithTags("Medias");

        app.MapPut("/api/medias/{id}", UpdateMediaById)
            .WithTags("Medias");

        app.MapPost("/api/medias", CreateMedia)
            .WithTags("Medias");

        //Delete Media by Id
        app.MapDelete("/api/medias/{id}", DeleteMedia)
            .WithTags("Medias");
    }

    private static async Task<IResult> GetAllMedias(ApiContext context)
    {
        return Results.Ok(await context.Medias
            .Include(m => m.Auteur)
            .ToListAsync());
    }

    private static async Task<IResult> GetMediaById(ApiContext context, int id)
    {
        var media = await context.Medias
            .Include(m => m.Auteur)
            .FirstOrDefaultAsync();
                
        if (media == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(media);
    }

    private static async Task<IResult> GetMediaByAuteur(ApiContext context, int auteurId)
    {
        if (await AuteurExists(context, auteurId))
        {
            var medias = await context.Medias
                .Include(m => m.Auteur)
                .Where(m => m.Auteur.Id == auteurId).ToListAsync();
                
            if (medias.Count == 0)
            {
                return Results.NoContent();
            }

            return Results.Ok(medias);
        }

        return Results.NotFound();
    }

    private static async Task<IResult> UpdateMediaById(ApiContext context, MediaRequest request, int id)
    {
        var media = await context.Medias.FindAsync(id); 
            
        if (media == null)
        {
            return Results.BadRequest();
        }
        
        var auteur = context.Auteurs.FirstOrDefault(a => a.Id == request.AuteurId);
        
        media.Name = request.Name ?? media.Name;
        media.Description = request.Description ?? media.Description;
        media.Reserved = request.Reserved ?? media.Reserved;
        media.Auteur = auteur ?? media.Auteur;
        media.Edition = request.Edition ?? media.Edition;
        media.DateSortie = request.DateSortie ?? media.DateSortie;

        if (await MediaExists(context, id))
        {
            await context.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }

    private static async Task<IResult> CreateMedia(ApiContext context, MediaRequest request)
    {
        var auteur = context.Auteurs.FirstOrDefault(a => a.Id == request.AuteurId);
        
        if (auteur != null)
        {
            request.AuteurId = auteur.Id;
            var mediaToAdd = new Media
            {
                Description = request.Description,
                Edition = request.Edition,
                Name = request.Name,
                Reserved = false,
                Auteur = auteur
            };
            await context.AddAsync(mediaToAdd);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }
        return Results.NotFound();
    }

    private static async Task<IResult> DeleteMedia(ApiContext context, int id)
    {
        var media = await context.Medias.FindAsync(id);
        if (media == null)
        {
            return Results.NotFound();
        }
        
        context.Medias.Remove(media);
        await context.SaveChangesAsync();
        
        return Results.Ok();
    }
    
    private static async Task<bool> MediaExists(ApiContext context, int id)
    {
        return await context.Medias.AnyAsync(e => e.Id == id);
    }
        
    private static async Task<bool> AuteurExists(ApiContext context, int id)
    {
        return await context.Auteurs.AnyAsync(e => e.Id == id);
    }
}
