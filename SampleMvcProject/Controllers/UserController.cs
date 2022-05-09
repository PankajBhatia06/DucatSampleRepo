using SampleMvcProject.Database;
using SampleMvcProject.Filters;
using SampleMvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleMvcProject.Controllers
{
    [AuthenticationFilter]
    public class UserController : Controller
    {
        private readonly UserService objUserService;
        public UserController()
        {
            objUserService = new UserService();
        }
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            return View(objUserService.GetAllUsers());
        }


        [HttpGet]
        public ActionResult Create()
        {
            var user = new User();
            return View(user);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var user = objUserService.GetUserById(id);
            return View("Create", user);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var user = objUserService.GetUserById(id);
            ViewBag.IsReadOnly = true;
            return View("Create", user);

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = objUserService.GetUserById(id);
            return View(user);
        }


        [HttpPost]
        public ActionResult Delete(User user)
        {
            if (objUserService.DeleteUser(user.Id))
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }


        [HttpPost]
        public ActionResult Update(User user)
        {

            if (ModelState.IsValid)
            {
                if (objUserService.UpdateUser(user))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(user);
                }
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                if (objUserService.CreateUser(user.Name, user.Password))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(user);
                }
            }
            return View(user);
        }
    }
}