using NSwag.AssemblyLoader.Utilities;
using Xunit;

namespace NSwag.AssemblyLoader.Tests
{
    public class PathUtilitiesTests
    {
        [Fact]
        public void TwoAbsolutePaths_ConvertToRelativePath_RelativeDirectoryPath()
        {
            // Act
            var relative = PathUtilities.MakeRelativePath("C:\\Foo\\Bar", "C:\\Foo");

            // Assert
            ClassicAssert.Equal("Bar", relative);
        }

        [Fact]
        public void RelativeAndAbsolutePath_ConvertToAbsolutePath_AbsolutePath()
        {
            // Act
            var absolute = PathUtilities.MakeAbsolutePath("Bar", "C:\\Foo");

            // Assert
            ClassicAssert.Equal("C:\\Foo\\Bar", absolute);
        }

        [Fact]
        public void TwoAbsoluteEqualPaths_ConvertToRelativePath_CurrentDirectory()
        {
            // Act
            var relative = PathUtilities.MakeRelativePath("C:\\Foo\\Bar", "C:\\Foo\\Bar");

            // Assert
            ClassicAssert.Equal(".", relative);
        }

        [Fact]
        public void CurrentDirectoryAndAbsolutePath_ConvertToAbsolutePath_SameAbsolutePath()
        {
            // Act
            var absolute = PathUtilities.MakeAbsolutePath(".", "C:\\Foo");

            // Assert
            ClassicAssert.Equal("C:\\Foo", absolute);
        }
    }
}
