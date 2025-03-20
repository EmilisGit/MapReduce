Hadoop komanda:

Paleidžiant kodą:
mapred streaming  -input duom_full.txt  -output output  -mapper "dotnet run --project Map/Map.csproj"  -reducer "dotnet run --project Reduce/Reduce.csproj"

Paleidžiant sukompiliuotus .exe failus: 
mapred streaming  -input duom_full.txt  -output output  -mapper "Map/bin/Release/net9.0/Map"  -reducer "Reduce/bin/Release/net9.0/Reduce"