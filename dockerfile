FROM microsoft/dotnet:2.2-sdk-alpine3.8
RUN apk update
RUN apk add --no-cache bash
COPY . /app
WORKDIR /app
EXPOSE 8081
RUN ["dotnet", "restore"]
RUN ["dotnet", "publish","--configuration", "Release","--framework","netcoreapp2.2"]
RUN ["ls", "/app/bin/Release/netcoreapp2.2/publish/"]
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
# ENTRYPOINT [ "dotnet", "--server.urls", "http://*:80", "/app/bin/Release/netcoreapp2.2/publish/AllNewsServer.dll" ]