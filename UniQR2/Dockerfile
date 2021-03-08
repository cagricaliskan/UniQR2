FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish UniQR2.csproj -c Release -o out
WORKDIR out
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:1453  
EXPOSE 1453
ENTRYPOINT ["dotnet", "UniQR2.dll"]
