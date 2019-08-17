FROM dotnetroundhouse/roundhouse
WORKDIR /db

# Copy all db files
COPY . .

ENTRYPOINT [ "rh", "-dt", "mysql" ]