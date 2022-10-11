using System;

namespace Suhock.Osc;

public interface IOscArgumentFactory
{
    /// <summary>
    /// Creates an <c>OscMessage</c> object from a natively-typed value.
    /// </summary>
    /// <param name="value">The natively-typed value</param>
    /// <returns>An OSC message argument or <c>null</c> if the type of the <c>value</c> is not understood</returns>
    public IOscArgument FromValue(object value);
}