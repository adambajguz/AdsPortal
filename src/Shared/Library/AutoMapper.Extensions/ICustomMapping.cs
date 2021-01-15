namespace AutoMapper.Extensions
{
    using AutoMapper;

    public interface ICustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}
