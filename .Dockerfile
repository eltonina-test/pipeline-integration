FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

# Install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
       libc6-dev \
       libgdiplus \
       libx11-dev \
    && rm -rf /var/lib/apt/lists/*

# Install Microsoft core fonts
RUN echo "deb http://deb.debian.org/debian stable main contrib non-free" > /etc/apt/sources.list \
    && echo "ttf-mscorefonts-installer msttcorefonts/accepted-mscorefonts-eula select true" | debconf-set-selections \
    && apt-get update \
    && apt-get install -y \
        ttf-mscorefonts-installer \
    && apt-get clean \
    && apt-get autoremove -y \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "CrossCutting.PdfHelper.Tests/CrossCutting.PdfHelper.Tests.csproj"
 
RUN dotnet test "CrossCutting.PdfHelper.Tests.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "MyProject.csproj" -c Release -o /app/publish
# 
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "MyProject.dll"]