using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace bibliotheque.Endpoints;

public static class AuteurEndpoints
{
    public static void MapAuteur(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/Auteurs", GetAllAuteur)
            .Produces(StatusCodes.Status200OK)
            .WithTags("Auteurs");

        app.MapGet("/api/Auteurs/{id}", GetAuteurById)
            .WithTags("Auteurs");

        app.MapPut("/api/Auteurs/{id}", PutAuteur)
            .WithTags("Auteurs");

        app.MapPost("/api/Auteurs", CreateAuteur)
            .WithTags("Auteurs");

        app.MapDelete("/api/Auteurs/{id}", DeleteAuteur)
            .WithTags("Auteurs");
    }

    private static async Task<IResult> GetAllAuteur(ApiContext context)
    {
        return Ok(await context.Auteurs.ToListAsync());
    }

    private static async Task<IResult> GetAuteurById(ApiContext context, int id)
    {
        return await context.Auteurs.FindAsync(id) 
            is Auteur auteur
            ? Ok(auteur)
            : NotFound();
    }

    private static async Task<IResult> PutAuteur(ApiContext context, int id, AuteurRequest request)
    {
        var auteur = await context.Auteurs.FindAsync(id); 
        
        if (auteur == null)
        {
            return Results.BadRequest();
        }

        context.Entry(auteur).State = EntityState.Modified;

        if (await AuteurExists(context, id))
        {
            await context.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }

    private static async Task<IResult> CreateAuteur(ApiContext context, AuteurRequest auteur)
    {
        var auteurToAdd = new Auteur { FirstName = auteur.FirstName, LastName = auteur.LastName };
        await context.AddAsync(auteurToAdd);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteAuteur(ApiContext context, int id)
    {
        var auteur = await context.Auteurs.FindAsync(id);
        if (auteur == null)
        {
            return Results.NotFound();
        }

        context.Auteurs.Remove(auteur);
        await context.SaveChangesAsync();

        return Results.Ok();
    }
    
    private static async Task<bool> AuteurExists(ApiContext context, int id)
    {
        return await context.Auteurs.AnyAsync(e => e.Id == id);
    }
} 
