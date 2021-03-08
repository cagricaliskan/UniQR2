FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish UniQR2.csproj -c Release -o out
WORKDIR out
ENTRYPOINT ["dotnet", "UniQR2.dll"]
