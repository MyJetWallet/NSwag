using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Requests;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests.Requests
{
    public class ConsumesTests : AspNetCoreTestsBase
    {
        // These test required the CustomTextInputFormatter

        [Fact]
        public async Task When_consumes_is_defined_on_all_operations_then_it_is_added_to_the_document()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(ConsumesController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "Consumes_ConsumesOnOperation").Operation;

            ClassicAssert.Contains("text/html", document.Consumes);
            ClassicAssert.Contains("text/html", operation.ActualConsumes);
        }

        [Fact]
        public async Task When_consumes_is_defined_on_single_operations_then_it_is_added_to_the_operation()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(ConsumesController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "Consumes_ConsumesOnOperation").Operation;

            ClassicAssert.DoesNotContain("foo/bar", document.Consumes);
            ClassicAssert.Contains("foo/bar", operation.Consumes);
            ClassicAssert.Contains("foo/bar", operation.ActualConsumes);
        }

        [Fact]
        public async Task When_operation_consumes_is_different_in_several_controllers_then_they_are_added_to_the_operation()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(ConsumesController), typeof(MultipartConsumesController));
            var json = document.ToJson();

            // Assert
            const string expectedTestContentType = "foo/bar";
            const string expectedMultipartContentType = "multipart/form-data";
            
            var operation = document.Operations
                .First(o => o.Operation.OperationId == "Consumes_ConsumesOnOperation")
                .Operation;

            var multipartOperation = document.Operations
                .First(o => o.Operation.OperationId == "MultipartConsumes_ConsumesOnOperation")
                .Operation;
            
            ClassicAssert.DoesNotContain(expectedTestContentType, document.Consumes);
            ClassicAssert.DoesNotContain(expectedMultipartContentType, document.Consumes);

            ClassicAssert.Contains(expectedTestContentType, operation.Consumes);
            ClassicAssert.Contains(expectedTestContentType, operation.ActualConsumes);

            ClassicAssert.Contains(expectedMultipartContentType, multipartOperation.Consumes);
            ClassicAssert.Contains(expectedMultipartContentType, multipartOperation.ActualConsumes);
        }
    }
}