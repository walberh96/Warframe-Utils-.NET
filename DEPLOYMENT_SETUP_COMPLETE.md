# Deployment Setup Complete! 🎉

Your Warframe Utils application is now ready for deployment to Linode using Docker.

## What Was Created

### Docker Configuration Files
1. **`Warframe Utils .NET/Dockerfile`** - Multi-stage build for .NET backend
2. **`warframe-frontend/Dockerfile`** - Multi-stage build for Next.js frontend
3. **`docker-compose.yml`** - Orchestrates all services (PostgreSQL, Backend, Frontend, Nginx)
4. **`.dockerignore` files** - Excludes unnecessary files from Docker builds

### Configuration Files
1. **`Warframe Utils .NET/appsettings.Production.json`** - Production settings for backend
2. **`nginx/nginx.conf`** - Reverse proxy configuration
3. **`.env.example`** - Template for environment variables

### Deployment Scripts
1. **`deploy.sh`** - Automated deployment script
2. **`run-migrations.sh`** - Database migration helper

### Documentation
1. **`DEPLOYMENT_GUIDE.md`** - Comprehensive step-by-step deployment guide
2. **`QUICK_DEPLOY.md`** - Quick reference for common tasks

## Changes Made to Your Code

### Backend (.NET)
- ✅ Updated CORS configuration to read from `appsettings.json`
- ✅ Created production appsettings file
- ✅ Dockerfile configured for .NET 8.0

### Frontend (Next.js)
- ✅ Updated `next.config.mjs` to support environment variables and standalone output
- ✅ Created API utility (`src/lib/api.ts`) for consistent API calls
- ✅ Updated all components to use relative URLs (works with Next.js rewrites)
- ✅ Updated `AuthContext` to use API utility

## Next Steps

### 1. Review the Deployment Guide
Read `DEPLOYMENT_GUIDE.md` for detailed instructions.

### 2. Prepare Your Linode Server
- Create a Linode instance (Ubuntu 22.04 LTS recommended)
- Note your server's IP address
- Set up SSH access

### 3. Configure Environment Variables
Create a `.env` file based on `.env.example`:
```bash
POSTGRES_PASSWORD=your_strong_password_here
NEXT_PUBLIC_API_URL=http://your-domain.com/api
BACKEND_URL=http://backend:8080
```

### 4. Update Production Settings
Edit `Warframe Utils .NET/appsettings.Production.json`:
- Add your domain/IP to CORS AllowedOrigins
- Update connection string if needed (though it's overridden by .env)

### 5. Deploy!
Follow the steps in `DEPLOYMENT_GUIDE.md` or use the quick start:
```bash
./deploy.sh
./run-migrations.sh
```

## Important Notes

### Security
- ⚠️ **Change all default passwords** in `.env`
- ⚠️ **Update CORS origins** in `appsettings.Production.json` with your actual domain
- ⚠️ **Set up SSL/HTTPS** for production (see deployment guide)

### Database
- The connection string in `appsettings.Production.json` is a fallback
- The actual connection string comes from the `ConnectionStrings__DefaultConnection` environment variable in docker-compose.yml
- Make sure `POSTGRES_PASSWORD` in `.env` matches what you want to use

### Ports
- **80** - HTTP (Nginx)
- **443** - HTTPS (Nginx, when configured)
- **8080** - Backend API (internal, proxied by Nginx)
- **3000** - Frontend (internal, proxied by Nginx)
- **5432** - PostgreSQL (internal only, not exposed)

## Testing Locally (Optional)

Before deploying to Linode, you can test the Docker setup locally:

```bash
# Create .env file
cp .env.example .env
# Edit .env with local settings

# Build and start
docker compose build
docker compose up -d

# Run migrations
docker compose exec backend dotnet ef database update

# Access at http://localhost
```

## Support

If you encounter issues:
1. Check `DEPLOYMENT_GUIDE.md` troubleshooting section
2. View logs: `docker compose logs -f`
3. Verify all environment variables are set correctly
4. Ensure ports are not already in use

## Architecture Overview

```
Internet
   ↓
Nginx (Port 80/443)
   ↓
   ├─→ Frontend (Next.js) - Port 3000
   └─→ Backend (.NET) - Port 8080
           ↓
       PostgreSQL - Port 5432
```

All services run in Docker containers and communicate through an internal network.

Good luck with your deployment! 🚀

