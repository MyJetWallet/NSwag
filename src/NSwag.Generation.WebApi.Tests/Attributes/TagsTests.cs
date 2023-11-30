using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSwag.Annotations;

namespace NSwag.Generation.WebApi.Tests.Attributes
{
    [TestClass]
    public class TagsTests
    {
        [SwaggerTags("x", "y")]
        [SwaggerTag("a1", Description = "a2")]
        [SwaggerTag("b1", Description = "b2", DocumentationDescription = "b3", DocumentationUrl = "b4")]
        public class TagsTest1Controller : ApiController
        {
            [Route("foo")]
            public void Foo()
            {

            }
        }

        [TestMethod]
        public async Task When_controller_has_tag_attributes_then_they_are_processed()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());

            // Act
            var document = await generator.GenerateForControllerAsync<TagsTest1Controller>();
            var json = document.ToJson();

            // Assert
            ClassicAssert.AreEqual(4, document.Tags.Count);

            ClassicAssert.AreEqual("x", document.Tags[0].Name);
            ClassicAssert.AreEqual("y", document.Tags[1].Name);

            ClassicAssert.AreEqual("a1", document.Tags[2].Name);
            ClassicAssert.AreEqual("a2", document.Tags[2].Description);
            ClassicAssert.AreEqual(null, document.Tags[2].ExternalDocumentation);

            ClassicAssert.AreEqual("b1", document.Tags[3].Name);
            ClassicAssert.AreEqual("b2", document.Tags[3].Description);
            ClassicAssert.AreEqual("b3", document.Tags[3].ExternalDocumentation.Description);
            ClassicAssert.AreEqual("b4", document.Tags[3].ExternalDocumentation.Url);
        }

        public class TagsTest2Controller : ApiController
        {
            [SwaggerTags("foo", "bar")]
            public void MyAction()
            {

            }
        }

        [TestMethod]
        public async Task When_operation_has_tags_attributes_then_they_are_processed()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());

            // Act
            var document = await generator.GenerateForControllerAsync<TagsTest2Controller>();

            // Assert
            ClassicAssert.AreEqual("[\"foo\",\"bar\"]", JsonConvert.SerializeObject(document.Operations.First().Operation.Tags));
        }



        // AddToDocument tests

        public class TagsTest3Controller : ApiController
        {
            [SwaggerTag("foo", AddToDocument = true)]
            public void MyAction()
            {

            }
        }

        [TestMethod]
        public async Task When_operation_has_tag_attribute_with_AddToDocument_then_it_is_added_to_document()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());

            // Act
            var document = await generator.GenerateForControllerAsync<TagsTest3Controller>();

            // Assert
            ClassicAssert.AreEqual(1, document.Tags.Count);
            ClassicAssert.AreEqual("foo", document.Tags[0].Name);

            ClassicAssert.AreEqual(1, document.Operations.First().Operation.Tags.Count);
            ClassicAssert.AreEqual("foo", document.Operations.First().Operation.Tags[0]);
        }

        public class TagsTest4Controller : ApiController
        {
            [SwaggerTags("foo", "bar", AddToDocument = true)]
            public void MyAction()
            {

            }
        }

        [TestMethod]
        public async Task When_operation_has_tags_attribute_with_AddToDocument_then_it_is_added_to_document()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());

            // Act
            var document = await generator.GenerateForControllerAsync<TagsTest4Controller>();

            // Assert
            ClassicAssert.AreEqual(2, document.Tags.Count);
            ClassicAssert.AreEqual("foo", document.Tags[0].Name);
            ClassicAssert.AreEqual("bar", document.Tags[1].Name);

            ClassicAssert.AreEqual(2, document.Operations.First().Operation.Tags.Count);
            ClassicAssert.AreEqual("foo", document.Operations.First().Operation.Tags[0]);
            ClassicAssert.AreEqual("bar", document.Operations.First().Operation.Tags[1]);
        }
    }
}
