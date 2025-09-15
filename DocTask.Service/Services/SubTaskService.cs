using DocTask.Core.Dtos.SubTasks;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Paginations;
using DocTask.Service.Mappers;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Service.Services;

public class SubTaskService : ISubTaskService
{
    private readonly ISubTaskRepository _subTaskRepository;

    public SubTaskService(ISubTaskRepository subTaskRepository, ITaskRepository taskRepository)
    {
        _subTaskRepository = subTaskRepository;
    }

    public async Task<SubTaskDto?> GetByIdAsync(int subTaskId)
    {
        var subTask = await _subTaskRepository.GetByIdAsync(subTaskId);
        return subTask != null ? SubTaskMapper.ToSubTaskDto(subTask) : null;
    }


    public async Task<TaskEntity> CreateAsync(int parentTaskId, CreateSubTaskRequest request)
    {
        // Xác thực Task cha có không ?
        var parentTask = await _subTaskRepository.GetByIdAsync(parentTaskId);
        if (parentTask == null)
            throw new ArgumentException("Task cha không có ");
        //Tạo Subtask
        var subTaskEntity = SubTaskMapper.ToEntity(request);
        subTaskEntity.ParentTaskId = parentTaskId;
        return await _subTaskRepository.CreateAsync(subTaskEntity);
    }

    public async Task<SubTaskDto?> UpdateSubtask(int parentTaskId, int subTaskId, UpdateSubTaskRequest request)
    {
        var existingSubTask = await _subTaskRepository.GetBySubIdAsync(parentTaskId, subTaskId);
        if (existingSubTask == null)
            return null;

        // // NO VALIDATION - Cập nhật subtask không kiểm tra ngày tháng
        // SubTaskMapper.UpdateEntity(existingSubTask, request);
        // return await _subTaskRepository.UpdateSubtask(parentTaskId, existingSubTask);

        SubTaskMapper.UpdateEntity(existingSubTask, request);
        var updatedSubtask = await _subTaskRepository.UpdateSubtask(parentTaskId, subTaskId, existingSubTask);
        return updatedSubtask != null ? SubTaskMapper.ToSubTaskDto(updatedSubtask) : null;
    }

    public async Task<bool> DeleteAsync(int subTaskId)
    {
        return await _subTaskRepository.DeleteAsync(subTaskId);
    }
    public async Task<PaginatedList<SubTaskDto>> GetAllByParentIdAsync(int parentTaskId, PageOptionsRequest pageOptions)
    {
        var paginatedSubTasks = await _subTaskRepository.GetAllByParentIdPaginatedAsync(parentTaskId, pageOptions);
        var dtoList = paginatedSubTasks.Items.Select(SubTaskMapper.ToSubTaskDto).ToList();

        return new PaginatedList<SubTaskDto>(dtoList, paginatedSubTasks.MetaData);
    }

    public async Task<List<SubTaskDto>> GetByAssigneeIdAsync(int assigneeId)
    {
        var subTasks = await _subTaskRepository.GetByAssigneeIdAsync(assigneeId);
        return subTasks.Select(SubTaskMapper.ToSubTaskDto).ToList();
    }

    public async Task<PaginatedList<SubTaskDto>> GetByAssigneeIdPaginatedAsync(int assigneeId, PageOptionsRequest pageOptions)
    {
        var paginatedSubTasks = await _subTaskRepository.GetByAssigneeIdPaginatedAsync(assigneeId, pageOptions);
        var dtoList = paginatedSubTasks.Items.Select(SubTaskMapper.ToSubTaskDto).ToList();

        return new PaginatedList<SubTaskDto>(dtoList, paginatedSubTasks.MetaData);
    }

    public async Task<List<SubTaskDto>> GetByKeywordAsync(string keyword)
    {
        var subTasks = await _subTaskRepository.GetByKeywordAsync(keyword);
        return subTasks.Select(SubTaskMapper.ToSubTaskDto).ToList();
    }
}