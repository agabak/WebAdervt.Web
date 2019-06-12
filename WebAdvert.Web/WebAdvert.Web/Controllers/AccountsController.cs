using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class AccountsController : Controller
    {
        public AccountsController(SignInManager<CognitoUser> signInManager,
                                  UserManager<CognitoUser> userManager, CognitoUserPool pool)
        {
            _pool = pool;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private readonly CognitoUserPool _pool;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;

        public async Task<IActionResult> Signup()
        {
            var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(model.Email);

                if(user != null)
                {
                    ModelState.AddModelError("UserExist", $"User with {model.Email} as already have account");
                    return View(model);
                }

                var result = await _userManager.CreateAsync(user, model.Email);
                if(result.Succeeded)
                {
                    RedirectToAction("Confirm");
                }else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        return View(model);
                    }
                }

            }
            return View(model);
        }

        public async Task<IActionResult> Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Confirm(ConfirmModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("NotFound", " A user with a given email address was not found");
                    return View(model);
                }

                var result = await _userManager.ConfirmEmailAsync(user, model.Code);
                if (result.Succeeded)
                {
                    RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }

    }
}