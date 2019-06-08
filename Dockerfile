# Pull the image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /src

# Add the source
ADD ./Sumer /src/Sumer
WORKDIR Sumer

# Build
RUN mkdir /app
RUN dotnet publish -c Release -o /app

# Run
CMD dotnet /app/Sumer.dll "$TOKEN" "$ADDRESS" "$PORT" "$PREFIX"
