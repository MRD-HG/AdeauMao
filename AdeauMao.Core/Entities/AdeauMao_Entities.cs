using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdeauMao.Core.Entities
{
    // Base entity class for common properties
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public DateTime? DateModification { get; set; }
    }

    // Enums for various status and type fields
    public enum StatutCommande
    {
        EnAttenteValidation,
        Validee,
        Rejetee,
        Livree
    }

    public enum StatutDemande
    {
        Nouvelle,
        EnAttenteOT,
        Cloturee
    }

    public enum StatutOT
    {
        AFaire,
        EnCours,
        Termine,
        Valide
    }

    public enum Priorite
    {
        Urgent,
        Elevee,
        Moyenne,
        Faible
    }

    public enum TypeMaintenance
    {
        Corrective,
        PreventiveRepetitive,
        PreventiveConditionnelle
    }

    public enum TypeCategorie
    {
        Equipement,
        Panne,
        Activite
    }

    public enum TypeMouvement
    {
        Entree,
        Sortie
    }

    public enum FrequenceType
    {
        Hebdomadaire,
        Mensuel,
        Annuel,
        Compteur
    }

    public enum TypeDeclencheur
    {
        Compteur,
        Seuil,
        Evenement
    }

    public enum StatutEmploye
    {
        Disponible,
        Indisponible,
        EnTache,
        EnConge
    }

    public enum StatutEtape
    {
        EnCours,
        Terminee,
        Rejetee
    }

    // 1. Utilisateurs
    public class Utilisateur : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string NomUtilisateur { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string MotDePasseHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public bool EstActif { get; set; } = true;

        // Navigation properties
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Commande> CommandesValidees { get; set; } = new List<Commande>();
        public virtual ICollection<MouvementStock> MouvementsStock { get; set; } = new List<MouvementStock>();
        public virtual ICollection<OrdresDeTravail> OrdresDeTravailValides { get; set; } = new List<OrdresDeTravail>();
    }

    // 2. Roles
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string NomRole { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    // 3. Permissions
    public class Permission : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string NomPermission { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    // 4. RolePermissions (Many-to-Many)
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }

    // 5. Employes
    public class Employe : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Contact { get; set; }

        [StringLength(50)]
        public string? RoleInterne { get; set; }

        public int? UtilisateurId { get; set; }

        // Navigation properties
        public virtual Utilisateur? Utilisateur { get; set; }
        public virtual ICollection<Equipe> EquipesResponsables { get; set; } = new List<Equipe>();
        public virtual ICollection<EquipeMembre> EquipeMembres { get; set; } = new List<EquipeMembre>();
        public virtual ICollection<DemandeIntervention> DemandesIntervention { get; set; } = new List<DemandeIntervention>();
        public virtual ICollection<OrdresDeTravail> OrdresDeTravailAssignes { get; set; } = new List<OrdresDeTravail>();
        public virtual ICollection<Projet> ProjetsResponsables { get; set; } = new List<Projet>();
        public virtual ICollection<EmployeCompetence> EmployeCompetences { get; set; } = new List<EmployeCompetence>();
        public virtual ICollection<HistoriqueStatutEmploye> StatutsEmploye { get; set; } = new List<HistoriqueStatutEmploye>();
    }

    // 6. Equipes
    public class Equipe : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomEquipe { get; set; } = string.Empty;

        public int ResponsableId { get; set; }

        // Navigation properties
        public virtual Employe Responsable { get; set; } = null!;
        public virtual ICollection<EquipeMembre> EquipeMembres { get; set; } = new List<EquipeMembre>();
    }

    // 7. EquipeMembres (Many-to-Many)
    public class EquipeMembre
    {
        public int EquipeId { get; set; }
        public int EmployeId { get; set; }

        // Navigation properties
        public virtual Equipe Equipe { get; set; } = null!;
        public virtual Employe Employe { get; set; } = null!;
    }

    // 8. Sites
    public class Site : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomSite { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Adresse { get; set; }

        // Navigation properties
        public virtual ICollection<LigneProduction> LignesProduction { get; set; } = new List<LigneProduction>();
    }

    // 9. LignesProduction
    public class LigneProduction : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomLigne { get; set; } = string.Empty;

        public int SiteId { get; set; }

        // Navigation properties
        public virtual Site Site { get; set; } = null!;
        public virtual ICollection<Equipement> Equipements { get; set; } = new List<Equipement>();
    }

    // 10. Equipements
    public class Equipement : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Reference { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Nom { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TypeEquipement { get; set; }

        [StringLength(100)]
        public string? Fabricant { get; set; }

        [StringLength(100)]
        public string? Modele { get; set; }

        public DateTime? DateMiseEnService { get; set; }

        [StringLength(255)]
        public string? Localisation { get; set; }

        public int? LigneProductionId { get; set; }

        public string? Description { get; set; }

        [StringLength(50)]
        public string? EtatOperationnel { get; set; }

        // Navigation properties
        public virtual LigneProduction? LigneProduction { get; set; }
        public virtual ICollection<Organe> Organes { get; set; } = new List<Organe>();
        public virtual ICollection<DemandeIntervention> DemandesIntervention { get; set; } = new List<DemandeIntervention>();
        public virtual ICollection<OrdresDeTravail> OrdresDeTravail { get; set; } = new List<OrdresDeTravail>();
        public virtual ICollection<PlanMaintenancePreventive> PlansMaintenancePreventive { get; set; } = new List<PlanMaintenancePreventive>();
        public virtual ICollection<Declencheur> Declencheurs { get; set; } = new List<Declencheur>();
    }

    // 11. Organes
    public class Organe : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomOrgane { get; set; } = string.Empty;

        public int EquipementId { get; set; }

        public string? Description { get; set; }

        // Navigation properties
        public virtual Equipement Equipement { get; set; } = null!;
        public virtual ICollection<OrdresDeTravail> OrdresDeTravail { get; set; } = new List<OrdresDeTravail>();
        public virtual ICollection<PlanMaintenancePreventive> PlansMaintenancePreventive { get; set; } = new List<PlanMaintenancePreventive>();
        public virtual ICollection<Declencheur> Declencheurs { get; set; } = new List<Declencheur>();
    }

    // 12. Categories
    public class Categorie : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomCategorie { get; set; } = string.Empty;

        [Required]
        public TypeCategorie TypeCategorie { get; set; }

        // Navigation properties
        public virtual ICollection<CausePanne> CausesPanne { get; set; } = new List<CausePanne>();
        public virtual ICollection<Activite> Activites { get; set; } = new List<Activite>();
    }

    // 13. CausesPanne
    public class CausePanne : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string NomCause { get; set; } = string.Empty;

        public int CategorieId { get; set; }

        // Navigation properties
        public virtual Categorie Categorie { get; set; } = null!;
        public virtual ICollection<OrdresDeTravail> OrdresDeTravail { get; set; } = new List<OrdresDeTravail>();
    }

    // 14. Activites
    public class Activite : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string NomActivite { get; set; } = string.Empty;

        public int CategorieId { get; set; }

        // Navigation properties
        public virtual Categorie Categorie { get; set; } = null!;
    }

    // 15. SousTraitants
    public class SousTraitant : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string NomSousTraitant { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Contact { get; set; }

        [StringLength(50)]
        public string? Telephone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        // Navigation properties
        public virtual ICollection<OrdresDeTravail> OrdresDeTravail { get; set; } = new List<OrdresDeTravail>();
    }

    // 16. Projets
    public class Projet : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string NomProjet { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        [StringLength(50)]
        public string? Statut { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }

        public int? ResponsableId { get; set; }

        // Navigation properties
        public virtual Employe? Responsable { get; set; }
    }

    // 17. Fournisseurs
    public class Fournisseur : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string NomFournisseur { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Contact { get; set; }

        [StringLength(50)]
        public string? Telephone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        // Navigation properties
        public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
        public virtual ICollection<PieceRechange> PiecesRechange { get; set; } = new List<PieceRechange>();
    }

    // 18. Commandes
    public class Commande : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string NumeroCommande { get; set; } = string.Empty;

        public DateTime DateCommande { get; set; }

        public int FournisseurId { get; set; }

        public StatutCommande StatutCommande { get; set; }

        public DateTime? DateValidation { get; set; }

        public int? ValidateurId { get; set; }

        // Navigation properties
        public virtual Fournisseur Fournisseur { get; set; } = null!;
        public virtual Utilisateur? Validateur { get; set; }
        public virtual ICollection<LigneCommande> LignesCommande { get; set; } = new List<LigneCommande>();
    }

    // 19. LignesCommande
    public class LigneCommande : BaseEntity
    {
        public int CommandeId { get; set; }

        public int PieceRechangeId { get; set; }

        public int Quantite { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrixUnitaire { get; set; }

        // Navigation properties
        public virtual Commande Commande { get; set; } = null!;
        public virtual PieceRechange PieceRechange { get; set; } = null!;
    }

    // 20. PiecesRechange
    public class PieceRechange : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string CodePiece { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Designation { get; set; } = string.Empty;

        [StringLength(50)]
        public string? UniteMesure { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrixUnitaire { get; set; }

        public int? FournisseurPrincipalId { get; set; }

        public int? SeuilMinimum { get; set; }

        public int? SeuilMaximum { get; set; }

        public int QuantiteEnStock { get; set; } = 0;

        // Navigation properties
        public virtual Fournisseur? FournisseurPrincipal { get; set; }
        public virtual ICollection<LigneCommande> LignesCommande { get; set; } = new List<LigneCommande>();
        public virtual ICollection<MouvementStock> MouvementsStock { get; set; } = new List<MouvementStock>();
    }

    // 21. MouvementsStock
    public class MouvementStock : BaseEntity
    {
        public int PieceRechangeId { get; set; }

        public TypeMouvement TypeMouvement { get; set; }

        public int Quantite { get; set; }

        public DateTime DateMouvement { get; set; }

        public int? OrdreDeTravailId { get; set; }

        public int? UtilisateurId { get; set; }

        [StringLength(100)]
        public string? ReferenceDocument { get; set; }

        // Navigation properties
        public virtual PieceRechange PieceRechange { get; set; } = null!;
        public virtual OrdresDeTravail? OrdreDeTravail { get; set; }
        public virtual Utilisateur? Utilisateur { get; set; }
    }

    // 22. DemandesIntervention
    public class DemandeIntervention : BaseEntity
    {
        public int EquipementId { get; set; }

        [Required]
        public string DescriptionProbleme { get; set; } = string.Empty;

        public DateTime DateDemande { get; set; }

        public int DemandeurId { get; set; }

        public StatutDemande Statut { get; set; }

        public Priorite Priorite { get; set; }

        // Navigation properties
        public virtual Equipement Equipement { get; set; } = null!;
        public virtual Employe Demandeur { get; set; } = null!;
        public virtual ICollection<OrdresDeTravail> OrdresDeTravail { get; set; } = new List<OrdresDeTravail>();
    }

    // 23. OrdresDeTravail
    public class OrdresDeTravail : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string NumeroOT { get; set; } = string.Empty;

        public int EquipementId { get; set; }

        [Required]
        public string DescriptionTache { get; set; } = string.Empty;

        public DateTime? DateDebutPrevue { get; set; }

        public DateTime? DateFinPrevue { get; set; }

        public int? TechnicienAssigneeId { get; set; }

        public StatutOT Statut { get; set; }

        public Priorite Priorite { get; set; }

        public DateTime? DateDebutReelle { get; set; }

        public DateTime? DateFinReelle { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TempsPasse { get; set; }

        public int? CausePanneId { get; set; }

        public string? SolutionApportee { get; set; }

        public string? Remarques { get; set; }

        public int? DemandeInterventionId { get; set; }

        public TypeMaintenance TypeMaintenance { get; set; }

        public int? OrganeId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CoutReel { get; set; }

        public int PourcentageProgression { get; set; } = 0;

        public int? SousTraitantId { get; set; }

        public DateTime? DateValidation { get; set; }

        public int? ValidateurId { get; set; }

        public int? WorkflowId { get; set; }

        // Navigation properties
        public virtual Equipement Equipement { get; set; } = null!;
        public virtual Employe? TechnicienAssignee { get; set; }
        public virtual CausePanne? CausePanne { get; set; }
        public virtual DemandeIntervention? DemandeIntervention { get; set; }
        public virtual Organe? Organe { get; set; }
        public virtual SousTraitant? SousTraitant { get; set; }
        public virtual Utilisateur? Validateur { get; set; }
        public virtual Workflow? Workflow { get; set; }
        public virtual ICollection<MouvementStock> MouvementsStock { get; set; } = new List<MouvementStock>();
        public virtual ICollection<HistoriqueWorkflow> HistoriqueWorkflows { get; set; } = new List<HistoriqueWorkflow>();
    }

    // 24. PlansMaintenancePreventive
    public class PlanMaintenancePreventive : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string NomPlan { get; set; } = string.Empty;

        public int EquipementId { get; set; }

        public FrequenceType FrequenceType { get; set; }

        public int FrequenceValeur { get; set; }

        [Required]
        public string DescriptionTaches { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? DureeEstimee { get; set; }

        public DateTime? DateProchaineExecution { get; set; }

        public TypeMaintenance TypeMaintenance { get; set; }

        public int? OrganeId { get; set; }

        public int? DeclencheurId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValeurSeuilDeclenchement { get; set; }

        // Navigation properties
        public virtual Equipement Equipement { get; set; } = null!;
        public virtual Organe? Organe { get; set; }
        public virtual Declencheur? Declencheur { get; set; }
    }

    // 25. Declencheurs
    public class Declencheur : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomDeclencheur { get; set; } = string.Empty;

        public TypeDeclencheur TypeDeclencheur { get; set; }

        [StringLength(50)]
        public string? UniteMesure { get; set; }

        public string? Description { get; set; }

        public int EquipementId { get; set; }

        public int? OrganeId { get; set; }

        // Navigation properties
        public virtual Equipement Equipement { get; set; } = null!;
        public virtual Organe? Organe { get; set; }
        public virtual ICollection<PlanMaintenancePreventive> PlansMaintenancePreventive { get; set; } = new List<PlanMaintenancePreventive>();
    }

    // 26. Competences
    public class Competence : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomCompetence { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<EmployeCompetence> EmployeCompetences { get; set; } = new List<EmployeCompetence>();
    }

    // 27. EmployeCompetences (Many-to-Many)
    public class EmployeCompetence
    {
        public int EmployeId { get; set; }
        public int CompetenceId { get; set; }

        // Navigation properties
        public virtual Employe Employe { get; set; } = null!;
        public virtual Competence Competence { get; set; } = null!;
    }

    // 28. HistoriqueStatutEmploye
    public class HistoriqueStatutEmploye
    {
        public int EmployeId { get; set; }
        public DateTime DateStatut { get; set; }
        public Entities.StatutEmploye Statut { get; set; }
        public string? Details { get; set; }

        // Navigation properties
        public virtual Employe Employe { get; set; } = null!;
    }

    // 29. Workflows
    public class Workflow : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string NomWorkflow { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<EtapeWorkflow> EtapesWorkflow { get; set; } = new List<EtapeWorkflow>();
        public virtual ICollection<OrdresDeTravail> OrdresDeTravail { get; set; } = new List<OrdresDeTravail>();
    }

    // 30. EtapesWorkflow
    public class EtapeWorkflow : BaseEntity
    {
        public int WorkflowId { get; set; }

        [Required]
        [StringLength(100)]
        public string NomEtape { get; set; } = string.Empty;

        public int Ordre { get; set; }

        public string? Description { get; set; }

        // Navigation properties
        public virtual Workflow Workflow { get; set; } = null!;
        public virtual ICollection<HistoriqueWorkflow> HistoriqueWorkflows { get; set; } = new List<HistoriqueWorkflow>();
    }

    // 31. HistoriqueWorkflow
    public class HistoriqueWorkflow : BaseEntity
    {
        public int OrdreDeTravailId { get; set; }

        public int EtapeId { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public StatutEtape StatutEtape { get; set; }

        public string? Commentaire { get; set; }

        // Navigation properties
        public virtual OrdresDeTravail OrdreDeTravail { get; set; } = null!;
        public virtual EtapeWorkflow Etape { get; set; } = null!;
    }

    // 32. DocumentsAttaches
    public class DocumentAttache : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string NomFichier { get; set; } = string.Empty;

        [Required]
        public string CheminFichier { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TypeEntite { get; set; } = string.Empty;

        public int EntiteId { get; set; }

        public DateTime DateUpload { get; set; }
    }
}

