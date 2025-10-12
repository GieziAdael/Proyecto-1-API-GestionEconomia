using System.Security.Cryptography;
using System.Text;

namespace API_GestionEconomia.Services
{
    public class CreadorCodigos
    {
        public static string GenerarCodigoAleatorio(int longitud = 16)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            byte[] bytesAleatorios = new byte[longitud];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytesAleatorios);
            }

            var sb = new StringBuilder(longitud);
            foreach (byte b in bytesAleatorios)
            {
                sb.Append(caracteres[b % caracteres.Length]);
            }

            return sb.ToString();
        }

    }
}
