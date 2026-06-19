using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Application.Leituras;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnergyMeterCollector.Api.Controllers;

[ApiController]
[Route("api/medidores/{numeroSerie}/leituras")]
public class LeiturasController : ControllerBase
{
    private readonly ILeituraRepository _leituraRepository;

    public LeiturasController(ILeituraRepository leituraRepository)
    {
        _leituraRepository = leituraRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeituraDto>>> Listar(string numeroSerie, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim, CancellationToken ct)
    {
        var leituras = await _leituraRepository.ObterPorMedidorAsync(numeroSerie, inicio, fim, ct);
        var dtos = leituras.Select(l => new LeituraDto(l.Timestamp, l.TensaoVolts, l.CorrenteAmperes, l.EnergiaAtivaKwh));

        return Ok(dtos);
    }
}
