using System.Linq;
using System.Reflection;
using Namotion.Reflection;
using NJsonSchema;
using NJsonSchema.Generation;
using Xunit;

namespace NSwag.Generation.Tests
{
    public class OpenApiDocumentGeneratorTests
    {

        public class TestController
        {
            public void HasArrayParameter(string[] foo)
            {
            }
        }

        private OpenApiParameter GetParameter(SchemaType schemaType)
        {
            var generatorSettings = new OpenApiDocumentGeneratorSettings
            {
                SchemaType = schemaType,
                ReflectionService = new DefaultReflectionService()
            };
            var schemaResolver = new JsonSchemaResolver(new OpenApiDocument(), generatorSettings);
            var generator = new OpenApiDocumentGenerator(generatorSettings, schemaResolver);
            var methodInfo = typeof(TestController)
                .ToContextualType()
                .Methods
                .Single(m => m.Name == "HasArrayParameter");

            return generator.CreatePrimitiveParameter("foo", "bar", methodInfo.Parameters.First());
        }

        [Fact]
        public void CreatePrimitiveParameter_QueryStringArray_OpenApi()
        {
            var parameter = GetParameter(SchemaType.OpenApi3);

            ClassicAssert.True(parameter.Explode);
            ClassicAssert.Equal(OpenApiParameterStyle.Form, parameter.Style);
            ClassicAssert.Equal(OpenApiParameterCollectionFormat.Undefined, parameter.CollectionFormat);
        }

        [Fact]
        public void CreatePrimitiveParameter_QueryStringArray_Swagger()
        {
            var parameter = GetParameter(SchemaType.Swagger2);

            ClassicAssert.False(parameter.Explode);
            ClassicAssert.Equal(OpenApiParameterStyle.Undefined, parameter.Style);
            ClassicAssert.Equal(OpenApiParameterCollectionFormat.Multi, parameter.CollectionFormat);
        }
    }
}