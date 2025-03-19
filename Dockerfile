FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Frontend build stage
FROM node:20-alpine AS frontend-build
WORKDIR /frontend
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ .
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the rest of the code and build the app
COPY . .
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet publish --configuration Release --no-restore -o /app/publish

# Copy the build to the base image and define the entry point
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=frontend-build /frontend/dist /app/wwwroot
ENTRYPOINT ["dotnet", "Quizlet.dll"]
