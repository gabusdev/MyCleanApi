FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

COPY publish .

# ENV ASPNETCORE_URLS=https://+:5050;http://+:5060
ENV ASPNETCORE_URLS=http://+:5060
# EXPOSE 5050
EXPOSE 5060

ENTRYPOINT ["dotnet", "WebApi.dll"]