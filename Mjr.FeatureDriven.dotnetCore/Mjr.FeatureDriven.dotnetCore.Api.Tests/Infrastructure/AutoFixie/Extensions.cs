using System;
using System.Linq;
using System.Threading.Tasks;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using MediatR;
using Shouldly;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.AutoFixie
{
    public static class Extensions
    {
        public static async Task CatchException<T>(this IAsyncRequest<T> command, TestContextFixture testContextFixture, Type exception)
        {
            try
            {
                await testContextFixture.SendAsync(command);
            }

            catch (Exception p)
            {
                p.ShouldBeOfType(exception);
            }
        }

       
    }
}
