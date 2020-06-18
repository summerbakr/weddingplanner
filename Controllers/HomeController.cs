using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
       private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("processuser")]
        public IActionResult ProcessUser(User user)
        {
        if(ModelState.IsValid)
            {
        // If a User exists with provided email
                if(dbContext.Users.Any(u => u.Email == user.Email))
        {
            // Manually add a ModelState error to the Email field, with provided
            // error message
            ModelState.AddModelError("Email", "Email already in use, must use unique email!");
            
            return View("Index");
        }
        else{
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            HttpContext.Session.SetInt32("UserId", user.UserId);
            return Redirect($"dashboard/{user.UserId}");
            }
        }
        return View("Index");

        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("processlogin")]

        public IActionResult ProcessLogin(LoginUser login)
        {
            if(ModelState.IsValid)
            {
            // If inital ModelState is valid, query for a user with provided email
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == login.LoginEmail);
            // If no user exists with provided email
            if(userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Login");
            }
            else {

            // Initialize hasher object
            var hasher = new PasswordHasher<LoginUser>();
            
            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(login, userInDb.Password, login.LoginPassword);
            
            // result can be compared to 0 for failure
            if(result == 0)
            {
                ModelState.AddModelError("LoginPassword", "Incorrect password!");
                return View("Login");
            }
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return Redirect($"dashboard/{userInDb.UserId}");

            }
    
        }
        return View("Login");

        }

        [HttpGet]
        [Route("dashboard/{userId}")]

        public IActionResult Dashboard(int userId)
        {
            var loggedinuser=dbContext.Users.FirstOrDefault(u=>u.UserId==userId);

            ViewBag.AllWeddings=dbContext.Weddings.Include(w=>w.EventGuests).ToList();
            return View(loggedinuser);

        }

        [HttpGet]
        [Route("createwedding")]

        public IActionResult CreateWedding()
        {
            
            return View();
        }

        [HttpPost]
        [Route("processwedding")]

        public IActionResult ProcessWedding(Wedding newwedding)
        {
            int? plannerid=HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
        

            newwedding.UserId=(int) plannerid;
            dbContext.Weddings.Add(newwedding);
            dbContext.SaveChanges();
            return Redirect($"/dashboard/{plannerid}");
            }

            return View("CreateWedding");
            
        }

        [HttpGet]
        [Route("delete/{weddingId}")]

        public IActionResult DeleteWedding(int weddingId)
        {
            int? userId=HttpContext.Session.GetInt32("UserId");
            var weddingtodelete=dbContext.Weddings.FirstOrDefault(w=>w.WeddingId==weddingId);
            dbContext.Weddings.Remove(weddingtodelete);
            dbContext.SaveChanges();

            return Redirect($"/dashboard/{userId}");


        }

        [HttpGet]
        [Route("join/{weddingId}/{userId}")]

        public IActionResult JoinWedding(int weddingId, int userId)
        {
            Guest newguest=new Guest();
            newguest.WeddingId=weddingId;
            newguest.UserId=userId;
            dbContext.Guests.Add(newguest);
            dbContext.SaveChanges();
            return Redirect($"/dashboard/{userId}");
        }

        [HttpGet]
        [Route("leave/{weddingId}/{userId}")]

        public IActionResult LeaveWedding(int weddingId, int userId)
        {
            Guest leavingguest=dbContext.Guests.FirstOrDefault(a=>a.WeddingId==weddingId && a.UserId==userId);
            dbContext.Guests.Remove(leavingguest);
            dbContext.SaveChanges();
            return Redirect($"/dashboard/{userId}");
        }

        [HttpGet]
        [Route("display/{weddingId}")]

        public IActionResult DisplayWedding(int weddingId)
        {
            ViewBag.DisplayWedding=dbContext.Weddings.Include(w=>w.EventGuests).ThenInclude(a=>a.User).FirstOrDefault(w=>w.WeddingId==weddingId);

            return View();
        }

       


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
