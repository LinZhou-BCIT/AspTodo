using AspTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositoreis
{
    public interface ITodoItemRepo
    {
        Task<TodoItemAPIModel> CreateItemAsync(TodoItemAPIModel newItemModel);
        Task<IEnumerable<TodoItemAPIModel>> GetAllItemsForListAsync(string listID);
        Task<IEnumerable<TodoItemAPIModel>> GetActiveItemsForListAsync(string listID);
        Task<IEnumerable<TodoItemAPIModel>> GetCompletedItemsForListAsync(string listID);
        Task<TodoItemAPIModel> GetItemByIDAsync(string itemID);
        Task<TodoItemAPIModel> UpdateItemAsync(string itemID, TodoItemAPIModel updatedItemModel);
        Task<IEnumerable<TodoItemAPIModel>> UpdateAllItemsInListAsync(string listID, TodoItemAPIModel[] items);

        Task<bool> ToggleItemCompleteAsync(string itemID);
        Task<bool> RemoveListAsync(string ListID);

    }
}
