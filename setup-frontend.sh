#!/bin/bash

# Warframe Utils - Quick Setup Script
# Run this script to set up the frontend

echo "========================================"
echo "  Warframe Utils - Frontend Setup"
echo "========================================"
echo ""

# Check if Node.js is installed
echo "Checking prerequisites..."
if ! command -v node &> /dev/null; then
    echo "❌ Node.js is not installed!"
    echo "Please install Node.js from https://nodejs.org/"
    exit 1
fi
echo "✓ Node.js version: $(node --version)"

# Check if npm is installed
if ! command -v npm &> /dev/null; then
    echo "❌ npm is not installed!"
    exit 1
fi
echo "✓ npm version: $(npm --version)"
echo ""

# Navigate to frontend directory
echo "Navigating to frontend directory..."
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
FRONTEND_PATH="$SCRIPT_DIR/warframe-frontend"

if [ ! -d "$FRONTEND_PATH" ]; then
    echo "❌ Frontend directory not found!"
    exit 1
fi

cd "$FRONTEND_PATH"
echo "✓ Found frontend directory"
echo ""

# Install dependencies
echo "Installing dependencies..."
echo "This may take a few minutes..."
npm install

if [ $? -ne 0 ]; then
    echo "❌ Failed to install dependencies!"
    exit 1
fi

echo "✓ Dependencies installed successfully"
echo ""

# Done
echo "========================================"
echo "  ✓ Setup Complete!"
echo "========================================"
echo ""
echo "Next steps:"
echo "1. Ensure your .NET backend is running"
echo "   cd 'Warframe Utils .NET'"
echo "   dotnet run"
echo ""
echo "2. Start the frontend development server"
echo "   cd warframe-frontend"
echo "   npm run dev"
echo ""
echo "3. Open your browser to:"
echo "   http://localhost:3000"
echo ""
echo "For detailed instructions, see SETUP_GUIDE.md"
echo ""
