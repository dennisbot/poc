FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["mailmerge/mailmerge.csproj", "mailmerge/"]
RUN dotnet restore "mailmerge/mailmerge.csproj"
COPY . .
WORKDIR "/src/mailmerge"
RUN dotnet build "mailmerge.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "mailmerge.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "mailmerge.dll"]