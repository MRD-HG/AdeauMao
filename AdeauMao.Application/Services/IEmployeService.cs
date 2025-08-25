using AdeauMao.Application.DTOs;

namespace AdeauMao.Application.Services
{
    public interface IEmployeService
    {
        Task<ApiResponseDto<PagedResultDto<EmployeDto>>> GetEmployesAsync(SearchFilterDto filter);
        Task<ApiResponseDto<EmployeDto>> GetEmployeByIdAsync(int id);
        Task<ApiResponseDto<EmployeDto>> CreateEmployeAsync(CreateEmployeDto createDto);
        Task<ApiResponseDto<EmployeDto>> UpdateEmployeAsync(UpdateEmployeDto updateDto);
        Task<ApiResponseDto> DeleteEmployeAsync(int id);
        Task<ApiResponseDto<IEnumerable<EmployeDto>>> GetEmployesByEquipeAsync(int equipeId);
        Task<ApiResponseDto<IEnumerable<EmployeDto>>> GetEmployesByCompetenceAsync(int competenceId);
        Task<ApiResponseDto> AssignEmployeToEquipeAsync(AssignEmployeToEquipeDto assignDto);
        Task<ApiResponseDto> RemoveEmployeFromEquipeAsync(int equipeId, int employeId);
        Task<ApiResponseDto> AssignCompetenceToEmployeAsync(AssignCompetenceToEmployeDto assignDto);
        Task<ApiResponseDto> RemoveCompetenceFromEmployeAsync(int employeId, int competenceId);
        
        // Team management
        Task<ApiResponseDto<PagedResultDto<EquipeDto>>> GetEquipesAsync(SearchFilterDto filter);
        Task<ApiResponseDto<EquipeDto>> GetEquipeByIdAsync(int id);
        Task<ApiResponseDto<EquipeDto>> CreateEquipeAsync(CreateEquipeDto createDto);
        Task<ApiResponseDto> DeleteEquipeAsync(int id);
        
        // Competence management
        Task<ApiResponseDto<PagedResultDto<CompetenceDto>>> GetCompetencesAsync(SearchFilterDto filter);
        Task<ApiResponseDto<CompetenceDto>> GetCompetenceByIdAsync(int id);
        Task<ApiResponseDto<CompetenceDto>> CreateCompetenceAsync(CreateCompetenceDto createDto);
        Task<ApiResponseDto> DeleteCompetenceAsync(int id);
    }
}

