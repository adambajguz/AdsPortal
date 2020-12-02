﻿namespace AdsPortal.Application.Mapping
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;

    public static class MappingExpressionExtensions
    {
        public static void ForAllMembersTryPatchProperty<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression,
                                                                                Action<IMemberConfigurationExpression<TSource, TDestination, object>>? memberOptions = null)
        {
            mappingExpression.ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcValue) => !(srcValue is IPatchProperty patchProperty) || patchProperty.Include);
                memberOptions?.Invoke(opt);
            });
        }
    }
}
