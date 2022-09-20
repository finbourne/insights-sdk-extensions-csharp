using NUnit.Framework;

namespace Finbourne.Insights.Sdk.Extensions.IntegrationTests
{
    [TestFixture]
    public class TokenProviderConfigurationTest
    {
        private static readonly Lazy<ApiConfiguration> ApiConfig =
            new Lazy<ApiConfiguration>(() => ApiConfigurationBuilder.Build("secrets.json"));

        //Test requires [assembly: InternalsVisibleTo("namespace Finbourne.Insights.Sdk.Extensions.IntegrationTests")] in ClientCredentialsFlowTokenProvider
        [Test]
        public void Construct_AccessToken_NonNull()
        {
            ITokenProvider tokenProvider;
            if (ApiConfig.Value.MissingSecretVariables)
            {
                tokenProvider = new PersonalAccessTokenProvider(ApiConfig.Value.PersonalAccessToken);
            }
            else 
            {
                tokenProvider = new ClientCredentialsFlowTokenProvider(ApiConfigurationBuilder.Build("secrets.json")); 
            }

            var config = new TokenProviderConfiguration(tokenProvider);
            Assert.IsNotNull(config.AccessToken);
        }
    }
}
