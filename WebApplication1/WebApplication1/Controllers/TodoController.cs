using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoSql;
using WebApplication1.Data;
using WebApplication1.Models.Todo;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            IndexViewModel model = new IndexViewModel(_repository.GetActive(new Guid(user.Id))
                .Select(item => new TodoViewModel() { Title = item.Text, DateDue = item.DateDue, Id = item.Id, LinkText = "Mark as completed", ShowOffset = true })
                .ToList());
            return View(model);
        }

        public async Task<IActionResult> Completed()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            CompletedViewModel model = new CompletedViewModel(_repository.GetCompleted(new Guid(user.Id))
                .Select(item => new TodoViewModel() { Title = item.Text, DateDue = item.DateDue, Id = item.Id, LinkText = "Remove from completed" }).ToList());
            return View(model);
        }


        public async Task<IActionResult> ToggleCompleted(Guid id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            TodoItem item = _repository.Get(id, new Guid(user.Id));
            if (item.IsCompleted)
            {
                item.DateCompleted = null;
                _repository.Update(item, new Guid(user.Id));
            }
            else
            {
                item.DateCompleted = DateTime.Now;
                _repository.Update(item, new Guid(user.Id));
            }

            return Redirect("/todo");
        }

        public IActionResult AddItem()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddItem(AddTodoViewModel addTodoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addTodoViewModel);
            }
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            TodoItem item = new TodoItem(addTodoViewModel.TodoText, new Guid(user.Id));
            item.DateCreated = DateTime.Now;
            item.DateDue = addTodoViewModel.DateDue;
            _repository.Add(item);

            if (addTodoViewModel.Labels != null)
            {
                string[] labels = addTodoViewModel.Labels.Split(',');
                foreach (string label in labels)
                {
                    _repository.AddLabel(label.Trim().ToLower(), item.Id);
                }
            }

            return RedirectToAction("Index");
        }

        


    }
}