#!/bin/bash

# BlazerEditor Demo Runner
echo "🎨 BlazerEditor Demo Runner"
echo "================================"
echo ""

# Step 1: Build the library
echo "📦 Step 1: Building BlazerEditor library..."
dotnet build BlazerEditor.csproj
if [ $? -ne 0 ]; then
    echo "❌ Build failed!"
    exit 1
fi
echo "✅ Library built successfully!"
echo ""

# Step 2: Run the demo
echo "🚀 Step 2: Starting demo application..."
echo ""
echo "Demo will be available at:"
echo "  https://localhost:5001"
echo "  http://localhost:5000"
echo ""
echo "Press Ctrl+C to stop the demo"
echo ""

cd Demo
dotnet run
