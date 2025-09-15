FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY backend/Feedback.Api/Feedback.Api.csproj ./backend/Feedback.Api/
RUN dotnet restore "backend/Feedback.Api/Feedback.Api.csproj"
COPY . .
RUN dotnet build "backend/Feedback.Api/Feedback.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "backend/Feedback.Api/Feedback.Api.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feedback.Api.dll"]
