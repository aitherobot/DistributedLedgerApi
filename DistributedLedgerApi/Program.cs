using DistributedLedgerApi.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Servers = [ new OpenApiServer { Url = "/"} ];
                return Task.CompletedTask;
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Distributed Ledger API");
            });
        }

        app.MapWalletEndpoints();

        app.Run();
    }
}
