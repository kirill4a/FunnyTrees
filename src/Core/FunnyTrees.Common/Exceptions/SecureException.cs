using System;

namespace FunnyTrees.Common.Exceptions;

public class SecureException : Exception
{
    public SecureException(string message) : base(message) { }
}
