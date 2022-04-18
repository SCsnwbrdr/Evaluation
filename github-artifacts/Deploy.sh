#!/bin/bash
echo 'Starting GitHub Artifact Deployment'
ScriptRootPath=$(dirname "$0")
source $(dirname "$0")/../SharedFunctions.sh

##Verify we have all the variables we need
[ ! -z "$AZ_RESOURCEGROUPNAME" ] || die "You must set the variable RESOURCEGROUPNAME to continue."
[ ! -z "$FUNCTIONAPP_NAME" ] || die "You must set the variable FUNCTIONAPP_NAME to continue."
[ ! -z "$GIT_ORGANIZATION" ] || die "You must set the variable DEPLOYMENTNAME to continue."

#Set initial variables
OrgPath="orgs/$GIT_ORGANIZATION/hooks"
looptracking=0

keyName="allgithubhooks"
key=$(az functionapp keys set --key-name $keyName --key-type functionKeys --resource-group $AZ_RESOURCEGROUPNAME --name $FUNCTIONAPP_NAME --query value -o tsv)
    

#Function that runs against every mapping value
ProvisiongGitHubArtifact(){
    iteration=$1
    TEMPLATE=$(cat $ScriptRootPath/$2.json)
    targetString=$p
    IFS=' '; arrIN=($targetString); unset IFS;
    functionPath="${arrIN[0]}"
    events="${arrIN[1]}"

    #gets the hostname from the functionapp
    hostname=$(az functionapp show --resource-group $AZ_RESOURCEGROUPNAME --name $FUNCTIONAPP_NAME --query defaultHostName -o tsv)
    url="https://$hostname$functionPath"
    echo "Checking Webhooks to Ensure None exist for $url"
    
    for i in "${arrResults[@]}" ; do
        ## This checks a URL, if it matches, then we need to clear it out to avoid calculating deltas and making this more complicated.
        if [[ $i == *"$url"* ]]; then
            IFS=';'; singleRecord=($i); unset IFS;
            echo "Found Webhook for $url"
            echo "Deleting Webhook"
            
            ## This may be a bit "dirty" but I'm pressed for time with everything I have left to complete, I can come back later.
            id=$(echo "$singleRecord[0]" | awk -F"E" 'BEGIN{OFMT="%10.10f"} {print $1 * (10 ^ $2)}')
            targetPath="$OrgPath/$id"
            echo "Deleting $targetPath"
            gh api -X DELETE $targetPath
        fi
    done

    url="$url?code=$key&clientId=$keyName"

    tempfile=${TEMPLATE/@@URL@@/"$url"}
    tempfile=${tempfile/@@EVENTS@@/"$events"}

    filePath=$ScriptRootPath/temp.$iteration.json

    echo $tempfile > $filePath

    gh api $OrgPath --input $filePath --silent

    rm $filePath

    echo "Webhook for $url created with events $events"
    echo
}

##Pre-fetching the existing hooks on the org. This is to prevent 422 responses from the API. 
results=$(gh api -H "Accept: application/vnd.github.v3+json" $OrgPath --paginate --template '{{range .}}{{.id}};{{.config.url}};{{.events}}{{"@"}}{{end}}')
IFS='@'; arrResults=($results); unset IFS;

#Takes the mapping file and then provisions the webhooks based on the mappings
while IFS="" read -r p || [ -n "$p" ]
do
  ProvisiongGitHubArtifact $looptracking "WebhookPayloadTemplate" $p
  looptracking=$((looptracking+1))
done < $ScriptRootPath/Mappings.tsv

