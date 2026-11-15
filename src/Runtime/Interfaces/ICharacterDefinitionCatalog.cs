using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces
{
    /// <summary>
    /// Provides read-only access to character definitions from the reference data snapshot.
    /// </summary>
    public interface ICharacterDefinitionCatalog
    {
        /// <summary>
        /// Gets all character definitions.
        /// </summary>
        /// <returns>An immutable array of all character definitions.</returns>
        ImmutableArray<ReferenceCharacterDefinition> GetAll();

        /// <summary>
        /// Gets a character definition by its unique code.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <returns>The character definition.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when no character with the specified code exists.
        /// </exception>
        ReferenceCharacterDefinition GetByCode(CharacterCode code);

        /// <summary>
        /// Determines whether the specified character code exists.
        /// </summary>
        /// <param name="code">The character code to locate.</param>
        /// <returns><see langword="true"/> if the character code exists; otherwise, <see langword="false"/>.</returns>
        bool Exists(CharacterCode code);

        /// <summary>
        /// Gets the character definition associated with the specified code.
        /// </summary>
        /// <param name="code">The character code to look up.</param>
        /// <param name="characterDefinition">When this method returns true, the character definition associated with code; otherwise, null.</param>
        /// <returns>true if a character definition with the specified code is found; otherwise, false.</returns>
        bool TryGetByCode(CharacterCode code, [NotNullWhen(true)] out ReferenceCharacterDefinition? characterDefinition);
    }
}