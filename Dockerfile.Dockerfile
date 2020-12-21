FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update && apt-get install -y libgdiplus

WORKDIR /app 
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
 
COPY . .

RUN dotnet restore ./CrossCutting.PdfHelper.Tests/CrossCutting.PdfHelper.Tests.csproj

RUN dotnet test ./CrossCutting.PdfHelper.Tests/CrossCutting.PdfHelper.Tests.csproj
 
  
# ENTRYPOINT ["dotnet", "CrossCutting.PdfHelper.dll"]