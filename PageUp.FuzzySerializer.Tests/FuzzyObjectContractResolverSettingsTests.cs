using System;
using Xunit;
using PageUp.FuzzySerializer;

namespace PageUp.FuzzySerializer.Tests
{
    public class FuzzyObjectContractResolverSettingsTests
    {
        [Fact]
        public void DefaultSettings_Initiated_ShuffleResponseIsOn() {
            var settings = new FuzzyObjectContractResolverSettings();
            Assert.Equal(true, settings.ShuffleResponse);
        }

        [Fact]
        public void DefaultSettings_Initiated_AddRandomPropertyToObjectsIsOn() {
            var settings = new FuzzyObjectContractResolverSettings();
            Assert.Equal(true, settings.AddRandomPropertyToObjects);
        }

        [Fact]
        public void DefaultSettings_Initiated_AddPropertyInRandomPositionIsOn() {
            var settings = new FuzzyObjectContractResolverSettings();
            Assert.Equal(true, settings.AddPropertyInRandomPosition);
        }

        [Fact]
        public void DefaultSettings_Initiated_UseCamelCaseNamingStrategyIsOff()
        {
            var settings = new FuzzyObjectContractResolverSettings();
            Assert.Equal(false, settings.UseCamelCaseNamingStrategy);
        }
    }
}