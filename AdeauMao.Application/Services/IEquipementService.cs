using AdeauMao.Application.DTOs;

namespace AdeauMao.Application.Services
{
    public interface IEquipementService
    {
        Task<ApiResponseDto<PagedResultDto<EquipementDto>>> GetEquipementsAsync(SearchFilterDto filter);
        Task<ApiResponseDto<EquipementDto>> GetEquipementByIdAsync(int id);
        Task<ApiResponseDto<EquipementDto>> GetEquipementByReferenceAsync(string reference);
        Task<ApiResponseDto<EquipementDto>> CreateEquipementAsync(CreateEquipementDto createDto);
        Task<ApiResponseDto<EquipementDto>> UpdateEquipementAsync(UpdateEquipementDto updateDto);
        Task<ApiResponseDto> DeleteEquipementAsync(int id);
        Task<ApiResponseDto<IEnumerable<EquipementDto>>> GetEquipementsByLigneProductionAsync(int ligneProductionId);
        Task<ApiResponseDto<IEnumerable<EquipementDto>>> GetEquipementsByTypeAsync(string type);
        Task<ApiResponseDto<IEnumerable<OrganeDto>>> GetOrganesByEquipementAsync(int equipementId);
        Task<ApiResponseDto<OrganeDto>> CreateOrganeAsync(CreateOrganeDto createDto);
        Task<ApiResponseDto> DeleteOrganeAsync(int id);
    }
}

