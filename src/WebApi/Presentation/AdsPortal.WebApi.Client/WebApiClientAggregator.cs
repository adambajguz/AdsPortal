namespace AdsPortal.WebApi.Client
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class WebApiClientAggregator : IEnumerable<IBaseWebApiClient>
    {
        public IAdvertisementClient AdvertisementClient { get; }
        public ICategoryClient CategoryClient { get; }
        public IEntityAuditLogClient EntityAuditLogClient { get; }
        public IMediaItemClient MediaItemClient { get; }
        public IUserClient UserClient { get; }

        public WebApiClientAggregator(IAdvertisementClient advertisementClient,
                                      ICategoryClient categoryClient,
                                      IEntityAuditLogClient entityAuditLogClient,
                                      IMediaItemClient mediaItemClient,
                                      IUserClient userClient)
        {
            AdvertisementClient = advertisementClient;
            CategoryClient = categoryClient;
            EntityAuditLogClient = entityAuditLogClient;
            MediaItemClient = mediaItemClient;
            UserClient = userClient;
        }

        public void SetToken(string? token)
        {
            foreach(IBaseWebApiClient client in this)
            {
                client.JwtToken = token;
            }
        }

        public void ClearToken()
        {
            foreach(IBaseWebApiClient client in this)
            {
                client.JwtToken = null;
            }
        }

        public IEnumerator<IBaseWebApiClient> GetEnumerator()
        {
            yield return AdvertisementClient;
            yield return CategoryClient;
            yield return EntityAuditLogClient;
            yield return MediaItemClient;
            yield return UserClient;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
