using ServicioSoftware.API.Models;

namespace ServicioSoftware.API.Interfaces;

public interface IPdfService
{
    byte[] GenerarPdf(SolicitudCotizacion solicitud);
}