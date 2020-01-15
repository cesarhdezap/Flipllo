using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using LogicaDeNegocios.ClasesDeDominio;

namespace LogicaDeNegocios.Servicios
{
    public static class ServiciosDeEnvioDeCorreos
    {
        private static readonly MailAddress correoDeFlipllo = new MailAddress("fliplloservice.noreply@gmail.com");
        private static readonly string contraseñaDeFlipllo = "o1R3EAUD3mP3CsWxngvZ";
        private static readonly string hostDeGmail = "smtp.gmail.com";
        private static readonly int puertoDeCorreo = 587;

        public static void EnviarCorreoDeVerficiacion(Usuario usuario)
        {
            MailAddress destinatario = new MailAddress(usuario.CorreoElectronico);
            string asunto = "Bienvenido a flipllo!";
            string cuerpo = "<h1>Flipllo</h1><h2 style = \"color: #2e6c80;\"> &iexcl;{nombreDelUsuario}, tu cuenta de flipllo esta casi lista!</h2><p><strong> Tu codigo de verificación es:</strong></p><h2 style = \"color: #ff0000;\">{codigo}</h2><footer><p><strong>Si no fuiste tu quien solicito este correo, solo ignoralo.</strong></p></footer>";


            using (SmtpClient clienteSMTP = new SmtpClient
            {
                Host = hostDeGmail,
                Port = puertoDeCorreo,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(correoDeFlipllo.Address, contraseñaDeFlipllo),
                Timeout = 5000
            })
            {
                MailDefinition definicionDeCorreo = new MailDefinition()
                {
                    From = correoDeFlipllo.Address,
                    IsBodyHtml = true,
                    Subject = asunto,
                };

                ListDictionary reemplazos = new ListDictionary
                {
                    { "{nombreDelUsuario}", usuario.NombreDeUsuario },
                    { "{codigo}", usuario.CodigoDeVerificacion }
                };
                using (MailMessage mensaje = definicionDeCorreo.CreateMailMessage(destinatario.Address, reemplazos, cuerpo, new System.Web.UI.Control()))
                {
                    clienteSMTP.Send(mensaje);
                }
            }
        }
    }
}
