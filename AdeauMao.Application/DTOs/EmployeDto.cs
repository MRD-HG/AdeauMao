using System.ComponentModel.DataAnnotations;

namespace AdeauMao.Application.DTOs
{
    public class EmployeDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? RoleInterne { get; set; }
        public int? UtilisateurId { get; set; }
        public string? UtilisateurNom { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public ICollection<CompetenceDto>? Competences { get; set; }
        public ICollection<EquipeDto>? Equipes { get; set; }
    }

    public class CreateEmployeDto
    {
        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
        public string Prenom { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Le contact ne peut pas dépasser 100 caractères")]
        public string? Contact { get; set; }

        [StringLength(50, ErrorMessage = "Le rôle interne ne peut pas dépasser 50 caractères")]
        public string? RoleInterne { get; set; }

        public int? UtilisateurId { get; set; }
    }

    public class UpdateEmployeDto
    {
        [Required(ErrorMessage = "L'ID est requis")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
        public string Prenom { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Le contact ne peut pas dépasser 100 caractères")]
        public string? Contact { get; set; }

        [StringLength(50, ErrorMessage = "Le rôle interne ne peut pas dépasser 50 caractères")]
        public string? RoleInterne { get; set; }

        public int? UtilisateurId { get; set; }
    }

    public class CompetenceDto
    {
        public int Id { get; set; }
        public string NomCompetence { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateCreation { get; set; }
    }

    public class CreateCompetenceDto
    {
        [Required(ErrorMessage = "Le nom de la compétence est requis")]
        [StringLength(100, ErrorMessage = "Le nom de la compétence ne peut pas dépasser 100 caractères")]
        public string NomCompetence { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    public class EquipeDto
    {
        public int Id { get; set; }
        public string NomEquipe { get; set; } = string.Empty;
        public int ResponsableId { get; set; }
        public string? ResponsableNom { get; set; }
        public DateTime DateCreation { get; set; }
        public ICollection<EmployeDto>? Membres { get; set; }
    }

    public class CreateEquipeDto
    {
        [Required(ErrorMessage = "Le nom de l'équipe est requis")]
        [StringLength(100, ErrorMessage = "Le nom de l'équipe ne peut pas dépasser 100 caractères")]
        public string NomEquipe { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID du responsable est requis")]
        public int ResponsableId { get; set; }
    }

    public class AssignEmployeToEquipeDto
    {
        [Required(ErrorMessage = "L'ID de l'équipe est requis")]
        public int EquipeId { get; set; }

        [Required(ErrorMessage = "L'ID de l'employé est requis")]
        public int EmployeId { get; set; }
    }

    public class AssignCompetenceToEmployeDto
    {
        [Required(ErrorMessage = "L'ID de l'employé est requis")]
        public int EmployeId { get; set; }

        [Required(ErrorMessage = "L'ID de la compétence est requis")]
        public int CompetenceId { get; set; }
    }
}

