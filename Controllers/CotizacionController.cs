using Microsoft.AspNetCore.Mvc;
using ServicioSoftware.API.Documents;
using ServicioSoftware.API.Interfaces;
using ServicioSoftware.API.Models;

namespace ServicioSoftware.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CotizacionController : ControllerBase
{
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    public CotizacionController(
        IPdfService pdfService,
        IEmailService emailService)
    {
        _pdfService = pdfService;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> EnviarSolicitud(
        [FromBody] SolicitudCotizacion solicitud)
    {
        var pdf = _pdfService.GenerarPdf(solicitud);

        await _emailService.EnviarCorreoAsync(solicitud, pdf);

        return Ok(new
        {
            mensaje = "La cotización fue enviada correctamente."
        });
    }
}