using AspTodo.Data;
using AspTodo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositoreis
{
    public class TodoListRepo: ITodoListRepo
    {
        ApplicationDbContext _context;
        public TodoListRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        // no validation of model is implemented here
        // meaning it relies on the controllers for validation
        // one option to implement validation would be expand the return object to include potential errors
        // another would be making an error Enum and throw them for the controller to catch
        // I'll come back to it if I have the time

        // methods in this repo also does not handle db exceptions
        // so that's another thing to improve given time
        public async Task<TodoListAPIModel> CreateListAsync(TodoListAPIModel newListModel)
        {
            TodoList newList = new TodoList()
            {
                ListID = Guid.NewGuid().ToString(),
                ListName = newListModel.ListName,
                OwnerID = newListModel.OwnerID
            };
            await _context.TodoLists.AddAsync(newList);
            await _context.SaveChangesAsync();
            return ConvertToAPIModel(newList);
        }

        public async Task<TodoListAPIModel> CreateListAsync(string listName, string ownerID)
        {
            TodoList newList = new TodoList()
            {
                ListID = Guid.NewGuid().ToString(),
                ListName = listName,
                OwnerID = ownerID
            };
            await _context.TodoLists.AddAsync(newList);
            await _context.SaveChangesAsync();
            return ConvertToAPIModel(newList);

        }
        public async Task<IEnumerable<TodoListAPIModel>> GetOwnedListsAsync(string userID)
        {
            IQueryable<TodoListAPIModel> ownedLists = _context.TodoLists
                .Where(tl => tl.OwnerID == userID)
                .Select(tl => ConvertToAPIModel(tl));
            return await ownedLists.ToListAsync();
        }
        public async Task<IEnumerable<TodoListAPIModel>> GetSharedListsAsync(string userID)
        {
            IQueryable<TodoListAPIModel> sharedLists = _context.TodoLists
                .Join(_context.Sharings, 
                        tl => tl.ListID, 
                        s => s.ListID,
                        (tl, s) => new { tl, s })
                        .Where(joined => joined.s.ShareeID == userID)
                        .Select(j => ConvertToAPIModel(j.tl));
            return await sharedLists.ToListAsync();
        }
        public async Task<TodoListAPIModel> GetListByIDAsync(string listID)
        {
            IQueryable<TodoListAPIModel> query = _context.TodoLists
                .Where(tl => tl.ListID == listID)
                .Select(tl => ConvertToAPIModel(tl));
            return await query.FirstOrDefaultAsync();
        }
        public async Task<TodoListAPIModel> UpdateListAsync(TodoListAPIModel updatedListModel)
        {
            TodoList todoList = await _context.TodoLists
                .Where(tl => tl.ListID == updatedListModel.ListID).FirstOrDefaultAsync();
            if(!String.IsNullOrEmpty(updatedListModel.ListName) && updatedListModel.ListName != todoList.ListName)
            {
                todoList.ListName = updatedListModel.ListName;
            }
            // the validity of ownerID foreign key constraint is up to the controller to validate via usermanager
            if (!String.IsNullOrEmpty(updatedListModel.OwnerID) && updatedListModel.OwnerID != todoList.OwnerID)
            {
                todoList.OwnerID = updatedListModel.OwnerID;
            }
            await _context.SaveChangesAsync();
            return ConvertToAPIModel(todoList);
        }

        public async Task<bool> LeaveSharedListAsync(string userID, string listID)
        {
            Sharing sharing = await _context.Sharings.Select(s => s)
                .Where(s => s.ListID == listID && s.ShareeID == userID).FirstOrDefaultAsync();
            if (sharing != null)
            {
                _context.Sharings.Remove(sharing);
                await _context.SaveChangesAsync();
                return true;
            }
            // this basically means not found
            return false;
        }
        public async Task<bool> RemoveListAsync(string listID)
        {
            TodoList todoList = await _context.TodoLists.Select(tl => tl)
                .Where(tl => tl.ListID == listID).FirstOrDefaultAsync();
            if (todoList != null)
            {
                _context.TodoLists.Remove(todoList);
                await _context.SaveChangesAsync();
                return true;
            }
            // this basically means not found
            return false;
        }

        private TodoListAPIModel ConvertToAPIModel(TodoList list) => new TodoListAPIModel()
        {
            ListID = list.ListID,
            ListName = list.ListName,
            OwnerID = list.OwnerID
        };
    }
}
