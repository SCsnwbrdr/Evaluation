#!/bin/bash
ScriptRootPath=$(dirname "$0")
source $(dirname "$0")/../SharedFunctions.sh

[ ! -z "$AZ_RESOURCEGROUPNAME" ] || die "You must set the variable RESOURCEGROUPNAME to continue."
[ ! -z "$FUNCTIONAPP_NAME" ] || die "You must set the variable FUNCTIONAPP_NAME to continue."
[ ! -z "$GIT_ORGANIZATION" ] || die "You must set the variable ORGNAME to continue."

OrgPath="orgs/$GIT_ORGANIZATION/hooks"

##Pre-fetching the existing hooks on the org. This is to prevent 422 responses from the API. 
results=$(gh api -H "Accept: application/vnd.github.v3+json" $OrgPath --paginate --template '{{range .}}{{.id}};{{.config.url}};{{.events}}{{"@"}}{{end}}')
IFS='@'; arrResults=($results); unset IFS;

## Lets find all hooks with hostname registered to this functionapp
functionappHostname=$(az functionapp show --resource-group $AZ_RESOURCEGROUPNAME --name $FUNCTIONAPP_NAME --query defaultHostName -o tsv)

hostname=$(az functionapp show --resource-group $AZ_RESOURCEGROUPNAME --name $FUNCTIONAPP_NAME --query defaultHostName -o tsv)

#Loop through each webhook on the org and delete it if it matches the functionapp hostname
for i in "${arrResults[@]}" ; do
    ## This checks a URL, if it matches, then we need to clear it out to avoid calculating deltas and making this more complicated.
    if [[ $i == *"$hostname"* ]]; then
        IFS=';'; singleRecord=($i); unset IFS;
        echo "Found Webhook for $hostname"
        echo "Deleting Webhook"
        ## This may be a bit "dirty" but I'm pressed for time with everything I have left to complete, I can come back later.
        id=$(echo "$singleRecord[0]" | awk -F"E" 'BEGIN{OFMT="%10.10f"} {print $1 * (10 ^ $2)}')
        targetPath="$OrgPath/$id"
        echo "Deleting $targetPath"
        gh api -X DELETE $targetPath
    fi
done
