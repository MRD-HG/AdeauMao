using AutoMapper;
using Microsoft.Extensions.Logging;
using AdeauMao.Application.Services;
using AdeauMao.Application.DTOs;
using AdeauMao.Core.Entities;
using AdeauMao.Core.Interfaces;

namespace AdeauMao.Infrastructure.Services
{
    public class EquipementService : IEquipementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EquipementService> _logger;

        public EquipementService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EquipementService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponseDto<PagedResultDto<EquipementDto>>> GetEquipementsAsync(SearchFilterDto filter)
        {
            try
            {
                var (equipements, totalCount) = await _unitOfWork.Equipements.GetPagedAsync(
                    filter.PageNumber,
                    filter.PageSize,
                    e => string.IsNullOrEmpty(filter.SearchTerm) || 
                         e.Nom.Contains(filter.SearchTerm) || 
                         e.Reference.Contains(filter.SearchTerm) ||
                         (e.TypeEquipement != null && e.TypeEquipement.Contains(filter.SearchTerm)),
                    orderBy: query => filter.SortDescending 
                        ? query.OrderByDescending(GetSortExpression(filter.SortBy))
                        : query.OrderBy(GetSortExpression(filter.SortBy)),
                    includeProperties: "LigneProduction,Organes"
                );

                var equipementDtos = _mapper.Map<IEnumerable<EquipementDto>>(equipements);

                var pagedResult = new PagedResultDto<EquipementDto>
                {
                    Items = equipementDtos,
                    TotalCount = totalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };

                return new ApiResponseDto<PagedResultDto<EquipementDto>>
                {
                    Success = true,
                    Message = "Équipements récupérés avec succès",
                    Data = pagedResult
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving equipments");
                return new ApiResponseDto<PagedResultDto<EquipementDto>>
                {
                    Success = false,
                    Message = "Erreur lors de la récupération des équipements",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<EquipementDto>> GetEquipementByIdAsync(int id)
        {
            try
            {
                var equipement = await _unitOfWork.Equipements.GetByIdAsync(id);
                if (equipement == null)
                {
                    return new ApiResponseDto<EquipementDto>
                    {
                        Success = false,
                        Message = "Équipement non trouvé"
                    };
                }

                var equipementDto = _mapper.Map<EquipementDto>(equipement);

                return new ApiResponseDto<EquipementDto>
                {
                    Success = true,
                    Message = "Équipement récupéré avec succès",
                    Data = equipementDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving equipment with ID {Id}", id);
                return new ApiResponseDto<EquipementDto>
                {
                    Success = false,
                    Message = "Erreur lors de la récupération de l'équipement",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<EquipementDto>> GetEquipementByReferenceAsync(string reference)
        {
            try
            {
                var equipement = await _unitOfWork.Equipements.SingleOrDefaultAsync(e => e.Reference == reference);
                if (equipement == null)
                {
                    return new ApiResponseDto<EquipementDto>
                    {
                        Success = false,
                        Message = "Équipement non trouvé"
                    };
                }

                var equipementDto = _mapper.Map<EquipementDto>(equipement);

                return new ApiResponseDto<EquipementDto>
                {
                    Success = true,
                    Message = "Équipement récupéré avec succès",
                    Data = equipementDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving equipment with reference {Reference}", reference);
                return new ApiResponseDto<EquipementDto>
                {
                    Success = false,
                    Message = "Erreur lors de la récupération de l'équipement",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<EquipementDto>> CreateEquipementAsync(CreateEquipementDto createDto)
        {
            try
            {
                // Check if reference already exists
                var existingEquipement = await _unitOfWork.Equipements.SingleOrDefaultAsync(e => e.Reference == createDto.Reference);
                if (existingEquipement != null)
                {
                    return new ApiResponseDto<EquipementDto>
                    {
                        Success = false,
                        Message = "Un équipement avec cette référence existe déjà"
                    };
                }

                var equipement = _mapper.Map<Equipement>(createDto);
                await _unitOfWork.Equipements.AddAsync(equipement);
                await _unitOfWork.SaveChangesAsync();

                var equipementDto = _mapper.Map<EquipementDto>(equipement);

                _logger.LogInformation("Equipment created with ID {Id} and reference {Reference}", equipement.Id, equipement.Reference);

                return new ApiResponseDto<EquipementDto>
                {
                    Success = true,
                    Message = "Équipement créé avec succès",
                    Data = equipementDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating equipment");
                return new ApiResponseDto<EquipementDto>
                {
                    Success = false,
                    Message = "Erreur lors de la création de l'équipement",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<EquipementDto>> UpdateEquipementAsync(UpdateEquipementDto updateDto)
        {
            try
            {
                var equipement = await _unitOfWork.Equipements.GetByIdAsync(updateDto.Id);
                if (equipement == null)
                {
                    return new ApiResponseDto<EquipementDto>
                    {
                        Success = false,
                        Message = "Équipement non trouvé"
                    };
                }

                // Check if reference already exists for another equipment
                var existingEquipement = await _unitOfWork.Equipements.SingleOrDefaultAsync(e => e.Reference == updateDto.Reference && e.Id != updateDto.Id);
                if (existingEquipement != null)
                {
                    return new ApiResponseDto<EquipementDto>
                    {
                        Success = false,
                        Message = "Un autre équipement avec cette référence existe déjà"
                    };
                }

                _mapper.Map(updateDto, equipement);
                await _unitOfWork.Equipements.UpdateAsync(equipement);
                await _unitOfWork.SaveChangesAsync();

                var equipementDto = _mapper.Map<EquipementDto>(equipement);

                _logger.LogInformation("Equipment updated with ID {Id}", equipement.Id);

                return new ApiResponseDto<EquipementDto>
                {
                    Success = true,
                    Message = "Équipement mis à jour avec succès",
                    Data = equipementDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating equipment with ID {Id}", updateDto.Id);
                return new ApiResponseDto<EquipementDto>
                {
                    Success = false,
                    Message = "Erreur lors de la mise à jour de l'équipement",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto> DeleteEquipementAsync(int id)
        {
            try
            {
                var equipement = await _unitOfWork.Equipements.GetByIdAsync(id);
                if (equipement == null)
                {
                    return new ApiResponseDto
                    {
                        Success = false,
                        Message = "Équipement non trouvé"
                    };
                }

                // Check if equipment has related work orders or intervention requests
                var hasWorkOrders = await _unitOfWork.OrdresDeTravail.ExistsAsync(ot => ot.EquipementId == id);
                var hasInterventionRequests = await _unitOfWork.DemandesIntervention.ExistsAsync(di => di.EquipementId == id);

                if (hasWorkOrders || hasInterventionRequests)
                {
                    return new ApiResponseDto
                    {
                        Success = false,
                        Message = "Impossible de supprimer cet équipement car il est référencé dans des ordres de travail ou des demandes d'intervention"
                    };
                }

                await _unitOfWork.Equipements.DeleteAsync(equipement);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Equipment deleted with ID {Id}", id);

                return new ApiResponseDto
                {
                    Success = true,
                    Message = "Équipement supprimé avec succès"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting equipment with ID {Id}", id);
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Erreur lors de la suppression de l'équipement",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<IEnumerable<EquipementDto>>> GetEquipementsByLigneProductionAsync(int ligneProductionId)
        {
            try
            {
                var equipements = await _unitOfWork.Equipements.FindAsync(e => e.LigneProductionId == ligneProductionId);
                var equipementDtos = _mapper.Map<IEnumerable<EquipementDto>>(equipements);

                return new ApiResponseDto<IEnumerable<EquipementDto>>
                {
                    Success = true,
                    Message = "Équipements récupérés avec succès",
                    Data = equipementDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving equipments for production line {LigneProductionId}", ligneProductionId);
                return new ApiResponseDto<IEnumerable<EquipementDto>>
                {
                    Success = false,
                    Message = "Erreur lors de la récupération des équipements",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<IEnumerable<EquipementDto>>> GetEquipementsByTypeAsync(string type)
        {
            try
            {
                var equipements = await _unitOfWork.Equipements.FindAsync(e => e.TypeEquipement == type);
                var equipementDtos = _mapper.Map<IEnumerable<EquipementDto>>(equipements);

                return new ApiResponseDto<IEnumerable<EquipementDto>>
                {
                    Success = true,
                    Message = "Équipements récupérés avec succès",
                    Data = equipementDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving equipments of type {Type}", type);
                return new ApiResponseDto<IEnumerable<EquipementDto>>
                {
                    Success = false,
                    Message = "Erreur lors de la récupération des équipements",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<IEnumerable<OrganeDto>>> GetOrganesByEquipementAsync(int equipementId)
        {
            try
            {
                var organes = await _unitOfWork.Organes.FindAsync(o => o.EquipementId == equipementId);
                var organeDtos = _mapper.Map<IEnumerable<OrganeDto>>(organes);

                return new ApiResponseDto<IEnumerable<OrganeDto>>
                {
                    Success = true,
                    Message = "Organes récupérés avec succès",
                    Data = organeDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving organes for equipment {EquipementId}", equipementId);
                return new ApiResponseDto<IEnumerable<OrganeDto>>
                {
                    Success = false,
                    Message = "Erreur lors de la récupération des organes",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<OrganeDto>> CreateOrganeAsync(CreateOrganeDto createDto)
        {
            try
            {
                // Verify equipment exists
                var equipement = await _unitOfWork.Equipements.GetByIdAsync(createDto.EquipementId);
                if (equipement == null)
                {
                    return new ApiResponseDto<OrganeDto>
                    {
                        Success = false,
                        Message = "Équipement non trouvé"
                    };
                }

                var organe = _mapper.Map<Organe>(createDto);
                await _unitOfWork.Organes.AddAsync(organe);
                await _unitOfWork.SaveChangesAsync();

                var organeDto = _mapper.Map<OrganeDto>(organe);

                _logger.LogInformation("Organe created with ID {Id} for equipment {EquipementId}", organe.Id, organe.EquipementId);

                return new ApiResponseDto<OrganeDto>
                {
                    Success = true,
                    Message = "Organe créé avec succès",
                    Data = organeDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating organe");
                return new ApiResponseDto<OrganeDto>
                {
                    Success = false,
                    Message = "Erreur lors de la création de l'organe",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto> DeleteOrganeAsync(int id)
        {
            try
            {
                var organe = await _unitOfWork.Organes.GetByIdAsync(id);
                if (organe == null)
                {
                    return new ApiResponseDto
                    {
                        Success = false,
                        Message = "Organe non trouvé"
                    };
                }

                // Check if organe has related work orders
                var hasWorkOrders = await _unitOfWork.OrdresDeTravail.ExistsAsync(ot => ot.OrganeId == id);
                if (hasWorkOrders)
                {
                    return new ApiResponseDto
                    {
                        Success = false,
                        Message = "Impossible de supprimer cet organe car il est référencé dans des ordres de travail"
                    };
                }

                await _unitOfWork.Organes.DeleteAsync(organe);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Organe deleted with ID {Id}", id);

                return new ApiResponseDto
                {
                    Success = true,
                    Message = "Organe supprimé avec succès"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting organe with ID {Id}", id);
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "Erreur lors de la suppression de l'organe",
                    Errors = new[] { ex.Message }
                };
            }
        }

        private static System.Linq.Expressions.Expression<Func<Equipement, object>> GetSortExpression(string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "nom" => e => e.Nom,
                "reference" => e => e.Reference,
                "type" => e => e.TypeEquipement ?? "",
                "fabricant" => e => e.Fabricant ?? "",
                "datecreation" => e => e.DateCreation,
                _ => e => e.Id
            };
        }
    }
}

