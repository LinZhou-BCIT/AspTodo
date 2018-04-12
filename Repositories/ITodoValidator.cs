using AspTodo.Models;
using AspTodo.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositories
{
    public interface ITodoValidator
    {
        Task<TodoListAPIModel> CreateListAsync(string userID, TodoListCreateDto newList);
        Task<TodoListAPIModel> GetListByIDAsync(string userID, string listID);
        Task<TodoListAPIModel> UpdateListAsync(string userID, TodoListUpdateDto updatedList);
        Task<bool> RemoveListAsync(string userID, string listID);

        Task<IEnumerable<TodoListAPIModel>> GetOwnedListsAsync(string userID);
        Task<IEnumerable<TodoListAPIModel>> GetSharedListsAsync(string userID);
        Task<bool> JoinSharedListAsync(string userID, string listID);
        Task<bool> LeaveSharedListAsync(string userID, string listID);

        Task<TodoItemAPIModel> CreateItemAsync(string userID, TodoItemCreateDto newItemModel);
        Task<TodoItemAPIModel> GetItemByIDAsync(string userID, string itemID);
        Task<TodoItemAPIModel> UpdateItemAsync(string userID, TodoItemUpdateDto updatedItemModel);
        Task<bool> RemoveItemAsync(string userID, string LitemID);

        Task<IEnumerable<TodoItemAPIModel>> GetAllItemsForListAsync(string userID, string listID);
        Task<IEnumerable<TodoItemAPIModel>> GetActiveItemsForListAsync(string userID, string listID);
        Task<IEnumerable<TodoItemAPIModel>> GetCompletedItemsForListAsync(string userID, string listID);

        Task<bool> ToggleItemCompleteAsync(string userID, string itemID);

    }
}
