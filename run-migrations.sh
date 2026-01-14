#!/bin/bash

# Database Migration Script
# This script runs Entity Framework migrations in the Docker container

set -e

echo "🔄 Running database migrations..."

# Check if backend container is running
if ! docker compose ps backend | grep -q "Up"; then
    echo "❌ Backend container is not running. Please start it first with: docker compose up -d"
    exit 1
fi

# Run migrations
echo "📦 Executing migrations..."
docker compose exec backend dotnet ef database update

echo "✅ Migrations completed successfully!"

