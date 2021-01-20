namespace AdsPortal.WebApi.Persistence.EntityConfigurations
{
    using System;
    using AdsPortal.WebApi.Domain.Constants;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                Id = IdentityConstants.AdminUserId,
                CreatedOn = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = null,
                LastSavedOn = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                LastSavedBy = null,
                Email = "admin@adsportal.com",
                Password = "00K7UWlOrhhboShjjWT4jSZafPuFHaJDlZxqsxnxfJI5FN4+HPA7r9wlGHQKhIoM2/mfXXWzjVX0aaVo1Uo0JlTQ==co8P2fSN/QYvZSkYBAVanudgRvtXYG3rXNb04AYzR7gbcY4SbWrFFNea9qsaOa+PGDCpW7+fq8rB2uey83mdyQ==", //Pass123$
                Name = "Admin",
                Surname = "AdsPortal",
                Role = Roles.Admin | Roles.User,
                IsActive = true,
            });
        }
    }
}
