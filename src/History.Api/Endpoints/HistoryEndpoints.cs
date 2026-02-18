using History.Api.DTOs;
using History.Api.Interfaces;
using History.Api.Mappers;

namespace History.Api.Endpoints;

public static class HistoryEndpoints
{
    public static void MapHistoryEndpoints(this WebApplication app)
    {
        app.MapPost("/history/records", (HistoryRecordDto dto, IHistoryStore store) =>
        {
            store.Add(dto.ToDomain());
            return Results.Accepted();
        });
            
        app.MapGet("/history", (IHistoryStore store) =>
        {
            return Results.Ok(store.GetHistory());
        });
    }
}