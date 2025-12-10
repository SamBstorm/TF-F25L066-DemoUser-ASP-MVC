using DemoUser.BLL.Services.Interfaces;
using DemoUser.ASP.Models.Todo;
using Microsoft.AspNetCore.Mvc;

namespace DemoUser.ASP.Controllers.Todo
{
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: /Todo
        public IActionResult Index()
        {
            var todos = _todoService.GetAll();

            var model = todos.Select(t => new TodoListItemViewModel
            {
                Id = t.Id,
                Title = t.Title,
                IsDone = t.IsDone,
                CreatedAt = t.CreatedAt
            }).ToList();

            return View("Index",model);
        }

        // GET: /Todo/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create", new TodoEditViewModel());
        }

        // POST: /Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TodoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            _todoService.Create(model.Title);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Todo/Edit/{id}
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var todo = _todoService.GetById(id);
            if (todo is null) return NotFound();

            var model = new TodoEditViewModel
            {
                Id = todo.Id,
                Title = todo.Title,
                IsDone = todo.IsDone
            };

            return View("Edit", model);
        }

        // POST: /Todo/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TodoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            if (model.Id is null) return BadRequest();

            // On met à jour Title & IsDone via le service
            _todoService.Rename(model.Id.Value, model.Title);

            if (model.IsDone)
                _todoService.MarkAsDone(model.Id.Value);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Todo/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            _todoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
