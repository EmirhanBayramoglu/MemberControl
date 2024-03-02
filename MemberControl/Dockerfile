#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY *.sln .
COPY ["MembersControlSystem/MembersControlSystem.csproj", "MembersControlSystem/"]
COPY ["Dtos/Dtos.csproj", "Dtos/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["ExceptionsClass/ExceptionsClass.csproj", "ExceptionsClass/"]
COPY ["Extensions/Extensions.csproj", "Extensions/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
RUN dotnet restore "MembersControlSystem/MembersControlSystem.csproj"
COPY . .
WORKDIR "/app/MembersControlSystem"
RUN dotnet publish -c Release -o out 
#RUN dotnet build "MembersControlSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MembersControlSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/MembersControlSystem/out ./
ENTRYPOINT ["dotnet", "MembersControlSystem.dll"]