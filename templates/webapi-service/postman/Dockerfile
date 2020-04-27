FROM postman/newman:alpine

# Copy all collections
COPY ./collections /etc/newman

ENTRYPOINT [ "newman", "run", "Template.postman_collection.json" ]
CMD ["--environment", "local.postman_environment.json"]
