using DocTask.Core.Models;

namespace DocTask.Core.Dtos.SubTasks
{
    public class SubTaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? AssignerId { get; set; }
        public int? AssigneeId { get; set; }
        public int? OrgId { get; set; }
        public int? PeriodId { get; set; }
        public int? AttachedFile { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UnitId { get; set; }
        public int? FrequencyId { get; set; }
        public int? Percentagecomplete { get; set; }
        public int? ParentTaskId { get; set; }
    }

    public class CreateSubTaskRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? AssignerId { get; set; }
        public int? AssigneeId { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Frequency { get; set; }
        public int? Percentagecomplete { get; set; }
    }

    public class UpdateSubTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? AssigneeId { get; set; }
        public int? AssignerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}