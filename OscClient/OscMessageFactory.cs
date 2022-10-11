using System;
using System.Collections.Generic;

namespace Suhock.Osc;

public sealed class OscMessageFactory: IOscMessageFactory
{
    private readonly IOscArgumentFactory _argumentFactory;

    public OscMessageFactory() : this(new OscArgumentFactory())
    {
    }
    
    public OscMessageFactory(IOscArgumentFactory argumentFactory)
    {
        _argumentFactory = argumentFactory;
    }

    public OscMessage Create(string address, params object[] args)
    {
        var argList = new List<IOscArgument>(args.Length);

        foreach (var value in args)
        {
            var arg = _argumentFactory.FromValue(value);

            if (arg == null)
            {
                throw new Exception($"Invalid type: {value.GetType()}");
            }

            argList.Add(arg);
        }

        return new OscMessage(address, argList);
    }
}