using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EnergyMeterCollector.Tests.Integration;

public class LeiturasApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public LeiturasApiTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Get_leituras_de_medidor_inexistente_Retorna_lista_vazia()
    {
        var resp = await _client.GetAsync("/api/medidores/NAO-EXISTE/leituras");
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadAsStringAsync();
        json.Should().Be("[]");
    }
}
