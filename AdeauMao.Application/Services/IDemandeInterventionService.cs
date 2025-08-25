using AdeauMao.Application.DTOs;

namespace AdeauMao.Application.Services
{
    public interface IDemandeInterventionService
    {
        Task<ApiResponseDto<PagedResultDto<DemandeInterventionDto>>> GetDemandesInterventionAsync(SearchFilterDto filter);
        Task<ApiResponseDto<DemandeInterventionDto>> GetDemandeInterventionByIdAsync(int id);
        Task<ApiResponseDto<DemandeInterventionDto>> CreateDemandeInterventionAsync(CreateDemandeInterventionDto createDto);
        Task<ApiResponseDto<DemandeInterventionDto>> UpdateDemandeInterventionAsync(UpdateDemandeInterventionDto updateDto);
        Task<ApiResponseDto> DeleteDemandeInterventionAsync(int id);
        Task<ApiResponseDto<DemandeInterventionDto>> UpdateStatutAsync(UpdateStatutDemandeDto updateDto);
        Task<ApiResponseDto<IEnumerable<DemandeInterventionDto>>> GetDemandesInterventionByEquipementAsync(int equipementId);
        Task<ApiResponseDto<IEnumerable<DemandeInterventionDto>>> GetDemandesInterventionByDemandeurAsync(int demandeurId);
        Task<ApiResponseDto<IEnumerable<DemandeInterventionDto>>> GetDemandesInterventionByStatutAsync(string statut);
        Task<ApiResponseDto<IEnumerable<DemandeInterventionDto>>> GetDemandesInterventionByPrioriteAsync(string priorite);
        Task<ApiResponseDto<OrdresDeTravailDto>> CreateOrdreDeTravailFromDemandeAsync(int demandeId, CreateOrdresDeTravailDto createOtDto);
    }
}

