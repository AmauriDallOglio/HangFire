using System.ComponentModel.DataAnnotations;

namespace HangFire.Api.Util
{
    public class Validador
    {
        public string Validar(object objeto)
        {
            string erros = string.Empty;
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(objeto);

            if (!Validator.TryValidateObject(objeto, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    Console.WriteLine($"Erro de validação: {validationResult.ErrorMessage}");
                    erros += $" {validationResult.ErrorMessage}";
                }
                if (!string.IsNullOrEmpty(erros))
                {
                    throw new InvalidOperationException(erros);
                }
            }

            return erros;
        }
    }
}
