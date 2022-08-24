# == BUILD CLIENT ==
FROM node:18.0-alpine AS client_build

# Install dependencies
WORKDIR /client
COPY client/package.json client/package-lock.json ./
RUN npm ci

# Build client
COPY client .
RUN npm run build

# == BUILD SERVER ==
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS server_build

# Install dependencies
WORKDIR /server
COPY Backend/*.csproj ./
RUN dotnet restore

# Build server
COPY Backend .
RUN dotnet publish -c Release -o out

# == RUN ==
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS production
COPY --from=client_build /client/dist client/dist
COPY --from=server_build /server/out Backend
CMD dotnet ./Backend/Backend.dll
