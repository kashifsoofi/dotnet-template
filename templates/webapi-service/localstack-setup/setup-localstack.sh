#!/bin/sh

set -e

until aws --region eu-west-1 --endpoint-url=http://localstack:4566 sqs list-queues; do
  >&2 echo "Localstack SQS is unavailable - sleeping"
  sleep 1
done

>&2 echo "Localstack SQS is up - executing command"
aws --region eu-west-1 --endpoint-url=http://localstack:4566 sqs create-queue --queue-name Template-Api
aws --region eu-west-1 --endpoint-url=http://localstack:4566 sqs create-queue --queue-name Template-Api-1
aws --region eu-west-1 --endpoint-url=http://localstack:4566 sqs create-queue --queue-name Template-Host
aws --region eu-west-1 --endpoint-url=http://localstack:4566 sqs create-queue --queue-name error
aws --region eu-west-1 --endpoint-url=http://localstack:4566 s3api create-bucket --bucket template
aws --region eu-west-1 --endpoint-url=http://localstack:4566 s3api put-bucket-acl --bucket template --acl public-read