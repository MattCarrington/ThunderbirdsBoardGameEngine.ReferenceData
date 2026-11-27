using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.KnownIdentities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Fakes;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Testing.TestData
{
    public class EventCardsTestData
    {
        public static FakeEventCardDefinitionCatalog CreateEventCardsCatalog()
        {
            var theHoodInterferes = new ReferenceEventCardDefinition(
                code: KnownEventCardCodes.TheHoodInterferes,
                displayName: "The Hood Interferes",
                effectDescription: "Put this under the left-most disaster card on the disaster track that requires a dice roll to avert. Subtract 2 from all dice roll totals for this disaster.");

            var attackOfTheZombites = new ReferenceEventCardDefinition(
                code: KnownEventCardCodes.AttackOfTheZombites,
                displayName: "Attack of the Zombites",
                effectDescription: "Thunderbird 1 has a top speed of 1 until a character in its space discards a Technology token.");

            var usnSentinelMissileStrike = new ReferenceEventCardDefinition(
                code: KnownEventCardCodes.UsnSentinelMissileStrike,
                displayName: "USN Sentinel Missile Strike",
                effectDescription: "Thunderbird 2 has a top speed of 1 until a character in its space discards a Technology token.");

            var rocketMalfunction = new ReferenceEventCardDefinition(
                code: KnownEventCardCodes.RocketMalfunction,
                displayName: "Rocket Malfunction",
                effectDescription: "Thunderbird 3 has a top speed of 1 until a character in its space discards a Technology token.");

            var icelandicVolcanoEruption = new ReferenceEventCardDefinition(
                code: KnownEventCardCodes.IcelandicVolcanoEruption,
                displayName: "Icelandic Volcano Eruption",
                effectDescription: "Move actions cannot be made across the edges of the game board until a player in the North Atlantic discards a Logistics token.");

            var explosionOnTracyIsland = new ReferenceEventCardDefinition(
                code: new CardCode("explosion-on-tracy-island"),
                displayName: "Explosion on Tracy Island",
                effectDescription: "Put 1 pod vehicle from Tracy Island (if there is one available) back on to Brains' Notebook.");

            return new FakeEventCardDefinitionCatalog(theHoodInterferes, attackOfTheZombites, usnSentinelMissileStrike, rocketMalfunction, icelandicVolcanoEruption, explosionOnTracyIsland);
        }
    }
}
