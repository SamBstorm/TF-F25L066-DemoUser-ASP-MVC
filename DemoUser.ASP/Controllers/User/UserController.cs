using DemoUser.ASP.Models.User;
using DemoUser.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoUser.ASP.Controllers.User
{

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public UserController(IUserService userService,  ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        // GET: UserController
        public ActionResult Index()
        {
            var users = _userService.Get();

            var model = users.Select(user => new UserListItemViewModel
            {
                Id = user.Id,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                IsActive = user.isActive
            }).ToList();
            
            return View("UserListItemViewModel", model);
        }

        // GET: UserController/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View("UserRegisterViewModel", new UserRegisterViewModel());
        }
        
        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserRegisterViewModel", model);
            }

            try
            {
                _userService.Register(model.Email, model.Password);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View("UserRegisterViewModel", model);
            }
        }
        
        // GET /UserController/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View("UserLoginViewModel", new UserLoginViewModel());
        }
        
        // Post /User/Login
        [HttpPost]
        public ActionResult Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserLoginViewModel", model);
            }
            
            var user = _userService.Login(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Invalid login attempt.");
                return View("UserLoginViewModel", model);
            }
            
            TempData["LoginMessage"] = $"Welcome {user.Email}!";
            return RedirectToAction(nameof(Index));
        }


        // GET /UserController/LoginWithSession
        [HttpGet]
        public ActionResult LoginWithSession()
        {
            return Login();
        }

        // Post /User/LoginWithSession
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginWithSession(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserLoginViewModel", model);
            }
            
            var user = _userService.Login(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Invalid login attempt.");
                return View("UserLoginViewModel", model);
            }

            var session = _sessionService.CreateForUser(user.Id, TimeSpan.FromHours(4));
            
            // Pour cette démo: on montre juste le token (dans un vai projet -> cookie / header)
            TempData["LoginMessage"] = $"Welcome {user.Email}! Session: {session.Token}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disable(Guid id)
        {
            _userService.Disable(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
