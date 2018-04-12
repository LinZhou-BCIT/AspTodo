using AspTodo.Models;
using AspTodo.Models.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Repositories
{
    public class TodoValidator : ITodoValidator
    {
        private ITodoListRepo _todoListRepo;
        private ITodoItemRepo _todoItemRepo;

        public TodoValidator(TodoListRepo todoListRepo, ITodoItemRepo todoItemRepo)
        {
            _todoListRepo = todoListRepo;
            _todoItemRepo = todoItemRepo;
        }

        public async Task<TodoListAPIModel> CreateListAsync(string userID, TodoListCreateDto newList)
        {
            string newListID = Guid.NewGuid().ToString();
            TodoList toAdd = Mapper.Map<TodoList>(newList);
            toAdd.ListID = newListID;
            toAdd.OwnerID = userID;
            TodoList added = await _todoListRepo.CreateListAsync(toAdd);
            // failed to save
            if (!(await _todoListRepo.SaveAsync()))
            {
                return null;
            }
            return Mapper.Map<TodoListAPIModel>(added);
        }
        public async Task<TodoListAPIModel> GetListByIDAsync(string userID, string listID)
        {
            TodoList list = await _todoListRepo.GetListByIDAsync(listID);
            if (list == null)
            {
                return null;
            }
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, listID);
            // user is not owner/sharee
            if (list.OwnerID != userID && checkSharing == null)
            {
                return null;
            } else
            {
                return Mapper.Map<TodoListAPIModel>(list);
            }        
        }
        public async Task<TodoListAPIModel> UpdateListAsync(string userID, TodoListUpdateDto updatedList)
        {
            // This does not seem to be efficient...
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, updatedList.ListID);
            if (!isOwner)
            {
                return null;
            }
            TodoList toUpdate = Mapper.Map<TodoList>(updatedList);
            toUpdate.OwnerID = userID;
            TodoList list = await _todoListRepo.UpdateListAsync(toUpdate);
            if (list == null)
            {
                return null;
            }
            else
            {
                // failed to save
                if (!(await _todoListRepo.SaveAsync()))
                {
                    return null;
                }
                return Mapper.Map<TodoListAPIModel>(list);
            }

        }
        public async Task<bool> RemoveListAsync(string userID, string listID)
        {
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, listID);
            if (!isOwner)
            {
                return false;
            }
            bool removeResult = await _todoListRepo.RemoveListAsync(listID);
            if (!removeResult)
            {
                return false;
            }
            // failed to save
            if (!(await _todoListRepo.SaveAsync()))
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<TodoListAPIModel>> GetOwnedListsAsync(string userID)
        {
            IEnumerable<TodoList> ownedList = await _todoListRepo.GetOwnedListsAsync(userID);
            if (ownedList == null)
            {
                return null;
            } else
            {
                return Mapper.Map<List<TodoListAPIModel>>(ownedList);
            }               
        }
        public async Task<IEnumerable<TodoListAPIModel>> GetSharedListsAsync(string userID)
        {
            IEnumerable<TodoList> sharedList = await _todoListRepo.GetSharedListsAsync(userID);
            if (sharedList == null)
            {
                return null;
            }
            else
            {
                return Mapper.Map<List<TodoListAPIModel>>(sharedList);
            }
        }
        public async Task<bool> JoinSharedListAsync(string userID, string listID)
        {
            await _todoListRepo.JoinSharedListAsync(userID, listID);
            // failed to save
            if (!(await _todoListRepo.SaveAsync()))
            {
                return false;
            }
            return true;
        }
        public async Task<bool> LeaveSharedListAsync(string userID, string listID)
        {
            await _todoListRepo.LeaveSharedListAsync(userID, listID);
            // failed to save
            if (!(await _todoListRepo.SaveAsync()))
            {
                return false;
            }
            return true;
        }

        public async Task<TodoItemAPIModel> CreateItemAsync(string userID, TodoItemCreateDto newItemModel)
        {
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, newItemModel.ListID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, newItemModel.ListID);
            if (!isOwner && checkSharing == null)
            {
                return null;
            }
            TodoItem toAdd = Mapper.Map<TodoItem>(newItemModel);
            string newItemID = Guid.NewGuid().ToString();
            toAdd.ItemID = newItemID;
            TodoItem itemCreated = await _todoItemRepo.CreateItemAsync(toAdd);
            if (itemCreated == null)
            {
                return null;
            }
            else
            {
                // failed to save
                if (!(await _todoItemRepo.SaveAsync()))
                {
                    return null;
                }
                return Mapper.Map<TodoItemAPIModel>(itemCreated);
            }

        }
        public async Task<TodoItemAPIModel> GetItemByIDAsync(string userID, string itemID)
        {
            TodoItem item = await _todoItemRepo.GetItemByIDAsync(itemID);
            if (item == null)
            {
                return null;
            }
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, item.ListID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, item.ListID);
            if (!isOwner && checkSharing == null)
            {
                return null;
            }
            return Mapper.Map<TodoItemAPIModel>(item);
        }
        public async Task<TodoItemAPIModel> UpdateItemAsync(string userID, TodoItemUpdateDto updatedItemModel)
        {
            TodoItem item = await _todoItemRepo.GetItemByIDAsync(updatedItemModel.ItemID);
            if (item == null)
            {
                return null;
            }
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, item.ListID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, item.ListID);
            if (!isOwner && checkSharing == null)
            {
                return null;
            }
            TodoItem toUpdate = Mapper.Map<TodoItem>(updatedItemModel);
            TodoItem updatedItem = await _todoItemRepo.UpdateItemAsync(toUpdate);
            if (updatedItem == null)
            {
                return null;
            }
            else
            {
                // failed to save
                if (!(await _todoListRepo.SaveAsync()))
                {
                    return null;
                }
                return Mapper.Map<TodoItemAPIModel>(updatedItem);
            }
        }
        public async Task<bool> RemoveItemAsync(string userID, string itemID)
        {
            TodoItem item = await _todoItemRepo.GetItemByIDAsync(itemID);
            if (item == null)
            {
                return false;
            }
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, item.ListID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, item.ListID);
            if (!isOwner && checkSharing == null)
            {
                return false;
            }
            bool removalResult = await _todoItemRepo.RemoveItemAsync(itemID);
            if (!removalResult)
            {
                return false;
            }
            else
            {
                // failed to save
                if (!(await _todoListRepo.SaveAsync()))
                {
                    return false;
                }
                return true;
            }

        }

        public async Task<IEnumerable<TodoItemAPIModel>> GetAllItemsForListAsync(string userID, string listID)
        {
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, listID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, listID);
            if (!isOwner && checkSharing == null)
            {
                return null;
            }
            IEnumerable<TodoItem> items = await _todoItemRepo.GetAllItemsForListAsync(listID);
            return Mapper.Map<IEnumerable<TodoItemAPIModel>>(items);
        }
        public async Task<IEnumerable<TodoItemAPIModel>> GetActiveItemsForListAsync(string userID, string listID)
        {
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, listID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, listID);
            if (!isOwner && checkSharing == null)
            {
                return null;
            }
            IEnumerable<TodoItem> items = await _todoItemRepo.GetActiveItemsForListAsync(listID);
            return Mapper.Map<IEnumerable<TodoItemAPIModel>>(items);
        }
        public async Task<IEnumerable<TodoItemAPIModel>> GetCompletedItemsForListAsync(string userID, string listID)
        {
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, listID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, listID);
            if (!isOwner && checkSharing == null)
            {
                return null;
            }
            IEnumerable<TodoItem> items = await _todoItemRepo.GetCompletedItemsForListAsync(listID);
            return Mapper.Map<IEnumerable<TodoItemAPIModel>>(items);
        }

        public async Task<bool> ToggleItemCompleteAsync(string userID, string itemID)
        {
            TodoItem item = await _todoItemRepo.GetItemByIDAsync(itemID);
            if (item == null)
            {
                return false;
            }
            bool isOwner = await _todoListRepo.IsOwnerAsync(userID, item.ListID);
            Sharing checkSharing = await _todoListRepo.GetSharingAsync(userID, item.ListID);
            if (!isOwner && checkSharing == null)
            {
                return false;
            }
            bool toggleResult = await _todoItemRepo.ToggleItemCompleteAsync(itemID);
            if (!toggleResult)
            {
                return false;
            }
            else
            {
                // failed to save
                if (!(await _todoListRepo.SaveAsync()))
                {
                    return false;
                }
                return true;
            }

        }
    }
}
