using Finbourne.Insights.Sdk.Api;
using Finbourne.Insights.Sdk.Client;
using Finbourne.Insights.Sdk.Model;
using NUnit.Framework;
using System;

namespace Finbourne.Insights.Sdk.Extensions.IntegrationTests
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

        [Test]
        public void NetworkConnectivityErrors_ThrowsException()
        {
            var apiConfig = ApiConfigurationBuilder.Build("secrets.json");
            // nothing should be listening on this, so we should get a "No connection could be made" error...
            apiConfig.InsightsUrl = "https://localhost:56789/insights"; 

            var factory = new ApiFactory(apiConfig);
            var api = factory.Api<IVendorLogsApi>();

            // Can't be more specific as we get different exceptions locally vs in the build pipeline
            var expectedMsg = "Internal SDK error occurred when calling GetVendorResponse";

            Assert.That(
                () => api.GetVendorResponseWithHttpInfo("$@!-"),
                Throws.InstanceOf<ApiException>()
                    .With.Message.Contains(expectedMsg));

            // Note: these non-"WithHttpInfo" methods just unwrap the `Data` property from the call above.
            // But these were the problematic ones, as they would previously just return a null value in this scenario.
            Assert.That(
                () => api.GetVendorResponse("$@!-"),
                Throws.InstanceOf<ApiException>()
                    .With.Message.Contains(expectedMsg));
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
