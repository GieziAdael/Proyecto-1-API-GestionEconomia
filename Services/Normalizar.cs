using System.Text;

namespace API_GestionEconomia.Services
{
    public class Normalizar
    {
        public static string NormalizarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.");

            string normalizado = email.Trim().ToLowerInvariant();
            normalizado = normalizado.Normalize(NormalizationForm.FormC);

            return normalizado;
        }
    }
}
