using System;
using System.Collections.Generic;

namespace Suhock.X32.Types.Floats
{
    public abstract class DiscreteFloat
    {
        public const float MinEncodedValue = 0.0f;

        public const float MaxEncodedValue = 1.0f;

        private float _encodedValue;

        protected DiscreteFloat()
        { }

        protected DiscreteFloat(float unitValue)
        {
            UnitValue = unitValue;
        }

        protected DiscreteFloat(int stepValue)
        {
            StepValue = stepValue;
        }

        public float EncodedValue
        {
            get => _encodedValue;

            protected set => _encodedValue = Math.Clamp((float)Math.Round(value * MaxStepValue) / MaxStepValue, 0.0f, 1.0f);
        }

        public abstract float UnitValue
        {
            get;

            protected set;
        }

        public int StepValue
        {
            get => (int)Math.Round(EncodedValue * MaxStepValue);

            set => EncodedValue = (float)value / MaxStepValue;
        }

        public abstract float MinUnitValue { get; }

        public abstract float MaxUnitValue { get; }

        public abstract int Steps { get; }

        public abstract string Unit { get; }

        public int MaxStepValue { get => Steps - 1; }

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

        public static bool operator ==(DiscreteFloat left, DiscreteFloat right)
        {
            return left?.Equals(right) ?? false;
        }

        public static bool operator !=(DiscreteFloat left, DiscreteFloat right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return GetType() == obj?.GetType() && ((DiscreteFloat)obj).EncodedValue == EncodedValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EncodedValue);
        }

        public override string ToString()
        {
            return UnitValue.ToString() + Unit;
        }
    }
}
