﻿
using GuestDialog2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace GuestDialog2.Controllers
{
	public class UserController(MesContext context) : Controller
	{
		private readonly MesContext _context = context;
		

		[HttpGet]
		public async Task<IActionResult> GetUserById(int id)
		{
			var user = await _context.users.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null)
			{
				return NotFound();
			}
			string response = JsonConvert.SerializeObject(user);
			return Json(response);
		}

		public ActionResult Login()
		{

			return PartialView();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(LoginModel logon)
		{
			if (ModelState.IsValid)
			{
				if (_context.users.ToList().Count == 0)
				{
					ModelState.AddModelError("", "Wrong login or password!");
					return PartialView(logon);
				}
				var users = _context.users.Where(a => a.Login == logon.Login);
				if (users.ToList().Count == 0)
				{
					ModelState.AddModelError("", "Wrong login or password!");
					return PartialView(logon);
				}
				var user = users.First();
				string? salt = user.Salt;
				byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);
				var md5 = MD5.Create();

				byte[] byteHash = md5.ComputeHash(password);

				StringBuilder hash = new StringBuilder(byteHash.Length);
				for (int i = 0; i < byteHash.Length; i++)
					hash.Append(string.Format("{0:X2}", byteHash[i]));

				if (user.Password != hash.ToString())
				{
					ModelState.AddModelError("", "Wrong login or password!");
					return PartialView(logon);
				}
				HttpContext.Session.SetString("FirstName", user.FirstName);
				HttpContext.Session.SetString("LastName", user.LastName);
				HttpContext.Session.SetString("Login", user.Login);
				HttpContext.Session.SetString("Id", user.Id.ToString());
				return PartialView("~/Views/Mes/Success.cshtml", "Mes");
			}
			return RedirectToAction("Index", "Mes");
		
			//  return View(logon);
		}
		
		public IActionResult Register()
		{
			return PartialView();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Register(RegisterModel reg)
		{
			var checkUser = _context.users.Where(e => e.Login == reg.Login).FirstOrDefault();
			if (checkUser != null)
			{
				ModelState.AddModelError("Login", "Такой логин уже существует");
				return PartialView(reg);
			}

			if (ModelState.IsValid)
			{
				Users user = new Users();
				user.FirstName = reg.FirstName;
				user.LastName = reg.LastName;
				user.Login = reg.Login;

				byte[] saltbuf = new byte[16];

				RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
				randomNumberGenerator.GetBytes(saltbuf);

				StringBuilder sb = new StringBuilder(16);
				for (int i = 0; i < 16; i++)
					sb.Append(string.Format("{0:X2}", saltbuf[i]));
				string salt = sb.ToString();
				byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);
				var md5 = MD5.Create();

				byte[] byteHash = md5.ComputeHash(password);

				StringBuilder hash = new StringBuilder(byteHash.Length);
				for (int i = 0; i < byteHash.Length; i++)
					hash.Append(string.Format("{0:X2}", byteHash[i]));

				user.Password = hash.ToString();
				user.Salt = salt;
				_context.users.Add(user);
				_context.SaveChanges();
				return PartialView("~/Views/Mes/Success.cshtml", "Mes");

			}

			return RedirectToAction("Index", "Mes");
		}
	}
}