using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class ClientEndpoints
{
    public static void MapClient(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/Clients", async (ApiContext context) => await context.Clients.ToListAsync())
            .WithTags("Clients");

        app.MapGet("/api/Clients/{id}", async (ApiContext context, int id) =>
            {
                var client = await context.Clients.FindAsync(id);
                if (client == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(client);
            })
            .WithTags("Clients");

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
            })
            .WithTags("Clients");

        app.MapPost("/api/Clients", async (ApiContext context, ClientRequest client) =>
            {
                var clientToAdd = new Client { Name = client.Name, Mail = client.Mail, Phone = client.Phone };
                await context.AddAsync(clientToAdd);
                await context.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithTags("Clients");

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
            })
            .WithTags("Clients");

        bool ClientExists(ApiContext context, int id)
        {
            return context.Clients.Any(e => e.Id == id);
        }
    } 
}
