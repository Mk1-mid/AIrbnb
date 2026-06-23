FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/RentalPlatform.Domain/RentalPlatform.Domain.csproj src/RentalPlatform.Domain/
COPY src/RentalPlatform.Application/RentalPlatform.Application.csproj src/RentalPlatform.Application/
COPY src/RentalPlatform.Infrastructure/RentalPlatform.Infrastructure.csproj src/RentalPlatform.Infrastructure/
COPY src/RentalPlatform.Web/RentalPlatform.Web.csproj src/RentalPlatform.Web/
COPY src/RentalPlatform.Tests/RentalPlatform.Tests.csproj src/RentalPlatform.Tests/
COPY RentalPlatform.sln .

RUN dotnet restore RentalPlatform.sln

COPY . .
RUN dotnet publish src/RentalPlatform.Web/RentalPlatform.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y \
    tesseract-ocr \
    tesseract-ocr-spa \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "RentalPlatform.Web.dll"]
