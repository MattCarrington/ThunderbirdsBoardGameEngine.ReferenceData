using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Catalogs;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Builders;
using Xunit;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.UnitTests.Catalogs
{
    public class EventCardDefinitionCatalogTests
    {
        [Fact]
        public void Exists_WhenExistingCardCode_ReturnsTrue()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.Exists(new CardCode("event-1"));

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
        public void GetByCode_WhenExistingCardCode_ReturnsEventCardDefinition()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.GetByCode(new CardCode("event-1"));

            // Assert
            Assert.Equal(new CardCode("event-1"), result.Code);
            Assert.Equal("Event 1", result.DisplayName);
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
        public void TryGetByCode_WhenExistingCardCode_ReturnsTrueAndEventCardDefinition()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.TryGetByCode(new CardCode("event-1"), out var eventCardDefinition);

            // Assert
            Assert.True(result);
            Assert.NotNull(eventCardDefinition);
            Assert.Equal(new CardCode("event-1"), eventCardDefinition!.Code);
            Assert.Equal("Event 1", eventCardDefinition.DisplayName);
        }

        [Fact]
        public void TryGetByCode_WhenNonExistingCardCode_ReturnsFalseAndNull()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.TryGetByCode(new CardCode("non-existing-code"), out var eventCardDefinition);

            // Assert
            Assert.False(result);
            Assert.Null(eventCardDefinition);
        }

        [Fact]
        public void GetAll_ReturnsAllEventCardDefinitions()
        {
            // Arrange
            var catalog = CreateCatalog();

            // Act
            var result = catalog.GetAll();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Contains(result, card => card.Code == new CardCode("event-1") && card.DisplayName == "Event 1");
            Assert.Contains(result, card => card.Code == new CardCode("event-2") && card.DisplayName == "Event 2");
        }

        private static EventCardDefinitionCatalog CreateCatalog()
        {
            var snapshot = new ReferenceDataSnapshotBuilder()
                .WithEventCard("event-1", "Event 1")
                .WithEventCard("event-2", "Event 2")
                .Build();

            return new EventCardDefinitionCatalog(snapshot);
        }
    }
}
