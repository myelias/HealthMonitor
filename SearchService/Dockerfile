FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
# /App is a directory inside Docker's file system
WORKDIR /App 
# The port we are using internally is 80
EXPOSE 80
# Copy everything source -> destination
COPY backend.sln backend.sln 
# Now copy all the csproj files source -> destination
COPY FitbitHeartRateDataService/FitbitHeartRateDataService.csproj FitbitHeartRateDataService/FitbitHeartRateDataService.csproj
COPY SearchService/SearchService.csproj SearchService/SearchService.csproj
COPY IdentityService/IdentityService.csproj IdentityService/IdentityService.csproj
COPY GatewayService/GatewayService.csproj GatewayService/GatewayService.csproj
COPY Contracts/Contracts.csproj Contracts/Contracts.csproj 
# Restore as distinct layers
RUN dotnet restore backend.sln 

COPY SearchService SearchService
COPY Contracts Contracts
WORKDIR /App/SearchService


# Build and publish a release
RUN dotnet publish -c Release -o /App/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "SearchService.dll"]