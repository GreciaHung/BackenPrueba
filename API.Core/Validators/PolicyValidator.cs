using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Validators
{
    
    public class PolicyValidator : AbstractValidator<Policy>
    {
        public PolicyValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Debe ingresar nombre del cliente.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Precio debe ser mayor a 0.");
            RuleFor(x => x.StartDate.Date).GreaterThanOrEqualTo(DateTimeOffset.Now.Date).WithMessage("La fecha de Inicio debe ser mayor o igual a la fecha de registro.");
            RuleFor(x => x.EndDate).LessThanOrEqualTo(x=> x.StartDate.AddYears(2)).WithMessage("El Periodo de cobertura no puede ser mayor a 2 años.");
            RuleFor(x => x.PercentageCoverage).GreaterThan(0).WithMessage("El porcentaje de cobertura debe ser superior a 0.");
            RuleFor(x => x.PercentageCoverage).LessThanOrEqualTo(50).When(x => x.TypeRisk == TypeRisk.High).WithMessage("Poliza de alto riesgo, el porcentaje de cobertura no puede ser superior al 50%.");
            RuleFor(x => x.TypeRisk).IsInEnum().WithMessage("Debe ingresar tipo de riesgo.");
            RuleFor(x => x.TypeCover).IsInEnum().WithMessage("Debe ingresar tipo de cobertura.");
        }
    }
}
