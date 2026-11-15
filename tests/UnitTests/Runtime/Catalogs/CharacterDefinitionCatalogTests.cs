using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Catalogs;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Builders;
using Xunit;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.UnitTests.Catalogs
{
    public class CharacterDefinitionCatalogTests
    {
        [Fact]
        public void GetAll_WithValidSnapshot_ReturnsAllCharacters()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.GetAll();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Contains(result, c => c.Code == new CharacterCode("character-1") && c.DisplayName == "Character 1");
            Assert.Contains(result, c => c.Code == new CharacterCode("character-2") && c.DisplayName == "Character 2");
        }

        [Fact]
        public void GetByCode_WithValidCode_ReturnsCharacter()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.GetByCode(new CharacterCode("character-2"));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new CharacterCode("character-2"), result.Code);
            Assert.Equal("Character 2", result.DisplayName);
        }

        [Fact]
        public void GetByCode_WithInvalidCode_ThrowsException()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => catalog.GetByCode(new CharacterCode("invalid-code")));
        }

        [Fact]
        public void Constructor_WithNullSnapshot_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CharacterDefinitionCatalog(null!));
        }

        [Fact]
        public void Exists_WithValidCode_ReturnsTrue()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var exists = catalog.Exists(new CharacterCode("character-1"));

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void Exists_WithInvalidCode_ReturnsFalse()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var exists = catalog.Exists(new CharacterCode("invalid-code"));

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void TryGetByCode_WithValidCode_ReturnsTrueAndCharacter()
        {
            // Arrange
            var catalog = CreateCatalog();
            // Act
            var result = catalog.TryGetByCode(new CharacterCode("character-1"), out var characterDefinition);
            // Assert
            Assert.True(result);
            Assert.NotNull(characterDefinition);
            Assert.Equal(new CharacterCode("character-1"), characterDefinition!.Code);
            Assert.Equal("Character 1", characterDefinition.DisplayName);
        }

        [Fact]
        public void TryGetByCode_WithInvalidCode_ReturnsFalseAndNull()
        {
            // Arrange
            var catalog = CreateCatalog();
            // Act
            var result = catalog.TryGetByCode(new CharacterCode("invalid-code"), out var characterDefinition);
            // Assert
            Assert.False(result);
            Assert.Null(characterDefinition);
        }

        private static CharacterDefinitionCatalog CreateCatalog()
        {
            var snapshot = new ReferenceDataSnapshotBuilder()
                .WithCharacter("character-1", "Character 1")
                .WithCharacter("character-2", "Character 2")
                .Build();

            return new CharacterDefinitionCatalog(snapshot);
        }
    }
}
