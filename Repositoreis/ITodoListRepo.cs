using AspTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositoreis
{
    public interface ITodoListRepo
    {
        Task<TodoListAPIModel> CreateListAsync(TodoListAPIModel newListModel);
        Task<TodoListAPIModel> CreateListAsync(string listName, string ownerID);
        Task<IEnumerable<TodoListAPIModel>> GetOwnedListsAsync(string userID);
        Task<IEnumerable<TodoListAPIModel>> GetSharedListsAsync(string userID);
        Task<TodoListAPIModel> GetListByIDAsync(string listID);
        Task<TodoListAPIModel> UpdateListAsync(TodoListAPIModel updatedListModel);

        Task<bool> LeaveSharedListAsync(string userID, string listID);
        Task<bool> RemoveListAsync(string listID);

    }
}
