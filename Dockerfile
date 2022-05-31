# == CLIENT ==
FROM node:18.0-alpine AS client_build

# Install dependencies
WORKDIR /client
COPY client/package.json client/package-lock.json ./
RUN npm ci

# Build client
COPY client .
RUN npm run build

# == SERVER ==
FROM python:3.10-alpine AS production
WORKDIR /app

# Install dependencies
COPY requirements.txt ./
RUN pip install --no-cache-dir --requirement requirements.txt

# Run server
COPY . .
COPY --from=client_build /client/dist client/dist
CMD python server/main.py
