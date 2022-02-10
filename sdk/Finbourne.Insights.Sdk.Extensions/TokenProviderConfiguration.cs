using System;
using Finbourne.Insights.Sdk.Client;

namespace Finbourne.Insights.Sdk.Extensions
{
    /// <summary>
    /// Custom class, derived from the auto-generated Configuration, which allows the access token to be pulled from an ITokenProvider when required
    ///</summary>
    internal class TokenProviderConfiguration : Configuration
    {
        private readonly ITokenProvider _tokenProvider;

        /// <summary>
        /// Create a new Configuration using the supplied token provider
        ///</summary>
        public TokenProviderConfiguration(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Gets/sets the accesstoken
        ///</summary>
        public override string AccessToken
        {
            get => _tokenProvider.GetAuthenticationTokenAsync().Result;
            set => throw new InvalidOperationException("AccessToken is not assignable");
        }
    }
}
