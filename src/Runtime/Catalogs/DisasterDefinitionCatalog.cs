using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Catalogs
{
    internal class DisasterDefinitionCatalog : IDisasterDefinitionCatalog
    {
        private readonly FrozenDictionary<CardCode, ReferenceDisasterDefinition> _byCode;

        private readonly ImmutableArray<ReferenceDisasterDefinition> _disasters;

        public DisasterDefinitionCatalog(ReferenceDataSnapshot snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            _byCode = snapshot.DisasterDefinitions
                .ToDictionary(d => d.Code)
                .ToFrozenDictionary();

            _disasters = snapshot.DisasterDefinitions.ToImmutableArray();
        }

        public bool Exists(CardCode code)
        {
            return _byCode.ContainsKey(code);
        }

        public ImmutableArray<ReferenceDisasterDefinition> GetAll()
        {
            return _disasters;
        }

        public ReferenceDisasterDefinition GetByCode(CardCode code)
        {
            if (!_byCode.TryGetValue(code, out var disaster))
            {
                throw new KeyNotFoundException($"No disaster found with code '{code}'.");
            }

            return disaster;
        }

        public bool TryGetByCode(CardCode code, [NotNullWhen(true)] out ReferenceDisasterDefinition? disasterDefinition)
        {
            return _byCode.TryGetValue(code, out disasterDefinition);
        }
    }
}
