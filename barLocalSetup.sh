#!/bin/zsh

formatedFile="$(echo $(jq '.ConnectionStrings.ApplicationDbContext = "Server=localhost;Database=aspLearningDB;User ID=sa;Password=yourStrong(!)Password"' appsettings.Development.json) | python -m json.tool)"
echo $formatedFile > appsettings.Development.json

dotnet restore
dotnet ef database update
dotnet watch run