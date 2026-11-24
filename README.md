# DistributedLedgerApi

DistributedLedgerApi is a minimal ASP.NET Core Web API for managing in-memory wallet records. It provides CRUD-style endpoints and development-only OpenAPI and Scalar documentation.

## Requirements

- .NET 10 SDK
- A supported terminal or IDE

## Getting started

From the repository root, restore and run the API:

```powershell
dotnet restore .\DistributedLedgerApi\DistributedLedgerApi.csproj
dotnet run --project .\DistributedLedgerApi\DistributedLedgerApi.csproj
```

The default launch profile uses:

- HTTP: `http://localhost:5273`
- HTTPS: `https://localhost:7081`

The HTTP and HTTPS profile settings are defined in `DistributedLedgerApi/Properties/launchSettings.json`.

## API documentation

When running with the `Development` environment, OpenAPI and Scalar are available at:

- Scalar UI: `http://localhost:5273/scalar`
- OpenAPI document: `http://localhost:5273/openapi/v1.json`

The Scalar and OpenAPI routes are configured in `DistributedLedgerApi/Program.cs`.

## Wallet endpoints

All wallet routes use the `/api/wallets` base path.

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/wallets/` | Create a wallet |
| `GET` | `/api/wallets` | Get all wallets |
| `GET` | `/api/wallets/{id}` | Get a wallet by GUID |
| `PUT` | `/api/wallets/{id}` | Update a wallet name |
| `DELETE` | `/api/wallets/{id}` | Delete a wallet |

### Create a wallet

```http
POST /api/wallets/
Content-Type: application/json

{
  "name": "Primary wallet",
  "initialBalance": 100.50
}
```

Returns `201 Created` with the new wallet and a `Location` header containing the wallet URL.

### Update a wallet

```http
PUT /api/wallets/{id}
Content-Type: application/json

{
  "name": "Updated wallet"
}
```

Returns `200 OK` with the updated wallet.

### Wallet response

```json
{
  "id": "0b1d1e50-7b25-4fc1-b1d2-3e5f4fcfc1e9",
  "name": "Primary wallet",
  "balance": 100.50
}
```

Missing wallets return `404 Not Found`. Deleting an existing wallet returns `204 No Content`.

## Storage

Wallets are stored in a static in-memory dictionary. Data is lost whenever the application restarts and is not shared across multiple running instances. This project does not currently include database persistence, authentication, authorization, or transaction support.

## Project structure

```text
DistributedLedgerApi/
├── Endpoints/
│   └── WalletEndpoint.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── DistributedLedgerApi.csproj
├── global.json
└── Program.cs
```