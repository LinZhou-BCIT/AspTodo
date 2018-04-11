using AspTodo.Models;
using AspTodo.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositoreis
{
    public interface ITodoValidator
    {
        Task<TodoListAPIModel> CreateListAsync(TodoListCreateDto newList);
        Task<TodoListAPIModel> GetListByIDAsync(string listID);
        Task<TodoListAPIModel> UpdateListAsync(TodoListUpdateDto updatedList);
        Task<bool> RemoveListAsync(string listID);

        Task<IEnumerable<TodoListAPIModel>> GetOwnedListsAsync(string userID);
        Task<IEnumerable<TodoListAPIModel>> GetSharedListsAsync(string userID);
        Task<bool> JoinSharedListAsync(string userID, string listID);
        Task<bool> LeaveSharedListAsync(string userID, string listID);

        Task<TodoItemAPIModel> CreateItemAsync(TodoItemCreateDto newItemModel);
        Task<TodoItemAPIModel> GetItemByIDAsync(string itemID);
        Task<TodoItemAPIModel> UpdateItemAsync(TodoItemUpdateDto updatedItemModel);
        Task<bool> RemoveItemAsync(string ListID);

        Task<IEnumerable<TodoItemAPIModel>> UpdateAllItemsInListAsync(TodoItemUpdateDto[] items);
        Task<IEnumerable<TodoItemAPIModel>> GetAllItemsForListAsync(string listID);
        Task<IEnumerable<TodoItemAPIModel>> GetActiveItemsForListAsync(string listID);
        Task<IEnumerable<TodoItemAPIModel>> GetCompletedItemsForListAsync(string listID);

        Task<bool> ToggleItemCompleteAsync(string itemID);

    }
}
