using System;

namespace X32ClassGenerator;

class X32ClassGeneratorApp
{
    private static LinearTypeConfig[] LinearTypes = new LinearTypeConfig[]
    {
        new("AttackTime", 0.0f, 120.0f, 1.0f, "ms")
    };

    static void Main(string[] args)
    {
        foreach (var ltc in LinearTypes)
        {
            Console.WriteLine(ApplyTemplate(LinearTypeTemplate, ltc));
        }
    }

    private static string ApplyTemplate(string template, LinearTypeConfig config)
    {
        return template
            .Replace("$ClassName$", config.ClassName)
            .Replace("$Min$", config.Min + "f")
            .Replace("$Max$", config.Max + "f")
            .Replace("$Interval$", config.Interval + "f")
            .Replace("$Unit$", '"' + config.Unit + '"');
    }

    const string LinearTypeTemplate = @"
namespace Suhock.X32.Types.Floats;

class $ClassName$ : LinearFloat
{
    const float MinUnitValue = $Min$;
    
    const float MaxUnitValue = $Max$;
    
    const float Interval = $Interval$;

    const int MaxStepValue = $MaxStepValue$;

    const string Unit = $Unit$;

    private static $ClassName$ _minValue;

    public static $ClassName$ MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    private static $ClassName$ _maxValue;

    public static $ClassName$ MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    protected $ClassName$(float encodedValue) : base(encodedValue) { }

    protected $ClassName(int step) : base(step) { }

    public static $ClassName$ FromEncodedValue(float encodedValue) => new $ClassName$(encodedValue);

    public static $ClassName$ FromUnitValue(float unitValue) => new $ClassName$(FloatConversions.LinearToEncoded(unitValue));

    public static $ClassName$ FromStepValue(int step) => new $ClassName$(step);
    
    public float UnitValue => FloatConversions.EncodedToLinear(EncodedValue, MinUnitValue, MaxUnitValue);

    public float StepValue => (int)Math.Round(1 / StepInterval) + 1;
}
";
}

class LinearTypeConfig
{
    public LinearTypeConfig(string className, float min, float max, float interval, string unit)
    {
        ClassName = className;
        Min = min;
        Max = max;
        Interval = interval;
        Unit = unit;
    }

    public string ClassName { get; }

    public float Min { get; }

    public float Max { get; }

    public float Interval { get; }

    public string Unit = "";
}

class LogTypeConfig
{
    public LogTypeConfig(string className, float min, float max, int intervals, string unit)
    {
        ClassName = className;
        Min = min;
        Max = max;
        Intervals = intervals;
        Unit = unit;
    }

    public string ClassName { get; }

    public float Min { get; }

    public float Max { get; }

    public int Intervals { get; }

    public string Unit = "";
}

class LevelTypeConfig
{
    public LevelTypeConfig(string className)
    {
        ClassName = className;
    }

    public string ClassName { get; }
}