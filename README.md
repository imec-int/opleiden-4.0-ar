# Opleiden 4.0 - AR

## About

Note that the automated setup for Roslyn analysers only supports Visual Studio Code for the moment.

## Setup

If you are running on Windows you require either Git Bash or WSL to be installed.

After cloning the project for the first time you should run the following command inside bash: ./Setup.sh.

This will perform the following steps:
- Setup Git LFS
- Add additional git hooks
	- Prevent large files to be pushed without LFS
	- Prevent .meta files to be committed that do not have a corresponding file or directory
	- Remove empty directories in the Assets folder so Unity does not create unnecessary .meta files
- Create an omnisharp configuration file in you user directory configured for Roslynator and vscode

In order to have proper Unity C# support you require at least the following vscode extensions:
- [C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
- [Debugger for Unity](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug)
- [Roslynator](https://marketplace.visualstudio.com/items?itemName=josefpihrt-vscode.roslynator)
