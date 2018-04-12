using AspTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositories
{
    public interface ITodoItemRepo
    {
        Task<TodoItem> CreateItemAsync(TodoItem newItem);
        Task<TodoItem> GetItemByIDAsync(string itemID);
        Task<TodoItem> UpdateItemAsync(TodoItem updatedItem);
        Task<bool> RemoveItemAsync(string itemID);

        //Task<IEnumerable<TodoItem>> UpdateAllItemsInListAsync(TodoItem[] items);
        Task<IEnumerable<TodoItem>> GetAllItemsForListAsync(string listID);
        Task<IEnumerable<TodoItem>> GetActiveItemsForListAsync(string listID);
        Task<IEnumerable<TodoItem>> GetCompletedItemsForListAsync(string listID);

        Task<TodoItem> ToggleItemCompleteAsync(string itemID);
        //Task<int> GetLargestOrderAsync(string listID);
        Task<bool> SaveAsync();
    }
}
