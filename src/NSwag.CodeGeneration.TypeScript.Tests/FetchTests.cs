﻿using System.Threading.Tasks;
using Xunit;
using NSwag.Generation.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace NSwag.CodeGeneration.TypeScript.Tests
{
    public class FetchTests
    {
        public class Foo
        {
            public string Bar { get; set; }
        }

        public class DiscussionController : Controller
        {
            [HttpPost]
            public void AddMessage([FromBody]Foo message)
            {
            }
        }

        public class UrlEncodedRequestConsumingController: Controller
        {
            [HttpPost]
            [Consumes("application/x-www-form-urlencoded")]
            public void AddMessage([FromForm]Foo message, [FromForm]string messageId, [FromForm]System.DateTime date, [FromForm]System.Collections.Generic.List<string> list)
            {
            }
        }

        [Fact]
        public async Task When_export_types_is_true_then_add_export_before_classes()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateForControllerAsync<DiscussionController>();
            var json = document.ToJson();

            // Act
            var codeGen = new TypeScriptClientGenerator(document, new TypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Fetch,
                GenerateClientInterfaces = true,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 2.0m,
                    ExportTypes = true
                }
            });
            var code = codeGen.GenerateFile();

            // Assert
            ClassicAssert.Contains("export class DiscussionClient", code);
            ClassicAssert.Contains("export interface IDiscussionClient", code);
        }

        [Fact]
        public async Task When_export_types_is_false_then_dont_add_export_before_classes()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateForControllerAsync<DiscussionController>();
            var json = document.ToJson();

            // Act
            var codeGen = new TypeScriptClientGenerator(document, new TypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Fetch,
                GenerateClientInterfaces = true,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 2.0m,
                    ExportTypes = false
                }
            });
            var code = codeGen.GenerateFile();

            // Assert
            ClassicAssert.DoesNotContain("export class DiscussionClient", code);
            ClassicAssert.DoesNotContain("export interface IDiscussionClient", code);
        }

        [Fact]
        public async Task When_consumes_is_url_encoded_then_construct_url_encoded_request()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateForControllerAsync<UrlEncodedRequestConsumingController>();
            var json = document.ToJson();

            // Act
            var codeGen = new TypeScriptClientGenerator(document, new TypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Fetch,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 2.0m
                }
            });
            var code = codeGen.GenerateFile();

            // Assert
            ClassicAssert.Contains("content_", code);
            ClassicAssert.DoesNotContain("FormData", code);
            ClassicAssert.Contains("\"Content-Type\": \"application/x-www-form-urlencoded\"", code);
        }

        [Fact]
        public async Task When_abort_signal()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateForControllerAsync<UrlEncodedRequestConsumingController>();
            var json = document.ToJson();

            // Act
            var codeGen = new TypeScriptClientGenerator(document, new TypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Fetch,
                UseAbortSignal = true,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 2.7m
                }
            });
            var code = codeGen.GenerateFile();

            // Assert
            ClassicAssert.Contains("signal?: AbortSignal | undefined", code);
        }

        [Fact]
        public async Task When_no_abort_signal()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateForControllerAsync<UrlEncodedRequestConsumingController>();
            var json = document.ToJson();

            // Act
            var codeGen = new TypeScriptClientGenerator(document, new TypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Fetch,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 2.0m
                }
            });
            var code = codeGen.GenerateFile();

            // Assert
            ClassicAssert.DoesNotContain("signal?: AbortSignal | undefined", code);
            ClassicAssert.DoesNotContain("signal", code);;
        }

        [Fact]
        public async Task When_includeHttpContext()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateForControllerAsync<UrlEncodedRequestConsumingController>();

            // Act
            var codeGen = new TypeScriptClientGenerator(document, new TypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Angular,
                IncludeHttpContext = true,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 2.7m
                }
            });
            var code = codeGen.GenerateFile();

            // Assert
            ClassicAssert.Contains("httpContext?: HttpContext", code);
            ClassicAssert.Contains("context: httpContext", code);
        }

        [Fact]
        public async Task When_no_includeHttpContext()
        {
            // Arrange
            var generator = new WebApiOpenApiDocumentGenerator(new WebApiOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateForControllerAsync<UrlEncodedRequestConsumingController>();
            var json = document.ToJson();

            // Act
            var codeGen = new TypeScriptClientGenerator(document, new TypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Angular,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 2.7m
                }
            });
            var code = codeGen.GenerateFile();

            // Assert
            ClassicAssert.DoesNotContain("httpContext?: HttpContext", code);
            ClassicAssert.DoesNotContain("context: httpContext", code);
        }
    }
}
