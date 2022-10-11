﻿namespace Suhock.X32.Types.Floats;

public sealed class EqGain : AbstractLinearDecimal
{
    private static EqGain? _minValue;

    private static EqGain? _maxValue;

    public static EqGain MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static EqGain MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private EqGain()
    {
    }

    public EqGain(float unitValue) : base(unitValue)
    {
    }

    public EqGain(int stepValue) : base(stepValue)
    {
    }

    public static EqGain FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -15f;

    public override float MaxUnitValue => 15f;

    public override float StepInterval => 0.25f;

    public override string Unit => "dB";
}