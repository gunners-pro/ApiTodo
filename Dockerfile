# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# restore
COPY [".", "apitodo/"]
RUN dotnet restore 'apitodo/ApiTodo.csproj'

# build
WORKDIR /src/apitodo
RUN dotnet build -c Release -o /app/build

# Stage 2: Publish Stage
FROM build as publish
RUN dotnet publish --property:PublishDir=/app/publish

# Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "ApiTodo.dll" ]
