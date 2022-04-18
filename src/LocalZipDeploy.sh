#!/bin/bash

#Load Shared Functions
source $(dirname "$0")/../SharedFunctions.sh

#Validate that variables exist
[ ! -z "$AZ_RESOURCEGROUPNAME" ] || die "You must set the variable RESOURCEGROUPNAME to continue."
[ ! -z "$DEPLOYMENTNAME" ] || die "You must set the variable DEPLOYMENTNAME to continue."

if [ -z "$FUNCTIONAPP_NAME" ]; then
    echo "No Function App name found in output. Please ensure that properties.output.functionName.value has an output in the ARM Deployment"
    exit 1
fi

echo "Got Function App Name: $FUNCTIONAPP_NAME"
cd $(dirname "$0")
func azure functionapp publish $FUNCTIONAPP_NAME --output none || die "Failed to publish function app $FUNCTIONAPP_NAME"
cd ..