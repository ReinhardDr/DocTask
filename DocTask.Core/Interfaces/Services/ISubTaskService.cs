using DocTask.Core.Dtos.SubTasks;
using DocTask.Core.Models;
using DocTask.Core.Paginations;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Core.Interfaces.Services;

public interface ISubTaskService
{
    // Basic CRUD operations
    Task<SubTaskDto?> GetByIdAsync(int subTaskId);
    Task<TaskEntity> CreateAsync(int parentTaskId, CreateSubTaskRequest request);
    Task<SubTaskDto?> UpdateSubtask(int parentTaskId, int subtaskId, UpdateSubTaskRequest request);

    // Task<SubTaskDto?> UpdateSubtask(int parentTaskId, int subtaskId);
    Task<bool> DeleteAsync(int subTaskId);

    // Query operations
    Task<PaginatedList<SubTaskDto>> GetAllByParentIdAsync(int parentTaskId, PageOptionsRequest pageOptions);
    Task<List<SubTaskDto>> GetByAssigneeIdAsync(int assigneeId);
    Task<PaginatedList<SubTaskDto>> GetByAssigneeIdPaginatedAsync(int assigneeId, PageOptionsRequest pageOptions);
    Task<List<SubTaskDto>> GetByKeywordAsync(string keyword);
}