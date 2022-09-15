FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN mkdir -p /usr/src
WORKDIR /usr/src

COPY Finbourne.Insights.Sdk.Extensions.sln /usr/src/
COPY Finbourne.Insights.Sdk.Extensions/Finbourne.Insights.Sdk.Extensions.csproj /usr/src/Finbourne.Insights.Sdk.Extensions/
COPY Finbourne.Insights.Sdk.Extensions.Tests/Finbourne.Insights.Sdk.Extensions.Tests.csproj /usr/src/Finbourne.Insights.Sdk.Extensions.Tests/
COPY Finbourne.Insights.Sdk.Extensions.IntegrationTests/Finbourne.Insights.Sdk.Extensions.IntegrationTests.csproj /usr/src/Finbourne.Insights.Sdk.Extensions.IntegrationTests/
RUN dotnet restore Finbourne.Insights.Sdk.Extensions

ENTRYPOINT dotnet test --configuration Release --verbosity normal