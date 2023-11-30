using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests
{
    public class LanguageParameterTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_implicit_language_parameter_is_used_then_it_is_in_spec()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();
            settings.SchemaType = SchemaType.OpenApi3;

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(LanguagesController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.Single();

            ClassicAssert.Equal(2, operation.Operation.ActualParameters.Count);
            ClassicAssert.Contains(operation.Operation.ActualParameters, p => p.Name == "language");

            var parameter = operation.Operation.ActualParameters.Single(p => p.Name == "language");
            ClassicAssert.Equal(JsonObjectType.String, parameter.ActualTypeSchema.Type);
            ClassicAssert.False(parameter.IsNullable(settings.SchemaType));
        }
    }
}
