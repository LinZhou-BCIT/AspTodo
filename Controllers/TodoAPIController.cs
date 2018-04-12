using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspTodo.Models;
using AspTodo.Models.Dtos;
using AspTodo.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspTodo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodoAPI : Controller
    {
        private readonly ITodoValidator _todoValidator;

        public TodoAPI(ITodoValidator todoValidator)
        {
            _todoValidator = todoValidator;
        }

        [HttpGet]
        public async Task<object> GetList(string listID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // there is the possibility that within the expiration time of the token
            // the user has been deleted
            // but For simplicity's sake userID will nt be validated here

            if (String.IsNullOrEmpty(listID))
            {
                return StatusCode(400);
            }
            TodoListAPIModel list = await _todoValidator.GetListByIDAsync(userID, listID);
            if (list != null)
            {
                return Ok(list);
            } else
            {
                return StatusCode(404);
            }
        }

        [HttpPost]
        public async Task<object> CreateList([FromBody] TodoListCreateDto model)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            TodoListAPIModel created = await _todoValidator.CreateListAsync(userID, model);
            if (created != null)
            {
                return Ok(created);
            }
            else
            {
                // with the current way of layering it's hard to convey exact errors
                // it is possible to be more specific with a more complex result structure
                // btw is it a potential security problem to be too specific with the errors?
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<object> UpdateList([FromBody] TodoListUpdateDto model)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            TodoListAPIModel updated = await _todoValidator.UpdateListAsync(userID, model);
            if (updated != null)
            {
                return Ok(updated);
            }
            else
            {
                // with the current way of layering it's hard to convey exact errors
                // it is possible to be more specific with a more complex result structure
                // btw is it a potential security problem to be too specific with the errors?
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public async Task<object> RemoveList(string listID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // there is the possibility that within the expiration time of the token
            // the user has been deleted
            // but For simplicity's sake userID will nt be validated here

            if (String.IsNullOrEmpty(listID))
            {
                return StatusCode(400);
            }
            bool result = await _todoValidator.RemoveListAsync(userID, listID);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<object> GetOwnedLists()
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // there is the possibility that within the expiration time of the token
            // the user has been deleted
            // but For simplicity's sake userID will nt be validated here

            IEnumerable<TodoListAPIModel> ownedLists = await _todoValidator.GetOwnedListsAsync(userID);
            //if (ownedLists != null)
            //{
            //    return Ok(ownedLists);
            //}
            //else
            //{
            //    return StatusCode(404);
            //}
            return Ok(ownedLists);
        }

        [HttpGet]
        public async Task<object> GetSharedLists()
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // there is the possibility that within the expiration time of the token
            // the user has been deleted
            // but For simplicity's sake userID will nt be validated here

            IEnumerable<TodoListAPIModel> sharedLists = await _todoValidator.GetSharedListsAsync(userID);
            //if (sharedLists != null)
            //{
            //    return Ok(sharedLists);
            //}
            //else
            //{
            //    return StatusCode(404);
            //}
            return Ok(sharedLists);
        }

        [HttpGet]
        public async Task<object> GetAllItemsForList(string listID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (String.IsNullOrEmpty(listID))
            {
                return StatusCode(400);
            }
            IEnumerable<TodoItemAPIModel> items = await _todoValidator.GetAllItemsForListAsync(userID, listID);
            if (items != null)
            {
                return Ok(items);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        public async Task<object> GetActiveItemsForList(string listID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (String.IsNullOrEmpty(listID))
            {
                return StatusCode(400);
            }
            IEnumerable<TodoItemAPIModel> items = await _todoValidator.GetActiveItemsForListAsync(userID, listID);
            if (items != null)
            {
                return Ok(items);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        public async Task<object> GetCompletedItemsForList(string listID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (String.IsNullOrEmpty(listID))
            {
                return StatusCode(400);
            }
            IEnumerable<TodoItemAPIModel> items = await _todoValidator.GetCompletedItemsForListAsync(userID, listID);
            if (items != null)
            {
                return Ok(items);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        public async Task<object> GetItem(string itemID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (String.IsNullOrEmpty(itemID))
            {
                return StatusCode(400);
            }
            TodoItemAPIModel item = await _todoValidator.GetItemByIDAsync(userID, itemID);
            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpPost]
        public async Task<object> CreateItem([FromBody] TodoItemCreateDto model)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            TodoItemAPIModel created = await _todoValidator.CreateItemAsync(userID, model);
            if (created != null)
            {
                return Ok(created);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<object> UpdateItem([FromBody] TodoItemUpdateDto model)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            TodoItemAPIModel updated = await _todoValidator.UpdateItemAsync(userID, model);
            if (updated != null)
            {
                return Ok(updated);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<object> ToggleCompletion(string itemID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;

            if (String.IsNullOrEmpty(itemID))
            {
                return StatusCode(400);
            }
            TodoItemAPIModel updated = await _todoValidator.ToggleItemCompleteAsync(userID, itemID);
            if (updated != null)
            {
                return Ok(updated);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public async Task<object> RemoveItem(string itemID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // there is the possibility that within the expiration time of the token
            // the user has been deleted
            // but For simplicity's sake userID will nt be validated here

            if (String.IsNullOrEmpty(itemID))
            {
                return StatusCode(400);
            }
            bool result = await _todoValidator.RemoveItemAsync(userID, itemID);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        // Should this be a get or a post?
        [HttpGet]
        public async Task<object> JoinList(string listID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // there is the possibility that within the expiration time of the token
            // the user has been deleted
            // but For simplicity's sake userID will nt be validated here

            if (String.IsNullOrEmpty(listID))
            {
                return StatusCode(400);
            }
            bool result = await _todoValidator.JoinSharedListAsync(userID, listID);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(404);
            }
        }

        // Should this be a get or a post or a delete?
        [HttpDelete]
        public async Task<object> LeaveList(string listID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // there is the possibility that within the expiration time of the token
            // the user has been deleted
            // but For simplicity's sake userID will nt be validated here

            if (String.IsNullOrEmpty(listID))
            {
                return StatusCode(400);
            }
            bool result = await _todoValidator.LeaveSharedListAsync(userID, listID);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(404);
            }
        }
    }
}