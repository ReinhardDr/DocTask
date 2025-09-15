using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using DocTask.Core.Paginations;
using Microsoft.EntityFrameworkCore;
using TaskEntity = DocTask.Core.Models.Task;

namespace DocTask.Data.Repositories
{
    public class SubTaskRepository : ISubTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public SubTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaskEntity?> GetByIdAsync(int subTaskId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == subTaskId && t.ParentTaskId == null);
        }
        public async Task<TaskEntity?> GetBySubIdAsync(int parentTaskId, int subTaskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == subTaskId && t.ParentTaskId == parentTaskId);
        }

        public async Task<TaskEntity> CreateAsync(TaskEntity subTask)
        {
            subTask.CreatedAt = DateTime.UtcNow;
            _context.Tasks.Add(subTask);
            await _context.SaveChangesAsync();
            return subTask;
        }
        //Tu dong
        public async Task<TaskEntity?> UpdateSubtask(int parentTaskId, int subTaskId, TaskEntity subtask)
        {
            var existingSubtask = await _context.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == subtask.TaskId && t.ParentTaskId == parentTaskId);
            if (existingSubtask == null)
                return null;

            _context.Entry(existingSubtask).CurrentValues.SetValues(subtask);
            await _context.SaveChangesAsync();
            return existingSubtask;
        }

        public async Task<bool> DeleteAsync(int subTaskId)
        {
            var subTask = await GetByIdAsync(subTaskId);
            if (subTask == null)
                return false;

            _context.Tasks.Remove(subTask);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int subTaskId)
        {
            return await _context.Tasks
                .AnyAsync(t => t.TaskId == subTaskId && t.ParentTaskId != null);
        }

        public async Task<List<TaskEntity>> GetAllByParentIdAsync(int parentTaskId)
        {
            return await _context.Tasks
                .Where(t => t.ParentTaskId == parentTaskId)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<PaginatedList<TaskEntity>> GetAllByParentIdPaginatedAsync(int parentTaskId, PageOptionsRequest pageOptions)
        {
            var query = _context.Tasks
                .Where(t => t.ParentTaskId == parentTaskId)
                .OrderBy(t => t.CreatedAt);

            return await query.ToPaginatedListAsync(pageOptions);
        }

        public async Task<List<TaskEntity>> GetByAssigneeIdAsync(int assigneeId)
        {
            return await _context.Tasks
                .Where(t => t.AssigneeId == assigneeId && t.ParentTaskId != null)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<PaginatedList<TaskEntity>> GetByAssigneeIdPaginatedAsync(int assigneeId, PageOptionsRequest pageOptions)
        {
            var query = _context.Tasks
                .Where(t => t.AssigneeId == assigneeId && t.ParentTaskId != null)
                .OrderBy(t => t.DueDate);

            return await query.ToPaginatedListAsync(pageOptions);
        }

        public async Task<List<TaskEntity>> GetByKeywordAsync(string keyword)
        {
            return await _context.Tasks
                .Where(t => t.ParentTaskId != null &&
                           (t.Title.Contains(keyword) ||
                            (t.Description != null && t.Description.Contains(keyword))))
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

    }
}