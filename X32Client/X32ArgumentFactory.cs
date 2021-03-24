using Suhock.Osc;

namespace Suhock.X32
{
    public class X32ArgumentFactory : OscArgumentFactory
    {
        public override IOscArgument FromValue(object value)
        {
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    return base.FromValue((int)value);
                }

                if (value.GetType() == typeof(bool))
                {
                    return base.FromValue((bool)value ? 1 : 0);
                }
            }

            return base.FromValue(value);
        }
    }
}
