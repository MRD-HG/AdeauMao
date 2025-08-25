using System.ComponentModel.DataAnnotations;
using AdeauMao.Core.Entities;

namespace AdeauMao.Application.DTOs
{
    public class OrdresDeTravailDto
    {
        public int Id { get; set; }
        public string NumeroOT { get; set; } = string.Empty;
        public int EquipementId { get; set; }
        public string? EquipementNom { get; set; }
        public string? EquipementReference { get; set; }
        public string DescriptionTache { get; set; } = string.Empty;
        public DateTime? DateDebutPrevue { get; set; }
        public DateTime? DateFinPrevue { get; set; }
        public int? TechnicienAssigneeId { get; set; }
        public string? TechnicienNom { get; set; }
        public StatutOT Statut { get; set; }
        public Priorite Priorite { get; set; }
        public DateTime? DateDebutReelle { get; set; }
        public DateTime? DateFinReelle { get; set; }
        public decimal? TempsPasse { get; set; }
        public int? CausePanneId { get; set; }
        public string? CausePanneNom { get; set; }
        public string? SolutionApportee { get; set; }
        public string? Remarques { get; set; }
        public int? DemandeInterventionId { get; set; }
        public TypeMaintenance TypeMaintenance { get; set; }
        public int? OrganeId { get; set; }
        public string? OrganeNom { get; set; }
        public decimal? CoutReel { get; set; }
        public int PourcentageProgression { get; set; }
        public int? SousTraitantId { get; set; }
        public string? SousTraitantNom { get; set; }
        public DateTime? DateValidation { get; set; }
        public int? ValidateurId { get; set; }
        public string? ValidateurNom { get; set; }
        public int? WorkflowId { get; set; }
        public string? WorkflowNom { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
    }

    public class CreateOrdresDeTravailDto
    {
        [Required(ErrorMessage = "Le numéro OT est requis")]
        [StringLength(50, ErrorMessage = "Le numéro OT ne peut pas dépasser 50 caractères")]
        public string NumeroOT { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID de l'équipement est requis")]
        public int EquipementId { get; set; }

        [Required(ErrorMessage = "La description de la tâche est requise")]
        public string DescriptionTache { get; set; } = string.Empty;

        public DateTime? DateDebutPrevue { get; set; }

        public DateTime? DateFinPrevue { get; set; }

        public int? TechnicienAssigneeId { get; set; }

        [Required(ErrorMessage = "Le statut est requis")]
        public StatutOT Statut { get; set; } = StatutOT.AFaire;

        [Required(ErrorMessage = "La priorité est requise")]
        public Priorite Priorite { get; set; } = Priorite.Moyenne;

        public int? CausePanneId { get; set; }

        public int? DemandeInterventionId { get; set; }

        [Required(ErrorMessage = "Le type de maintenance est requis")]
        public TypeMaintenance TypeMaintenance { get; set; } = TypeMaintenance.Corrective;

        public int? OrganeId { get; set; }

        public int? SousTraitantId { get; set; }

        public int? WorkflowId { get; set; }
    }

    public class UpdateOrdresDeTravailDto
    {
        [Required(ErrorMessage = "L'ID est requis")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le numéro OT est requis")]
        [StringLength(50, ErrorMessage = "Le numéro OT ne peut pas dépasser 50 caractères")]
        public string NumeroOT { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID de l'équipement est requis")]
        public int EquipementId { get; set; }

        [Required(ErrorMessage = "La description de la tâche est requise")]
        public string DescriptionTache { get; set; } = string.Empty;

        public DateTime? DateDebutPrevue { get; set; }

        public DateTime? DateFinPrevue { get; set; }

        public int? TechnicienAssigneeId { get; set; }

        [Required(ErrorMessage = "Le statut est requis")]
        public StatutOT Statut { get; set; }

        [Required(ErrorMessage = "La priorité est requise")]
        public Priorite Priorite { get; set; }

        public DateTime? DateDebutReelle { get; set; }

        public DateTime? DateFinReelle { get; set; }

        public decimal? TempsPasse { get; set; }

        public int? CausePanneId { get; set; }

        public string? SolutionApportee { get; set; }

        public string? Remarques { get; set; }

        public int? DemandeInterventionId { get; set; }

        [Required(ErrorMessage = "Le type de maintenance est requis")]
        public TypeMaintenance TypeMaintenance { get; set; }

        public int? OrganeId { get; set; }

        public decimal? CoutReel { get; set; }

        [Range(0, 100, ErrorMessage = "Le pourcentage de progression doit être entre 0 et 100")]
        public int PourcentageProgression { get; set; }

        public int? SousTraitantId { get; set; }

        public int? WorkflowId { get; set; }
    }

    public class UpdateOTProgressionDto
    {
        [Required(ErrorMessage = "L'ID est requis")]
        public int Id { get; set; }

        [Range(0, 100, ErrorMessage = "Le pourcentage de progression doit être entre 0 et 100")]
        public int PourcentageProgression { get; set; }

        public StatutOT? Statut { get; set; }

        public DateTime? DateDebutReelle { get; set; }

        public DateTime? DateFinReelle { get; set; }

        public decimal? TempsPasse { get; set; }

        public string? SolutionApportee { get; set; }

        public string? Remarques { get; set; }
    }

    public class ValidateOTDto
    {
        [Required(ErrorMessage = "L'ID est requis")]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'ID du validateur est requis")]
        public int ValidateurId { get; set; }

        public string? Commentaires { get; set; }
    }
}

