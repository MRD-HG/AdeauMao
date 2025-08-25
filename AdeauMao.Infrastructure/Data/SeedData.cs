using Microsoft.AspNetCore.Identity;
using AdeauMao.Core.Entities;

namespace AdeauMao.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Identity Roles
            await SeedIdentityRoles(roleManager);

            // Seed Identity Users
            await SeedIdentityUsers(userManager);

            // Seed Custom Roles and Permissions
            await SeedCustomRolesAndPermissions(context);

            // Seed Categories
            await SeedCategories(context);

            // Seed Sites and Production Lines
            await SeedSitesAndProductionLines(context);

            // Seed Employees
            await SeedEmployees(context);

            // Seed Competences
            await SeedCompetences(context);

            // Seed Suppliers
            await SeedSuppliers(context);

            // Seed Workflows
            await SeedWorkflows(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedIdentityRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Administrator", "Manager", "Technician", "Operator", "Viewer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedIdentityUsers(UserManager<IdentityUser> userManager)
        {
            // Admin user
            if (await userManager.FindByEmailAsync("admin@adeaumao.com") == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@adeaumao.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            // Manager user
            if (await userManager.FindByEmailAsync("manager@adeaumao.com") == null)
            {
                var managerUser = new IdentityUser
                {
                    UserName = "manager",
                    Email = "manager@adeaumao.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(managerUser, "Manager123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(managerUser, "Manager");
                }
            }

            // Technician user
            if (await userManager.FindByEmailAsync("tech@adeaumao.com") == null)
            {
                var techUser = new IdentityUser
                {
                    UserName = "technician",
                    Email = "tech@adeaumao.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(techUser, "Tech123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(techUser, "Technician");
                }
            }
        }

        private static async Task SeedCustomRolesAndPermissions(ApplicationDbContext context)
        {
            if (!context.RolesCustom.Any())
            {
                var roles = new List<Role>
                {
                    new Role { NomRole = "Administrateur" },
                    new Role { NomRole = "Responsable Maintenance" },
                    new Role { NomRole = "Technicien" },
                    new Role { NomRole = "Operateur" },
                    new Role { NomRole = "Consultant" }
                };

                context.RolesCustom.AddRange(roles);
                await context.SaveChangesAsync();
            }

            if (!context.Permissions.Any())
            {
                var permissions = new List<Permission>
                {
                    new Permission { NomPermission = "creer_ot" },
                    new Permission { NomPermission = "modifier_ot" },
                    new Permission { NomPermission = "supprimer_ot" },
                    new Permission { NomPermission = "valider_ot" },
                    new Permission { NomPermission = "creer_equipement" },
                    new Permission { NomPermission = "modifier_equipement" },
                    new Permission { NomPermission = "supprimer_equipement" },
                    new Permission { NomPermission = "gerer_stock" },
                    new Permission { NomPermission = "creer_commande" },
                    new Permission { NomPermission = "valider_commande" },
                    new Permission { NomPermission = "consulter_rapports" },
                    new Permission { NomPermission = "gerer_utilisateurs" }
                };

                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Categorie>
                {
                    new Categorie { NomCategorie = "Mécanique", TypeCategorie = TypeCategorie.Equipement },
                    new Categorie { NomCategorie = "Électrique", TypeCategorie = TypeCategorie.Equipement },
                    new Categorie { NomCategorie = "Hydraulique", TypeCategorie = TypeCategorie.Equipement },
                    new Categorie { NomCategorie = "Pneumatique", TypeCategorie = TypeCategorie.Equipement },
                    new Categorie { NomCategorie = "Usure normale", TypeCategorie = TypeCategorie.Panne },
                    new Categorie { NomCategorie = "Défaut matériel", TypeCategorie = TypeCategorie.Panne },
                    new Categorie { NomCategorie = "Erreur humaine", TypeCategorie = TypeCategorie.Panne },
                    new Categorie { NomCategorie = "Inspection", TypeCategorie = TypeCategorie.Activite },
                    new Categorie { NomCategorie = "Lubrification", TypeCategorie = TypeCategorie.Activite },
                    new Categorie { NomCategorie = "Nettoyage", TypeCategorie = TypeCategorie.Activite },
                    new Categorie { NomCategorie = "Réparation", TypeCategorie = TypeCategorie.Activite }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedSitesAndProductionLines(ApplicationDbContext context)
        {
            if (!context.Sites.Any())
            {
                var sites = new List<Site>
                {
                    new Site { NomSite = "Usine Principale", Adresse = "123 Rue de l'Industrie, Casablanca" },
                    new Site { NomSite = "Atelier Central", Adresse = "456 Avenue de la Production, Rabat" },
                    new Site { NomSite = "Dépôt Nord", Adresse = "789 Boulevard du Commerce, Tanger" }
                };

                context.Sites.AddRange(sites);
                await context.SaveChangesAsync();

                var lignesProduction = new List<LigneProduction>
                {
                    new LigneProduction { NomLigne = "Ligne d'assemblage 1", SiteId = sites[0].Id },
                    new LigneProduction { NomLigne = "Ligne d'assemblage 2", SiteId = sites[0].Id },
                    new LigneProduction { NomLigne = "Ligne de conditionnement", SiteId = sites[0].Id },
                    new LigneProduction { NomLigne = "Atelier mécanique", SiteId = sites[1].Id },
                    new LigneProduction { NomLigne = "Atelier électrique", SiteId = sites[1].Id }
                };

                context.LignesProduction.AddRange(lignesProduction);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedEmployees(ApplicationDbContext context)
        {
            if (!context.Employes.Any())
            {
                var employes = new List<Employe>
                {
                    new Employe { Nom = "Alami", Prenom = "Ahmed", Contact = "0661234567", RoleInterne = "Responsable Maintenance" },
                    new Employe { Nom = "Benali", Prenom = "Fatima", Contact = "0662345678", RoleInterne = "Technicien Électricien" },
                    new Employe { Nom = "Cherkaoui", Prenom = "Mohamed", Contact = "0663456789", RoleInterne = "Technicien Mécanicien" },
                    new Employe { Nom = "Douiri", Prenom = "Aicha", Contact = "0664567890", RoleInterne = "Opérateur" },
                    new Employe { Nom = "El Fassi", Prenom = "Youssef", Contact = "0665678901", RoleInterne = "Technicien Hydraulique" }
                };

                context.Employes.AddRange(employes);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedCompetences(ApplicationDbContext context)
        {
            if (!context.Competences.Any())
            {
                var competences = new List<Competence>
                {
                    new Competence { NomCompetence = "Électricité industrielle", Description = "Maintenance des systèmes électriques industriels" },
                    new Competence { NomCompetence = "Mécanique générale", Description = "Maintenance mécanique des équipements" },
                    new Competence { NomCompetence = "Hydraulique", Description = "Systèmes hydrauliques et pneumatiques" },
                    new Competence { NomCompetence = "Automatisme", Description = "Programmation et maintenance des automates" },
                    new Competence { NomCompetence = "Soudure", Description = "Techniques de soudage industriel" },
                    new Competence { NomCompetence = "Usinage", Description = "Techniques d'usinage et de tournage" }
                };

                context.Competences.AddRange(competences);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedSuppliers(ApplicationDbContext context)
        {
            if (!context.Fournisseurs.Any())
            {
                var fournisseurs = new List<Fournisseur>
                {
                    new Fournisseur { NomFournisseur = "TechnoMaint Maroc", Contact = "Hassan Benjelloun", Telephone = "0522123456", Email = "contact@technomaint.ma" },
                    new Fournisseur { NomFournisseur = "Pièces Industrielles SA", Contact = "Laila Amrani", Telephone = "0522234567", Email = "commandes@pieces-ind.ma" },
                    new Fournisseur { NomFournisseur = "Équipements Modernes", Contact = "Omar Tazi", Telephone = "0522345678", Email = "ventes@equip-modernes.ma" },
                    new Fournisseur { NomFournisseur = "Maintenance Pro", Contact = "Nadia Berrada", Telephone = "0522456789", Email = "info@maintenance-pro.ma" }
                };

                context.Fournisseurs.AddRange(fournisseurs);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedWorkflows(ApplicationDbContext context)
        {
            if (!context.Workflows.Any())
            {
                var workflows = new List<Workflow>
                {
                    new Workflow { NomWorkflow = "Workflow Standard OT", Description = "Processus standard pour les ordres de travail" },
                    new Workflow { NomWorkflow = "Workflow Urgence", Description = "Processus accéléré pour les interventions urgentes" },
                    new Workflow { NomWorkflow = "Workflow Validation DI", Description = "Processus de validation des demandes d'intervention" }
                };

                context.Workflows.AddRange(workflows);
                await context.SaveChangesAsync();

                // Add workflow steps
                var etapesWorkflow = new List<EtapeWorkflow>
                {
                    // Standard OT Workflow
                    new EtapeWorkflow { WorkflowId = workflows[0].Id, NomEtape = "Création", Ordre = 1, Description = "Création de l'ordre de travail" },
                    new EtapeWorkflow { WorkflowId = workflows[0].Id, NomEtape = "Planification", Ordre = 2, Description = "Planification des ressources" },
                    new EtapeWorkflow { WorkflowId = workflows[0].Id, NomEtape = "Exécution", Ordre = 3, Description = "Exécution des travaux" },
                    new EtapeWorkflow { WorkflowId = workflows[0].Id, NomEtape = "Validation", Ordre = 4, Description = "Validation et clôture" },
                    
                    // Emergency Workflow
                    new EtapeWorkflow { WorkflowId = workflows[1].Id, NomEtape = "Alerte", Ordre = 1, Description = "Déclenchement de l'alerte" },
                    new EtapeWorkflow { WorkflowId = workflows[1].Id, NomEtape = "Intervention immédiate", Ordre = 2, Description = "Intervention d'urgence" },
                    new EtapeWorkflow { WorkflowId = workflows[1].Id, NomEtape = "Rapport", Ordre = 3, Description = "Rapport d'intervention" },
                    
                    // DI Validation Workflow
                    new EtapeWorkflow { WorkflowId = workflows[2].Id, NomEtape = "Soumission", Ordre = 1, Description = "Soumission de la demande" },
                    new EtapeWorkflow { WorkflowId = workflows[2].Id, NomEtape = "Évaluation", Ordre = 2, Description = "Évaluation technique" },
                    new EtapeWorkflow { WorkflowId = workflows[2].Id, NomEtape = "Approbation", Ordre = 3, Description = "Approbation managériale" },
                    new EtapeWorkflow { WorkflowId = workflows[2].Id, NomEtape = "Création OT", Ordre = 4, Description = "Création de l'ordre de travail" }
                };

                context.EtapesWorkflow.AddRange(etapesWorkflow);
                await context.SaveChangesAsync();
            }
        }
    }
}

