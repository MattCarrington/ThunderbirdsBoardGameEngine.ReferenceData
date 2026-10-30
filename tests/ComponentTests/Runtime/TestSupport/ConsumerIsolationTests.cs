using Microsoft.Extensions.DependencyInjection;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Enums;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime.Interfaces;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Builders;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Fixtures;
using ThunderbirdsBoardGameEngine.ReferenceData.Testing.Fakes;
using Xunit;

namespace ThunderbirdsBoardGameEngine.ReferenceData.ComponentTests.TestSupport;

public class ConsumerIsolationTests
{
    [Fact]
    public void ConsumerCanArrangeItsOwnDefinitionsWithoutLoadingProductionSnapshot()
    {
        var code = new ThunderbirdCode("consumer-test-vehicle");
        var definition = ReferenceDefinitionFactory.Thunderbird(
            code, "Consumer test vehicle", MovementDomain.Earth, 7, 9);
        var services = new ServiceCollection();
        services.AddSingleton<IThunderbirdDefinitionCatalog>(
            new FakeThunderbirdDefinitionCatalog(definition));
        using var provider = services.BuildServiceProvider();

        var catalogue = provider.GetRequiredService<IThunderbirdDefinitionCatalog>();
        var result = catalogue.GetByCode(code);

        Assert.Equal(7, result.TopSpeed);
        Assert.Equal(9, result.CharacterCapacity);
        Assert.Single(catalogue.GetAll());
        Assert.False(catalogue.TryGetByCode(new ThunderbirdCode("thunderbird-2"), out _));
    }

    [Fact]
    public void FixtureRegistrationProvidesCoherentInMemoryCatalogues()
    {
        var services = new ServiceCollection();
        services.AddFakeCatalogs();
        using var provider = services.BuildServiceProvider();

        var locations = provider.GetRequiredService<ILocationDefinitionCatalog>();
        var disasters = provider.GetRequiredService<IDisasterDefinitionCatalog>();
        var thunderbirds = provider.GetRequiredService<IThunderbirdDefinitionCatalog>();
        Assert.NotEmpty(thunderbirds.GetAll());
        foreach (var disaster in disasters.GetAll())
        {
            Assert.NotNull(locations.GetByCode(disaster.Location));
        }
    }
}