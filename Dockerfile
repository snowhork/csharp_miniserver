FROM microsoft/dotnet

WORKDIR /app
COPY . /app
CMD dotnet watch run