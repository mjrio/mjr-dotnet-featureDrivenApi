using System.Linq;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie;
using Shouldly;
using Microsoft.EntityFrameworkCore;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.Data
{
    public class CanAutoFixtureSaveAllEntitiesTests
    {
        public void Should_SaveCustomer(TestContextFixture testContextFixture, User user)
        {
            testContextFixture.Save(user);
            User saved = null;
            testContextFixture.DoClean((context) => saved = context.Set<User>()
                        .Include(s => s.Posts)
                        .SingleOrDefault(s => s.LastName == user.LastName));
            saved.ShouldNotBeNull();
            saved.Posts.ShouldNotBeNull();
        }
        public void Should_Save(TestContextFixture testContextFixture, Post post)
        {
            testContextFixture.Save(post);
            Post saved = null;
            testContextFixture.DoClean((context) => saved = context.Set<Post>()
                        .Include(s=>s.From)
                        .SingleOrDefault(s => s.Content == post.Content));
            saved.ShouldNotBeNull();
            saved.From.ShouldNotBeNull();
        }
    }
}
