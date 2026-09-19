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
        try
        {
            var resultado = _trilhaService.Checkin(request.IdTime, request.IdUsuario, request.IdPasso);
            if (!resultado)
            {
                return BadRequest("Você já fez o check-in para este time hoje.");
            }
            return Ok("Check-in realizado com sucesso.");
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("verificarCheckin/{idUsuario:int}")] 
    public IActionResult VerificarCheckin([FromRoute] int idUsuario)
    {
        var jaFezCheckin = _trilhaService.VerificarCheckin(idUsuario);
        return Ok(jaFezCheckin);
    }
}