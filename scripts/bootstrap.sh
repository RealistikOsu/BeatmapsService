#!/usr/bin/env bash
set -eo pipefail

if [ -z "$APP_ENV" ]; then
  echo "Please set APP_ENV"
  exit 1
fi

if [[ $PULL_SECRETS_FROM_VAULT -eq 1 ]]; then
  echo "Fetching secrets from vault"
  # TODO: revert to $APP_ENV
  akatsuki vault get beatmaps-service production-k8s -o .env
  echo "Fetched secrets from vault"
  source .env
  echo "Sourced secrets from vault"
fi

cd /app
dotnet BeatmapsService.dll