This project was created to help the Under-Night In-Birth 2 Beginner Lobbies (UNI2BL) discord server report on weekly brawl matches via the Start.GG graphql API

To run this project locally you will need:
- VSCode (Recommended) (Or another dotnet IDE)
- Azure Function Core Tools (https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp#install-the-azure-functions-core-tools)
- Start.GG Authorization Key (https://developer.start.gg/docs/intro/)

When you open the project and have azure fucntions core tools installed, you should be able to run 

```func init```

on the root directory. That will create a ```local.settings.json``` file for you. 

Add a setting in the values object:

```
"AuthToken": "<your auth key from above>"
```
