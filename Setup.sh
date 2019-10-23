#!/bin/bash

# Setup GIT

## Set custom git hooks folder
git config core.hooksPath .githooks

## Create git lfs hooks
git lfs install

## Move conflicting git lfs hooks to subdirectory
mv ./.githooks/post-checkout ./.githooks/post-checkout.d/git-lfs
mv ./.githooks/post-merge ./.githooks/post-merge.d/git-lfs

cp ./.githooks/multiple-git-hooks ./.githooks/post-checkout
cp ./.githooks/multiple-git-hooks ./.githooks/post-merge


# Setup omnisharp

if [[ $(uname -r) =~ Microsoft$ ]]; then
  HOME=$(wslpath $(cmd.exe /C "echo %USERPROFILE%") | tr -d '\r')
  
  CONFIGHOME=$(cmd.exe /C "echo %USERPROFILE%") 
  CONFIGHOME=$(echo $CONFIGHOME | tr '\\' '/' | tr -d '\r') 
else
  HOME=~
  CONFIGHOME=~
fi

## Create omnisharp folder
mkdir -p $HOME/.omnisharp

## Create omnisharp configuration
cat > $HOME/.omnisharp/omnisharp.json <<- EOM
{
	"RoslynExtensionsOptions": {
		"enableAnalyzersSupport": true,
		"LocationPaths": [
			"$CONFIGHOME/.vscode/extensions/josefpihrt-vscode.roslynator-2.2.0/roslyn/common",
			"$CONFIGHOME/.vscode/extensions/josefpihrt-vscode.roslynator-2.2.0/roslyn/analyzers",
			"$CONFIGHOME/.vscode/extensions/josefpihrt-vscode.roslynator-2.2.0/roslyn/refactorings",
			"$CONFIGHOME/.vscode/extensions/josefpihrt-vscode.roslynator-2.2.0/roslyn/fixes"
		]
	}
}
EOM
