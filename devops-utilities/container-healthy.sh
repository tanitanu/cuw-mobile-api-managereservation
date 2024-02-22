#!/bin/bash
set -e          # fail on command errors
set -o pipefail # fail if there is a failure within a pipe
# This script ensures the environment is ready by accessing a lambda behind the API Gateway.

# API_BASE_URL is the base URL used to access the env.  It defaults to he hashed env.
# Then it allows for overrides with $SUPPRESS_GIT_HASH and $BASE_URL
API_BASE_URL="http://localhost:90/$HTTP_SERVICE_NAME_URI/api/HealthCheck"


timeout=$ENV_READY_TIMEOUT

if [ "$timeout" == "" ]; then
  timeout=36000  # Timeout defaults to 60 minutes in seconds
fi

dateInSeconds=$(date +%s)
timeoutDate=$((dateInSeconds+timeout))

echo "Seeing if ${API_BASE_URL} is ready for use"

sleepSecs=15
ready="false"
while [ "$ready" != "true" ]; do
#  validate if healtcheck url returns Healthy
  if curl -s "${API_BASE_URL}" | grep "Healthy" ; then
    echo "${API_BASE_URL} is ready"
    ready=true
  else
    echo "Not ready, sleeping ${sleepSecs} seconds"
    sleep $sleepSecs
  fi
  currentDateInSeconds=$(date +%s)
  if [[ $currentDateInSeconds -gt $timeoutDate ]]; then
    echo "$HTTP_SERVICE_NAME_URI did not start within the timeout time of ${timeout} seconds"
    exit 1
  fi
done
