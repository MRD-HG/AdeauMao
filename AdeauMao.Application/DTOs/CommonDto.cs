using System.ComponentModel.DataAnnotations;

namespace AdeauMao.Application.DTOs
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class SearchFilterDto
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class SiteDto
    {
        public int Id { get; set; }
        public string NomSite { get; set; } = string.Empty;
        public string? Adresse { get; set; }
        public DateTime DateCreation { get; set; }
        public ICollection<LigneProductionDto>? LignesProduction { get; set; }
    }

    public class CreateSiteDto
    {
        [Required(ErrorMessage = "Le nom du site est requis")]
        [StringLength(100, ErrorMessage = "Le nom du site ne peut pas dépasser 100 caractères")]
        public string NomSite { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "L'adresse ne peut pas dépasser 255 caractères")]
        public string? Adresse { get; set; }
    }

    public class LigneProductionDto
    {
        public int Id { get; set; }
        public string NomLigne { get; set; } = string.Empty;
        public int SiteId { get; set; }
        public string? SiteNom { get; set; }
        public DateTime DateCreation { get; set; }
        public ICollection<EquipementDto>? Equipements { get; set; }
    }

    public class CreateLigneProductionDto
    {
        [Required(ErrorMessage = "Le nom de la ligne est requis")]
        [StringLength(100, ErrorMessage = "Le nom de la ligne ne peut pas dépasser 100 caractères")]
        public string NomLigne { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID du site est requis")]
        public int SiteId { get; set; }
    }

    public class CategorieDto
    {
        public int Id { get; set; }
        public string NomCategorie { get; set; } = string.Empty;
        public string TypeCategorie { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
    }

    public class CreateCategorieDto
    {
        [Required(ErrorMessage = "Le nom de la catégorie est requis")]
        [StringLength(100, ErrorMessage = "Le nom de la catégorie ne peut pas dépasser 100 caractères")]
        public string NomCategorie { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le type de catégorie est requis")]
        [StringLength(50, ErrorMessage = "Le type de catégorie ne peut pas dépasser 50 caractères")]
        public string TypeCategorie { get; set; } = string.Empty;
    }

    public class FournisseurDto
    {
        public int Id { get; set; }
        public string NomFournisseur { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreation { get; set; }
    }

    public class CreateFournisseurDto
    {
        [Required(ErrorMessage = "Le nom du fournisseur est requis")]
        [StringLength(255, ErrorMessage = "Le nom du fournisseur ne peut pas dépasser 255 caractères")]
        public string NomFournisseur { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Le contact ne peut pas dépasser 255 caractères")]
        public string? Contact { get; set; }

        [StringLength(50, ErrorMessage = "Le téléphone ne peut pas dépasser 50 caractères")]
        public string? Telephone { get; set; }

        [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string? Email { get; set; }
    }

    public class SousTraitantDto
    {
        public int Id { get; set; }
        public string NomSousTraitant { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreation { get; set; }
    }

    public class CreateSousTraitantDto
    {
        [Required(ErrorMessage = "Le nom du sous-traitant est requis")]
        [StringLength(255, ErrorMessage = "Le nom du sous-traitant ne peut pas dépasser 255 caractères")]
        public string NomSousTraitant { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Le contact ne peut pas dépasser 255 caractères")]
        public string? Contact { get; set; }

        [StringLength(50, ErrorMessage = "Le téléphone ne peut pas dépasser 50 caractères")]
        public string? Telephone { get; set; }

        [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string? Email { get; set; }
    }
}

