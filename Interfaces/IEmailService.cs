using ServicioSoftware.API.Models;

namespace ServicioSoftware.API.Interfaces;

public interface IEmailService
{
    Task EnviarCorreoAsync(SolicitudCotizacion solicitud, byte[] pdf);
}