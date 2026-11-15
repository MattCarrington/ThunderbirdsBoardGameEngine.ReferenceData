using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Catalogs;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Builders;
using Xunit;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.UnitTests.Catalogs
{
    public class FabCardDefinitionCatalogTests
    {
        [Fact]
        public void Exists_WhenExistingCardCode_ReturnsTrue()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.Exists(new CardCode("fab-1"));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Exists_WhenNonExistingCardCode_ReturnsFalse()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.Exists(new CardCode("non-existing-code"));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetAll_ReturnsAllFabCardDefinitions()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.GetAll();

            // Assert
            Assert.Equal(2, result.Length);

            Assert.Contains(result, card => card.Code == new CardCode("fab-1") && card.DisplayName == "Fab 1");
            Assert.Contains(result, card => card.Code == new CardCode("fab-2") && card.DisplayName == "Fab 2");
        }

        [Fact]
        public void GetByCode_WhenExistingCardCode_ReturnsFabCardDefinition()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.GetByCode(new CardCode("fab-1"));

            // Assert
            Assert.Equal(new CardCode("fab-1"), result.Code);
            Assert.Equal("Fab 1", result.DisplayName);
        }

        [Fact]
        public void GetByCode_WhenNonExistingCardCode_ThrowsKeyNotFoundException()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => catalog.GetByCode(new CardCode("non-existing-code")));
        }

        [Fact]
        public void TryGetByCode_WhenExistingCardCode_ReturnsTrueAndFabCardDefinition()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.TryGetByCode(new CardCode("fab-1"), out var fabCardDefinition);

            // Assert
            Assert.True(result);
            Assert.NotNull(fabCardDefinition);
            Assert.Equal(new CardCode("fab-1"), fabCardDefinition!.Code);
            Assert.Equal("Fab 1", fabCardDefinition.DisplayName);
        }

        [Fact]
        public void TryGetByCode_WhenNonExistingCardCode_ReturnsFalseAndNull()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.TryGetByCode(new CardCode("non-existing-code"), out var fabCardDefinition);

            // Assert
            Assert.False(result);
            Assert.Null(fabCardDefinition);
        }

        private static FabCardDefinitionCatalog CreateCatalog()
        {
            var snapshot = new ReferenceDataSnapshotBuilder()
                .WithFabCard("fab-1", "Fab 1")
                .WithFabCard("fab-2", "Fab 2")
                .Build();

            return new FabCardDefinitionCatalog(snapshot);
        }
    }
}
