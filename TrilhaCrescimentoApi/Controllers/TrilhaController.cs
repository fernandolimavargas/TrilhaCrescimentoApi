using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TrilhaController : Controller
{
    private readonly TrilhaService _trilhaService;

    public TrilhaController(TrilhaService trilhaService)
    {
        _trilhaService = trilhaService;
    }

    [HttpGet("times")]
    public IActionResult BuscarTimes()
    {
        var times = _trilhaService.BuscarTimes();
        return Ok(times);
    }

    [HttpGet("passos")]
    public IActionResult BuscarPassos()
    {
        var passos = _trilhaService.BuscarPassos();
        return Ok(passos);
    }

    [HttpPost("checkin")]
    public IActionResult Checkin([FromBody] CheckinRequest request)
    {
        if (request == null)
            return BadRequest("Dados do check-in não informados.");

        try
        {
            var resultado = _trilhaService.Checkin(
                request.IdTime,
                request.IdUsuario,
                request.IdPasso
            );

            if (resultado == false)
            {
                return BadRequest(
                    "Você já fez o check-in para este passo hoje."
                );
            }

            return Ok("Check-in realizado com sucesso.");
        }
        catch (Exception ex)
        {
            // Registre o erro completo no log do servidor.
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("verificarCheckin/{idUsuario:int}")] 
    public IActionResult VerificarCheckin([FromRoute] CheckinRequest request)
    {
        var jaFezCheckin = _trilhaService.VerificarCheckin(request.IdUsuario, request.IdPasso);
        return Ok(jaFezCheckin);
    }
}