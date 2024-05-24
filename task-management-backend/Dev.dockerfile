# Use the SDK image with the runtime included
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet build -c Debug --no-restore

# Run the application with hot-reload
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]
