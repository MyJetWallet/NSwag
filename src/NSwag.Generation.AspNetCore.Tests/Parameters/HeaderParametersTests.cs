using System.Linq;
using System.Threading.Tasks;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Parameters;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests.Parameters
{
    public class HeaderParametersTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_complex_query_parameters_are_nullable_and_set_to_null_they_are_optional_in_spec()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { RequireParametersWithoutDefault = true };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(HeaderParametersController));

            // Assert
            var operation = document.Operations.First().Operation;

            ClassicAssert.Equal(2, operation.ActualParameters.Count);
            ClassicAssert.True(operation.ActualParameters.First().IsRequired);
            ClassicAssert.False(operation.ActualParameters.Last().IsRequired);
        }
    }
}