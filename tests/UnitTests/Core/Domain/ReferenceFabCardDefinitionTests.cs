using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.TestInfrastructure.ClassData;
using Xunit;

namespace ThunderbirdsBoardGameEngine.ReferenceData.UnitTests.Domain
{
    public class ReferenceFabCardDefinitionTests
    {
        private static CardCode ValidCardCode => new("fab-1");

        private static string ValidDisplayName => "Fab Card 1";

        private static string ValidEffectDescription => "Some description";

        [Fact]
        public void Constructor_WhenAllInputsValid_CreatesInstance()
        {
            // Arrange
            var instructionalText = "some instruction";

            // Act
            var result = new ReferenceFabCardDefinition(
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
            var result = new ReferenceFabCardDefinition(
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
            Assert.ThrowsAny<ArgumentException>(() => new ReferenceFabCardDefinition(
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
            Assert.ThrowsAny<ArgumentException>(() => new ReferenceFabCardDefinition(
                code: ValidCardCode,
                displayName: ValidDisplayName,
                effectDescription: effectDescription
            ));
        }
    }
}
