using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces
{
    /// <summary>
    /// Provides read-only access to event card definitions from the reference data snapshot.
    /// </summary>
    public interface IEventCardDefinitionCatalog
    {
        /// <summary>
        /// Determines whether a card with the specified code exists.
        /// </summary>
        /// <param name="cardCode">The code of the card to locate. Cannot be null.</param>
        /// <returns>true if a card with the specified code exists; otherwise, false.</returns>
        bool Exists(CardCode cardCode);

        /// <summary>
        /// Gets all reference event card definitions.
        /// </summary>
        /// <returns>An immutable array containing all reference event card definitions.</returns>
        ImmutableArray<ReferenceEventCardDefinition> GetAll();

        /// <summary>
        /// Gets the event card definition associated with the specified code.
        /// </summary>
        /// <param name="cardCode">The code of the event card to retrieve.</param>
        /// <returns>The event card definition associated with <paramref name="cardCode"/>.</returns>
        ReferenceEventCardDefinition GetByCode(CardCode cardCode);

        /// <summary>
        /// Retrieves the event card definition associated with the specified code.
        /// </summary>
        /// <param name="cardCode">The code of the event card to retrieve.</param>
        /// <param name="eventCardDefinition">When this method returns true, the matching event card definition; otherwise, null.</param>
        /// <returns>true if a matching event card definition is found; otherwise, false.</returns>
        bool TryGetByCode(CardCode cardCode, [NotNullWhen(true)] out ReferenceEventCardDefinition? eventCardDefinition);
    }
}