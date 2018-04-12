using AspTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositories
{
    public interface ITodoListRepo
    {
        Task<TodoList> CreateListAsync(TodoList newList);
        Task<TodoList> GetListByIDAsync(string listID);
        Task<TodoList> UpdateListAsync(TodoList updatedList);
        Task<bool> RemoveListAsync(string listID);

        Task<IEnumerable<TodoList>> GetOwnedListsAsync(string userID);
        Task<IEnumerable<TodoList>> GetSharedListsAsync(string userID);
        Task<bool> JoinSharedListAsync(string userID, string listID);
        Task<bool> LeaveSharedListAsync(string userID, string listID);
        Task<bool> IsOwnerAsync(string userID, string listID);
        Task<Sharing> GetSharingAsync(string userID, string listID);

        Task<bool> SaveAsync();
    }
}
