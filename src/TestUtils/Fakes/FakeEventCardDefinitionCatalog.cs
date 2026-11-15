using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Testing.Fakes
{
    public class FakeEventCardDefinitionCatalog : IEventCardDefinitionCatalog
    {
        private readonly FrozenDictionary<CardCode, ReferenceEventCardDefinition> _eventCards;

        public FakeEventCardDefinitionCatalog(params ReferenceEventCardDefinition[] eventCards)
        {
            _eventCards = eventCards.ToFrozenDictionary(eventCard => eventCard.Code);
        }

        public bool Exists(CardCode cardCode)
        {
            return _eventCards.ContainsKey(cardCode);
        }

        public ImmutableArray<ReferenceEventCardDefinition> GetAll()
        {
            return _eventCards.Values.ToImmutableArray();
        }

        public ReferenceEventCardDefinition GetByCode(CardCode cardCode)
        {
            if (!_eventCards.TryGetValue(cardCode, out var eventCardDefinition))
            {
                throw new KeyNotFoundException($"Event card definition with code '{cardCode}' not found.");
            }

            return eventCardDefinition;
        }

        public bool TryGetByCode(CardCode cardCode, [NotNullWhen(true)] out ReferenceEventCardDefinition? eventCardDefinition)
        {
            return _eventCards.TryGetValue(cardCode, out eventCardDefinition);
        }
    }
}
