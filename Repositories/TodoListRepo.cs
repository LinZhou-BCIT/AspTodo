using AspTodo.Data;
using AspTodo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositories
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

        public async Task<TodoList> CreateListAsync(TodoList newList)
        {
            // Assume guid is generated in the service layer
            //TodoList createdList = new TodoList()
            //{
            //    ListID = Guid.NewGuid().ToString(),
            //    ListName = newList.ListName,
            //    OwnerID = newList.OwnerID
            //};
            var result = await _context.TodoLists.AddAsync(newList);
            //await _context.SaveChangesAsync();
            return newList;
        }

        public async Task<TodoList> GetListByIDAsync(string listID)
        {
            TodoList list = await _context.TodoLists
                .Where(tl => tl.ListID == listID).FirstOrDefaultAsync();
            return list;
        }
        public async Task<TodoList> UpdateListAsync(TodoList updatedList)
        {
            // the validity of ownerID foreign key constraint is up to the controller to validate via usermanager
            // TodoList todoList = await GetListByIDAsync(updatedListModel.ListID);

            // check if list exist? ***************
            _context.TodoLists.Attach(updatedList);
            _context.Entry(updatedList).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            return updatedList;
        }
        public async Task<bool> RemoveListAsync(string listID)
        {
            TodoList todoList = await GetListByIDAsync(listID);
            if (todoList != null)
            {
                _context.TodoLists.Remove(todoList);
                //await _context.SaveChangesAsync();
                return true;
            }
            // this basically means not found
            return false;
        }

        public async Task<IEnumerable<TodoList>> GetOwnedListsAsync(string userID)
        {
            IQueryable<TodoList> ownedLists = _context.TodoLists
                .Where(tl => tl.OwnerID == userID);
            return await ownedLists.ToListAsync();
        }
        public async Task<IEnumerable<TodoList>> GetSharedListsAsync(string userID)
        {
            IQueryable<TodoList> sharedLists = _context.TodoLists
                .Join(_context.Sharings,
                        tl => tl.ListID,
                        s => s.ListID,
                        (tl, s) => new { tl, s })
                        .Where(joined => joined.s.ShareeID == userID)
                        .Select(j => j.tl);
            return await sharedLists.ToListAsync();
        }

        public async Task<bool> JoinSharedListAsync(string userID, string listID)
        {
            Sharing sharing = await GetSharingAsync(userID, listID);
            // maybe check if list exist here? or wait to catch sqlexception?
            if (sharing == null)
            {
                Sharing newSharing = new Sharing()
                {
                    ListID = listID,
                    ShareeID = userID
                };
                await _context.Sharings.AddAsync(newSharing);
                return true;
            }
            // already in shared list
            return false;
        }
        public async Task<bool> LeaveSharedListAsync(string userID, string listID)
        {
            Sharing sharing = await GetSharingAsync(userID, listID);
            if (sharing != null)
            {
                _context.Sharings.Remove(sharing);
                //await _context.SaveChangesAsync();
                return true;
            }
            // this basically means not found
            return false;
        }

        public async Task<bool> IsOwnerAsync(string userID, string listID)
        {
            TodoList todoList = await _context.TodoLists
                .Where(tl => tl.ListID == listID && tl.OwnerID == userID).AsNoTracking().FirstOrDefaultAsync();
            if (todoList == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
        public async Task<Sharing> GetSharingAsync(string userID, string listID)
        {
            Sharing sharing = await _context.Sharings.Select(s => s)
                .Where(s => s.ListID == listID && s.ShareeID == userID).AsNoTracking().FirstOrDefaultAsync();
            return sharing;
        }


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

        // Not that Automapper is a thing...

        //private TodoListAPIModel ConvertToAPIModel(TodoList list) => new TodoListAPIModel()
        //{
        //    ListID = list.ListID,
        //    ListName = list.ListName,
        //    OwnerID = list.OwnerID
        //};
    }
}
