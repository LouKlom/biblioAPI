using bibliotheque.Models;
using bibliotheque.Requests;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservation(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/reservations", GetAllReservations)
            .WithTags("Reservations");

        app.MapGet("/api/reservations/{id}", GetReservationById)
            .WithTags("Reservations");

        app.MapGet("/api/reservations/client/{clientId}", GetReservationByClientId)
            .WithTags("Reservations");

        app.MapGet("/api/reservations/{dateDebut}/{dateFin}", GetReservationByDate)
            .WithTags("Reservations");

        app.MapPatch("/api/reservations/{id}", UpdateReservation)
            .WithTags("Reservations");
        
        app.MapPut("/api/reservations/{id}/rendu", UpdateReservationRendu)
            .WithTags("Reservations");

        app.MapPost("/api/reservations", CreateReservation)
            .WithTags("Reservations");

        app.MapDelete("/api/reservations/{id}", DeleteReservation)
            .WithTags("Reservations");
    }

    private static async Task<IResult> GetAllReservations(ApiContext context)
    {
        return Results.Ok(await context.Reservations
            .Include(r => r.Client)
            .Include(r => r.Media)
            .Include(r => r.Media.Auteur)
            .ToListAsync());
    }

    private static async Task<IResult> GetReservationById(ApiContext context, int id)
    {
        var reservation = await context.Reservations
            .Include(r => r.Client)
            .Include(r => r.Media)
            .Include(r => r.Media.Auteur)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(reservation);
    }

    private static async Task<IResult> GetReservationByClientId(ApiContext context, int clientId)
    {
        if (!await ClientExists(context, clientId))
        {
            return Results.NotFound();
        }

        var reservations = await context.Reservations
            .Include(r => r.Client)
            .Include(r => r.Media)
            .Include(r => r.Media.Auteur)
            .Where(r => r.Client.Id == clientId)
            .ToListAsync();

        return Results.Ok(reservations);
    }

    private static async Task<IResult> GetReservationByDate(ApiContext context, DateTime dateDebut, DateTime dateFin)
    {
        var reservations = await context.Reservations
            .Include(r => r.Client)
            .Include(r => r.Media)
            .Include(r => r.Media.Auteur)
            .Where(r => r.DateDebut >= dateDebut && r.DateFin <= dateFin)
            .ToListAsync();

        return Results.Ok(reservations);
    }

    private static async Task<IResult> UpdateReservation(ApiContext context, int id, ReservationRequest request)
    {
        var reservation = await context.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return Results.NotFound();
        }

        var client = context.Clients.FirstOrDefault(c => c.Id == request.ClientId);
        var media = context.Medias.Include(media => media.Auteur).FirstOrDefault(m => m.Id == request.MediaId);

        reservation.Client = client ?? reservation.Client;
        reservation.Media = media ?? reservation.Media;
        reservation.DateDebut = request.DateDebut ?? reservation.DateDebut;
        reservation.DateFin = request.DateFin ?? reservation.DateFin;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    
    
    private static async Task<IResult> UpdateReservationRendu(ApiContext context, int id)
    {
        var reservation = await context.Reservations
            .Include(r => r.Media)
            .FirstOrDefaultAsync();

        if (reservation == null)
        {
            return Results.NotFound();
        }

        reservation.Media.Reserved = false;
        reservation.Rendu = true;
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> CreateReservation(ApiContext context, ReservationRequest request)
    {
        if (await IsMediaReserved(context, request.MediaId.Value))
        {
            return Results.BadRequest("Le média est déjà reservé");
        }
        
        var media = context.Medias.FirstOrDefault(m => m.Id == request.MediaId);
        var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == request.ClientId);
        
        if (media == null || client == null)
        {
            return Results.NotFound("Le media ou le client renseigné n'a pas été trouvé");
        }
            
        request.MediaId = media.Id;
        request.ClientId = client.Id;
        media.Reserved = true;
        var reservationToAdd =
            new Reservation
            {
                Client = client,
                Media = media,
                DateDebut = DateTime.Now,
                DateFin = DateTime.Now.AddDays(+ 15),
                Rendu = false
            };

        await context.AddAsync(reservationToAdd);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteReservation(ApiContext context, int id)
    {
        var reservation = await context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return Results.NotFound();
        }

        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<bool> ReservationExists(ApiContext context, int id)
    {
        return await context.Reservations.AnyAsync(e => e.Id == id);
    }
        
    private static async Task<bool> ClientExists(ApiContext context, int id)
    {
        return await context.Clients.AnyAsync(e => e.Id == id);
    }
        
    private static async Task<bool> MediaExists(ApiContext context, int id)
    {
        return await context.Medias.AnyAsync(e => e.Id == id);
    }

    private static async Task<bool> IsMediaReserved(ApiContext context, int id)
    {
        return await context.Medias.AnyAsync(m => m.Id == id && m.Reserved);
    }
}