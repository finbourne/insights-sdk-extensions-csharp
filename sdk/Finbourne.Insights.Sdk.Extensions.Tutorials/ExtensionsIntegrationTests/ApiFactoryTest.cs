using Finbourne.Insights.Sdk.Api;
using Finbourne.Insights.Sdk.Client;
using Finbourne.Insights.Sdk.Model;
using NUnit.Framework;
using System;

namespace Finbourne.Insights.Sdk.Extensions.Tutorials
{
    public class ApiFactoryTest
    {
        private IApiFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = IntegrationTestApiFactoryBuilder.CreateApiFactory("secrets.json");
        }

        [Test]
        public void Create_AccessEvaluationsApi()
        {
            var api = _factory.Api<AccessEvaluationsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<AccessEvaluationsApi>());
        }

        [Test]
        public void Create_ApplicationMetadataApi()
        {
            var api = _factory.Api<ApplicationMetadataApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<ApplicationMetadataApi>());
        }

        [Test]
        public void Create_AuditingApi()
        {
            var api = _factory.Api<AuditingApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<AuditingApi>());
        }

        [Test]
        public void Create_RequestsApi()
        {
            var api = _factory.Api<RequestsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<RequestsApi>());
        }

        [Test]
        public void Create_VendorLogsApi()
        {
            var api = _factory.Api<VendorLogsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<VendorLogsApi>());
        }

        [Test]
        public void Api_From_Interface()
        {
            var api = _factory.Api<IVendorLogsApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<VendorLogsApi>());
        }

        [Test]
        public void Invalid_Requested_Api_Throws()
        {
            Assert.That(() => _factory.Api<InvalidApi>(), Throws.TypeOf<InvalidOperationException>());
        }

        class InvalidApi : IApiAccessor
        {
            public IReadableConfiguration Configuration { get; set; }
            public string GetBasePath()
            {
                throw new NotImplementedException();
            }

            public ExceptionFactory ExceptionFactory { get; set; }
        }
    }
}
