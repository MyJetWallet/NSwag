﻿using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.Generation;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Responses;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests.Responses
{
    public class NullableResponseTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_handling_is_NotNull_then_response_is_not_nullable()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(ResponsesController));

            // Assert
            var operation = document.Operations.First().Operation;

            ClassicAssert.False(operation.ActualResponses.First().Value.Schema.IsNullable(SchemaType.OpenApi3));
        }

        [Fact]
        public async Task When_handling_is_Null_then_response_is_nullable()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.Null
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(ResponsesController));

            // Assert
            var operation = document.Operations.First().Operation;

            ClassicAssert.True(operation.ActualResponses.First().Value.Schema.IsNullable(SchemaType.OpenApi3));
        }

        [Fact]
        public async Task When_nullable_xml_docs_is_set_to_true_then_response_is_nullable()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(NullableResponseController));

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(NullableResponseController.OperationWithNullableResponse))).Operation;

            ClassicAssert.True(operation.ActualResponses.First().Value.Schema.IsNullable(SchemaType.OpenApi3));
        }

        [Fact]
        public async Task When_nullable_xml_docs_is_set_to_false_then_response_is_not_nullable()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.Null
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(NullableResponseController));

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(NullableResponseController.OperationWithNonNullableResponse))).Operation;

            ClassicAssert.False(operation.ActualResponses.First().Value.Schema.IsNullable(SchemaType.OpenApi3));
        }

        [Fact]
        public async Task When_nullable_xml_docs_is_not_set_then_default_setting_NotNull_is_used()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(NullableResponseController));

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(NullableResponseController.OperationWithNoXmlDocs))).Operation;

            ClassicAssert.False(operation.ActualResponses.First().Value.Schema.IsNullable(SchemaType.OpenApi3));
        }

        [Fact]
        public async Task When_nullable_xml_docs_is_not_set_then_default_setting_Null_is_used()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.Null
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(NullableResponseController));

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(NullableResponseController.OperationWithNoXmlDocs))).Operation;

            ClassicAssert.True(operation.ActualResponses.First().Value.Schema.IsNullable(SchemaType.OpenApi3));
        }
    }
}