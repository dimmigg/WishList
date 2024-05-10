FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY . ./
RUN dotnet restore WishList.Bot
RUN dotnet publish -c Release -o out WishList.Bot

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

CMD dotnet WishList.Bot.dll