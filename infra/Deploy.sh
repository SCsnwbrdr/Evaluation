#!/bin/bash

#Load Shared Functions
source $(dirname "$0")/../SharedFunctions.sh

#Validate Requried Variables Exist
[ ! -z "$AZ_RESOURCEGROUPNAME" ] || die "You must set the variable RESOURCEGROUPNAME to continue."
[ ! -z "$DEPLOYMENTNAME" ] || die "You must set the variable DEPLOYMENTNAME to continue."
[ ! -z "$ENVIRONMENT" ] || die "You must set the variable ENVIRONMENT to continue."
[ ! -z "$UNIQUEID" ] || die "You must set the variable UNIQUEID to continue."
[[ -z "$PEMSTRING" || -z "$PAT" ]] || die "You must set the variable PEMSTRING or PAT to continue."

## Create Resource Group

echo "Creating Azure Resource Group $AZ_RESOURCEGROUPNAME"
az group create -n $AZ_RESOURCEGROUPNAME -l eastus2 --output none || die "Failed to provision Azure Resource Group $RGGROUP"
echo "Created Group!"

## Deploy out the Resources

echo "Starting Deployment with $DEPLOYMENTNAME in $AZ_RESOURCEGROUPNAME"

az deployment group create --resource-group $AZ_RESOURCEGROUPNAME \
--name "$DEPLOYMENTNAME" \
--template-file ./infra/Templates/template.json \
--parameters environment=$ENVIRONMENT \
--parameters unique_string=$UNIQUEID \
--parameters deployment_user_id=$(az ad signed-in-user show --query objectId -o tsv) \
--parameters keyValue="$PEMSTRING" \
--parameters patValue="$PAT" --output none || die "Deployment Failed"

echo "Completed Deployment!"

