﻿using NJsonSchema;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Requests;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests.Requests
{
    public class PostBodyTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_OpenApiBodyParameter_is_applied_with_JSON_then_request_body_is_any_type()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaType = SchemaType.OpenApi3 };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(PostBodyController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "PostBody_JsonPostBodyOperation").Operation;
            var parameter = operation.Parameters.Single(p => p.Kind == OpenApiParameterKind.Body);

            ClassicAssert.Equal(1, operation.Parameters.Count);
            ClassicAssert.True(operation.RequestBody.Content["application/json"].Schema.IsAnyType);
        }

        [Fact]
        public async Task When_OpenApiBodyParameter_is_applied_with_text_then_request_body_is_file()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings { SchemaType = SchemaType.OpenApi3 };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(PostBodyController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Operation.OperationId == "PostBody_FilePostBodyOperation").Operation;
            var parameter = operation.Parameters.Single(p => p.Kind == OpenApiParameterKind.Body);

            ClassicAssert.Equal(1, operation.Parameters.Count);
            ClassicAssert.Equal(JsonObjectType.String, operation.RequestBody.Content["text/plain"].Schema.Type);
            ClassicAssert.Equal("binary", operation.RequestBody.Content["text/plain"].Schema.Format);
        }
    }
}