# Warframe Utils .NET

Warframe Utils .NET is a .NET web app that combines:
- Warframe Market API (v2) (read-only) for item/mod/arcane pricing + user price alerts
- WarframeStatus (PC) for live worldstate info (alerts, Baro, cycles, etc.)

Frontend is a React SPA. In production, the ASP.NET Core app serves the built React files.

---

## MVP Decisions (Locked)

- Backend: ASP.NET Core MVC
- .NET: Latest LTS (.NET 10 LTS)
- Auth: ASP.NET Core Identity (email/password)
- Database: SQL Server + EF Core
- Market scope: read-only (link out to warframe.market for posting orders)
- Alerts evaluate: every minute
- Alert basis: lowest online in-game price + user target price
- Platform: PC only
- Notifications: in-app only (while the page is open)
- License: MIT

---

## Local Development (No Docker)

### Prerequisites
- .NET SDK (LTS)
- Node.js (LTS)
- SQL Server (Developer or Express) installed locally

### Run locally
1) Start SQL Server (local install)  
2) Set connection string for Development  
3) Apply EF migrations  
4) Run backend + frontend:

Backend:
- `dotnet watch run --project src/WarframeUtils.Web`

Frontend:
- `cd src/WarframeUtils.Client`
- `npm install`
- `npm run dev`

React dev server proxies `/api` and `/hubs` to the backend.

---

## Production (Later: Linode + Docker)

Once the app is ready:
- Dockerize the app and SQL Server
- Run with Docker Compose on Linode
- Reverse proxy + HTTPS (Caddy or Nginx)
- Keep SQL Server port private (not exposed publicly)

---

## Disclaimer

Not affiliated with Digital Extremes or warframe.market.
Warframe and related trademarks belong to their respective owners.

---

## License

MIT
