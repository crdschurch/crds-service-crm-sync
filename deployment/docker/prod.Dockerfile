# Step 0: build, test, and publish application 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env 
WORKDIR /app 
 
# Copy files to /app 
COPY . ./ 

# Run Unit Tests 
RUN dotnet test Crossroads.Service.Contact.Tests/Crossroads.Service.CrmSync.Test.csproj 
RUN dotnet test MinistryPlatform.Test/MinistryPlatform.Test.csproj 

# Change working directory to Crossroads.Service.CrmSync
WORKDIR /app/Crossroads.Service.CrmSync
 
# Publish build to out directory 
RUN dotnet publish -c Release -o out 
 
# Step 1: Build runtime image 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 

WORKDIR /app/crmsync
 
# Copy over the build from the previous step 
COPY --from=build-env /app/Crossroads.Service.CrmSync/out . 

# Install wget
RUN echo 'installing wget' \
&& apt-get update \
&& apt-get install -y wget

RUN echo 'installing gnupg' \
&& apt-get install -y gnupg

# Install new relic
RUN echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget -O- https://download.newrelic.com/548C16BF.gpg | apt-key add - \
&& apt-get update \
&& apt-get install newrelic-netcore20-agent

ENV CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent

CMD $CORECLR_NEWRELIC_HOME/run.sh dotnet Crossroads.Service.CrmSync.dll
