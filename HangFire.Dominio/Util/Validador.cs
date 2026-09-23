using System.ComponentModel.DataAnnotations;

namespace HangFire.Api.Util
{
    public class Validador
    {
        //public string Validar(object entidade)
        //{
        //    var context = new ValidationContext(entidade, serviceProvider: null, items: null);
        //    var results = new List<ValidationResult>();

        //    bool isValid = Validator.TryValidateObject(entidade, context, results, true);

        //    if (!isValid)
        //    {
        //        return string.Join("; ", results.Select(r => r.ErrorMessage));
        //    }

        //    return string.Empty;
        //}

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
