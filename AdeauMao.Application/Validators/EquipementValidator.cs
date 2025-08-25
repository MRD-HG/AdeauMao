using FluentValidation;
using AdeauMao.Application.DTOs;

namespace AdeauMao.Application.Validators
{
    public class CreateEquipementDtoValidator : AbstractValidator<CreateEquipementDto>
    {
        public CreateEquipementDtoValidator()
        {
            RuleFor(x => x.Reference)
                .NotEmpty().WithMessage("La référence est requise")
                .MaximumLength(50).WithMessage("La référence ne peut pas dépasser 50 caractères")
                .Matches("^[A-Z0-9-]+$").WithMessage("La référence ne peut contenir que des lettres majuscules, des chiffres et des tirets");

            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom est requis")
                .MaximumLength(255).WithMessage("Le nom ne peut pas dépasser 255 caractères");

            RuleFor(x => x.TypeEquipement)
                .MaximumLength(100).WithMessage("Le type d'équipement ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.TypeEquipement));

            RuleFor(x => x.Fabricant)
                .MaximumLength(100).WithMessage("Le fabricant ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.Fabricant));

            RuleFor(x => x.Modele)
                .MaximumLength(100).WithMessage("Le modèle ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.Modele));

            RuleFor(x => x.DateMiseEnService)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La date de mise en service ne peut pas être dans le futur")
                .When(x => x.DateMiseEnService.HasValue);

            RuleFor(x => x.Localisation)
                .MaximumLength(255).WithMessage("La localisation ne peut pas dépasser 255 caractères")
                .When(x => !string.IsNullOrEmpty(x.Localisation));

            RuleFor(x => x.EtatOperationnel)
                .MaximumLength(50).WithMessage("L'état opérationnel ne peut pas dépasser 50 caractères")
                .When(x => !string.IsNullOrEmpty(x.EtatOperationnel));
        }
    }

    public class UpdateEquipementDtoValidator : AbstractValidator<UpdateEquipementDto>
    {
        public UpdateEquipementDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("L'ID doit être supérieur à 0");

            RuleFor(x => x.Reference)
                .NotEmpty().WithMessage("La référence est requise")
                .MaximumLength(50).WithMessage("La référence ne peut pas dépasser 50 caractères")
                .Matches("^[A-Z0-9-]+$").WithMessage("La référence ne peut contenir que des lettres majuscules, des chiffres et des tirets");

            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom est requis")
                .MaximumLength(255).WithMessage("Le nom ne peut pas dépasser 255 caractères");

            RuleFor(x => x.TypeEquipement)
                .MaximumLength(100).WithMessage("Le type d'équipement ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.TypeEquipement));

            RuleFor(x => x.Fabricant)
                .MaximumLength(100).WithMessage("Le fabricant ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.Fabricant));

            RuleFor(x => x.Modele)
                .MaximumLength(100).WithMessage("Le modèle ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.Modele));

            RuleFor(x => x.DateMiseEnService)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La date de mise en service ne peut pas être dans le futur")
                .When(x => x.DateMiseEnService.HasValue);

            RuleFor(x => x.Localisation)
                .MaximumLength(255).WithMessage("La localisation ne peut pas dépasser 255 caractères")
                .When(x => !string.IsNullOrEmpty(x.Localisation));

            RuleFor(x => x.EtatOperationnel)
                .MaximumLength(50).WithMessage("L'état opérationnel ne peut pas dépasser 50 caractères")
                .When(x => !string.IsNullOrEmpty(x.EtatOperationnel));
        }
    }

    public class CreateOrganeDtoValidator : AbstractValidator<CreateOrganeDto>
    {
        public CreateOrganeDtoValidator()
        {
            RuleFor(x => x.NomOrgane)
                .NotEmpty().WithMessage("Le nom de l'organe est requis")
                .MaximumLength(100).WithMessage("Le nom de l'organe ne peut pas dépasser 100 caractères");

            RuleFor(x => x.EquipementId)
                .GreaterThan(0).WithMessage("L'ID de l'équipement doit être supérieur à 0");
        }
    }
}

