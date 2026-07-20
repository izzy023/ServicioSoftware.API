using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ServicioSoftware.API.Models;
using System.IO;

namespace ServicioSoftware.API.Documents;

public class CotizacionPdfDocument : IDocument
{
    private readonly SolicitudCotizacion solicitud;

    public CotizacionPdfDocument(SolicitudCotizacion solicitud)
    {
        this.solicitud = solicitud;
    }

    // Colores institucionales
    private const string Azul = "#2563EB";
    private const string AzulOscuro = "#1E3A8A";
    private const string Gris = "#F1F5F9";
    private const string Texto = "#334155";

    public DocumentMetadata GetMetadata()
    {
        return DocumentMetadata.Default;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);

            page.Margin(30);

            page.PageColor(Colors.White);

            page.DefaultTextStyle(x => x.FontSize(11));

            page.Header().Element(Encabezado);

            page.Content().Element(Contenido);

            page.Footer().Element(PiePagina);
        });
    }

    private void Encabezado(IContainer container)
    {
        var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Logo Institucional.png");

        container
            .Background(Azul)
            .Padding(20)
            .Row(row =>
            {
                // LOGO
                row.ConstantItem(90)
                    .Height(90)
                    .AlignMiddle()
                    .Element(c =>
                    {
                        if (File.Exists(logoPath))
                            c.Image(logoPath);
                    });

                // TEXTO
                row.RelativeItem()
                    .AlignMiddle()
                    .Column(column =>
                    {
                        column.Item()
                            .AlignCenter()
                            .Text("SERVICIO DE SOFTWARE GERVACIO")
                            .FontColor(Colors.White)
                            .Bold()
                            .FontSize(24);

                        column.Item()
                            .AlignCenter()
                            .Text("Solicitud de Cotización")
                            .FontColor(Colors.White)
                            .FontSize(14);

                        column.Item()
                            .PaddingTop(8)
                            .AlignCenter()
                            .Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                            .FontColor(Colors.White)
                            .FontSize(10);
                    });
            });
    }

    private void Contenido(IContainer container)
    {
        container.PaddingVertical(20)
            .Column(column =>
            {
                column.Spacing(20);

                // Cliente
                column.Item().Element(c =>
                {
                    Tarjeta(c, "DATOS DEL CLIENTE", col =>
                    {
                        col.Item().Text($"Nombre: {solicitud.Nombre}");
                        col.Item().Text($"Correo: {solicitud.Correo}");
                        col.Item().Text($"Teléfono: {solicitud.Telefono}");
                        col.Item().Text($"Empresa: {solicitud.Empresa}");
                    });
                });

                // Proyecto
                column.Item().Element(c =>
                {
                    Tarjeta(c, "INFORMACIÓN DEL PROYECTO", col =>
                    {
                        col.Item().Text($"Servicio: {solicitud.Servicio}");
                        col.Item().Text($"Presupuesto: {solicitud.Presupuesto}");
                        col.Item().Text($"Inicio: {solicitud.Inicio}");
                    });
                });

                // Descripción
                column.Item().Element(c =>
                {
                    Tarjeta(c, "DESCRIPCIÓN DEL PROYECTO", col =>
                    {
                        col.Item().Text(solicitud.Descripcion);
                    });
                });
            });
    }

    private void PiePagina(IContainer container)
    {
        container
            .BorderTop(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingTop(10)
            .Row(row =>
            {
                row.RelativeItem()
                    .Text("Servicio de Software Gervacio")
                    .FontSize(10);

                row.ConstantItem(120)
                    .AlignRight()
                    .Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
            });
    }

    private void Tarjeta(IContainer container, string titulo, Action<ColumnDescriptor> contenido)
    {
        container
            .Border(1)
            .BorderColor("#D6E4F0")
            .Background("#F8FAFC")
            .Padding(20)
            .Column(column =>
            {
                column.Item()
                    .Text(titulo)
                    .Bold()
                    .FontSize(16)
                    .FontColor(Azul);

                column.Item().PaddingTop(10);

                contenido(column);
            });
    }

}