# Use the SDK image with the runtime included
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Install EF tool globally
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools" 

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN rm entrypoint.sh
RUN dotnet build -c Debug --no-restore

COPY entrypoint.sh /usr/bin/entrypoint.sh
RUN chmod +x /usr/bin/entrypoint.sh

# Run the application with hot-reload
ENTRYPOINT ["entrypoint.sh"]
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]
