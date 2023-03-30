# https://github.com/dotnet/announcements/issues/197
FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy and build everything else
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/runtime:2.1
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "DrayTek WAN Status.dll"]

VOLUME /app/config /app/data
EXPOSE 51400/udp
