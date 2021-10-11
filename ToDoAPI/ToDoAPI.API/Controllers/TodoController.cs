using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.DATA.EF;
using ToDoAPI.API.Models;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TodoController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        public IHttpActionResult GetTodo()
        {
            List<TodoViewModels> Todos = db.ToDoItems.Include("Category").Select(t => new TodoViewModels()
            {
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = (int) t.CategoryId,
                    CategoryName = t.Category.Name,
                    CategoryDescription = t.Category.Description
                }
            }).ToList<TodoViewModels>();

            if (Todos.Count == 0)
            {
                return NotFound();
            }
            
            return Ok(Todos);
        }

        public IHttpActionResult GetTodo(int id)
        {
            TodoViewModels todos = db.ToDoItems.Include("Category").Where(t => t.TodoId == id).Select(t => new TodoViewModels()
            {
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = (int)t.CategoryId,
                    CategoryName = t.Category.Name,
                    CategoryDescription = t.Category.Description
                }

            }).FirstOrDefault();

            if(todos == null)
            {
                return NotFound();
            }
            return Ok(todos);          
        }

        public IHttpActionResult PostToDo(TodoViewModels todo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");

            ToDoItem newTodoItem = new ToDoItem()
            {
                TodoId = todo.TodoId,
                Action = todo.Action,
                Done = todo.Done,
                CategoryId = todo.CategoryId
            };

            db.ToDoItems.Add(newTodoItem);
            db.SaveChanges();
            return Ok(newTodoItem);
        }

        public IHttpActionResult PutToDo(TodoViewModels todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }//end if

            ToDoItem existingTodo = db.ToDoItems.Where(r => r.TodoId == todo.TodoId).FirstOrDefault();

            if (existingTodo != null)
            {
                existingTodo.TodoId = todo.TodoId;
                existingTodo.Action = todo.Action;
                existingTodo.Done = todo.Done;
                existingTodo.CategoryId = todo.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        
        public IHttpActionResult DeleteTodo(int id)
        {
            ToDoItem resource = db.ToDoItems.Where(r => r.TodoId == id).FirstOrDefault();

            if (resource != null)
            {
                db.ToDoItems.Remove(resource);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
     
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
