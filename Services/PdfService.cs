using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ServicioSoftware.API.Documents;
using ServicioSoftware.API.Interfaces;
using ServicioSoftware.API.Models;

namespace ServicioSoftware.API.Services;

public class PdfService : IPdfService
{
    public byte[] GenerarPdf(SolicitudCotizacion solicitud)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var documento = new CotizacionPdfDocument(solicitud);

        return documento.GeneratePdf();
    }
}