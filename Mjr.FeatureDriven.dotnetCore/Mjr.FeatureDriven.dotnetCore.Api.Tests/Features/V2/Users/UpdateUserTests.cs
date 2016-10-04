using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using Mjr.FeatureDriven.dotnetCore.Api.Features.V2.Users;
using Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Features.V2.Users
{
    public class UpdateUserTests
    {
        public void ConcurrentUserNameChanged_ShouldThrow(TestContextFixture testContextFixture,
            User user, UpdateUser.Command updateUser)
        {
            //arrange
            testContextFixture.Save(user);
            user.FirstName = "Piet";
            testContextFixture.DoClean(db =>
             db.Database.ExecuteSqlCommand("UPDATE dbo.Users set firstname='Karel'")
            );

            //act & assert
            Should.Throw<DbUpdateConcurrencyException>(() =>
            testContextFixture.SendAsync(updateUser)
            );
        }
    }
}
