string? line, currentKey = null, key = null;
float currentValue = 0;
float value = 0;

while ((line = Console.ReadLine()) is not null)
{
    line = line.Trim();
    string[] keyValue = line.Split("\t");
    if (keyValue.Length < 2)
        continue;

    key = keyValue[0];

    if (float.TryParse(keyValue[1], out float number)){
        value = number;
    }
    else{
        continue;
    }
        
    if (currentKey == key){
        currentValue += value;
    }
    else{
        if (currentKey is not null)
            Console.WriteLine($"{currentKey}\t{currentValue}");
        currentKey = key;
        currentValue = value;
    }
}

if (currentKey == key)
{
    Console.WriteLine($"{currentKey}\t{currentValue}");
}