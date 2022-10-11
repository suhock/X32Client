using System;

namespace Suhock.Osc;

public interface IOscMessageFactory
{
    /// <summary>
    /// Creates an <see cref="OscMessage"/> object with the specified natively-typed values mapped to their
    /// respective <see cref="OscArgument{T}"/> types.
    /// </summary>
    /// <param name="address">An OSC address</param>
    /// <param name="values">A list of natively-typed argument values</param>
    /// <returns>An <see cref="OscMessage"/> object with the specified address and argument values</returns>
    public OscMessage Create(string address, params object[] values);
}