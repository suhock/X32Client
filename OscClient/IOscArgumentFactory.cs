﻿using System;

namespace Suhock.Osc
{
    public interface IOscArgumentFactory
    {
        /// <summary>
        /// Decodes a binary-encoded OSC message argument into an <c>OscMessage</c> object.
        /// </summary>
        /// <param name="typeTag">The type tag for this message from the OSC message's type tag string</param>
        /// <param name="bytes">The span of bytes starting at the beginning of the argument to be read</param>
        /// <param name="length">The number of bytes read</param>
        /// <returns>The decoded OSC message argument or <c>null</c> if the <c>typeTag</c> is not understood</returns>
        public IOscArgument FromBytes(byte typeTag, ReadOnlySpan<byte> bytes, out int length);

        /// <summary>
        /// Creates an <c>OscMessage</c> object from a natively-typed value.
        /// </summary>
        /// <param name="value">The natively-typed value</param>
        /// <returns>An OSC message argument or <c>null</c> if the type of the <c>value</c> is not understood</returns>
        public IOscArgument FromValue(object value);
    }
}