﻿using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct FaderFineLevel : ILevelFloat
{
    public static float MinUnitValue => float.NegativeInfinity;
    public static float MaxUnitValue => 10.0f;
    public static int Steps => 1024;
    public static string Unit => "dB";

    public static FaderFineLevel MinValue => new(IEncodedFloat.MinEncodedValue);
    public static FaderFineLevel MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private FaderFineLevel(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static FaderFineLevel FromEncodedValue(float encodedValue) => new(encodedValue);

    public static FaderFineLevel FromUnitValue(float unitValue) =>
        new(FloatConversions.LevelToEncoded(unitValue));

    public static FaderFineLevel FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}