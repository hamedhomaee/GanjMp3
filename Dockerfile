FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /website
COPY WebSite .
RUN dotnet publish --configuration Release -o ../built

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /website
COPY --from=build /built .
COPY /WebSite/Databases ./Databases
RUN mkdir wwwroot/uploads
RUN apt update && apt upgrade -y
RUN apt install ffmpeg -y
ENV Send_Grid=empty
EXPOSE 80
ENTRYPOINT [ "dotnet", "WebSite.dll" ]

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /webapi
# COPY WebApi .
# RUN dotnet publish --configuration Release -o ../built

# FROM mcr.microsoft.com/dotnet/aspnet:6.0
# WORKDIR /webapi
# RUN apt update && apt upgrade -y
# COPY --from=build /built .
# COPY /WebApi/Databases ./Databases
# EXPOSE 80
# ENTRYPOINT [ "dotnet", "WebApi.dll" ]