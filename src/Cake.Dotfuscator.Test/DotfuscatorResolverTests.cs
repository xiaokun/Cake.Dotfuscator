using System;
using Cake.Dotfuscaotr.Test.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cake.Core;

namespace Cake.Dotfuscaotr.Test
{
    [TestClass]
    public class DotfuscatorResolverTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_Throw_If_File_System_Is_Null()
        {
            // Given
            var fixture = new DotfuscatorResolverFixture();
            fixture.FileSystem = null;

            fixture.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_Throw_If_Environment_Is_Null()
        {
            // Given
            var fixture = new DotfuscatorResolverFixture();
            fixture.Environment = null;

            fixture.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_Throw_If_Registry_Is_Null()
        {
            // Given
            var fixture = new DotfuscatorResolverFixture();
            fixture.Registry = null;
            fixture.Resolve();
        }
    }

    [TestClass]
    public sealed class TheResolveMethod
    {
        [TestMethod]
        public void Should_Return_From_Disc_If_Found(bool is64Bit)
        {
            // Given
            var fixture = new DotfuscatorResolverFixture(is64Bit);
            fixture.GivenThatToolExistInKnownPath();

            // When
            var result = fixture.Resolve();

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Should_Return_From_Registry_If_Found()
        {
            // Given
            var fixture = new DotfuscatorResolverFixture();
            fixture.GivenThatToolHasRegistryKey();

            // When
            var result = fixture.Resolve();

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(CakeException))]
        public void Should_Throw_If_Not_Found_On_Disc_And_SDK_Registry_Path_Cannot_Be_Resolved()
        {
            // Given
            var fixture = new DotfuscatorResolverFixture();
            fixture.GivenThatNoSdkRegistryKeyExist();

            fixture.Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof(CakeException))]
        public void Should_Throw_If_SignTool_Cannot_Be_Resolved()
        {
            // Given
            var fixture = new DotfuscatorResolverFixture();
            fixture.Resolve();
        }
    }
}
