using System.Text.Json.Serialization;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Core.Model
{
    public sealed record ReferenceFabCardDefinition
    {
        public CardCode Code { get; }

        public string DisplayName { get; }

        public string EffectDescription { get; }

        public string? InstructionalText { get; }

        [JsonConstructor]
        internal ReferenceFabCardDefinition(
            CardCode code,
            string displayName,
            string effectDescription,
            string? instructionalText = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
            ArgumentException.ThrowIfNullOrWhiteSpace(effectDescription);

            Code = code;
            DisplayName = displayName;
            EffectDescription = effectDescription;
            InstructionalText = instructionalText;
        }
    }
}
