using System.Text.RegularExpressions;
using bibliotheque.Common.Constants;
using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class ClientEndpoints
{
    private static ApiContext _context;
    public static void MapClient(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/Clients", GetAllClients)
            .WithTags("Clients");

        app.MapGet("/api/Clients/{id}", GetClientById)
            .WithTags("Clients");

        app.MapPut("/api/Clients/{id}", PutClient)
            .WithTags("Clients");

        app.MapPost("/api/Clients", CreateClient)
            .WithTags("Clients");

        app.MapDelete("/api/Clients/{id}", DeleteClient)
            .WithTags("Clients");
    }

    private static async Task<IResult> GetAllClients(ApiContext context)
    {
        return TypedResults.Ok(await context.Clients.ToListAsync());
    }
    
    private static async Task<IResult> GetClientById(ApiContext context, int id)
    {
        return await context.Clients.FindAsync(id) 
            is Client client
            ? TypedResults.Ok(client)
            : TypedResults.NotFound();
    }

    private static async Task<IResult> CreateClient(ApiContext context, ClientRequest client)
    {
        if (!Regex.IsMatch(client.Phone, PhoneValidator.PhoneRegex))
        {
            return Results.BadRequest("Le numéro de téléphone n'est pas au bon format");
        }

        if (!Regex.IsMatch(client.Mail, EmailValidator.EmailRegex))
        {
            return Results.BadRequest("L'adresse mail n'est pas au bon format");
        }

        if (await FindByNameOrMailAsync(context, client.Name, client.Mail))
        {
            return Results.BadRequest("Le nom ou l'adresse mail est déjà utilisée");
        }
                
        var clientToAdd = new Client { Name = client.Name, Mail = client.Mail, Phone = client.Phone };
        await context.AddAsync(clientToAdd);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    
    private static async Task<IResult> PutClient(ApiContext context, int id, ClientRequest request)
    {
        var client = await context.Clients.FindAsync(id); 
        
        if (client == null)
        {
            return Results.BadRequest();
        }

        context.Entry(client).State = EntityState.Modified;

        if (await ClientExists(context, id))
        {
            if (await FindByNameOrMailAsync(context, request.Name, request.Mail))
            {
                return Results.BadRequest("Le nom ou l'adresse mail est déjà utilisée");
            }
            await context.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }
    
    private static async Task<IResult> DeleteClient(ApiContext context, int id)
    {
        var client = await context.Clients.FindAsync(id);
        if (client == null)
        {
            return Results.NotFound();
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync();

        return Results.Ok();
    }
    
    private static async Task<bool> ClientExists(ApiContext context, int id)
    {
        return await context.Clients.AnyAsync(e => e.Id == id);
    }

    private static async Task<bool> FindByNameOrMailAsync(ApiContext context ,string name, string mail)
    {
        return await context.Clients
            .AnyAsync(c => c.Name == name || c.Mail == mail);
    }
}