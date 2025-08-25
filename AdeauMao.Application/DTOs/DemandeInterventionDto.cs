using System.ComponentModel.DataAnnotations;
using AdeauMao.Core.Entities;

namespace AdeauMao.Application.DTOs
{
    public class DemandeInterventionDto
    {
        public int Id { get; set; }
        public int EquipementId { get; set; }
        public string? EquipementNom { get; set; }
        public string? EquipementReference { get; set; }
        public string DescriptionProbleme { get; set; } = string.Empty;
        public DateTime DateDemande { get; set; }
        public int DemandeurId { get; set; }
        public string? DemandeurNom { get; set; }
        public StatutDemande Statut { get; set; }
        public Priorite Priorite { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public ICollection<OrdresDeTravailDto>? OrdresDeTravail { get; set; }
    }

    public class CreateDemandeInterventionDto
    {
        [Required(ErrorMessage = "L'ID de l'équipement est requis")]
        public int EquipementId { get; set; }

        [Required(ErrorMessage = "La description du problème est requise")]
        public string DescriptionProbleme { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID du demandeur est requis")]
        public int DemandeurId { get; set; }

        [Required(ErrorMessage = "La priorité est requise")]
        public Priorite Priorite { get; set; } = Priorite.Moyenne;
    }

    public class UpdateDemandeInterventionDto
    {
        [Required(ErrorMessage = "L'ID est requis")]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'ID de l'équipement est requis")]
        public int EquipementId { get; set; }

        [Required(ErrorMessage = "La description du problème est requise")]
        public string DescriptionProbleme { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID du demandeur est requis")]
        public int DemandeurId { get; set; }

        [Required(ErrorMessage = "Le statut est requis")]
        public StatutDemande Statut { get; set; }

        [Required(ErrorMessage = "La priorité est requise")]
        public Priorite Priorite { get; set; }
    }

    public class UpdateStatutDemandeDto
    {
        [Required(ErrorMessage = "L'ID est requis")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le statut est requis")]
        public StatutDemande Statut { get; set; }

        public string? Commentaires { get; set; }
    }
}

