using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class MediaEndpoints
{
    public static void MapMedia(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/medias", async (ApiContext context) => await context.Medias.ToListAsync())
            .WithTags("Medias");

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

        app.MapPost("/api/medias", async (ApiContext context, Media media) =>
            {
                var auteur = context.Auteurs.FirstOrDefault(a => a.Id == media.Auteur.Id);
                if (auteur != null)
                {
                    media.Auteur = auteur;
                }

                await context.AddAsync(media);
                return Results.NoContent();
            })
            .WithTags("Medias");

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
    } 
}
