using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Parameters;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests.Parameters
{
    public class FormDataTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task WhenOperationHasFormDataFile_ThenItIsInRequestBody()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaType = SchemaType.OpenApi3 };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(FileUploadController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "FileUpload_UploadFile").Operation;
            var schema = operation.RequestBody.Content["multipart/form-data"].Schema;

            ClassicAssert.Equal("binary", schema.Properties["file"].Format);
            ClassicAssert.Equal(JsonObjectType.String, schema.Properties["file"].Type);
            ClassicAssert.Equal(JsonObjectType.String, schema.Properties["test"].Type);

            ClassicAssert.Contains(@"    ""/api/FileUpload/UploadFiles"": {
      ""post"": {
        ""tags"": [
          ""FileUpload""
        ],
        ""operationId"": ""FileUpload_UploadFiles"",
        ""requestBody"": {
          ""content"": {
            ""multipart/form-data"": {
              ""schema"": {
                ""properties"": {
                  ""files"": {
                    ""type"": ""array"",
                    ""items"": {
                      ""type"": ""string"",
                      ""format"": ""binary""
                    }
                  },
                  ""test"": {
                    ""type"": ""string""
                  }
                }
              }
            }
          }
        },".Replace("\r", ""), json.Replace("\r", ""));
        }

        [Fact]
        public async Task WhenOperationHasFormDataComplex_ThenItIsInRequestBody()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaType = SchemaType.OpenApi3 };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(FileUploadController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "FileUpload_UploadAttachment").Operation;

            ClassicAssert.NotNull(operation);
            ClassicAssert.Contains(@"""requestBody"": {
          ""content"": {
            ""multipart/form-data"": {
              ""schema"": {
                ""type"": ""object"",
                ""properties"": {
                  ""Description"": {
                    ""type"": ""string"",
                    ""nullable"": true
                  },
                  ""Contents"": {
                    ""type"": ""string"",
                    ""format"": ""binary"",
                    ""nullable"": true
                  }
                }
              }
            }
          }
        },".Replace("\r", ""), json.Replace("\r", ""));
        }
    }
}