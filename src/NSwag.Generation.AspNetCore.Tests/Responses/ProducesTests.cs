using System.Linq;
using System.Threading.Tasks;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Responses;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests.Responses
{
    public class ProducesTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_produces_is_defined_on_all_operations_then_it_is_added_to_the_document()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(TextProducesController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "TextProduces_ProducesOnOperation").Operation;

            ClassicAssert.Contains("text/html", document.Produces);
            ClassicAssert.Contains("text/html", operation.ActualProduces);
            ClassicAssert.Null(operation.Produces);
        }
        
        [Fact]
        public async Task When_operation_produces_is_different_in_several_controllers_then_they_are_added_to_the_operation()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(TextProducesController), typeof(JsonProducesController));
            var json = document.ToJson();

            // Assert
            const string expectedTextContentType = "text/html";
            const string expectedJsonJsonType = "application/json";

            var textOperation = document.Operations.First(o => o.Operation.OperationId == "TextProduces_ProducesOnOperation").Operation;
            var jsonOperation = document.Operations.First(o => o.Operation.OperationId == "JsonProduces_ProducesOnOperation").Operation;

            ClassicAssert.DoesNotContain(expectedTextContentType, document.Produces);
            ClassicAssert.DoesNotContain(expectedJsonJsonType, document.Produces);

            ClassicAssert.Contains(expectedTextContentType, textOperation.Produces);
            ClassicAssert.Contains(expectedTextContentType, textOperation.ActualProduces);

            ClassicAssert.Contains(expectedJsonJsonType, jsonOperation.Produces);
            ClassicAssert.Contains(expectedJsonJsonType, jsonOperation.ActualProduces);
        }
    }
}
