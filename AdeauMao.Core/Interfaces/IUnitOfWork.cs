using AdeauMao.Core.Entities;

namespace AdeauMao.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Utilisateur> Utilisateurs { get; }
        IRepository<Role> Roles { get; }
        IRepository<Permission> Permissions { get; }
        IRepository<RolePermission> RolePermissions { get; }
        IRepository<Employe> Employes { get; }
        IRepository<Equipe> Equipes { get; }
        IRepository<EquipeMembre> EquipeMembres { get; }
        IRepository<Site> Sites { get; }
        IRepository<LigneProduction> LignesProduction { get; }
        IRepository<Equipement> Equipements { get; }
        IRepository<Organe> Organes { get; }
        IRepository<Categorie> Categories { get; }
        IRepository<CausePanne> CausesPanne { get; }
        IRepository<Activite> Activites { get; }
        IRepository<SousTraitant> SousTraitants { get; }
        IRepository<Projet> Projets { get; }
        IRepository<Fournisseur> Fournisseurs { get; }
        IRepository<Commande> Commandes { get; }
        IRepository<LigneCommande> LignesCommande { get; }
        IRepository<PieceRechange> PiecesRechange { get; }
        IRepository<MouvementStock> MouvementsStock { get; }
        IRepository<DemandeIntervention> DemandesIntervention { get; }
        IRepository<OrdresDeTravail> OrdresDeTravail { get; }
        IRepository<PlanMaintenancePreventive> PlansMaintenancePreventive { get; }
        IRepository<Declencheur> Declencheurs { get; }
        IRepository<Competence> Competences { get; }
        IRepository<EmployeCompetence> EmployeCompetences { get; }
        IRepository<StatutEmploye> StatutsEmploye { get; }
        IRepository<Workflow> Workflows { get; }
        IRepository<EtapeWorkflow> EtapesWorkflow { get; }
        IRepository<HistoriqueWorkflow> HistoriqueWorkflows { get; }
        IRepository<DocumentAttache> DocumentsAttaches { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

