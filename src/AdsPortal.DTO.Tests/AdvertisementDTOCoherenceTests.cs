namespace AdsPortal.DTO.Tests
{
    using Xunit;
    using Xunit.Abstractions;

    public class AdvertisementDTOCoherenceTests : BaseTest
    {
        public AdvertisementDTOCoherenceTests(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void Create()
        {
            EnsureCoherent<WebApi.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement.CreateAdvertisementCommand,
                           CLI.Application.Commands.Advertisement.CreateAdvertisementCommand,
                           WebPortal.Models.Advertisement.CreateAdvertisement>();
        }

        [Fact]
        public void Update()
        {
            EnsureCoherent<WebApi.Application.Operations.AdvertisementOperations.Commands.UpdateAdvertisement.UpdateAdvertisementCommand,
                           CLI.Application.Commands.Advertisement.UpdateAdvertisementCommand,
                           WebPortal.Models.Advertisement.UpdateAdvertisement>();
        }


        [Fact]
        public void GetDetails()
        {
            EnsureCoherent<WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementDetails.GetAdvertisementDetailsResponse,
                           WebPortal.Models.Advertisement.AdvertisementDetails>();
        }

        [Fact]
        public void GetAll()
        {
            EnsureCoherent<WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList.GetAdvertisementsListQuery,
                           CLI.Application.Commands.Advertisement.GetAllAdvertisementsCommand,
                           WebPortal.Models.Advertisement.GetAdvertisementsList>();

            EnsureCoherent<MediatR.GenericOperations.Models.ListResult<WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList.GetAdvertisementsListResponse>,
                           WebPortal.Models.Base.ListResult<WebPortal.Models.Advertisement.AdvertisementsListItem>>();
        }

        [Fact]
        public void GetPaged()
        {
            EnsureCoherent<WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList.GetPagedAdvertisementsListQuery,
                           CLI.Application.Commands.Advertisement.GetPagedAdvertisementsCommand,
                           WebPortal.Models.Advertisement.GetPagedAdvertisementsList>();

            EnsureCoherent<MediatR.GenericOperations.Models.PagedListResult<WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList.GetAdvertisementsListResponse>,
                           WebPortal.Models.Base.PagedListResult<WebPortal.Models.Advertisement.AdvertisementsListItem>>();
        }
    }
}
