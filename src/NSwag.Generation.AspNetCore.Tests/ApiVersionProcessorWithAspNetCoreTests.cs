﻿using System.Linq;
using System.Threading.Tasks;
using NSwag.Generation.AspNetCore.Tests.Web.Controllers;
using Xunit;

namespace NSwag.Generation.AspNetCore.Tests
{
    public class ApiVersionProcessorWithAspNetCoreTests : AspNetCoreTestsBase
    {
        [Fact]
        public async Task When_api_version_parameter_should_be_ignored_then_it_is_ignored()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();
            settings.ApiGroupNames = new[] { "1" };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(VersionedValuesController), typeof(VersionedV3ValuesController));
            var json = document.ToJson();

            // Assert
            var operations = document.Operations;
            ClassicAssert.True(operations.All(o => o.Operation.ActualParameters.All(p => p.Name != "api-version")));
        }

        [Fact]
        public async Task When_generating_v1_then_only_v1_operations_are_included()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();
            settings.ApiGroupNames = new[] { "1" };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(VersionedValuesController), typeof(VersionedV3ValuesController));
            var json = document.ToJson();

            // Assert
            var operations = document.Operations;

            ClassicAssert.Equal(4, operations.Count());
            ClassicAssert.True(operations.All(o => o.Path.Contains("/v1/")));

            // VersionedIgnoredValues tag should not be in json document
            ClassicAssert.Equal(1, document.Tags.Count);
        }

        [Fact]
        public async Task When_generating_v2_then_only_v2_operations_are_included()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();
            settings.ApiGroupNames = new[] { "2" };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(VersionedValuesController), typeof(VersionedV3ValuesController));

            // Assert
            var operations = document.Operations;

            ClassicAssert.Equal(2, operations.Count());
            ClassicAssert.True(operations.All(o => o.Path.Contains("/v2/")));
            ClassicAssert.True(operations.All(o => o.Operation.IsDeprecated));

            // VersionedIgnoredValues tag should not be in json document
            ClassicAssert.Equal(1, document.Tags.Count);
        }

        [Fact]
        public async Task When_generating_v3_then_only_v3_operations_are_included()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();
            settings.ApiGroupNames = new[] { "3" };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(VersionedValuesController), typeof(VersionedV3ValuesController));

            // Assert
            var operations = document.Operations;

            ClassicAssert.Equal(5, operations.Count());
            ClassicAssert.True(operations.All(o => o.Path.Contains("/v3/")));

            // VersionedIgnoredValues tag should not be in json document
            ClassicAssert.Equal(1, document.Tags.Count);
        }

        [Fact]
        public async Task When_generating_versioned_controllers_then_version_path_parameter_is_not_present()
        {
            // Arrange
            var settings = new AspNetCoreOpenApiDocumentGeneratorSettings{};
            settings.ApiGroupNames = new[] { "3" };

            // Act
            var document = await GenerateDocumentAsync(settings, typeof(VersionedValuesController), typeof(VersionedV3ValuesController));
            var json = document.ToJson();

            // Assert
            var operation = document.Operations.First();

            // check that implict unused path parameter is not in the spec
            ClassicAssert.DoesNotContain(operation.Operation.ActualParameters, p => p.Name == "version");
        }
    }
}
