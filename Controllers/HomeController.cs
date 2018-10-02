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
        private WeddingContext _context;
        public HomeController(WeddingContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("index");
        }

        [HttpPost("RegisterProcess")]
        public IActionResult Register(RegisterViewModel user){
                if(ModelState.IsValid){
                      var userList = _context.users.Where(p => p.email== user.email).ToList();
                      foreach (var item in userList)
                      {
                          if(user.email == item.email){
                            ModelState.AddModelError("email", "email exists");
                            return View("index");
                        }
                      }
                        
                PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                user.password = Hasher.HashPassword(user, user.password);
                users User = new users(){
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    password = user.password,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                _context.Add(User);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("Id", (int)User.id);
                return RedirectToAction("Wedding");
            }else{
                return View("index");
            }
        }

        [HttpPost("LoginProcess")]
        public IActionResult Login(LoginViewModel User){
            if(ModelState.IsValid){
                List<users> users = _context.users.Where(p => p.email== User.Email).ToList();
                foreach (var user in users)
                {
                    if(user != null && User.Password != null)
                        {
                            var Hasher = new PasswordHasher<users>();
                            if( 0 !=Hasher.VerifyHashedPassword(user, user.password, User.Password)){
                                HttpContext.Session.SetInt32("Id", (int)user.id);
                                int? id = HttpContext.Session.GetInt32("Id");

                            return RedirectToAction("Wedding");
                        }
                    }else{
                        ModelState.AddModelError("Email", "not a vaild email");
                        return View("index");
                    }
                }       
            }
            return View("index");
        }

        [HttpGet("wedding")]
        public IActionResult Wedding(){
            int? id = HttpContext.Session.GetInt32("Id");
            List<weddings> allWeddings = _context.weddings.Include(p=>p.guests).OrderBy(p=>p.id).ToList();
            // List<guests> allWeddings = _context.guests.Include(z=>z.wedding).OrderBy(p=>p.weddingsid).ToList();
            foreach (var item in allWeddings)
            {
                if(item.date < DateTime.Today){
                        foreach (var guest in item.guests)
                        {
                            _context.guests.Remove(guest);
                        }
                        _context.weddings.Remove(item);
                        _context.SaveChanges();
                        return RedirectToAction("Wedding");
                }
            }
            ViewBag.userid = (int)id;
            ViewBag.allWeddings = allWeddings;
            return View("wedding");
        }
        
        [HttpGet("newwedding")]
        public IActionResult NeWedding(){
            return View("createwedding");
        }

        [HttpPost("createWedding")]
        public IActionResult CreateWedding(WeddingViewModel NewWedding){
            int? id = HttpContext.Session.GetInt32("Id");
            if(ModelState.IsValid){

                weddings Nwedding = new weddings(){
                    usersid = (int)id,
                    wedder_1 = NewWedding.wedder_1,
                    wedder_2 = NewWedding.wedder_2,
                    date = NewWedding.date,
                    location = NewWedding.address,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                if(NewWedding.date < DateTime.Today){
                    ModelState.AddModelError("date", "Marry in the past? are you sure?");
                    return View("createwedding");
                }
                _context.Add(Nwedding);
                _context.SaveChanges();
                return RedirectToAction("Wedding");
            }       
            return View("createwedding");
        }

        [HttpPost("delete")]
        public IActionResult Delete(int weddingid){
            int? id = HttpContext.Session.GetInt32("Id");  
            List<guests> wedGuest = _context.guests.Where(p=>p.weddingsid == weddingid).ToList();
            var wedding = _context.weddings.Where(p=>p.id == weddingid).Where(g=>g.usersid == (int)id).SingleOrDefault();
            foreach (var guest in wedGuest)
            {
                _context.guests.Remove(guest);
            }
            _context.weddings.Remove(wedding);
            _context.SaveChanges();
            return RedirectToAction("wedding");
        }

        [HttpPost("rsvp")]
        public IActionResult RSVP(int weddingid){
            int? id = HttpContext.Session.GetInt32("Id");

            guests newguest = new guests{
                usersid = (int)id,
                weddingsid = weddingid
            };
            _context.Add(newguest);
            _context.SaveChanges();
            return RedirectToAction("wedding");
        }

        [HttpPost("unrsvp")]
        public IActionResult UNRSVP(int weddingid){
            int? id = HttpContext.Session.GetInt32("Id");
            var removeGuest = _context.guests.Where(p=>p.weddingsid == weddingid).Where(z=>z.usersid == (int)id).SingleOrDefault();
            _context.Remove(removeGuest);
            _context.SaveChanges();
            return RedirectToAction("wedding");
        }


        [HttpGet("weddinginfo/{weddingid}")]
        public IActionResult Weddinginfo(int weddingid){
            // List<weddings> Weddinginfo=_context.weddings.Include(s=>s.guests).Where(d=>d.id == weddingid).ToList();
            // List<guests> Weddinginfo = _context.guests.Include(s=>s.user).Include(z=>z.wedding)
            // .Where(d=>d.weddingsid == weddingid).ToList();
            weddings Weddinginfo = _context.weddings.Where(d=>d.id == weddingid).Include(s=>s.guests)
            .ThenInclude(z=>z.user).First();
                // foreach (var guest in Weddinginfo.guests)
                // {
                //     System.Console.WriteLine(guest.user.first_name);
                // }
            ViewBag.Weddinginfo = Weddinginfo;
            return View("weddinginfo");
        }


        [HttpGet("logout")]
        public IActionResult logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        




       
    }
}
