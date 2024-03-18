using bibliotheque.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class MediaEndpoints
{
    public static void MapMedia(this IEndpointRouteBuilder app)
    {
        //Get All Media
        app.MapGet("/api/medias", async (ApiContext context) => await context.Medias.ToListAsync())
            .WithTags("Medias");

        //Get Media by id
        app.MapGet("/api/medias/{id}", async (ApiContext context, int id) =>
            {
                var media = await context.Medias.FindAsync(id);
                if (media == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(media);
            })
            .WithTags("Medias");

        //Get Media by Auteur
        app.MapGet("/api/medias/auteur/{auteurId}", async (ApiContext context, int auteurId) =>
        {
            if (AuteurExists(context, auteurId))
            {
                var medias = await context.Medias.Where(m => m.Auteur.Id == auteurId).ToListAsync();
                if (medias.Count == 0)
                {
                    return Results.NoContent();
                }

                return Results.Ok(medias);
            }

            return Results.NotFound();
        }).WithTags("Medias");

        //Update Media by Id 
        app.MapPut("/api/medias/{id}", async (ApiContext context, int id, Media media) =>
            {
                if (id != media.Id)
                {
                    return Results.BadRequest();
                }

                context.Entry(media).State = EntityState.Modified;

                if (MediaExists(context, id))
                {
                    await context.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            })
            .WithTags("Medias");

        //Add Media
        app.MapPost("/api/medias", async (ApiContext context, MediaRequest media) =>
            {
                //On verifie que l'auteur existe
                var auteur = context.Auteurs.FirstOrDefault(a => a.Id == media.AuteurId);
                if (auteur != null)
                {
                    media.AuteurId = auteur.Id;
                    var mediaToAdd = new Media
                    {
                        Description = media.Description,
                        Edition = media.Edition,
                        Name = media.Name,
                        Reserved = media.Reserved.Value,
                        Auteur = auteur
                    };
                    
                    await context.AddAsync(mediaToAdd);
                    await context.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            })
            .WithTags("Medias");

        //Delete Media by Id
        app.MapDelete("/api/medias/{id}", async (ApiContext context, int id) =>
            {
                var media = await context.Medias.FindAsync(id);
                if (media == null)
                {
                    return Results.NotFound();
                }

                context.Medias.Remove(media);
                await context.SaveChangesAsync();

                return Results.Ok();
            })
            .WithTags("Medias");

        bool MediaExists(ApiContext context, int id)
        {
            return context.Medias.Any(e => e.Id == id);
        }
        
        bool AuteurExists(ApiContext context, int id)
        {
            return context.Auteurs.Any(e => e.Id == id);
        }
    } 
}
