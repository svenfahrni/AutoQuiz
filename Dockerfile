# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Frontend build stage
FROM node:20-alpine AS frontend-build
WORKDIR /frontend
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ .
RUN npm run build

# Build stage for the .NET app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only project file(s) and restore dependencies
COPY Quizlet/Quizlet.csproj ./Quizlet/
RUN dotnet restore Quizlet/Quizlet.csproj

# Copy the rest of the source code and build the project
COPY . .
RUN dotnet build --configuration Release --no-restore ./Quizlet/Quizlet.csproj && \
    dotnet publish --configuration Release --no-restore -o /app/publish ./Quizlet/Quizlet.csproj

# Final stage: Copy build artifacts and run as non-root
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=frontend-build /frontend/dist /app/wwwroot

# Create a non-root user and switch to it
RUN adduser --disabled-password --gecos "" appuser
USER appuser

# Healthcheck: ensure the app is running by checking the health endpoint
HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Quizlet.dll"]