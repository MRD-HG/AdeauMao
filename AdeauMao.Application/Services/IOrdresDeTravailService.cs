using AdeauMao.Application.DTOs;

namespace AdeauMao.Application.Services
{
    public interface IOrdresDeTravailService
    {
        Task<ApiResponseDto<PagedResultDto<OrdresDeTravailDto>>> GetOrdresDeTravailAsync(SearchFilterDto filter);
        Task<ApiResponseDto<OrdresDeTravailDto>> GetOrdreDeTravailByIdAsync(int id);
        Task<ApiResponseDto<OrdresDeTravailDto>> GetOrdreDeTravailByNumeroAsync(string numero);
        Task<ApiResponseDto<OrdresDeTravailDto>> CreateOrdreDeTravailAsync(CreateOrdresDeTravailDto createDto);
        Task<ApiResponseDto<OrdresDeTravailDto>> UpdateOrdreDeTravailAsync(UpdateOrdresDeTravailDto updateDto);
        Task<ApiResponseDto> DeleteOrdreDeTravailAsync(int id);
        Task<ApiResponseDto<OrdresDeTravailDto>> UpdateProgressionAsync(UpdateOTProgressionDto updateDto);
        Task<ApiResponseDto<OrdresDeTravailDto>> ValidateOrdreDeTravailAsync(ValidateOTDto validateDto);
        Task<ApiResponseDto<IEnumerable<OrdresDeTravailDto>>> GetOrdresDeTravailByEquipementAsync(int equipementId);
        Task<ApiResponseDto<IEnumerable<OrdresDeTravailDto>>> GetOrdresDeTravailByTechnicienAsync(int technicienId);
        Task<ApiResponseDto<IEnumerable<OrdresDeTravailDto>>> GetOrdresDeTravailByStatutAsync(string statut);
        Task<ApiResponseDto<IEnumerable<OrdresDeTravailDto>>> GetOrdresDeTravailByPrioriteAsync(string priorite);
        Task<ApiResponseDto<string>> GenerateNumeroOTAsync();
    }
}

