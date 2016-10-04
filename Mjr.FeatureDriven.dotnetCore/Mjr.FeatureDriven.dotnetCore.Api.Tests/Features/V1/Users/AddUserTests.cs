using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using Mjr.FeatureDriven.dotnetCore.Api.Features.V1.Users;
using Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie;
using System.Linq;
using Shouldly;
using Microsoft.EntityFrameworkCore;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Features.V1.Users
{
    public class AddUserTests
    {
        public void ShouldAddNewUser(TestContextFixture testContextFixture,
            AddUser.Command addUser)
        {
            //act
            testContextFixture.Send(addUser);

            //assert
            User saved = null;
            testContextFixture.DoClean(
                context => saved = context.Set<User>()
                .SingleOrDefault(s => s.Email == addUser.Email)
                );
            saved.ShouldNotBeNull();
        }

        public void FirstNameTooLong_ShouldThrow(TestContextFixture testContextFixture, 
            AddUser.Command addUser)
        {
            //arrange
            addUser.FirstName = "Hottentottentententtentoonstelling";

            //act
            Should.Throw<DbUpdateException>(() => testContextFixture.Send(addUser));
        }

       
    }
}
