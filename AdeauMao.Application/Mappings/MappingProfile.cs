using AutoMapper;
using AdeauMao.Core.Entities;
using AdeauMao.Application.DTOs;
using AdeauMao.Application.DTOs.Auth;

namespace AdeauMao.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Equipment mappings
            CreateMap<Equipement, EquipementDto>()
                .ForMember(dest => dest.LigneProductionNom, opt => opt.MapFrom(src => src.LigneProduction != null ? src.LigneProduction.NomLigne : null));
            
            CreateMap<CreateEquipementDto, Equipement>();
            CreateMap<UpdateEquipementDto, Equipement>();

            // Organe mappings
            CreateMap<Organe, OrganeDto>();
            CreateMap<CreateOrganeDto, Organe>();

            // Employee mappings
            CreateMap<Employe, EmployeDto>()
                .ForMember(dest => dest.UtilisateurNom, opt => opt.MapFrom(src => src.Utilisateur != null ? src.Utilisateur.NomUtilisateur : null))
                .ForMember(dest => dest.Competences, opt => opt.MapFrom(src => src.EmployeCompetences.Select(ec => ec.Competence)))
                .ForMember(dest => dest.Equipes, opt => opt.MapFrom(src => src.EquipeMembres.Select(em => em.Equipe)));
            
            CreateMap<CreateEmployeDto, Employe>();
            CreateMap<UpdateEmployeDto, Employe>();

            // Competence mappings
            CreateMap<Competence, CompetenceDto>();
            CreateMap<CreateCompetenceDto, Competence>();

            // Team mappings
            CreateMap<Equipe, EquipeDto>()
                .ForMember(dest => dest.ResponsableNom, opt => opt.MapFrom(src => $"{src.Responsable.Prenom} {src.Responsable.Nom}"))
                .ForMember(dest => dest.Membres, opt => opt.MapFrom(src => src.EquipeMembres.Select(em => em.Employe)));
            
            CreateMap<CreateEquipeDto, Equipe>();

            // Work Orders mappings
            CreateMap<OrdresDeTravail, OrdresDeTravailDto>()
                .ForMember(dest => dest.EquipementNom, opt => opt.MapFrom(src => src.Equipement.Nom))
                .ForMember(dest => dest.EquipementReference, opt => opt.MapFrom(src => src.Equipement.Reference))
                .ForMember(dest => dest.TechnicienNom, opt => opt.MapFrom(src => src.TechnicienAssignee != null ? $"{src.TechnicienAssignee.Prenom} {src.TechnicienAssignee.Nom}" : null))
                .ForMember(dest => dest.CausePanneNom, opt => opt.MapFrom(src => src.CausePanne != null ? src.CausePanne.NomCause : null))
                .ForMember(dest => dest.OrganeNom, opt => opt.MapFrom(src => src.Organe != null ? src.Organe.NomOrgane : null))
                .ForMember(dest => dest.SousTraitantNom, opt => opt.MapFrom(src => src.SousTraitant != null ? src.SousTraitant.NomSousTraitant : null))
                .ForMember(dest => dest.ValidateurNom, opt => opt.MapFrom(src => src.Validateur != null ? src.Validateur.NomUtilisateur : null))
                .ForMember(dest => dest.WorkflowNom, opt => opt.MapFrom(src => src.Workflow != null ? src.Workflow.NomWorkflow : null));
            
            CreateMap<CreateOrdresDeTravailDto, OrdresDeTravail>();
            CreateMap<UpdateOrdresDeTravailDto, OrdresDeTravail>();

            // Intervention Request mappings
            CreateMap<DemandeIntervention, DemandeInterventionDto>()
                .ForMember(dest => dest.EquipementNom, opt => opt.MapFrom(src => src.Equipement.Nom))
                .ForMember(dest => dest.EquipementReference, opt => opt.MapFrom(src => src.Equipement.Reference))
                .ForMember(dest => dest.DemandeurNom, opt => opt.MapFrom(src => $"{src.Demandeur.Prenom} {src.Demandeur.Nom}"));
            
            CreateMap<CreateDemandeInterventionDto, DemandeIntervention>()
                .ForMember(dest => dest.DateDemande, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => StatutDemande.Nouvelle));
            
            CreateMap<UpdateDemandeInterventionDto, DemandeIntervention>();

            // Site mappings
            CreateMap<Site, SiteDto>();
            CreateMap<CreateSiteDto, Site>();

            // Production Line mappings
            CreateMap<LigneProduction, LigneProductionDto>()
                .ForMember(dest => dest.SiteNom, opt => opt.MapFrom(src => src.Site.NomSite));
            
            CreateMap<CreateLigneProductionDto, LigneProduction>();

            // Category mappings
            CreateMap<Categorie, CategorieDto>()
                .ForMember(dest => dest.TypeCategorie, opt => opt.MapFrom(src => src.TypeCategorie.ToString()));
            
            CreateMap<CreateCategorieDto, Categorie>()
                .ForMember(dest => dest.TypeCategorie, opt => opt.MapFrom(src => Enum.Parse<TypeCategorie>(src.TypeCategorie)));

            // Supplier mappings
            CreateMap<Fournisseur, FournisseurDto>();
            CreateMap<CreateFournisseurDto, Fournisseur>();

            // Subcontractor mappings
            CreateMap<SousTraitant, SousTraitantDto>();
            CreateMap<CreateSousTraitantDto, SousTraitant>();

            // User mappings
            CreateMap<Utilisateur, UserInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NomUtilisateur))
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be populated separately

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NomRole));

            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NomPermission));
        }
    }

    // Additional DTOs for roles and permissions
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
    }

    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
    }
}

