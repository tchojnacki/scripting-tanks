# == BUILD CLIENT ==
FROM node:18.0-alpine AS build-client

# Install dependencies
WORKDIR /Client
COPY Client/package.json Client/package-lock.json ./
RUN npm ci

# Build client
COPY Client .
RUN npm run build

# == BUILD BACKEND ==
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-backend

# Install dependencies
WORKDIR /Backend
COPY Backend/*.csproj ./
RUN dotnet restore

# Build backend
COPY Backend .
RUN dotnet publish -c Release -o dist

# == RUN ==
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS production
COPY --from=build-client /Client/dist Client/dist
COPY --from=build-backend /Backend/dist Backend/dist
EXPOSE 3000
CMD dotnet ./Backend/dist/Backend.dll
