using CalculationHistory.Worker.Interfaces;
using CalculationHistory.Worker.Mappers;

namespace CalculationHistory.Worker.Endpoints;

public static class HistoryEndpoints
{
    public static void MapHistoryEndpoints(this WebApplication app)
    {
        app.MapGet("/api/history", (IHistoryStore store) =>
        {
            var records = store.GetHistory();
            
            var calculationHistoryDto = records.ToDto();
            
            return Results.Ok(calculationHistoryDto);
        });

        app.MapPost("/api/history/clear", (IHistoryStore store) =>
        {
            store.Clear();
            return Results.NoContent();
        });
    }
}