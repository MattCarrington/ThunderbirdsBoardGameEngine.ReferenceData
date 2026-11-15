using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Testing.Fakes
{
    public class FakeDisasterDefinitionCatalog : IDisasterDefinitionCatalog
    {
        private readonly FrozenDictionary<CardCode, ReferenceDisasterDefinition> _disasters;

        public FakeDisasterDefinitionCatalog(params ReferenceDisasterDefinition[] disasters)
        {
            _disasters = disasters.ToFrozenDictionary(disaster => disaster.Code);
        }

        public bool Exists(CardCode code)
        {
            return _disasters.ContainsKey(code);
        }

        public ImmutableArray<ReferenceDisasterDefinition> GetAll()
        {
            return _disasters.Values.ToImmutableArray();
        }

        public ReferenceDisasterDefinition GetByCode(CardCode code)
        {
            return _disasters[code];
        }

        public bool TryGetByCode(CardCode code, [NotNullWhen(true)] out ReferenceDisasterDefinition? disasterDefinition)
        {
            return _disasters.TryGetValue(code, out disasterDefinition);
        }
    }
}
