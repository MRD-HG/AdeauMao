using FluentValidation;
using AdeauMao.Application.DTOs.Auth;

namespace AdeauMao.Application.Validators
{
    public class UtilisateurValidator : AbstractValidator<RegisterDto>
    {
        public UtilisateurValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Le nom d'utilisateur est requis")
                .Length(3, 50).WithMessage("Le nom d'utilisateur doit contenir entre 3 et 50 caractères")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Le nom d'utilisateur ne peut contenir que des lettres, des chiffres et des underscores");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email est requis")
                .EmailAddress().WithMessage("Format d'email invalide");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Le mot de passe est requis")
                .MinimumLength(6).WithMessage("Le mot de passe doit contenir au moins 6 caractères")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("Le mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule et un chiffre");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("La confirmation du mot de passe est requise")
                .Equal(x => x.Password).WithMessage("Les mots de passe ne correspondent pas");

            RuleFor(x => x.FirstName)
                .MaximumLength(100).WithMessage("Le prénom ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères")
                .When(x => !string.IsNullOrEmpty(x.LastName));
        }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Le nom d'utilisateur est requis");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Le mot de passe est requis");
        }
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("L'ID utilisateur est requis");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Le mot de passe actuel est requis");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Le nouveau mot de passe est requis")
                .MinimumLength(6).WithMessage("Le nouveau mot de passe doit contenir au moins 6 caractères")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("Le nouveau mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule et un chiffre");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("La confirmation du nouveau mot de passe est requise")
                .Equal(x => x.NewPassword).WithMessage("Les mots de passe ne correspondent pas");
        }
    }

    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email est requis")
                .EmailAddress().WithMessage("Format d'email invalide");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Le token est requis");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Le nouveau mot de passe est requis")
                .MinimumLength(6).WithMessage("Le nouveau mot de passe doit contenir au moins 6 caractères")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("Le nouveau mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule et un chiffre");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("La confirmation du nouveau mot de passe est requise")
                .Equal(x => x.NewPassword).WithMessage("Les mots de passe ne correspondent pas");
        }
    }
}

