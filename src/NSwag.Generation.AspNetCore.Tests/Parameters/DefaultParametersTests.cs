﻿using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers.Parameters;
using Xunit;
using static NSwag.Generation.AspNetCore.Tests.Web.Controllers.Parameters.DefaultParametersController;

namespace NSwag.Generation.AspNetCore.Tests.Parameters
{
    public class DefaultParametersTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_parameter_has_default_and_schema_type_is_OpenApi3_then_schema_default_is_set()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                RequireParametersWithoutDefault = true
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(DefaultParametersController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(DefaultParametersController.WithDefault))).Operation;

            ClassicAssert.Equal(5, operation.Parameters.First().Schema.Default);
        }

        [Fact]
        public async Task When_parameter_has_default_and_schema_type_is_OpenApi3_then_schema_default_is_set_on_oneOf_reference()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                RequireParametersWithoutDefault = true
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(DefaultParametersController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(DefaultParametersController.WithDefaultEnum))).Operation;

            ClassicAssert.Equal((int)MyEnum.Def, operation.Parameters.First().Schema.Default);
            ClassicAssert.True(operation.Parameters.First().Schema.OneOf.Any());
        }

        [Fact]
        public async Task When_parameter_has_default_and_schema_type_is_OpenApi3_then_schema_default_is_set_and_is_string()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                RequireParametersWithoutDefault = true,
                SerializerSettings = new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                }
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(DefaultParametersController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(DefaultParametersController.WithDefaultEnum))).Operation;

            ClassicAssert.Equal("Def", operation.Parameters.First().Schema.Default);
        }

        [Fact]
        public async Task When_parameter_has_default_and_schema_type_is_Swagger2_then_parameter_default_is_set()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings
            {
                SchemaType = SchemaType.Swagger2,
                RequireParametersWithoutDefault = true
            };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(DefaultParametersController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First(o => o.Path.Contains(nameof(DefaultParametersController.WithDefault))).Operation;

            ClassicAssert.Null(operation.Parameters.First().Schema);
            ClassicAssert.Equal(5, operation.Parameters.First().Default);
        }
    }
}