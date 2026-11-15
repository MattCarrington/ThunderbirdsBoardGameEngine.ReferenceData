using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces
{
    /// <summary>
    /// Provides read-only access to fab card definitions from the reference data snapshot.
    /// </summary>
    public interface IFabCardDefinitionCatalog
    {
        /// <summary>
        /// Determines whether a card with the specified code exists.
        /// </summary>
        /// <param name="cardCode">The code of the card to locate. Cannot be null.</param>
        /// <returns>true if a card with the specified code exists; otherwise, false.</returns>
        bool Exists(CardCode cardCode);

        /// <summary>
        /// Gets all reference FAB card definitions.
        /// </summary>
        /// <returns>An immutable array containing all reference FAB card definitions.</returns>
        ImmutableArray<ReferenceFabCardDefinition> GetAll();

        /// <summary>
        /// Gets the Fab card definition with the specified code.
        /// </summary>
        /// <param name="cardCode">The code of the Fab card definition to retrieve.</param>
        /// <returns>The Fab card definition matching the specified code.</returns>
        ReferenceFabCardDefinition GetByCode(CardCode cardCode);

        /// <summary>
        /// Gets the card definition with the specified code.
        /// </summary>
        /// <param name="cardCode">The code to search for.</param>
        /// <param name="cardDefinition">When this method returns true, the matching card definition; otherwise, null.</param>
        /// <returns>true if a card definition with the specified code is found; otherwise, false.</returns>
        bool TryGetByCode(CardCode cardCode, [NotNullWhen(true)] out ReferenceFabCardDefinition? cardDefinition);
    }
}