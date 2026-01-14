#!/bin/bash

# Warframe Utils Deployment Script
# This script helps deploy the application to a Linode server

set -e  # Exit on error

echo "🚀 Warframe Utils Deployment Script"
echo "===================================="

# Check if .env file exists
if [ ! -f .env ]; then
    echo "❌ .env file not found!"
    echo "📝 Creating .env from .env.example..."
    if [ -f .env.example ]; then
        cp .env.example .env
        echo "⚠️  Please edit .env file with your configuration before continuing!"
        exit 1
    else
        echo "❌ .env.example not found either. Please create .env manually."
        exit 1
    fi
fi

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    exit 1
fi

# Check if Docker Compose is installed
if ! command -v docker compose &> /dev/null; then
    echo "❌ Docker Compose is not installed. Please install Docker Compose first."
    exit 1
fi

echo "✅ Prerequisites check passed"
echo ""

# Build images
echo "🔨 Building Docker images..."
docker compose build

# Start services
echo "🚀 Starting services..."
docker compose up -d

# Wait for services to be ready
echo "⏳ Waiting for services to start..."
sleep 10

# Check service status
echo "📊 Service status:"
docker compose ps

echo ""
echo "✅ Deployment complete!"
echo ""
echo "📝 Next steps:"
echo "1. Run database migrations: docker compose exec backend dotnet ef database update"
echo "2. Check logs: docker compose logs -f"
echo "3. Access your application at: http://your-server-ip"
echo ""

