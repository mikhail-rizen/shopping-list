#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ShoppingList.Items.Api/ShoppingList.Items.Api.csproj", "ShoppingList.Items.Api/"]
COPY ["ShoppingList.Items.Data/ShoppingList.Items.Data.csproj", "ShoppingList.Items.Data/"]
COPY ["ShoppingList.Items.Messaging.Receive/ShoppingList.Items.Messaging.Receive.csproj", "ShoppingList.Items.Messaging.Receive/"]
COPY ["ShoppingList.Items.Service/ShoppingList.Items.Service.csproj", "ShoppingList.Items.Service/"]
COPY ["ShoppingList.Items.Messaging.Send/ShoppingList.Items.Messaging.Send.csproj", "ShoppingList.Items.Messaging.Send/"]
COPY ["ShoppingList.Items.Messaging/ShoppingList.Items.Messaging.csproj", "ShoppingList.Items.Messaging/"]
RUN dotnet restore "ShoppingList.Items.Api/ShoppingList.Items.Api.csproj"
COPY . .
WORKDIR "/src/ShoppingList.Items.Api"
RUN dotnet build "ShoppingList.Items.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingList.Items.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingList.Items.Api.dll"]