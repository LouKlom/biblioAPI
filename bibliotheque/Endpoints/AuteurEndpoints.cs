using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class AuteurEndpoints
{
    public static void MapAuteur(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/Auteurs", async (ApiContext context) => await context.Auteurs.ToListAsync())
            .WithTags("Auteurs");

        app.MapGet("/api/Auteurs/{id}", async (ApiContext context, int id) =>
            {
                var auteur = await context.Auteurs.FindAsync(id);
                if (auteur == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(auteur);
            })
            .WithTags("Auteurs");

        app.MapPut("/api/Auteurs/{id}", async (ApiContext context, int id, Auteur auteur) =>
            {
                if (id != auteur.Id)
                {
                    return Results.BadRequest();
                }

                context.Entry(auteur).State = EntityState.Modified;

                if (AuteurExists(context, id))
                {
                    await context.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            })
            .WithTags("Auteurs");

        app.MapPost("/api/Auteurs", async (ApiContext context, Auteur auteur) =>
            {
                await context.AddAsync(auteur);
                await context.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithTags("Auteurs");

        app.MapDelete("/api/Auteurs/{id}", async (ApiContext context, int id) =>
            {
                var auteur = await context.Auteurs.FindAsync(id);
                if (auteur == null)
                {
                    return Results.NotFound();
                }

                context.Auteurs.Remove(auteur);
                await context.SaveChangesAsync();

                return Results.Ok();
            })
            .WithTags("Auteurs");

        bool AuteurExists(ApiContext context, int id)
        {
            return context.Auteurs.Any(e => e.Id == id);
        }
    } 
}
