using System;
using System.Net.Http;
using EFCoreRelationshipsPractice;
using Xunit;

namespace EFCoreRelationshipsPracticeTest
{
    public class TestBase : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        public TestBase(CustomWebApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }

        protected CustomWebApplicationFactory<Startup> Factory { get; }

        public void Dispose()
        {
            // var scope = Factory.Services.CreateScope();
            // var scopedServices = scope.ServiceProvider;
            // var context = scopedServices.GetRequiredService<CompanyDbContext>();
            //
            // context.Companies.RemoveRange(context.Companies);
            //
            // context.SaveChanges();
        }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }
    }
}