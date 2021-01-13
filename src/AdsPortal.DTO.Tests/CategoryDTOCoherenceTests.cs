namespace AdsPortal.DTO.Tests
{
    using Xunit;
    using Xunit.Abstractions;

    public class CategoryDTOCoherenceTests : BaseTest
    {
        public CategoryDTOCoherenceTests(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void Create()
        {
            EnsureCoherent<WebApi.Application.Operations.CategoryOperations.Commands.CreateCategory.CreateCategoryCommand,
                           CLI.Application.Commands.Category.CreateCategoryCommand,
                           WebPortal.Models.Category.CreateCategory>();
        }

        [Fact]
        public void Update()
        {
            EnsureCoherent<WebApi.Application.Operations.CategoryOperations.Commands.UpdateCategory.UpdateCategoryCommand,
                           CLI.Application.Commands.Category.UpdateCategoryCommand,
                           WebPortal.Models.Category.UpdateCategory>();
        }

        [Fact]
        public void GetDetails()
        {
            EnsureCoherent<WebApi.Application.Operations.CategoryOperations.Queries.GetCategoryDetails.GetCategoryDetailsResponse,
                           WebPortal.Models.Category.CategoryDetails>();
        }

        [Fact]
        public void GetAll()
        {
            EnsureCoherent<WebApi.Application.Operations.CategoryOperations.Queries.GetCategoriesList.GetCategoriesListQuery,
                           CLI.Application.Commands.Category.GetAllCategoriesCommand,
                           WebPortal.Models.Category.GetCategoriesList>();

            EnsureCoherent<MediatR.GenericOperations.Models.ListResult<WebApi.Application.Operations.CategoryOperations.Queries.GetCategoriesList.GetCategoriesListResponse>,
                           WebPortal.Models.Base.ListResult<WebPortal.Models.Category.CategoriesListItem>>();
        }

        [Fact]
        public void GetPaged()
        {
            EnsureCoherent<WebApi.Application.Operations.CategoryOperations.Queries.GetCategoriesList.GetPagedCategoriesListQuery,
                           CLI.Application.Commands.Category.GetPagedCategoriesCommand,
                           WebPortal.Models.Category.GetPagedCategoriesList>();

            EnsureCoherent<MediatR.GenericOperations.Models.PagedListResult<WebApi.Application.Operations.CategoryOperations.Queries.GetCategoriesList.GetCategoriesListResponse>,
                           WebPortal.Models.Base.PagedListResult<WebPortal.Models.Category.CategoriesListItem>>();
        }
    }
}
