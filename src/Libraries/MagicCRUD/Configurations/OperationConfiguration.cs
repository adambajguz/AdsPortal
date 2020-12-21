namespace MagicCRUD.Configurations
{
    using System;
    using System.Net.Http;

    public class OperationConfiguration
    {
        public OperationGroupConfiguration Group { get; internal set; } = default!;

        public string Template { get; }
        public HttpMethod Method { get; }

        public Type EntityType { get; }
        public Type DtoType { get; }

        public DataEmbeddings Embedding { get; }

        internal OperationConfiguration(string template, HttpMethod method, Type entityType, Type dtoType, DataEmbeddings embedding)
        {
            Template = template;
            Method = method;
            EntityType = entityType;
            DtoType = dtoType;
            Embedding = embedding;
        }
    }
}