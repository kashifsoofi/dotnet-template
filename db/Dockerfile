FROM dotnetroundhouse/roundhouse
WORKDIR /db

RUN dotnet tool install dotnet-script -g

# Copy all db files
COPY . .

ENTRYPOINT [ "dotnet-script", "Detector.csx", "--", ${CONNECTION_STRING}, "&&", "rh", "--silent", "-dt", "mysql", "-cs", ${CONNECTION_STRING} ]