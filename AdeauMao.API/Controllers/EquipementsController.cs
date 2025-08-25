using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdeauMao.Application.Services;
using AdeauMao.Application.DTOs;

namespace AdeauMao.API.Controllers
{
    [Authorize]
    public class EquipementsController : BaseController
    {
        private readonly IEquipementService _equipementService;

        public EquipementsController(IEquipementService equipementService)
        {
            _equipementService = equipementService;
        }

        /// <summary>
        /// Get all equipments with pagination and filtering
        /// </summary>
        /// <returns>Paginated list of equipments</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<PagedResultDto<EquipementDto>>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEquipements()
        {
            try
            {
                var filter = GetSearchFilter();
                var result = await _equipementService.GetEquipementsAsync(filter);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "getting equipments");
            }
        }

        /// <summary>
        /// Get equipment by ID
        /// </summary>
        /// <param name="id">Equipment ID</param>
        /// <returns>Equipment details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<EquipementDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEquipement(int id)
        {
            try
            {
                var result = await _equipementService.GetEquipementByIdAsync(id);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"getting equipment {id}");
            }
        }

        /// <summary>
        /// Get equipment by reference
        /// </summary>
        /// <param name="reference">Equipment reference</param>
        /// <returns>Equipment details</returns>
        [HttpGet("reference/{reference}")]
        [ProducesResponseType(typeof(ApiResponseDto<EquipementDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEquipementByReference(string reference)
        {
            try
            {
                var result = await _equipementService.GetEquipementByReferenceAsync(reference);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"getting equipment by reference {reference}");
            }
        }

        /// <summary>
        /// Create a new equipment
        /// </summary>
        /// <param name="createDto">Equipment creation data</param>
        /// <returns>Created equipment</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<EquipementDto>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> CreateEquipement([FromBody] CreateEquipementDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _equipementService.CreateEquipementAsync(createDto);
                
                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetEquipement), new { id = result.Data?.Id }, result);
                }

                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "creating equipment");
            }
        }

        /// <summary>
        /// Update an existing equipment
        /// </summary>
        /// <param name="id">Equipment ID</param>
        /// <param name="updateDto">Equipment update data</param>
        /// <returns>Updated equipment</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<EquipementDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateEquipement(int id, [FromBody] UpdateEquipementDto updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return BadRequest("L'ID dans l'URL ne correspond pas à l'ID dans les données");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _equipementService.UpdateEquipementAsync(updateDto);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"updating equipment {id}");
            }
        }

        /// <summary>
        /// Delete an equipment
        /// </summary>
        /// <param name="id">Equipment ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteEquipement(int id)
        {
            try
            {
                var result = await _equipementService.DeleteEquipementAsync(id);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"deleting equipment {id}");
            }
        }

        /// <summary>
        /// Get equipments by production line
        /// </summary>
        /// <param name="ligneProductionId">Production line ID</param>
        /// <returns>List of equipments</returns>
        [HttpGet("ligne-production/{ligneProductionId}")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EquipementDto>>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEquipementsByLigneProduction(int ligneProductionId)
        {
            try
            {
                var result = await _equipementService.GetEquipementsByLigneProductionAsync(ligneProductionId);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"getting equipments for production line {ligneProductionId}");
            }
        }

        /// <summary>
        /// Get equipments by type
        /// </summary>
        /// <param name="type">Equipment type</param>
        /// <returns>List of equipments</returns>
        [HttpGet("type/{type}")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EquipementDto>>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEquipementsByType(string type)
        {
            try
            {
                var result = await _equipementService.GetEquipementsByTypeAsync(type);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"getting equipments by type {type}");
            }
        }

        /// <summary>
        /// Get organes for an equipment
        /// </summary>
        /// <param name="equipementId">Equipment ID</param>
        /// <returns>List of organes</returns>
        [HttpGet("{equipementId}/organes")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OrganeDto>>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetOrganesByEquipement(int equipementId)
        {
            try
            {
                var result = await _equipementService.GetOrganesByEquipementAsync(equipementId);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"getting organes for equipment {equipementId}");
            }
        }

        /// <summary>
        /// Create a new organe for an equipment
        /// </summary>
        /// <param name="createDto">Organe creation data</param>
        /// <returns>Created organe</returns>
        [HttpPost("organes")]
        [Authorize(Roles = "Administrator,Manager,Technician")]
        [ProducesResponseType(typeof(ApiResponseDto<OrganeDto>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> CreateOrgane([FromBody] CreateOrganeDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _equipementService.CreateOrganeAsync(createDto);
                
                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetOrganesByEquipement), 
                        new { equipementId = createDto.EquipementId }, result);
                }

                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "creating organe");
            }
        }

        /// <summary>
        /// Delete an organe
        /// </summary>
        /// <param name="organeId">Organe ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("organes/{organeId}")]
        [Authorize(Roles = "Administrator,Manager")]
        [ProducesResponseType(typeof(ApiResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteOrgane(int organeId)
        {
            try
            {
                var result = await _equipementService.DeleteOrganeAsync(organeId);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"deleting organe {organeId}");
            }
        }
    }
}

