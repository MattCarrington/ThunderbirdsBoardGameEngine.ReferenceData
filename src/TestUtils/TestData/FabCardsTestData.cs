using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.KnownIdentities;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Model;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Fakes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Testing.TestData
{
    public class FabCardsTestData
    {
        public static FakeFabCardDefinitionCatalog CreateFabCardsCatalog()
        {
            var underwaterSealingUnit = new ReferenceFabCardDefinition(
                code: KnownFabCardCodes.UnderwaterSealingUnit,
                displayName: "Underwater Sealing Unit",
                effectDescription: "Add +3 to any Sea Rescue.",
                instructionalText: "Play before someone rolls for a rescue."
            );

            var astronautSpacewalk = new ReferenceFabCardDefinition(
                code: KnownFabCardCodes.AstronautSpacewalk,
                displayName: "Astronaut Spacewalk",
                effectDescription: "Add + 3 to any Space Rescue.",
                instructionalText: "Play before someone rolls for a rescue."
            );

            var personalHoverjet = new ReferenceFabCardDefinition(
                code: KnownFabCardCodes.PersonalHoverjet,
                displayName: "Personal Hoverjet",
                effectDescription: "Add +3 to any Land Rescue.",
                instructionalText: "Play before someone rolls for a rescue."
            );

            var remoteControlHoverCamera = new ReferenceFabCardDefinition(
                code: KnownFabCardCodes.RemoteControlHoverCamera,
                displayName: "Remote Control Hover Camera",
                effectDescription: "Add +3 to any Air Rescue.",
                instructionalText: "Play before someone rolls for a rescue."
            );

            var jeffsOrders = new ReferenceFabCardDefinition(
                code: new CardCode("jeff-s-orders"),
                displayName: "Jeff's Orders",
                effectDescription: "Move Thunderbird 1 or Thunderbird 2 or Thunderbird 4 to any space on Earth."
            );

            return new FakeFabCardDefinitionCatalog(underwaterSealingUnit, astronautSpacewalk, personalHoverjet, remoteControlHoverCamera, jeffsOrders);
        }
    }
}
