# Warframe Utils .NET

Warframe Utils .NET is a .NET web app that combines:
- **Warframe Market API (v2)** (read-only) for item/mod/arcane pricing + user price alerts
- **WarframeStatus (api.warframestat.us)** for **PC** live worldstate info (alerts, Baro, cycles, etc.)

Frontend is a **React SPA** that’s served by the ASP.NET Core app in production.

---

## MVP Decisions (Locked)

- **Backend:** ASP.NET Core MVC
- **.NET:** Latest LTS (**.NET 10 LTS**) :contentReference[oaicite:0]{index=0}
- **Auth:** ASP.NET Core Identity (email/password)
- **Database:** SQL Server (Docker container) + EF Core
- **Deployment:** Docker on Linode (SQL Server runs as a container too)
- **Market scope:** read-only (we link to warframe.market for posting orders)
- **Alerts evaluate:** every minute
- **Alert price basis:** lowest **online in-game** price (plus user target price)
- **Platform:** PC only
- **Notifications:** in-app only (while the page is open)
- **License:** MIT

---

## How Alerts Work (MVP)

A user alert contains:
- Item reference (Warframe Market item)
- Target price (platinum)
- Trigger condition:
  - `lowest_online_in_game_sell_price <= target_price`
- Delivery:
  - Real-time in-app notification (toast/popup) via SignalR while the user is connected

**Spam protection (planned):**
- Per-alert cooldown (e.g. don’t re-trigger the same alert every minute while the price stays under target)

---

## Tech Stack

### Backend
- ASP.NET Core MVC (.NET 10 LTS)
- ASP.NET Core Identity
- EF Core
- Background service for alert evaluation (1-minute interval)
- SignalR for real-time notifications

### Frontend
- React SPA (Vite)
- In production: built files are served from ASP.NET Core (`wwwroot`)

### Infrastructure
- Docker + Docker Compose
- Linode VPS
- Reverse proxy + HTTPS (Caddy or Nginx) (TBD)

---

## External APIs

### Warframe Market API
- Docs: https://warframe.market/api_docs  
- Note: v1 is deprecated; use v2 (linked in footer). :contentReference[oaicite:1]{index=1}

### WarframeStatus (Worldstate)
- Docs: https://docs.warframestat.us/ :contentReference[oaicite:2]{index=2}

---

## Repo Structure (Planned)

/src
/WarframeUtils.Web # ASP.NET Core MVC + API + Identity + SignalR
/WarframeUtils.Client # React (Vite)
/deploy
docker-compose.prod.yml
Caddyfile (or nginx config)

---

## Local Development (Planned)

### Prerequisites
- .NET 10 SDK
- Node.js (LTS)
- Docker + Docker Compose

### Run locally
1. Start SQL Server:
   - `docker compose up -d db`
2. Run backend:
   - `dotnet watch --project src/WarframeUtils.Web`
3. Run React dev server:
   - `cd src/WarframeUtils.Client && npm install && npm run dev`

React dev server will proxy `/api` and `/hubs` to the backend (no CORS headaches).

---

## Production (Linode + Docker) (Planned)

- One Compose stack:
  - `app` (ASP.NET Core serving React build)
  - `db` (SQL Server)
  - `proxy` (Caddy/Nginx for HTTPS)

**Security note:** SQL Server port should NOT be exposed publicly.

---

## Disclaimer

Not affiliated with Digital Extremes or warframe.market. Warframe and related trademarks belong to their respective owners.

---

## License

MIT
