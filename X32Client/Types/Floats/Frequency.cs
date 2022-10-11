namespace Suhock.X32.Types.Floats;

public abstract class Frequency : AbstractLogDecimal
{
    public override string Unit => "Hz";

    public override string ToNodeString()
    {
        var unitValue = UnitValue;

        if (unitValue < 1000f)
        {
            return ToFixedDecimalNodeString(1);
        }

        return ((int)unitValue / 1000) + "k" + ((int)unitValue % 1000 / 10);
    }


    protected Frequency()
    {
    }

    public Frequency(float unitValue) : base(unitValue)
    {
    }

    public Frequency(int stepValue) : base(stepValue)
    {
    }
}