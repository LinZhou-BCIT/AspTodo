using AspTodo.Data;
using AspTodo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositoreis
{
    public class TodoItemRepo : ITodoItemRepo
    {
        ApplicationDbContext _context;
        public TodoItemRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TodoItemAPIModel> CreateItemAsync(TodoItemAPIModel newItemModel)
        {
            int largestOrder = await GetLargestOrderAsync(newItemModel.ListID);
            TodoItem newItem = new TodoItem()
            {
                ItemID = Guid.NewGuid().ToString(),
                ListID = newItemModel.ListID,
                // new item by default takes the next available order
                ItemOrder = largestOrder + 1,
                ItemName = newItemModel.ItemName,
                // new item is not completed by default
                Completed = false,
                DueDate = newItemModel.DueDate,
                Notes = newItemModel.Notes
            };
            await _context.TodoItems.AddAsync(newItem);
            await _context.SaveChangesAsync();
            return ConvertToAPIModel(newItem);
        }
        public async Task<IEnumerable<TodoItemAPIModel>> GetAllItemsForListAsync(string listID)
        {
            IQueryable<TodoItemAPIModel> allItems = _context.TodoItems
                .Where(ti => ti.ListID == listID)
                .Select(ti => ConvertToAPIModel(ti));
            return await allItems.ToListAsync();
        }
        public async Task<IEnumerable<TodoItemAPIModel>> GetActiveItemsForListAsync(string listID)
        {
            IQueryable<TodoItemAPIModel> activeItems = _context.TodoItems
                .Where(ti => ti.ListID == listID && !ti.Completed)
                .Select(ti => ConvertToAPIModel(ti));
            return await activeItems.ToListAsync();
        }
        public async Task<IEnumerable<TodoItemAPIModel>> GetCompletedItemsForListAsync(string listID)
        {
            IQueryable<TodoItemAPIModel> completedItems = _context.TodoItems
                .Where(ti => ti.ListID == listID && ti.Completed)
                .Select(ti => ConvertToAPIModel(ti));
            return await completedItems.ToListAsync();
        }
        public async Task<TodoItemAPIModel> GetItemByIDAsync(string itemID)
        {
            IQueryable<TodoItemAPIModel> item = _context.TodoItems
                .Where(ti => ti.ItemID == itemID)
                .Select(ti => ConvertToAPIModel(ti));
            return await item.FirstOrDefaultAsync();
        }
        public async Task<TodoItemAPIModel> UpdateItemAsync(string itemID, TodoItemAPIModel updatedItemModel)
        {
            TodoItem todoItem = await _context.TodoItems.Where(ti => ti.ItemID == itemID).FirstOrDefaultAsync();
            
        }

        public async Task<IEnumerable<TodoItemAPIModel>> UpdateAllItemsInListAsync(string listID, TodoItemAPIModel[] items)
        {

        }

        public async Task<bool> ToggleItemCompleteAsync(string itemID)
        {

        }
        public async Task<bool> RemoveListAsync(string ListID)
        {

        }
        // Get largest order for active items in list
        private async Task<int> GetLargestOrderAsync(string listID)
        {
            TodoItem lastItem = await _context.TodoItems
                .Where(ti => ti.ListID == listID && !ti.Completed)
                .OrderByDescending(ti => ti.ItemOrder).FirstAsync();
            if (lastItem != null)
            {
                return lastItem.ItemOrder;
            } else
            {
                return 0;
            }
        }

        private TodoItemAPIModel ConvertToAPIModel(TodoItem item) => new TodoItemAPIModel()
        {
            ItemID = item.ItemID,
            ListID = item.ListID,
            ItemOrder = item.ItemOrder,
            ItemName = item.ItemName,
            Completed = item.Completed,
            DueDate = item.DueDate,
            Notes = item.Notes
        };
    }
}
