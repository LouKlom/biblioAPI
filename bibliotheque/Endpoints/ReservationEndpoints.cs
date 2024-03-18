using bibliotheque.Models;
using bibliotheque.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace bibliotheque.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservation(this IEndpointRouteBuilder app)
    {
        //Get All Reservations
        app.MapGet("/api/reservations", async (ApiContext context) => await context.Reservations.ToListAsync())
            .WithTags("Reservations");

        //Get Reservation by Id
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

        //Get Reservation by clientId
        app.MapGet("/api/reservations/client/{clientId}", async (ApiContext context, int clientId) =>
        {
            if (ClientExists(context, clientId))
            {
                var reservations = await context.Reservations.Where(r => r.Client.Id == clientId).ToListAsync();
                if (reservations.Count == 0)
                {
                    return Results.NotFound();
                }

                return Results.Ok(reservations);
            }
            return Results.NotFound();
        }).WithTags("Reservations");

        //Get Reservation by Date
        app.MapGet("/api/reservations/{dateDebut}/{dateFin}", async (ApiContext context, DateTime dateDebut, DateTime dateFin) =>
        {
            var reservations = await context.Reservations.Where(r => r.DateDebut >= dateDebut && r.DateDebut <= dateFin).ToListAsync();
            return Results.Ok(reservations);
        }).WithTags("Reservations");

        //Update Reservation by Id
        app.MapPut("/api/reservations/{id}", async (ApiContext context, int id, Reservation reservation) =>
            {
                if (id != reservation.Id)
                {
                    return Results.BadRequest("Les id ne correspondent pas ");
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

        //Add a Reservation
        app.MapPost("/api/reservations", async (ApiContext context, ReservationRequest reservation) =>
            {
                //Check if media is reserved 
                if (!( await MediaReserved(context, reservation.MediaId.Value)))
                {
                    //Create Media
                    var media = await context.Medias.FirstOrDefaultAsync(x => x.Id == reservation.MediaId);
                    var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == reservation.ClientId);
                    if (media != null && client != null)
                    {
                        reservation.MediaId = media.Id;
                        reservation.ClientId = client.Id;
                        var reservationToAdd =
                            new Reservation
                            {
                                Client = client, 
                                Media = media, 
                                DateDebut = reservation.DateDebut.Value
                            };
                
                        await context.AddAsync(reservationToAdd);
                        await context.SaveChangesAsync();
                        //Update Media.reserved + DateDeSortie
                        return Results.NoContent();
                    }
                    return Results.NotFound();
                }
                return Results.BadRequest("Le Media est déjà reservé");
            })
            .WithTags("Reservations");

        //Delete a Reservation
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
        
        bool ClientExists(ApiContext context, int id)
        {
            return context.Clients.Any(e => e.Id == id);
        }
        
        bool MediaExists(ApiContext context, int id)
        {
            return context.Medias.Any(e => e.Id == id);
        }

        async Task<bool> MediaReserved(ApiContext context, int id)
        {
            var media = await context.Medias.FindAsync(id);
            return media.Reserved;
        }
    } 
}
