using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AdeauMao.Core.Entities;

namespace AdeauMao.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Role> RolesCustom { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<EquipeMembre> EquipeMembres { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<LigneProduction> LignesProduction { get; set; }
        public DbSet<Equipement> Equipements { get; set; }
        public DbSet<Organe> Organes { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<CausePanne> CausesPanne { get; set; }
        public DbSet<Activite> Activites { get; set; }
        public DbSet<SousTraitant> SousTraitants { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LignesCommande { get; set; }
        public DbSet<PieceRechange> PiecesRechange { get; set; }
        public DbSet<MouvementStock> MouvementsStock { get; set; }
        public DbSet<DemandeIntervention> DemandesIntervention { get; set; }
        public DbSet<OrdresDeTravail> OrdresDeTravail { get; set; }
        public DbSet<PlanMaintenancePreventive> PlansMaintenancePreventive { get; set; }
        public DbSet<Declencheur> Declencheurs { get; set; }
        public DbSet<Competence> Competences { get; set; }
        public DbSet<EmployeCompetence> EmployeCompetences { get; set; }
        public DbSet<StatutEmploye> StatutsEmploye { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<EtapeWorkflow> EtapesWorkflow { get; set; }
        public DbSet<HistoriqueWorkflow> HistoriqueWorkflows { get; set; }
        public DbSet<DocumentAttache> DocumentsAttaches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and constraints

            // RolePermissions - Many-to-Many
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            // EquipeMembres - Many-to-Many
            modelBuilder.Entity<EquipeMembre>()
                .HasKey(em => new { em.EquipeId, em.EmployeId });

            modelBuilder.Entity<EquipeMembre>()
                .HasOne(em => em.Equipe)
                .WithMany(e => e.EquipeMembres)
                .HasForeignKey(em => em.EquipeId);

            modelBuilder.Entity<EquipeMembre>()
                .HasOne(em => em.Employe)
                .WithMany(e => e.EquipeMembres)
                .HasForeignKey(em => em.EmployeId);

            // EmployeCompetences - Many-to-Many
            modelBuilder.Entity<EmployeCompetence>()
                .HasKey(ec => new { ec.EmployeId, ec.CompetenceId });

            modelBuilder.Entity<EmployeCompetence>()
                .HasOne(ec => ec.Employe)
                .WithMany(e => e.EmployeCompetences)
                .HasForeignKey(ec => ec.EmployeId);

            modelBuilder.Entity<EmployeCompetence>()
                .HasOne(ec => ec.Competence)
                .WithMany(c => c.EmployeCompetences)
                .HasForeignKey(ec => ec.CompetenceId);

            // StatutEmploye - Composite Key
            modelBuilder.Entity<StatutEmploye>()
                .HasKey(se => new { se.EmployeId, se.DateStatut });

            modelBuilder.Entity<StatutEmploye>()
                .HasOne(se => se.Employe)
                .WithMany(e => e.StatutsEmploye)
                .HasForeignKey(se => se.EmployeId);

            // Utilisateur relationships
            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Utilisateurs)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.NomUtilisateur)
                .IsUnique();

            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Employe relationships
            modelBuilder.Entity<Employe>()
                .HasOne(e => e.Utilisateur)
                .WithMany()
                .HasForeignKey(e => e.UtilisateurId)
                .OnDelete(DeleteBehavior.SetNull);

            // Equipe relationships
            modelBuilder.Entity<Equipe>()
                .HasOne(e => e.Responsable)
                .WithMany(emp => emp.EquipesResponsables)
                .HasForeignKey(e => e.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);

            // LigneProduction relationships
            modelBuilder.Entity<LigneProduction>()
                .HasOne(lp => lp.Site)
                .WithMany(s => s.LignesProduction)
                .HasForeignKey(lp => lp.SiteId);

            // Equipement relationships
            modelBuilder.Entity<Equipement>()
                .HasOne(e => e.LigneProduction)
                .WithMany(lp => lp.Equipements)
                .HasForeignKey(e => e.LigneProductionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Equipement>()
                .HasIndex(e => e.Reference)
                .IsUnique();

            // Organe relationships
            modelBuilder.Entity<Organe>()
                .HasOne(o => o.Equipement)
                .WithMany(e => e.Organes)
                .HasForeignKey(o => o.EquipementId);

            // CausePanne relationships
            modelBuilder.Entity<CausePanne>()
                .HasOne(cp => cp.Categorie)
                .WithMany(c => c.CausesPanne)
                .HasForeignKey(cp => cp.CategorieId);

            // Activite relationships
            modelBuilder.Entity<Activite>()
                .HasOne(a => a.Categorie)
                .WithMany(c => c.Activites)
                .HasForeignKey(a => a.CategorieId);

            // Projet relationships
            modelBuilder.Entity<Projet>()
                .HasOne(p => p.Responsable)
                .WithMany(e => e.ProjetsResponsables)
                .HasForeignKey(p => p.ResponsableId)
                .OnDelete(DeleteBehavior.SetNull);

            // Commande relationships
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Fournisseur)
                .WithMany(f => f.Commandes)
                .HasForeignKey(c => c.FournisseurId);

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Validateur)
                .WithMany(u => u.CommandesValidees)
                .HasForeignKey(c => c.ValidateurId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Commande>()
                .HasIndex(c => c.NumeroCommande)
                .IsUnique();

            // LigneCommande relationships
            modelBuilder.Entity<LigneCommande>()
                .HasOne(lc => lc.Commande)
                .WithMany(c => c.LignesCommande)
                .HasForeignKey(lc => lc.CommandeId);

            modelBuilder.Entity<LigneCommande>()
                .HasOne(lc => lc.PieceRechange)
                .WithMany(pr => pr.LignesCommande)
                .HasForeignKey(lc => lc.PieceRechangeId);

            // PieceRechange relationships
            modelBuilder.Entity<PieceRechange>()
                .HasOne(pr => pr.FournisseurPrincipal)
                .WithMany(f => f.PiecesRechange)
                .HasForeignKey(pr => pr.FournisseurPrincipalId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PieceRechange>()
                .HasIndex(pr => pr.CodePiece)
                .IsUnique();

            // MouvementStock relationships
            modelBuilder.Entity<MouvementStock>()
                .HasOne(ms => ms.PieceRechange)
                .WithMany(pr => pr.MouvementsStock)
                .HasForeignKey(ms => ms.PieceRechangeId);

            modelBuilder.Entity<MouvementStock>()
                .HasOne(ms => ms.OrdreDeTravail)
                .WithMany(ot => ot.MouvementsStock)
                .HasForeignKey(ms => ms.OrdreDeTravailId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MouvementStock>()
                .HasOne(ms => ms.Utilisateur)
                .WithMany(u => u.MouvementsStock)
                .HasForeignKey(ms => ms.UtilisateurId)
                .OnDelete(DeleteBehavior.SetNull);

            // DemandeIntervention relationships
            modelBuilder.Entity<DemandeIntervention>()
                .HasOne(di => di.Equipement)
                .WithMany(e => e.DemandesIntervention)
                .HasForeignKey(di => di.EquipementId);

            modelBuilder.Entity<DemandeIntervention>()
                .HasOne(di => di.Demandeur)
                .WithMany(e => e.DemandesIntervention)
                .HasForeignKey(di => di.DemandeurId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrdresDeTravail relationships
            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.Equipement)
                .WithMany(e => e.OrdresDeTravail)
                .HasForeignKey(ot => ot.EquipementId);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.TechnicienAssignee)
                .WithMany(e => e.OrdresDeTravailAssignes)
                .HasForeignKey(ot => ot.TechnicienAssigneeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.CausePanne)
                .WithMany(cp => cp.OrdresDeTravail)
                .HasForeignKey(ot => ot.CausePanneId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.DemandeIntervention)
                .WithMany(di => di.OrdresDeTravail)
                .HasForeignKey(ot => ot.DemandeInterventionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.Organe)
                .WithMany(o => o.OrdresDeTravail)
                .HasForeignKey(ot => ot.OrganeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.SousTraitant)
                .WithMany(st => st.OrdresDeTravail)
                .HasForeignKey(ot => ot.SousTraitantId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.Validateur)
                .WithMany(u => u.OrdresDeTravailValides)
                .HasForeignKey(ot => ot.ValidateurId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasOne(ot => ot.Workflow)
                .WithMany(w => w.OrdresDeTravail)
                .HasForeignKey(ot => ot.WorkflowId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdresDeTravail>()
                .HasIndex(ot => ot.NumeroOT)
                .IsUnique();

            // PlanMaintenancePreventive relationships
            modelBuilder.Entity<PlanMaintenancePreventive>()
                .HasOne(pmp => pmp.Equipement)
                .WithMany(e => e.PlansMaintenancePreventive)
                .HasForeignKey(pmp => pmp.EquipementId);

            modelBuilder.Entity<PlanMaintenancePreventive>()
                .HasOne(pmp => pmp.Organe)
                .WithMany(o => o.PlansMaintenancePreventive)
                .HasForeignKey(pmp => pmp.OrganeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlanMaintenancePreventive>()
                .HasOne(pmp => pmp.Declencheur)
                .WithMany(d => d.PlansMaintenancePreventive)
                .HasForeignKey(pmp => pmp.DeclencheurId)
                .OnDelete(DeleteBehavior.SetNull);

            // Declencheur relationships
            modelBuilder.Entity<Declencheur>()
                .HasOne(d => d.Equipement)
                .WithMany(e => e.Declencheurs)
                .HasForeignKey(d => d.EquipementId);

            modelBuilder.Entity<Declencheur>()
                .HasOne(d => d.Organe)
                .WithMany(o => o.Declencheurs)
                .HasForeignKey(d => d.OrganeId)
                .OnDelete(DeleteBehavior.SetNull);

            // EtapeWorkflow relationships
            modelBuilder.Entity<EtapeWorkflow>()
                .HasOne(ew => ew.Workflow)
                .WithMany(w => w.EtapesWorkflow)
                .HasForeignKey(ew => ew.WorkflowId);

            // HistoriqueWorkflow relationships
            modelBuilder.Entity<HistoriqueWorkflow>()
                .HasOne(hw => hw.OrdreDeTravail)
                .WithMany(ot => ot.HistoriqueWorkflows)
                .HasForeignKey(hw => hw.OrdreDeTravailId);

            modelBuilder.Entity<HistoriqueWorkflow>()
                .HasOne(hw => hw.Etape)
                .WithMany(ew => ew.HistoriqueWorkflows)
                .HasForeignKey(hw => hw.EtapeId);

            // Configure enum conversions
            modelBuilder.Entity<Commande>()
                .Property(c => c.StatutCommande)
                .HasConversion<string>();

            modelBuilder.Entity<DemandeIntervention>()
                .Property(di => di.Statut)
                .HasConversion<string>();

            modelBuilder.Entity<DemandeIntervention>()
                .Property(di => di.Priorite)
                .HasConversion<string>();

            modelBuilder.Entity<OrdresDeTravail>()
                .Property(ot => ot.Statut)
                .HasConversion<string>();

            modelBuilder.Entity<OrdresDeTravail>()
                .Property(ot => ot.Priorite)
                .HasConversion<string>();

            modelBuilder.Entity<OrdresDeTravail>()
                .Property(ot => ot.TypeMaintenance)
                .HasConversion<string>();

            modelBuilder.Entity<Categorie>()
                .Property(c => c.TypeCategorie)
                .HasConversion<string>();

            modelBuilder.Entity<MouvementStock>()
                .Property(ms => ms.TypeMouvement)
                .HasConversion<string>();

            modelBuilder.Entity<PlanMaintenancePreventive>()
                .Property(pmp => pmp.FrequenceType)
                .HasConversion<string>();

            modelBuilder.Entity<PlanMaintenancePreventive>()
                .Property(pmp => pmp.TypeMaintenance)
                .HasConversion<string>();

            modelBuilder.Entity<Declencheur>()
                .Property(d => d.TypeDeclencheur)
                .HasConversion<string>();

            modelBuilder.Entity<StatutEmploye>()
                .Property(se => se.Statut)
                .HasConversion<string>();

            modelBuilder.Entity<HistoriqueWorkflow>()
                .Property(hw => hw.StatutEtape)
                .HasConversion<string>();

            // Configure decimal precision
            modelBuilder.Entity<Projet>()
                .Property(p => p.Budget)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LigneCommande>()
                .Property(lc => lc.PrixUnitaire)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PieceRechange>()
                .Property(pr => pr.PrixUnitaire)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrdresDeTravail>()
                .Property(ot => ot.TempsPasse)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrdresDeTravail>()
                .Property(ot => ot.CoutReel)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PlanMaintenancePreventive>()
                .Property(pmp => pmp.DureeEstimee)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PlanMaintenancePreventive>()
                .Property(pmp => pmp.ValeurSeuilDeclenchement)
                .HasPrecision(18, 2);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DateCreation = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.DateModification = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}

