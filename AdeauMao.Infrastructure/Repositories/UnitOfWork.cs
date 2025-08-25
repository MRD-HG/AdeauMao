using Microsoft.EntityFrameworkCore.Storage;
using AdeauMao.Core.Entities;
using AdeauMao.Core.Interfaces;
using AdeauMao.Infrastructure.Data;

namespace AdeauMao.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repository instances
        private IRepository<Utilisateur>? _utilisateurs;
        private IRepository<Role>? _roles;
        private IRepository<Permission>? _permissions;
        private IRepository<RolePermission>? _rolePermissions;
        private IRepository<Employe>? _employes;
        private IRepository<Equipe>? _equipes;
        private IRepository<EquipeMembre>? _equipeMembres;
        private IRepository<Site>? _sites;
        private IRepository<LigneProduction>? _lignesProduction;
        private IRepository<Equipement>? _equipements;
        private IRepository<Organe>? _organes;
        private IRepository<Categorie>? _categories;
        private IRepository<CausePanne>? _causesPanne;
        private IRepository<Activite>? _activites;
        private IRepository<SousTraitant>? _sousTraitants;
        private IRepository<Projet>? _projets;
        private IRepository<Fournisseur>? _fournisseurs;
        private IRepository<Commande>? _commandes;
        private IRepository<LigneCommande>? _lignesCommande;
        private IRepository<PieceRechange>? _piecesRechange;
        private IRepository<MouvementStock>? _mouvementsStock;
        private IRepository<DemandeIntervention>? _demandesIntervention;
        private IRepository<OrdresDeTravail>? _ordresDeTravail;
        private IRepository<PlanMaintenancePreventive>? _plansMaintenancePreventive;
        private IRepository<Declencheur>? _declencheurs;
        private IRepository<Competence>? _competences;
        private IRepository<EmployeCompetence>? _employeCompetences;
        private IRepository<StatutEmploye>? _statutsEmploye;
        private IRepository<Workflow>? _workflows;
        private IRepository<EtapeWorkflow>? _etapesWorkflow;
        private IRepository<HistoriqueWorkflow>? _historiqueWorkflows;
        private IRepository<DocumentAttache>? _documentsAttaches;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // Repository properties with lazy initialization
        public IRepository<Utilisateur> Utilisateurs =>
            _utilisateurs ??= new Repository<Utilisateur>(_context);

        public IRepository<Role> Roles =>
            _roles ??= new Repository<Role>(_context);

        public IRepository<Permission> Permissions =>
            _permissions ??= new Repository<Permission>(_context);

        public IRepository<RolePermission> RolePermissions =>
            _rolePermissions ??= new Repository<RolePermission>(_context);

        public IRepository<Employe> Employes =>
            _employes ??= new Repository<Employe>(_context);

        public IRepository<Equipe> Equipes =>
            _equipes ??= new Repository<Equipe>(_context);

        public IRepository<EquipeMembre> EquipeMembres =>
            _equipeMembres ??= new Repository<EquipeMembre>(_context);

        public IRepository<Site> Sites =>
            _sites ??= new Repository<Site>(_context);

        public IRepository<LigneProduction> LignesProduction =>
            _lignesProduction ??= new Repository<LigneProduction>(_context);

        public IRepository<Equipement> Equipements =>
            _equipements ??= new Repository<Equipement>(_context);

        public IRepository<Organe> Organes =>
            _organes ??= new Repository<Organe>(_context);

        public IRepository<Categorie> Categories =>
            _categories ??= new Repository<Categorie>(_context);

        public IRepository<CausePanne> CausesPanne =>
            _causesPanne ??= new Repository<CausePanne>(_context);

        public IRepository<Activite> Activites =>
            _activites ??= new Repository<Activite>(_context);

        public IRepository<SousTraitant> SousTraitants =>
            _sousTraitants ??= new Repository<SousTraitant>(_context);

        public IRepository<Projet> Projets =>
            _projets ??= new Repository<Projet>(_context);

        public IRepository<Fournisseur> Fournisseurs =>
            _fournisseurs ??= new Repository<Fournisseur>(_context);

        public IRepository<Commande> Commandes =>
            _commandes ??= new Repository<Commande>(_context);

        public IRepository<LigneCommande> LignesCommande =>
            _lignesCommande ??= new Repository<LigneCommande>(_context);

        public IRepository<PieceRechange> PiecesRechange =>
            _piecesRechange ??= new Repository<PieceRechange>(_context);

        public IRepository<MouvementStock> MouvementsStock =>
            _mouvementsStock ??= new Repository<MouvementStock>(_context);

        public IRepository<DemandeIntervention> DemandesIntervention =>
            _demandesIntervention ??= new Repository<DemandeIntervention>(_context);

        public IRepository<OrdresDeTravail> OrdresDeTravail =>
            _ordresDeTravail ??= new Repository<OrdresDeTravail>(_context);

        public IRepository<PlanMaintenancePreventive> PlansMaintenancePreventive =>
            _plansMaintenancePreventive ??= new Repository<PlanMaintenancePreventive>(_context);

        public IRepository<Declencheur> Declencheurs =>
            _declencheurs ??= new Repository<Declencheur>(_context);

        public IRepository<Competence> Competences =>
            _competences ??= new Repository<Competence>(_context);

        public IRepository<EmployeCompetence> EmployeCompetences =>
            _employeCompetences ??= new Repository<EmployeCompetence>(_context);

        public IRepository<StatutEmploye> StatutsEmploye =>
            _statutsEmploye ??= new Repository<StatutEmploye>(_context);

        public IRepository<Workflow> Workflows =>
            _workflows ??= new Repository<Workflow>(_context);

        public IRepository<EtapeWorkflow> EtapesWorkflow =>
            _etapesWorkflow ??= new Repository<EtapeWorkflow>(_context);

        public IRepository<HistoriqueWorkflow> HistoriqueWorkflows =>
            _historiqueWorkflows ??= new Repository<HistoriqueWorkflow>(_context);

        public IRepository<DocumentAttache> DocumentsAttaches =>
            _documentsAttaches ??= new Repository<DocumentAttache>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}

