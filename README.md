# Trilha Crescimento API

API ASP.NET Core 8 organizada em `Controllers → Services → Repositories`, usando Dapper e PostgreSQL.

## Configuração

1. Configure o banco PostgreSQL/Supabase separadamente.
2. Ajuste `ConnectionStrings:DefaultConnection` em `appsettings.json` ou, preferencialmente, via Secret Manager:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Port=5432;Database=postgres;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true" --project TrilhaCrescimentoApi
dotnet run --project TrilhaCrescimentoApi
```

Swagger fica disponível em `/swagger` no ambiente Development.

## Endpoints

- `POST /api/users` — cria usuário.
- `GET /api/users/{id}` — consulta usuário.
- `POST /api/auth/login` — valida e-mail e senha e retorna o JWT da API.
- `POST /api/auth/google` — valida o ID token do Google e retorna um JWT desta API.

As senhas nunca são retornadas ou guardadas em texto puro. O projeto usa PBKDF2-SHA512, com salt aleatório e 210.000 iterações, e compara os hashes em tempo constante.

## Login com Google

Configure o Client ID do OAuth Web criado no Google Cloud e uma chave JWT secreta (mínimo 32 caracteres) via User Secrets:

```bash
dotnet user-secrets set "Google:ClientId" "SEU_CLIENT_ID.apps.googleusercontent.com" --project TrilhaCrescimentoApi
dotnet user-secrets set "Jwt:SigningKey" "uma-chave-secreta-com-ao-menos-32-caracteres" --project TrilhaCrescimentoApi
```

O frontend deve obter o ID Token pelo Google Identity Services e enviá-lo para `POST /api/auth/google`:

```json
{ "idToken": "..." }
```

A API valida o token, encontra ou cria o usuário e retorna um `accessToken`, que deve ser enviado nas chamadas protegidas como `Authorization: Bearer <accessToken>`.
