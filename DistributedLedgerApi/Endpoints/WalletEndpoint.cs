using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DistributedLedgerApi.Endpoints;

public record CreateWalletRequest(string Name, decimal InitialBalance);

public record WalletResponse(
    Guid Id,
    string Name,
    decimal Balance
);

public static class WalletEndpoint
{
    private static readonly Dictionary<Guid, WalletResponse> Wallets = [];

    public static RouteGroupBuilder MapWalletEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/wallets");

        group.MapPost("/", CreateWalletAsync);

        group.MapGet("/{id:guid}", GetWalletByIdAsync);

        group.MapGet("/", GetAllWalletsAsync);

        return group;
    }

    private static async Task<Ok<List<WalletResponse>>> GetAllWalletsAsync()
    {
        var wallets = Wallets.Values.ToList();

        await Task.CompletedTask;
        return TypedResults.Ok(wallets);
    }

    private static async Task<Created<WalletResponse>> CreateWalletAsync(CreateWalletRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var wallet = new WalletResponse(
                Guid.NewGuid(),
                request.Name,
                request.InitialBalance
            );

        Wallets[wallet.Id] = wallet;

        await Task.CompletedTask;
        return TypedResults.Created($"/api/wallets/{wallet.Id}", wallet);
    }

    private static async Task<Results<Ok<WalletResponse>, NotFound>> GetWalletByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var wallet = Wallets.FirstOrDefault(w => w.Key == id).Value;
        if (wallet is null)
        {
            return TypedResults.NotFound();
        }

        await Task.CompletedTask;
        return TypedResults.Ok(wallet);
    }
}
