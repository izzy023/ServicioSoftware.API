using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ServicioSoftware.API.Configuration;
using ServicioSoftware.API.Interfaces;
using ServicioSoftware.API.Models;

namespace ServicioSoftware.API.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task EnviarCorreoAsync(SolicitudCotizacion solicitud, byte[] pdf)
    {
        var mensaje = new MimeMessage();

        mensaje.From.Add(new MailboxAddress("Servicio de Software Gervacio", _settings.Email));

        mensaje.To.Add(MailboxAddress.Parse(_settings.Recipient));

        mensaje.Subject = "Nueva Solicitud de Cotización";

        var builder = new BodyBuilder();

        builder.TextBody =
$@"Se ha recibido una nueva solicitud.

Nombre: {solicitud.Nombre}

Correo: {solicitud.Correo}

Teléfono: {solicitud.Telefono}

Empresa: {solicitud.Empresa}

Servicio: {solicitud.Servicio}

Presupuesto: {solicitud.Presupuesto}

Inicio: {solicitud.Inicio}

Descripción:

{solicitud.Descripcion}";

        builder.Attachments.Add("SolicitudCotizacion.pdf", pdf, new ContentType("application", "pdf"));

        mensaje.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _settings.Host,
            _settings.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _settings.Email,
            _settings.Password);

        await smtp.SendAsync(mensaje);

        await smtp.DisconnectAsync(true);
    }
}