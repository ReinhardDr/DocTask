using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.SubTasks;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubTaskModel = DocTask.Core.Models.Task;

namespace DocTask.Api.Controllers
{
    [ApiController]
    [Route("api/v1/subtask")]
    public class SubTaskController : ControllerBase
    {
        private readonly ISubTaskService _subTaskService;

        public SubTaskController(ISubTaskService subTaskService)
        {
            _subTaskService = subTaskService;
        }

        // POST: api/v1/subtask/{parentTaskId} - Tạo subtask mới
        [HttpPost("subtask/{parentTaskId}")]
        public async Task<IActionResult> CreateSubTask(int parentTaskId, [FromBody] CreateSubTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Error = "Dữ liệu không hợp lệ"
                });
            }

            try
            {
                var subTask = await _subTaskService.CreateAsync(parentTaskId, request);
                return Ok(new ApiResponse<SubTaskModel>
                {
                    Success = true,
                    Data = subTask,
                    Message = "Tạo subtask thành công"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Error = $"Lỗi khi tạo subtask: {ex.Message}"
                });
            }
        }

        // GET: api/v1/subtask?key=search_keyword&parentTaskId=32&page=1&size=10
        [HttpGet("subtask")]
        public async Task<IActionResult> GetSubTasks(
            [FromQuery] string? key = null,
            [FromQuery] int? parentTaskId = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            var pageOptions = new PageOptionsRequest { Page = page, Size = size };

            // Search by keyword
            if (!string.IsNullOrWhiteSpace(key))
            {
                var searchResults = await _subTaskService.GetByKeywordAsync(key);
                return Ok(new ApiResponse<List<SubTaskDto>>
                {
                    Success = true,
                    Data = searchResults,
                    Message = $"Tìm thấy {searchResults.Count} subtasks với từ khóa '{key}'"
                });
            }

            // Get by parent task ID with pagination
            if (parentTaskId.HasValue)
            {
                var subtasks = await _subTaskService.GetAllByParentIdAsync(parentTaskId.Value, pageOptions);
                return Ok(new ApiResponse<PaginatedList<SubTaskDto>>
                {
                    Success = true,
                    Data = subtasks,
                    Message = "Lấy danh sách subtasks thành công"
                });
            }

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Error = "Vui lòng cung cấp parentTaskId hoặc key để tìm kiếm"
            });
        }

        // GET: api/v1/subtask/assigned?assigneeId=2&page=1&size=10
        [HttpGet("subtask/assigned")]
        public async Task<IActionResult> GetAssignedSubTasks(
            [FromQuery] int assigneeId,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            if (assigneeId <= 0)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Error = "AssigneeId không hợp lệ"
                });
            }

            var pageOptions = new PageOptionsRequest { Page = page, Size = size };
            var assignedSubTasks = await _subTaskService.GetByAssigneeIdPaginatedAsync(assigneeId, pageOptions);

            return Ok(new ApiResponse<PaginatedList<SubTaskDto>>
            {
                Success = true,
                Data = assignedSubTasks,
                Message = $"Lấy danh sách subtasks được giao cho user {assigneeId} thành công"
            });
        }


        // PUT: api/v1/{parentTaskId}/subtask?subTaskId=33
        [HttpPut("{parentTaskId}/subtask")]
        public async Task<IActionResult> UpdateSubTask(int parentTaskId, [FromQuery] int subTaskId, [FromBody] UpdateSubTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Error = "Dữ liệu không hợp lệ"
                });
            }

            try
            {
                var updatedSubTask = await _subTaskService.UpdateSubtask(parentTaskId, subTaskId, request);
                if (updatedSubTask == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Error = "Subtask không tồn tại"
                    });
                }

                return Ok(new ApiResponse<SubTaskDto>
                {
                    Success = true,
                    Data = updatedSubTask,
                    Message = "Cập nhật subtask thành công"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Error = $"Lỗi khi cập nhật subtask: {ex.Message}"
                });
            }
        }

        // DELETE: api/v1/subtask?subTaskId=33
        [HttpDelete("subtask")]
        public async Task<IActionResult> DeleteSubTask([FromQuery] int subTaskId)
        {
            if (subTaskId <= 0)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Error = "SubTaskId không hợp lệ"
                });
            }
            try
            {
                var success = await _subTaskService.DeleteAsync(subTaskId);
                if (!success)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Error = "Subtask không tồn tại"
                    });
                }

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Xóa subtask thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Error = $"Lỗi khi xóa subtask: {ex.Message}"
                });
            }
        }
    }
}