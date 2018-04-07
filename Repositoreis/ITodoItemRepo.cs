using AspTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositoreis
{
    public interface ITodoItemRepo
    {
        Task<TodoItemAPIModel> CreateItem(TodoItemAPIModel newItem);
        Task<TodoItemAPIModel> CreateItem(string listID, string itemName);
        Task<IEnumerable<TodoItemAPIModel>> GetActiveItemsForList(string listID);
        Task<IEnumerable<TodoItemAPIModel>> GetCompletedItemsForList(string listID);
        Task<TodoItemAPIModel> GetItemByID(string itemID);
        Task<TodoItemAPIModel> UpdateItem(string itemID, TodoItemAPIModel updatedItem);
        Task<bool> ReorderItem(string itemID, int newOrder);

        Task<bool> CompleteItem(string itemID);
        Task<bool> RemoveList(string ListID);

    }
}
