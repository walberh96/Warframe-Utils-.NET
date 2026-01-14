# Quick Deployment Reference

## Prerequisites Checklist

- [ ] Linode server with Ubuntu 22.04 LTS
- [ ] SSH access to server
- [ ] Docker and Docker Compose installed
- [ ] Domain name (optional but recommended)

## Quick Start (5 Steps)

### 1. Connect to Your Server
```bash
ssh root@your-linode-ip
```

### 2. Install Docker
```bash
curl -fsSL https://get.docker.com -o get-docker.sh && sh get-docker.sh
apt install docker-compose-plugin -y
```

### 3. Upload Your Code
```bash
# Option A: Using Git
mkdir -p /opt/warframe-utils && cd /opt/warframe-utils
git clone <your-repo> .

# Option B: Using SCP (from your local machine)
scp -r "D:\Projects\Warframe-Utils-.NET OLD\*" root@your-ip:/opt/warframe-utils/
```

### 4. Configure Environment
```bash
cd /opt/warframe-utils
nano .env  # Edit with your settings
# Set POSTGRES_PASSWORD, NEXT_PUBLIC_API_URL, etc.
```

### 5. Deploy
```bash
chmod +x deploy.sh run-migrations.sh
./deploy.sh
./run-migrations.sh
```

## Essential Commands

```bash
# View logs
docker compose logs -f

# Restart services
docker compose restart

# Stop everything
docker compose down

# Start everything
docker compose up -d

# Rebuild after code changes
docker compose build && docker compose up -d
```

## Configuration Files

- `.env` - Environment variables (create from .env.example)
- `docker-compose.yml` - Service orchestration
- `nginx/nginx.conf` - Reverse proxy configuration
- `Warframe Utils .NET/appsettings.Production.json` - Backend config

## Common Issues

**Port 80 already in use:**
```bash
# Find what's using it
lsof -i :80
# Stop nginx if needed
systemctl stop nginx
```

**Database connection fails:**
- Check POSTGRES_PASSWORD in .env matches appsettings.Production.json
- Verify postgres container is running: `docker compose ps postgres`

**Frontend can't reach backend:**
- Check CORS settings in appsettings.Production.json
- Verify BACKEND_URL in .env

## Full Documentation

See [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md) for detailed instructions.

