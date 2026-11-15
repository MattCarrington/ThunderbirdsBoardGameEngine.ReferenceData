using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Testing.Fakes
{
    public class FakeFabCardDefinitionCatalog : IFabCardDefinitionCatalog
    {
        private readonly FrozenDictionary<CardCode, ReferenceFabCardDefinition> _fabCards;

        public FakeFabCardDefinitionCatalog(params ReferenceFabCardDefinition[] fabCards)
        {
            _fabCards = fabCards.ToFrozenDictionary(fabCard => fabCard.Code);
        }

        public bool Exists(CardCode cardCode)
        {
            return _fabCards.ContainsKey(cardCode);
        }

        public ImmutableArray<ReferenceFabCardDefinition> GetAll()
        {
            return _fabCards.Values.ToImmutableArray();
        }

        public ReferenceFabCardDefinition GetByCode(CardCode cardCode)
        {
            return _fabCards[cardCode];
        }

        public bool TryGetByCode(CardCode cardCode, [NotNullWhen(true)] out ReferenceFabCardDefinition? cardDefinition)
        {
            return _fabCards.TryGetValue(cardCode, out cardDefinition);
        }
    }
}
