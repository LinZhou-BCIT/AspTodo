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
        public async Task<TodoItem> CreateItemAsync(TodoItem newItem)
        {
            // put these in another layer that does validation and 
            //int largestOrder = await GetLargestOrderAsync(newItem.ListID);
            //TodoItem createdItem = new TodoItem()
            //{
            //    ItemID = Guid.NewGuid().ToString(),
            //    ListID = newItem.ListID,
            //    ItemOrder = largestOrder + 1,
            //    ItemName = newItem.ItemName,
            //    Completed = false,
            //    DueDate = newItem.DueDate,
            //    Notes = newItem.Notes
            //};
            await _context.TodoItems.AddAsync(newItem);
            return newItem;
        }
        public async Task<TodoItem> GetItemByIDAsync(string itemID)
        {
            TodoItem item = await _context.TodoItems
                .Where(ti => ti.ItemID == itemID).FirstOrDefaultAsync();
            return item;
        }
        public async Task<TodoItem> UpdateItemAsync(TodoItem updatedItem)
        {
            _context.TodoItems.Attach(updatedItem);
            _context.Entry(updatedItem).State = EntityState.Modified;
            return updatedItem;
        }
        public async Task<bool> RemoveItemAsync(string ItemID)
        {
            TodoItem todoItem = await GetItemByIDAsync(ItemID);
            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                return true;
            }
            // this basically means not found
            return false;
        }

        public async Task<IEnumerable<TodoItem>> GetAllItemsForListAsync(string listID)
        {
            IQueryable<TodoItem> allItems = _context.TodoItems
                .Where(ti => ti.ListID == listID);
            return await allItems.ToListAsync();
        }
        public async Task<IEnumerable<TodoItem>> GetActiveItemsForListAsync(string listID)
        {
            IQueryable<TodoItem> activeItems = _context.TodoItems
                .Where(ti => ti.ListID == listID && !ti.Completed);
            return await activeItems.ToListAsync();
        }
        public async Task<IEnumerable<TodoItem>> GetCompletedItemsForListAsync(string listID)
        {
            IQueryable<TodoItem> completedItems = _context.TodoItems
                .Where(ti => ti.ListID == listID && ti.Completed);
            return await completedItems.ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> UpdateAllItemsInListAsync(TodoItem[] items)
        {
            List<TodoItem> updatedItems = new List<TodoItem>();
            // ef core does not support mass update for now it seems
            foreach (TodoItem item in items)
            {
                TodoItem updatedItem = await UpdateItemAsync(item);
                if (updatedItem != null)
                {
                    updatedItems.Add(updatedItem);
                }                
            }
            return updatedItems;
        }

        public async Task<bool> ToggleItemCompleteAsync(string itemID)
        {
            TodoItem item = await _context.TodoItems.Where(ti => ti.ItemID == itemID)
                .FirstOrDefaultAsync();
            item.Completed = !item.Completed;
            return true;
        }

        // Get largest order for active items in list
        public async Task<int> GetLargestOrderAsync(string listID)
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

        //private TodoItemAPIModel ConvertToAPIModel(TodoItem item) => new TodoItemAPIModel()
        //{
        //    ItemID = item.ItemID,
        //    ListID = item.ListID,
        //    ItemOrder = item.ItemOrder,
        //    ItemName = item.ItemName,
        //    Completed = item.Completed,
        //    DueDate = item.DueDate,
        //    Notes = item.Notes
        //};

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            // this might not work? *************
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
