# Deployment Guide - Warframe Utils to Linode

This guide will walk you through deploying your Warframe Utils application to a Linode server using Docker.

## Prerequisites

- A Linode account with a running Linux server (Ubuntu 22.04 LTS recommended)
- SSH access to your Linode server
- A domain name (optional but recommended)
- Basic knowledge of Linux commands

## Step 1: Prepare Your Linode Server

### 1.1 Connect to Your Server

```bash
ssh root@your-linode-ip
```

### 1.2 Update System Packages

```bash
apt update && apt upgrade -y
```

### 1.3 Install Docker and Docker Compose

```bash
# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sh get-docker.sh

# Install Docker Compose
apt install docker-compose-plugin -y

# Add your user to docker group (if not using root)
usermod -aG docker $USER

# Verify installation
docker --version
docker compose version
```

### 1.4 Install Git (if not already installed)

```bash
apt install git -y
```

## Step 2: Clone Your Project

### 2.1 Create Application Directory

```bash
mkdir -p /opt/warframe-utils
cd /opt/warframe-utils
```

### 2.2 Clone Repository

If your code is in a Git repository:
```bash
git clone <your-repo-url> .
```

Or if you need to upload files manually, use SCP from your local machine:
```bash
# From your local machine
scp -r "D:\Projects\Warframe-Utils-.NET OLD\*" root@your-linode-ip:/opt/warframe-utils/
```

## Step 3: Configure Environment Variables

### 3.1 Create .env File

```bash
cd /opt/warframe-utils
nano .env
```

### 3.2 Add Environment Variables

```env
# PostgreSQL Database Configuration
POSTGRES_DB=warframe_utils
POSTGRES_USER=postgres
POSTGRES_PASSWORD=YOUR_STRONG_PASSWORD_HERE

# Backend URL (for frontend to connect)
BACKEND_URL=http://backend:8080
NEXT_PUBLIC_API_URL=http://your-domain.com/api

# Optional: If using a domain
DOMAIN=your-domain.com
```

**Important:** Replace `YOUR_STRONG_PASSWORD_HERE` with a strong password. Generate one using:
```bash
openssl rand -base64 32
```

### 3.3 Set Proper Permissions

```bash
chmod 600 .env
```

## Step 4: Configure Production Settings

### 4.1 Update appsettings.Production.json

Edit `Warframe Utils .NET/appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Database=warframe_utils;Username=postgres;Password=YOUR_PASSWORD_FROM_ENV"
  },
  "Cors": {
    "AllowedOrigins": [
      "http://your-domain.com",
      "https://your-domain.com",
      "http://your-linode-ip",
      "http://localhost:3000"
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Note:** The connection string will be overridden by the environment variable in docker-compose.yml, but it's good to have a fallback.

## Step 5: Configure Nginx (Optional but Recommended)

### 5.1 Update nginx.conf

Edit `nginx/nginx.conf` and replace `your-domain.com` with your actual domain name.

### 5.2 SSL Certificate (Recommended)

For production, you should set up SSL certificates. You can use Let's Encrypt:

```bash
# Install Certbot
apt install certbot python3-certbot-nginx -y

# Generate certificate (after DNS is configured)
certbot certonly --standalone -d your-domain.com
```

Then update `nginx/nginx.conf` to uncomment the HTTPS server block and point to your certificates.

## Step 6: Build and Start Services

### 6.1 Build Docker Images

```bash
cd /opt/warframe-utils
docker compose build
```

This may take several minutes the first time as it downloads base images and builds your applications.

### 6.2 Start Services

```bash
docker compose up -d
```

The `-d` flag runs containers in detached mode (background).

### 6.3 Check Container Status

```bash
docker compose ps
```

You should see all services running:
- postgres
- backend
- frontend
- nginx (if enabled)

### 6.4 View Logs

```bash
# All services
docker compose logs -f

# Specific service
docker compose logs -f backend
docker compose logs -f frontend
docker compose logs -f postgres
```

## Step 7: Run Database Migrations

### 7.1 Execute Migrations

```bash
# Enter the backend container
docker compose exec backend bash

# Run migrations
dotnet ef database update

# Exit container
exit
```

Or run it directly:
```bash
docker compose exec backend dotnet ef database update
```

**Note:** If `dotnet ef` is not available, you may need to install the EF tools in the Dockerfile or run migrations manually using SQL scripts.

## Step 8: Configure Firewall

### 8.1 Allow Required Ports

```bash
# Allow HTTP
ufw allow 80/tcp

# Allow HTTPS
ufw allow 443/tcp

# Allow SSH (important!)
ufw allow 22/tcp

# Enable firewall
ufw enable

# Check status
ufw status
```

## Step 9: Verify Deployment

### 9.1 Check Services

- Frontend: `http://your-linode-ip` or `http://your-domain.com`
- Backend API: `http://your-linode-ip:8080` or `http://your-domain.com/api`
- Database: Running internally (port 5432 not exposed)

### 9.2 Test the Application

1. Open your browser and navigate to your domain or IP
2. Try registering a new user
3. Test the search functionality
4. Create a price alert
5. Check that notifications work

## Step 10: Set Up Automatic Startup

### 10.1 Enable Docker to Start on Boot

```bash
systemctl enable docker
```

### 10.2 Create a Systemd Service (Optional)

Create `/etc/systemd/system/warframe-utils.service`:

```ini
[Unit]
Description=Warframe Utils Application
Requires=docker.service
After=docker.service

[Service]
Type=oneshot
RemainAfterExit=yes
WorkingDirectory=/opt/warframe-utils
ExecStart=/usr/bin/docker compose up -d
ExecStop=/usr/bin/docker compose down
TimeoutStartSec=0

[Install]
WantedBy=multi-user.target
```

Enable the service:
```bash
systemctl daemon-reload
systemctl enable warframe-utils.service
```

## Maintenance Commands

### View Logs
```bash
docker compose logs -f [service-name]
```

### Restart Services
```bash
docker compose restart [service-name]
# Or restart all
docker compose restart
```

### Stop Services
```bash
docker compose down
```

### Update Application
```bash
cd /opt/warframe-utils
git pull  # If using git
docker compose build
docker compose up -d
docker compose exec backend dotnet ef database update  # If migrations needed
```

### Backup Database
```bash
docker compose exec postgres pg_dump -U postgres warframe_utils > backup_$(date +%Y%m%d).sql
```

### Restore Database
```bash
docker compose exec -T postgres psql -U postgres warframe_utils < backup_YYYYMMDD.sql
```

## Troubleshooting

### Containers Won't Start
1. Check logs: `docker compose logs`
2. Verify .env file exists and has correct values
3. Check disk space: `df -h`
4. Verify Docker is running: `systemctl status docker`

### Database Connection Issues
1. Verify PostgreSQL is running: `docker compose ps postgres`
2. Check connection string in .env
3. View PostgreSQL logs: `docker compose logs postgres`

### Frontend Can't Connect to Backend
1. Verify CORS settings in appsettings.Production.json
2. Check backend logs: `docker compose logs backend`
3. Verify backend is accessible: `curl http://localhost:8080/api/GameStatus`

### Port Already in Use
If port 80, 443, or 8080 is already in use:
1. Find the process: `lsof -i :80` or `netstat -tulpn | grep :80`
2. Stop the conflicting service or change ports in docker-compose.yml

## Security Recommendations

1. **Change Default Passwords**: Always use strong, unique passwords
2. **Enable Firewall**: Use UFW or iptables
3. **Use SSL/HTTPS**: Set up Let's Encrypt certificates
4. **Regular Updates**: Keep system and Docker images updated
5. **Backup Database**: Set up automated backups
6. **Limit SSH Access**: Use key-based authentication
7. **Monitor Logs**: Set up log monitoring

## Next Steps

- Set up automated backups
- Configure monitoring (e.g., UptimeRobot)
- Set up CI/CD pipeline
- Configure email service for production notifications
- Set up log aggregation

## Support

If you encounter issues:
1. Check the logs first
2. Verify all environment variables are set correctly
3. Ensure all ports are accessible
4. Review the troubleshooting section above

