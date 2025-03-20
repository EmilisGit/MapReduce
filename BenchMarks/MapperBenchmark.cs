using BenchmarkDotNet.Attributes;
using Map;

namespace BenchMarks;

[MemoryDiagnoser]
public class MapperBenchmark
{
    private readonly Mapper _mapper;

    private readonly string _input =
        @"{{tipas=B2C}{Laikas po sustojimo=45.52}{Firma=TRA}{svorio grupe=<300}{sustojimo savaites diena=2}{pasto kodas=03114}{Aptarnavimo grupe=B2C+}{Ar reikalingos paletes=1}{sustojimo data=2018-01-02}{Sustojimo klientu skaicius=1}{geografine zona=Z1}{Marsruto tipas=van-city-unl}{kaina procentas=0.1332995951417}{sandelio id=580}{Laukia=0}{marsrutas=102}{sustojimo klientu sarasas=LT3001894}{kaina vienetais=2.60600708502024}{laikas=11:23:36}{Sustojimo numeris=4}{svoris=76}{Uzkrovimo tipas=KDR}{siuntu skaicius=1}{Masinos tipas=van}{Laikas iki sustojimo=20.33}}
        {{tipas=B2C}{Laikas po sustojimo=45.52}{Firma=TRA}{svorio grupe=<300}{sustojimo savaites diena=2}{pasto kodas=03114}{Aptarnavimo grupe=B2C+}{Ar reikalingos paletes=1}{sustojimo data=2018-01-02}{Sustojimo klientu skaicius=1}{geografine zona=Z1}{Marsruto tipas=van-city-unl}{kaina procentas=0.1332995951417}{sandelio id=580}{Laukia=0}{marsrutas=102}{sustojimo klientu sarasas=LT3001894}{kaina vienetais=2.60600708502024}{laikas=11:23:36}{Sustojimo numeris=4}{svoris=76}{Uzkrovimo tipas=KDR}{siuntu skaicius=1}{Masinos tipas=van}{Laikas iki sustojimo=20.33}}
        {{tipas=B2C}{Laikas po sustojimo=45.52}{Firma=TRA}{svorio grupe=<300}{sustojimo savaites diena=2}{pasto kodas=03114}{Aptarnavimo grupe=B2C+}{Ar reikalingos paletes=1}{sustojimo data=2018-01-02}{Sustojimo klientu skaicius=1}{geografine zona=Z1}{Marsruto tipas=van-city-unl}{kaina procentas=0.1332995951417}{sandelio id=580}{Laukia=0}{marsrutas=102}{sustojimo klientu sarasas=LT3001894}{kaina vienetais=2.60600708502024}{laikas=11:23:36}{Sustojimo numeris=4}{svoris=76}{Uzkrovimo tipas=KDR}{siuntu skaicius=1}{Masinos tipas=van}{Laikas iki sustojimo=20.33}}";
    public MapperBenchmark()
    {
        _mapper = new Mapper(["marsrutas"], ["siuntu skaicius", "svoris", "geografine zona"]);
    }

    [Benchmark]
    public void BenchmarkSequential()
    {
        Console.SetIn(new StringReader(_input));
        _mapper.Map();
    }
}