using NJsonSchema;
using Xunit;

namespace NSwag.Core.Tests.Serialization
{
    public class SwaggerSecuritySchemeTests
    {
        [Fact]
        public void When_security_schema_is_defined_as_Swagger_and_serialized_as_Swagger_then_it_is_correct()
        {
            // Arrange
            var document = CreateSwaggerDocument();

            // Act
            var json = document.ToJson(SchemaType.Swagger2);

            // Assert
            ClassicAssert.Contains(@"""type"": ""basic""", json);
            ClassicAssert.Contains(@"""authorizationUrl"": ""AuthUrl""", json);
            ClassicAssert.DoesNotContain(@"""flows""", json);
        }

        [Fact]
        public void When_security_schema_is_defined_as_Swagger_and_serialized_as_OpenApi_then_it_is_correct()
        {
            // Arrange
            var document = CreateSwaggerDocument();

            // Act
            var json = document.ToJson(SchemaType.OpenApi3);

            // Assert
            ClassicAssert.Contains(@"""type"": ""http""", json);
            ClassicAssert.Contains(@"""scheme"": ""basic""", json);
            ClassicAssert.Contains(@"""authorizationUrl"": ""AuthUrl""", json);
            ClassicAssert.Contains(@"""flows""", json);
            ClassicAssert.DoesNotContain(@"""flow""", json);
        }

        [Fact]
        public void When_security_schema_is_defined_as_OpenApi_and_serialized_as_Swagger_then_it_is_correct()
        {
            // Arrange
            var document = CreateOpenApiDocument();

            // Act
            var json = document.ToJson(SchemaType.Swagger2);

            // Assert
            ClassicAssert.Contains(@"""type"": ""basic""", json);
            ClassicAssert.Contains(@"""authorizationUrl"": ""AuthUrl""", json);
            ClassicAssert.DoesNotContain(@"""flows""", json);
        }

        [Fact]
        public void When_security_schema_is_defined_as_OpenApi_and_serialized_as_OpenApi_then_it_is_correct()
        {
            // Arrange
            var document = CreateOpenApiDocument();

            // Act
            var json = document.ToJson(SchemaType.OpenApi3);

            // Assert
            ClassicAssert.Contains(@"""type"": ""http""", json);
            ClassicAssert.Contains(@"""scheme"": ""basic""", json);
            ClassicAssert.Contains(@"""refreshUrl"": ""RefreshUrl""", json);
            ClassicAssert.Contains(@"""tokenUrl"": ""TokenUrl""", json);
            ClassicAssert.Contains(@"""authorizationUrl"": ""AuthUrl""", json);
            ClassicAssert.Contains(@"""flows""", json);
            ClassicAssert.DoesNotContain(@"""flow""", json);
        }

        private static OpenApiDocument CreateSwaggerDocument()
        {
            var document = new OpenApiDocument();
            document.SecurityDefinitions.Add("foo", new OpenApiSecurityScheme
            {
                Name = "Baz",
                Description = "Bar",
                Type = OpenApiSecuritySchemeType.Basic,
                Flow = OpenApiOAuth2Flow.Application,
                In = OpenApiSecurityApiKeyLocation.Header,
                AuthorizationUrl = "AuthUrl",
            });

            return document;
        }

        private static OpenApiDocument CreateOpenApiDocument()
        {
            var document = new OpenApiDocument();
            document.SecurityDefinitions.Add("foo", new OpenApiSecurityScheme
            {
                Name = "Baz",
                Description = "Bar",
                Type = OpenApiSecuritySchemeType.Http,
                Scheme = "basic",
                In = OpenApiSecurityApiKeyLocation.Cookie,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        RefreshUrl = "RefreshUrl",
                        TokenUrl = "TokenUrl",
                        AuthorizationUrl = "AuthUrl"
                    }
                }
            });

            return document;
        }
    }
}