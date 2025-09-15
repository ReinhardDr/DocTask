using DocTask.Core.Dtos.SubTasks;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Service.Mappers
{
    public static class SubTaskMapper
    {
        public static SubTaskDto ToSubTaskDto(TaskEntity subTask)
        {
            return new SubTaskDto
            {
                TaskId = subTask.TaskId,
                Title = subTask.Title,
                Description = subTask.Description,
                AssignerId = subTask.AssignerId,
                AssigneeId = subTask.AssigneeId,
                OrgId = subTask.OrgId,
                PeriodId = subTask.PeriodId,
                AttachedFile = subTask.AttachedFile,
                Status = subTask.Status,
                Priority = subTask.Priority,
                StartDate = subTask.StartDate?.ToDateTime(TimeOnly.MinValue), // Convert DateOnly? to DateTime?
                DueDate = subTask.DueDate?.ToDateTime(TimeOnly.MaxValue),     // Convert DateOnly? to DateTime?
                CreatedAt = subTask.CreatedAt,
                UnitId = subTask.UnitId,
                FrequencyId = subTask.FrequencyId,
                Percentagecomplete = subTask.Percentagecomplete,
                ParentTaskId = subTask.ParentTaskId
            };
        }

        public static TaskEntity ToEntity(CreateSubTaskRequest request)
        {
            return new TaskEntity
            {
                Title = request.Title,
                Description = request.Description,
                AssignerId = request.AssignerId,
                AssigneeId = request.AssigneeId,
                Status = request.Status ?? "pending",
                Priority = request.Priority ?? "medium",
                StartDate = request.StartDate.HasValue ? DateOnly.FromDateTime(request.StartDate.Value) : null, // Convert DateTime? to DateOnly?
                DueDate = request.DueDate.HasValue ? DateOnly.FromDateTime(request.DueDate.Value) : null,     // Convert DateTime? to DateOnly?
                Percentagecomplete = request.Percentagecomplete ?? 0,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateEntity(TaskEntity existingSubTask, UpdateSubTaskRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Title))
                existingSubTask.Title = request.Title;

            if (request.Description != null)
                existingSubTask.Description = request.Description;

            if (request.AssigneeId.HasValue)
                existingSubTask.AssigneeId = request.AssigneeId;

            // if (!string.IsNullOrWhiteSpace(request.Status))
            //     existingSubTask.Status = request.Status;

            // if (!string.IsNullOrWhiteSpace(request.Priority))
            //     existingSubTask.Priority = request.Priority;

            if (request.StartDate.HasValue)
                existingSubTask.StartDate = DateOnly.FromDateTime(request.StartDate.Value);

            if (request.DueDate.HasValue)
                existingSubTask.DueDate = DateOnly.FromDateTime(request.DueDate.Value);

            // if (request.Percentagecomplete.HasValue)
            //     existingSubTask.Percentagecomplete = request.Percentagecomplete;
        }
    }
}