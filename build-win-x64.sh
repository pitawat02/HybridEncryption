#!/bin/bash

# Define variables
APP_NAME="HybridEncryption_win-x64"
CONFIGURATION="Release"
RUNTIME="win-x64" # Change to win-arm64 for Apple Silicon if needed
OUTPUT_DIR="publish"

# Clean previous builds
dotnet clean

# Publish the application as a single file without trimming
dotnet publish -c $CONFIGURATION -r $RUNTIME --self-contained true \
    /p:PublishSingleFile=true  \
    /p:PublishTrimmed=true    \
    /p:SelfContained=true   \
    -o $OUTPUT_DIR

echo "Single-file executable published to $OUTPUT_DIR/$APP_NAME.exe"

# dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true  /p:PublishTrimmed=true    /p:SelfContained=true   -o publish