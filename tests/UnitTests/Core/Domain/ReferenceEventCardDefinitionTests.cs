using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.TestInfrastructure.ClassData;
using Xunit;

namespace ThunderbirdsBoardGameEngine.ReferenceData.UnitTests.Domain
{
    public class ReferenceEventCardDefinitionTests
    {
        private static CardCode ValidCardCode => new("event-1");

        private static string ValidDisplayName => "Event Card 1";

        private static string ValidEffectDescription => "Some description";

        [Fact]
        public void Constructor_WhenAllInputsValid_CreatesInstance()
        {
            // Arrange
            var instructionalText = "some instruction";


            // Act
            var result = new ReferenceEventCardDefinition(
                code: ValidCardCode,
                displayName: ValidDisplayName,
                effectDescription: ValidEffectDescription,
                instructionalText: instructionalText
            );

            // Assert
            Assert.Equal(ValidCardCode, result.Code);
            Assert.Equal(ValidDisplayName, result.DisplayName);
            Assert.Equal(ValidEffectDescription, result.EffectDescription);
            Assert.Equal(instructionalText, result.InstructionalText);
        }

        [Fact]
        public void Constructor_WhenInstructionalTextNotIncluded_ReturnsNullInstructionalText()
        {
            // Arrange

            // Act
            var result = new ReferenceEventCardDefinition(
                code: ValidCardCode,
                displayName: ValidDisplayName,
                effectDescription: ValidEffectDescription
            );

            // Assert
            Assert.Null(result.InstructionalText);
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStringData))]
        public void Constructor_WhenDisplayNameInvalid_ThrowsArgumentException(string? displayName)
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() => new ReferenceEventCardDefinition(
                code: ValidCardCode,
                displayName: displayName,
                effectDescription: ValidEffectDescription
            ));
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStringData))]
        public void Constructor_WhenEffectDescriptionInvalid_ThrowsArgumentException(string? effectDescription)
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() => new ReferenceEventCardDefinition(
                code: ValidCardCode,
                displayName: ValidDisplayName,
                effectDescription: effectDescription
            ));
        }
    }
}
