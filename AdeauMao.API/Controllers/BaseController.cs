using Microsoft.AspNetCore.Mvc;
using AdeauMao.Application.DTOs;

namespace AdeauMao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(ApiResponseDto<T> result)
        {
            if (result.Success)
            {
                return Ok(result);
            }

            if (result.Errors != null && result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(result);
        }

        protected IActionResult HandleResult(ApiResponseDto result)
        {
            if (result.Success)
            {
                return Ok(result);
            }

            if (result.Errors != null && result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(result);
        }

        protected IActionResult HandlePagedResult<T>(PagedResultDto<T> result)
        {
            var response = new ApiResponseDto<PagedResultDto<T>>
            {
                Success = true,
                Message = "Données récupérées avec succès",
                Data = result
            };

            return Ok(response);
        }

        protected ApiResponseDto<T> CreateSuccessResponse<T>(T data, string message = "Opération réussie")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        protected ApiResponseDto CreateSuccessResponse(string message = "Opération réussie")
        {
            return new ApiResponseDto
            {
                Success = true,
                Message = message
            };
        }

        protected ApiResponseDto<T> CreateErrorResponse<T>(string message, IEnumerable<string>? errors = null)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        protected ApiResponseDto CreateErrorResponse(string message, IEnumerable<string>? errors = null)
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        protected IActionResult HandleException(Exception ex, string operation)
        {
            var logger = HttpContext.RequestServices.GetService<ILogger<BaseController>>();
            logger?.LogError(ex, "Error during {Operation}", operation);

            var response = CreateErrorResponse("Une erreur interne s'est produite", new[] { ex.Message });
            return StatusCode(500, response);
        }

        protected SearchFilterDto GetSearchFilter()
        {
            var searchTerm = Request.Query["searchTerm"].FirstOrDefault();
            var pageNumber = int.TryParse(Request.Query["pageNumber"].FirstOrDefault(), out var pn) ? pn : 1;
            var pageSize = int.TryParse(Request.Query["pageSize"].FirstOrDefault(), out var ps) ? ps : 10;
            var sortBy = Request.Query["sortBy"].FirstOrDefault();
            var sortDescending = bool.TryParse(Request.Query["sortDescending"].FirstOrDefault(), out var sd) && sd;
            var dateFrom = DateTime.TryParse(Request.Query["dateFrom"].FirstOrDefault(), out var df) ? df : (DateTime?)null;
            var dateTo = DateTime.TryParse(Request.Query["dateTo"].FirstOrDefault(), out var dt) ? dt : (DateTime?)null;

            // Ensure page size is within reasonable limits
            pageSize = Math.Min(Math.Max(pageSize, 1), 100);
            pageNumber = Math.Max(pageNumber, 1);

            return new SearchFilterDto
            {
                SearchTerm = searchTerm,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortBy = sortBy,
                SortDescending = sortDescending,
                DateFrom = dateFrom,
                DateTo = dateTo
            };
        }

        protected string GetCurrentUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        protected string GetCurrentUserName()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "";
        }

        protected IEnumerable<string> GetCurrentUserRoles()
        {
            return User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
        }

        protected bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }

        protected bool IsAdminOrManager()
        {
            return IsInRole("Administrator") || IsInRole("Manager");
        }
    }
}

