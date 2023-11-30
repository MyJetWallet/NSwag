using System.Linq;
using NSwag.AssemblyLoader.Utilities;
using Xunit;

namespace NSwag.AssemblyLoader.Tests
{
    public class WildcardTests
    {
        [Fact(Skip = "Ignored")]
        public void When_path_has_wildcards_then_they_are_expanded_correctly()
        {
            // Arrange


            // Act
            var files = PathUtilities.ExpandFileWildcards("../../**/NSwag.*.dll").ToList();

            // Assert
            ClassicAssert.True(files.Any(f => f.Contains("bin\\Debug")) || files.Any(f => f.Contains("bin\\Release")));
        }

        [Fact]
        public void NoWildcard()
        {
            // Arrange
            var items = new string[] { "abc/def", "ghi/jkl" };

            // Act
            var matches = PathUtilities.FindWildcardMatches("abc/def", items, '/');

            // Assert
            ClassicAssert.Single(matches);
            ClassicAssert.Equal("abc/def", matches.First());
        }

        [Fact]
        public void SingleWildcardInTheMiddle()
        {
            // Arrange
            var items = new string[] { "abc/def/ghi", "abc/def/jkl", "abc/a/b/ghi" };

            // Act
            var matches = PathUtilities.FindWildcardMatches("abc/*/ghi", items, '/');

            // Assert
            ClassicAssert.Single(matches);
            ClassicAssert.Equal("abc/def/ghi", matches.First());
        }

        [Fact]
        public void DoubleWildcardInTheMiddle()
        {
            // Arrange
            var items = new string[] { "a/b/c", "a/b/d", "a/b/b/c" };

            // Act
            var matches = PathUtilities.FindWildcardMatches("a/**/c", items, '/');

            // Assert
            ClassicAssert.Equal(2, matches.Count());
            ClassicAssert.Equal("a/b/c", matches.First());
            ClassicAssert.Equal("a/b/b/c", matches.Last());
        }

        [Fact]
        public void DoubleWildcardAtTheEnd()
        {
            // Arrange
            var items = new string[] { "abc/a", "abc/b", "abc/c/d" };

            // Act
            var matches = PathUtilities.FindWildcardMatches("abc/**", items, '/');

            // Assert
            ClassicAssert.Equal(3, matches.Count());
        }

        [Fact]
        public void SingleWildcardAtTheEnd()
        {
            // Arrange
            var items = new string[] { "abc/a", "abc/b", "abc/c/d" };

            // Act
            var matches = PathUtilities.FindWildcardMatches("abc/*", items, '/');

            // Assert
            ClassicAssert.Equal(2, matches.Count());
        }

        [Fact]
        public void DoubleWildcardAtTheBeginning()
        {
            // Arrange
            var items = new string[] { "a/b/c", "a/b/d", "a/c" };

            // Act
            var matches = PathUtilities.FindWildcardMatches("**/c", items, '/');

            // Assert
            ClassicAssert.Equal(2, matches.Count());
            ClassicAssert.Equal("a/b/c", matches.First());
            ClassicAssert.Equal("a/c", matches.Last());
        }

        [Fact]
        public void SingleWildcardAtTheBeginning()
        {
            // Arrange
            var items = new string[] { "a/b/c", "x/y/c", "x/b/c" };

            // Act
            var matches = PathUtilities.FindWildcardMatches("*/b/c", items, '/');

            // Assert
            ClassicAssert.Equal(2, matches.Count());
            ClassicAssert.Equal("a/b/c", matches.First());
            ClassicAssert.Equal("x/b/c", matches.Last());
        }
    }
}
