using Finbourne.Insights.Sdk.Api;
using Finbourne.Insights.Sdk.Client;
using Finbourne.Insights.Sdk.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Finbourne.Insights.Sdk.Extensions.Tutorials
{
    [TestFixture]
    public class ApiExceptionExtensionsTest
    {
        private IApiFactory _factory;
        private const string RequestIdRegexPattern = "[a-zA-Z0-9]{13}:[0-9a-fA-F]{8}";

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = IntegrationTestApiFactoryBuilder.CreateApiFactory("secrets.json");
        }
        
        [Test]
        public void Generate_HttpStatusCode_BadRequest()
        {
            try
            {                
                _factory.Api<AccessEvaluationsApi>().GetAccessEvaluationLog("$@!-");
            }
            catch (ApiException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.ErrorCode);
            }
        }

        [Test]
        public void GetRequestId_MalformedInsightsUrl_ReturnsNull()
        {
            try
            {
                _factory.Api<AccessEvaluationsApi>().GetAccessEvaluationLog("$@!-");
            }
            catch (ApiException e)
            {
                var problemDetails = e.ProblemDetails();

                // Remove the InsightsURL which contains the requestId
                problemDetails.Instance = "";

                var apiExceptionMalformed = new ApiException(
                    errorCode: e.ErrorCode,
                    message: e.Message,
                    errorContent: JsonConvert.SerializeObject(problemDetails));

                var requestId = apiExceptionMalformed.GetRequestId();
                Assert.That(requestId, Is.Null);
            }
        }

        [Test]
        public void ProblemDetails_Converts_To_ProblemDetails_Detail()
        {
            try
            {
                _factory.Api<AccessEvaluationsApi>().GetAccessEvaluationLog("$@!-");
            }
            catch (ApiException e)
            {
                //    ApiException.ErrorContent contains a JSON serialized ErrorResponse
                LusidProblemDetails errorResponse = e.ProblemDetails();
                Assert.That(errorResponse.Detail, Does.Match("One or more elements of the request were invalid. Please check that all supplied identifiers are valid and of the correct format, and that all provided data is correctly structured."));
            }
        }

        [Test]
        public void ProblemDetails_Converts_To_ProblemDetails_Name()
        {
            try
            {
                _factory.Api<RequestsApi>().GetRequest("id_does_not_exist");
            }
            catch (ApiException e)
            {
                //    ApiException.ErrorContent contains a JSON serialized ErrorResponse
                LusidProblemDetails errorResponse = e.ProblemDetails();
                Assert.That(errorResponse.Name, Is.EqualTo("LogNotFound"));
            }
        }

        [Test]
        public void ApiException_Converts_To_ValidationProblemDetails_AllowedRegex()
        {
            try
            {
                _factory.Api<RequestsApi>().GetRequest("@£$@£%");
            }
            catch (ApiException e)
            {
                //Returns a 404 Not Found error
                Assert.That(e.ErrorCode, Is.EqualTo((int)HttpStatusCode.BadRequest), "Expect BadRequest error code");
                Assert.That(e.IsValidationProblem, Is.True, "Response should indicate that there was a validation error with the request. ");

                //    An ApiException.ErrorContent thrown because of a request validation contains a JSON serialized LusidValidationProblemDetails
                if (e.TryGetValidationProblemDetails(out var errorResponse))
                {
                    //Should identify that there was a validation error with the id
                    Assert.That(errorResponse.Errors, Contains.Key("id"));
                    Assert.That(errorResponse.Errors["id"].Single(), Is.EqualTo("Values for the field id must be comprised of either alphanumeric characters, hyphens, underscores, colons or plus signs. For more information please consult the documentation."));

                    Assert.That(errorResponse.Detail, Does.Match("One or more elements of the request were invalid.*"));
                    Assert.That(errorResponse.Name, Is.EqualTo("InvalidRequestFailure"));
                }
                else
                {
                    Assert.Fail("The request should have failed due to a validation error, and the validation details should be returned");
                }
            }
        }

        [Test]
        public void ApiException_Converts_To_ValidationProblemDetails_MaxLength()
        {
            try
            {
                //Values for the field id must be non-zero in length and have no more than 64 characters.
                //For more information please consult the documentation.
                var testId = new string('a', 65);
                _factory.Api<RequestsApi>().GetRequest(testId);
            }
            catch (ApiException e)
            {
                Assert.That(e.IsValidationProblem, Is.True, "Response should indicate that there was a validation error with the request");

                //    An ApiException.ErrorContent thrown because of a request validation contains a JSON serialized LusidValidationProblemDetails
                if (e.TryGetValidationProblemDetails(out var errorResponse))
                {
                    //Should identify that there was a validation error with the code
                    Assert.That(errorResponse.Errors, Contains.Key("id"));
                    Assert.That(errorResponse.Errors["id"].Single(), Is.EqualTo("Values for the field id must be non-zero in length and have no more than 64 characters. For more information please consult the documentation."));

                    Assert.That(errorResponse.Detail, Does.Match("One or more elements of the request were invalid.*"));
                    Assert.That(errorResponse.Name, Is.EqualTo("InvalidRequestFailure"));
                }
                else
                {
                    Assert.Fail("The request should have failed due to a validation error, and the validation details should be returned");
                }
            }
        }        
    }
}
