FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . /app

WORKDIR /app

RUN dotnet restore
RUN dotnet publish --property:PublishDir=build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app/build

COPY --from=build-env /app/build .

ENTRYPOINT [ "dotnet", "ApiTodo.dll" ]