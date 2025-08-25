using System.ComponentModel.DataAnnotations;

namespace AdeauMao.Application.DTOs
{
    public class EquipementDto
    {
        public int Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string? TypeEquipement { get; set; }
        public string? Fabricant { get; set; }
        public string? Modele { get; set; }
        public DateTime? DateMiseEnService { get; set; }
        public string? Localisation { get; set; }
        public int? LigneProductionId { get; set; }
        public string? LigneProductionNom { get; set; }
        public string? Description { get; set; }
        public string? EtatOperationnel { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public ICollection<OrganeDto>? Organes { get; set; }
    }

    public class CreateEquipementDto
    {
        [Required(ErrorMessage = "La référence est requise")]
        [StringLength(50, ErrorMessage = "La référence ne peut pas dépasser 50 caractères")]
        public string Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(255, ErrorMessage = "Le nom ne peut pas dépasser 255 caractères")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Le type d'équipement ne peut pas dépasser 100 caractères")]
        public string? TypeEquipement { get; set; }

        [StringLength(100, ErrorMessage = "Le fabricant ne peut pas dépasser 100 caractères")]
        public string? Fabricant { get; set; }

        [StringLength(100, ErrorMessage = "Le modèle ne peut pas dépasser 100 caractères")]
        public string? Modele { get; set; }

        public DateTime? DateMiseEnService { get; set; }

        [StringLength(255, ErrorMessage = "La localisation ne peut pas dépasser 255 caractères")]
        public string? Localisation { get; set; }

        public int? LigneProductionId { get; set; }

        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "L'état opérationnel ne peut pas dépasser 50 caractères")]
        public string? EtatOperationnel { get; set; }
    }

    public class UpdateEquipementDto
    {
        [Required(ErrorMessage = "L'ID est requis")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La référence est requise")]
        [StringLength(50, ErrorMessage = "La référence ne peut pas dépasser 50 caractères")]
        public string Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(255, ErrorMessage = "Le nom ne peut pas dépasser 255 caractères")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Le type d'équipement ne peut pas dépasser 100 caractères")]
        public string? TypeEquipement { get; set; }

        [StringLength(100, ErrorMessage = "Le fabricant ne peut pas dépasser 100 caractères")]
        public string? Fabricant { get; set; }

        [StringLength(100, ErrorMessage = "Le modèle ne peut pas dépasser 100 caractères")]
        public string? Modele { get; set; }

        public DateTime? DateMiseEnService { get; set; }

        [StringLength(255, ErrorMessage = "La localisation ne peut pas dépasser 255 caractères")]
        public string? Localisation { get; set; }

        public int? LigneProductionId { get; set; }

        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "L'état opérationnel ne peut pas dépasser 50 caractères")]
        public string? EtatOperationnel { get; set; }
    }

    public class OrganeDto
    {
        public int Id { get; set; }
        public string NomOrgane { get; set; } = string.Empty;
        public int EquipementId { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreation { get; set; }
    }

    public class CreateOrganeDto
    {
        [Required(ErrorMessage = "Le nom de l'organe est requis")]
        [StringLength(100, ErrorMessage = "Le nom de l'organe ne peut pas dépasser 100 caractères")]
        public string NomOrgane { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID de l'équipement est requis")]
        public int EquipementId { get; set; }

        public string? Description { get; set; }
    }
}

