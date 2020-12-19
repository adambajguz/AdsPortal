namespace AdsPortal.Application.Mapping
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;
    using AutoMapper;

    public static class MappingExpressionExtensions
    {
        public static void ForAllMembersTryPatchProperty<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression,
                                                                                Action<IMemberConfigurationExpression<TSource, TDestination, object>>? memberOptions = null)
        {
            mappingExpression.ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcValue) => srcValue is not IPatchProperty patchProperty || patchProperty.Include);
                memberOptions?.Invoke(opt);
            });
        }
    }
}
