using System;

namespace Suhock.X32.Types.Floats;

public abstract class AbstractSteppedDecimal
{
    protected const float MinEncodedValue = 0.0f;

    protected const float MaxEncodedValue = 1.0f;

    private readonly float _encodedValue;

    protected AbstractSteppedDecimal()
    {
    }

    protected AbstractSteppedDecimal(float unitValue)
    {
        UnitValue = unitValue;
    }

    protected AbstractSteppedDecimal(int stepValue)
    {
        StepValue = stepValue;
    }

    public float EncodedValue
    {
        get => _encodedValue;

        protected init =>
            _encodedValue = Math.Clamp((float)Math.Round(value * MaxStepValue) / MaxStepValue, 0.0f, 1.0f);
    }

    public abstract float UnitValue { get; protected init; }

    public int StepValue
    {
        get => (int)Math.Round(EncodedValue * MaxStepValue);

        init => EncodedValue = (float)value / MaxStepValue;
    }

    public abstract float MinUnitValue { get; }

    public abstract float MaxUnitValue { get; }

    public abstract int Steps { get; }

    public abstract string Unit { get; }

    public int MaxStepValue => Steps - 1;

    public virtual string ToNodeString()
    {
        return ToFixedDecimalNodeString(3);
    }

    protected string ToFixedDecimalNodeString(int decimalPlaces)
    {
        return UnitValue.ToString("F" + decimalPlaces.ToString());
    }

    protected string ToCompactNodeString(int maxDecimalPlaces)
    {
        return ToFixedDecimalNodeString(maxDecimalPlaces).TrimEnd(new char[] { '0', '.' });
    }

    public static bool operator ==(AbstractSteppedDecimal left, AbstractSteppedDecimal right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AbstractSteppedDecimal left, AbstractSteppedDecimal right)
    {
        return !left.Equals(right);
    }

    public override bool Equals(object? obj)
    {
        return GetType() == obj?.GetType() && Math.Abs(((AbstractSteppedDecimal)obj).EncodedValue - EncodedValue) < 0.00001;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EncodedValue);
    }

    public override string ToString()
    {
        return UnitValue + Unit;
    }
}