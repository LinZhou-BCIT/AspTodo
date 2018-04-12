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
    }
}