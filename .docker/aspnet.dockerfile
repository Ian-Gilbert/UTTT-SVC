FROM mcr.microsoft.com/dotnet/aspnet:5.0
LABEL maintainer="https://github.com/Ian-Gilbert"

WORKDIR /workspace

COPY . .

CMD [ "dotnet", "UtttApi.WebApi.dll" ]
