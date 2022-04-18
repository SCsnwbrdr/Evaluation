#!/bin/bash
# This Script is meant for local deployment cleanup only!

PS4='LINENO:'

#Load Shared Functions
source $(dirname "$0")/SharedFunctions.sh


# Checks the Variables for the deployments to ensure that they are set
source $(dirname "$0")/PreflightVariableChecks.sh

## Setup Variables the Script Needs
export ENVIRONMENT="local"
export AZ_RESOURCEGROUPNAME="security-repository-compliance-$ENVIRONMENT-$UNIQUEID"
## We only need one right now to delete
export FUNCTIONAPP_NAME=$(az functionapp list --resource-group $AZ_RESOURCEGROUPNAME --query [].name -o tsv)

echo
echo '###############################################################################'
echo 'Starting Github Hooks Destroy'
./github-artifacts/Destroy.sh || die "Local Github Webhook Destroy Script Failed"
echo 'Completed Github Hooks Destroy'
echo '###############################################################################'
echo

echo
echo '###############################################################################'
echo 'Starting Azure Resources Destroy'
./infra/Destroy.sh || die "Local Github Webhook Destroy Script Failed"
echo 'Completed Azure Resources Destroy'
echo '###############################################################################'
echo