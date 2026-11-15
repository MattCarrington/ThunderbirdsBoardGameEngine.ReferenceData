using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Catalogs
{
    internal sealed class FabCardDefinitionCatalog : IFabCardDefinitionCatalog
    {
        private readonly FrozenDictionary<CardCode, ReferenceFabCardDefinition> _byCode;

        private readonly ImmutableArray<ReferenceFabCardDefinition> _fabCards;

        public FabCardDefinitionCatalog(ReferenceDataSnapshot snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            _byCode = snapshot.FabCardDefinitions
                .ToDictionary(c => c.Code)
                .ToFrozenDictionary();

            _fabCards = snapshot.FabCardDefinitions.ToImmutableArray();
        }

        public bool Exists(CardCode cardCode)
        {
            return _byCode.ContainsKey(cardCode);
        }

        public ImmutableArray<ReferenceFabCardDefinition> GetAll()
        {
            return _fabCards;
        }

        public ReferenceFabCardDefinition GetByCode(CardCode cardCode)
        {
            if (!_byCode.TryGetValue(cardCode, out var fabCardDefinition))
            {
                throw new KeyNotFoundException($"FAB card definition with code '{cardCode}' not found.");
            }

            return fabCardDefinition;
        }

        public bool TryGetByCode(CardCode cardCode, [NotNullWhen(true)] out ReferenceFabCardDefinition? cardDefinition)
        {
            return _byCode.TryGetValue(cardCode, out cardDefinition);
        }
    }
}
