using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Catalogs
{
    internal sealed class EventCardDefinitionCatalog : IEventCardDefinitionCatalog
    {
        private readonly FrozenDictionary<CardCode, ReferenceEventCardDefinition> _byCode;

        private readonly ImmutableArray<ReferenceEventCardDefinition> _eventCards;

        public EventCardDefinitionCatalog(ReferenceDataSnapshot snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            _byCode = snapshot.EventCardDefinitions
                .ToDictionary(c => c.Code)
                .ToFrozenDictionary();

            _eventCards = snapshot.EventCardDefinitions.ToImmutableArray();
        }

        public bool Exists(CardCode cardCode)
        {
            return _byCode.ContainsKey(cardCode);
        }

        public ImmutableArray<ReferenceEventCardDefinition> GetAll()
        {
            return _eventCards;
        }

        public ReferenceEventCardDefinition GetByCode(CardCode cardCode)
        {
            if (!_byCode.TryGetValue(cardCode, out var eventCardDefinition))
            {
                throw new KeyNotFoundException($"Event card definition with code '{cardCode}' not found.");
            }

            return eventCardDefinition;
        }

        public bool TryGetByCode(CardCode cardCode, [NotNullWhen(true)] out ReferenceEventCardDefinition? eventCardDefinition)
        {
            return _byCode.TryGetValue(cardCode, out eventCardDefinition);
        }
    }
}
