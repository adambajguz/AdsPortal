namespace AutoMapper.Extensions
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    public static class IMapperConfigurationExpressionExtensions
    {
        public static IMapperConfigurationExpression AddCustomMappings(this IMapperConfigurationExpression mapperConfiguration, Assembly assembly, ILoggerFactory loggerFactory)
        {
            mapperConfiguration.AddProfile(new CustomAutoMapperProfile(assembly, loggerFactory.CreateLogger<CustomAutoMapperProfile>()));

            return mapperConfiguration;
        }
    }
}
