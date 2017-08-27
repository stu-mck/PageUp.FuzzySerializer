#!/bin/bash
set -e

dotnet restore
dotnet test ./PageUp.FuzzySerializer.Tests/PageUp.FuzzySerializer.Tests.csproj