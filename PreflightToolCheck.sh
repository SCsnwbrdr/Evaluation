
## Check to see if Azure CLI is Installed
output=$(az version) || die "Azure Cli was not detected. Please install Azure CLI and try again. https://docs.microsoft.com/en-us/cli/azure/install-azure-cli"

## Check to see if azure dotnet sdk is installed
output=$(dotnet --info) || die "Dotnet SDK was not detected. Please install  Dotnet SDK and try again. https://dotnet.microsoft.com/download/dotnet/6.0"

## Check to see if azure-functions-core-tools is installed
output=$(func --version) || die "Azure Functions Core Tools was not detected. Please install Azure Functions Core Tools and try again. https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=v4%2Cwindows%2Ccsharp%2Cportal%2Cbash#install-the-azure-functions-core-tools"

## Check to see if Github CLI Is installed
output=$(gh version) || die "Github CLI was not detected. Please install Github CLI and try again. https://github.com/cli/cli#installation"