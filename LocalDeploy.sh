#!/bin/bash
# This Script is meant for local deployment only!

PS4='LINENO:'

#Load Shared Functions
source $(dirname "$0")/SharedFunctions.sh

# Checks to See if tools are installed required for run
source $(dirname "$0")/PreflightToolCheck.sh

# Checks the Variables for the deployments to ensure that they are set
source $(dirname "$0")/PreflightVariableChecks.sh

## Setup Variables the Script Needs
export ENVIRONMENT="local"
export AZ_RESOURCEGROUPNAME="security-repository-compliance-$ENVIRONMENT-$UNIQUEID"
export PEMSTRING="$GITHUB_PRIVATEKEY"
export PAT="$GITHUB_PERSONAL_ACCESS_TOKEN"
export TIMESTAMP=$(date +%s)
export DEPLOYMENTNAME="$AZ_RESOURCEGROUPNAME-$TIMESTAMP" 

echo
echo '###############################################################################'
echo 'Starting Azure Deployment Script for Personal Testing'
./infra/Deploy.sh || die "Deployment Script Failed"
echo 'Completed Azure Deployment Script for Personal Testing'
echo '###############################################################################'
echo
# We will need this for upcomming commands
export FUNCTIONAPP_NAME=$(az deployment group show --resource-group $AZ_RESOURCEGROUPNAME --name $DEPLOYMENTNAME --query properties.outputs.functionName.value -o tsv)
echo
echo 'Starting Azure Zip Deploy for Function'
echo '###############################################################################'
./src/LocalZipDeploy.sh || die "Zip Deployment Script Failed"
echo '###############################################################################'
echo 'Completed Azure Zip Deploy for Function'
echo
echo 'Starting Github Hooks Deployment'
echo '###############################################################################'
./github-artifacts/Deploy.sh || die "Local Github Webhook Deploy Script Failed"
echo 'Completed Github Hooks Deployment'
echo '###############################################################################'
echo
