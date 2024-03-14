using bibliotheque.Models;
using bibliotheque.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservation(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/reservations", async (ApiContext context) => await context.Reservations.ToListAsync())
            .WithTags("Reservations");

        app.MapGet("/api/reservations/{id}", async (ApiContext context, int id) =>
            {
                var reservation = await context.Reservations.FindAsync(id);
                if (reservation == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(reservation);
            })
            .WithTags("Reservations");

        app.MapPut("/api/reservations/{id}", async (ApiContext context, int id, Reservation reservation) =>
            {
                if (id != reservation.Id)
                {
                    return Results.BadRequest();
                }

                context.Entry(reservation).State = EntityState.Modified;

                if (ReservationExists(context, id))
                {
                    await context.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            })
            .WithTags("Reservations");

        app.MapPost("/api/reservations", async (ApiContext context, ReservationRequest reservation) =>
            {
                var media = await context.Medias.FirstOrDefaultAsync(x => x.Id == reservation.MediaId);
                if (media != null)
                {
                    reservation.MediaId = media.Id;
                }

                var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == reservation.ClientId);
                if (client != null)
                {
                    reservation.ClientId = client.Id;
                }

                var reservationToAdd =
                    new Reservation { Client = client, Media = media, DateDebut = reservation.DateDebut };
                
                await context.AddAsync(reservationToAdd);
                await context.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithTags("Reservations");

        app.MapDelete("/api/reservations/{id}", async (ApiContext context, int id) =>
            {
                var reservation = await context.Reservations.FindAsync(id);
                if (reservation == null)
                {
                    return Results.NotFound();
                }

                context.Reservations.Remove(reservation);
                await context.SaveChangesAsync();

                return Results.Ok();
            })
            .WithTags("Reservations");

        bool ReservationExists(ApiContext context, int id)
        {
            return context.Reservations.Any(e => e.Id == id);
        }
    } 
}
