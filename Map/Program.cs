using Map;


Mapper mapper = new(["sustojimo savaites diena", "geografine zona"],
                    ["siuntu skaicius", "Sustojimo klientu skaicius"]);

foreach (var result in mapper.Map())
    Console.WriteLine(result);