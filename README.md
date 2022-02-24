# RoboRecords
A website made using ASP.NET Core to track and record SRB2 speedrun records

# How to get it running locally

## Prerequisites:

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [A MySql Server instance running](https://dev.mysql.com/doc/refman/8.0/en/installing.html)
- [Have a connection string for the MySql Server instance](https://dev.mysql.com/doc/connector-net/en/connector-net-connections-string.html)

## Building:

- Getting the source code:  
`git clone https://github.com/Leminn/RoboRecords.git`  
`cd RoboRecords`  
- Building the website:  
Using the .NET CLI:  
`dotnet build`  
`dotnet run --project RoboRecords`  
Or, open the solution in your IDE of choice and build / run it from there

Now you should get errors saying that you're missing some environment variables, you can let yourself guide by the error to know the next environment variable you need, you have multiple options for where to put them:

- .NET User secrets (Development profile only):  
`dotnet user-secrets --project RoboRecords set "variable" "value"`  
Note: Some characters may need to be escaped in your terminal, you should check the file containing the user-secrets afterwards to see if the values are correctly in.
- Environment variables  
- Editing the launchSettings.json (recommended):  
You can modify the launchSettings.json located [here](https://github.com/Leminn/RoboRecords/blob/master/RoboRecords/Properties/launchSettings.json) and add the variables in the `environmentVariables` section of the profile you're using i.e.:
    ```
    "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        
        "RoboRecords_MySqlDbConnectionString": "(your connection string)",
    }
    ```