using bibliotheque.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class ClientEndpoints
{
    public static void MapClient(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/Clients", async (ApiContext context) => await context.Clients.ToListAsync());
        
        app.MapGet("/api/Clients/{id}", async (ApiContext context, int id) =>
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(client);
        });

        app.MapPut("/api/Clients/{id}", async (ApiContext context, int id, Client client) =>
        {
            if (id != client.Id)
            {
                return Results.BadRequest();
            }
            
            context.Entry(client).State = EntityState.Modified;
            
            if (ClientExists(context, id))
            {
                await context.SaveChangesAsync();
                return Results.NoContent();
            }

            return Results.NotFound();
        });

        app.MapPost("/api/Clients", async (ApiContext context, Client client) =>
        {
            await context.AddAsync(client);
            return Results.NoContent();
        });

        app.MapDelete("/api/Clients/{id}", async (ApiContext context, int id) =>
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
            {
                return Results.NotFound();
            }
            
            context.Clients.Remove(client);
            await context.SaveChangesAsync();
            
            return Results.Ok();
        });

        bool ClientExists(ApiContext context, int id)
        {
            return context.Clients.Any(e => e.Id == id);
        }
    } 
}
