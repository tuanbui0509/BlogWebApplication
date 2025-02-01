# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and project files
COPY Blog.sln .
COPY src/ ./src/
COPY tests/ ./tests/  
# Add this line to copy the tests directory

# Restore dependencies
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o /app

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Blog.dll"]