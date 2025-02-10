﻿namespace Empresta.Infraestrutura.Settings;

public sealed class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
}