namespace MediatR.GenericOperations.Mapping
{
    using AutoMapper;

    public interface ICustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}
