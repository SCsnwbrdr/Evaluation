#!/bin/bash

#Preflight Checks
if [[ -z "$GITHUB_PRIVATEKEY" && -z "$GITHUB_PERSONAL_ACCESS_TOKEN" ]]; then
    echo "You have not set GITHUB_PRIVATEKEY OR GITHUB_PERSONAL_ACCESS_TOKEN. Set at least one of these environment variables to continue."
    exit 1
fi

if [[ ! -z "$GITHUB_PRIVATEKEY" && ! -z "$GITHUB_PERSONAL_ACCESS_TOKEN" ]]; then
    echo "You have set both GITHUB_PRIVATEKEY AND GITHUB_PERSONAL_ACCESS_TOKEN. Set only one of these environment variables to continue."
    exit 1
fi

if [ -z $UNIQUEID ]; then
    defaultHash=$(echo -n $HOSTNAME | md5sum | awk '{print $1}')
    defaultHash=${defaultHash:0:5}
    echo "The UNIQUEID environment variable is not set. We use this variable to uniquely identify the deployment and give you your own envrionment to test on."
    echo "Would you like to set it to the default value '$defaultHash' (a substring based on the hash of hostname)? [y/N]"
    read default
    if [ "$default" == "y" ]; then
        export UNIQUEID=${defaultHash}
    else
        echo "Please enter your unique identifier (up to 5 characters) for this deployment."
        read tempID
        if [ ${#tempID} -gt 5 ]; then
            echo "The identifier you entered was too long. Please enter a 5 or less character identifier."
            exit 1;
        fi
        export UNIQUEID=${tempID}
    fi
    echo "You have set the UNIQUEID environment variable to '$UNIQUEID'"
    echo
    echo "If you would like to no longer be prompted for this value, you can set the UNIQUEID you can run 'export UNIQUEID=\"$UNIQUEID\"'"
    echo "If you need to reset this variable, you can do so by running the following command: 'export UNIQUEID=\"\"'"
fi

echo
echo

if [ -z $GIT_ORGANIZATION ]; then
    echo "The GIT_ORGANIZATION environment variable is not set. You will need to set this variable to be able to deploy your webhooks for testing."
    echo "What is the name of your GitHub Organization that you would like to deploy to? (Double check this name before continuing as the webhook cannot deploy until the end!)"
    read orgtarget
    if [ -z orgtarget ]; then
        echo "The organization you entered was blank"
        exit 1;
    fi
    export GIT_ORGANIZATION=${orgtarget}
    echo "You have set the GIT_ORGANIZATION environment variable to '$GIT_ORGANIZATION'"
    echo 
    echo "If you would like to no longer be prompted for this value, you can set the GIT_ORGANIZATION you can run 'export GIT_ORGANIZATION=\"$GIT_ORGANIZATION\"'"
    echo "If you need to reset this variable, you can do so by running the following command: 'export GIT_ORGANIZATION=\"\"'"
fi