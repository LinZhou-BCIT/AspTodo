using AspTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositoreis
{
    public interface ITodoListRepo
    {
        Task<TodoListAPIModel> CreateList(TodoListAPIModel newList);
        Task<TodoListAPIModel> CreateList(string listName, string ownerID);
        Task<IEnumerable<TodoListAPIModel>> GetOwnedLists(string userID);
        Task<IEnumerable<TodoListAPIModel>> GetSharedLists(string userID);
        Task<TodoListAPIModel> GetListByID(string listID);
        Task<TodoListAPIModel> UpdateList(string listID, TodoListAPIModel updatedList);
        Task<TodoListAPIModel> UpdateListName(string listID, string listName);

        Task<bool> LeaveSharedList(string userID, string listID);
        Task<bool> RemoveList(string listID);

    }
}
