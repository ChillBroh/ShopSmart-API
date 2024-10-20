# Use the official .NET 6 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the csproj file and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining project files and build the app
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET 6 runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy the build result from the previous stage
COPY --from=build-env /app/out .

# Expose the port that your API will be running on
EXPOSE 7018

# Define the entry point for the Docker container
ENTRYPOINT ["dotnet", "webApi.dll"]
