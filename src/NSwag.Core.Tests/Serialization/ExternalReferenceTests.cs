﻿using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NSwag.Core.Tests.Serialization
{
    public class ExternalReferenceTests
    {
        [Fact]
        public async Task When_file_contains_schema_reference_to_another_file_it_is_loaded()
        {
            var document = await OpenApiDocument.FromFileAsync("TestFiles/schema-reference.json");

            ClassicAssert.NotNull(document);
            ClassicAssert.Equal("External object", document.Paths.First().Value.Values.First().Responses.First().Value.Content.First().Value.Schema.ActualSchema.Description);
        }

        [Fact]
        public async Task When_file_contains_path_reference_to_another_file_it_is_loaded()
        {
            var document = await OpenApiDocument.FromFileAsync("TestFiles/path-reference.json");

            ClassicAssert.NotNull(document);
            ClassicAssert.Equal("External path", document.Paths.First().Value.ActualPathItem.Values.First().Description);
        }

        [Fact]
        public async Task When_file_contains_response_reference_to_another_file_it_is_loaded()
        {
            var document = await OpenApiDocument.FromFileAsync("TestFiles/response-reference.json");

            ClassicAssert.NotNull(document);
            ClassicAssert.Equal("External response", document.Paths.First().Value.Values.First().Responses.First().Value.ActualResponse.Description);
        }

        [Fact]
        public async Task When_file_contains_parameter_reference_to_another_file_it_is_loaded()
        {
            var document = await OpenApiDocument.FromFileAsync("TestFiles/parameter-reference.json");

            ClassicAssert.NotNull(document);
            ClassicAssert.Equal(2, document.Paths.First().Value.Values.First().Parameters.Count);
            ClassicAssert.Equal("offset", document.Paths.First().Value.Values.First().Parameters.First().ActualParameter.Name);
        }
    }
}
