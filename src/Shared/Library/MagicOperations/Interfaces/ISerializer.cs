﻿namespace MagicOperations.Interfaces
{
    using System;

    public interface ISerializer
    {
        string Serialize(object obj);

        object? Deserialize(Type type, string json);

        T? Deserialize<T>(string json);
    }
}