using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces
{
    /// <summary>
    /// Provides read-only access to disaster definitions from the reference data snapshot.
    /// </summary>
    public interface IDisasterDefinitionCatalog
    {
        /// <summary>
        /// Gets all disaster definitions.
        /// </summary>
        /// <returns>An immutable array of all disaster definitions.</returns>
        ImmutableArray<ReferenceDisasterDefinition> GetAll();

        /// <summary>
        /// Gets a disaster definition by its unique code.
        /// </summary>
        /// <param name="code">The disaster card code.</param>
        /// <returns>The disaster definition.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when no disaster with the specified code exists.
        /// </exception>
        ReferenceDisasterDefinition GetByCode(CardCode code);

        /// <summary>
        /// Determines whether the specified card code exists.
        /// </summary>
        /// <param name="code">The card code to find.</param>
        /// <returns>true if the card code exists; otherwise, false.</returns>
        bool Exists(CardCode code);

        /// <summary>
        /// Gets the disaster definition associated with the specified card code.
        /// </summary>
        /// <param name="code">The card code to search for.</param>
        /// <param name="disasterDefinition">When this method returns true, the disaster definition associated with <paramref name="code"/>; otherwise,
        /// null.</param>
        /// <returns>true if a disaster definition with the specified code is found; otherwise, false.</returns>
        bool TryGetByCode(CardCode code, [NotNullWhen(true)] out ReferenceDisasterDefinition? disasterDefinition);
    }
}