using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuestDialog2.Models;
using Newtonsoft.Json;

namespace GuestDialog2.Controllers
{
    public class MesController : Controller
    {
        private readonly MesContext _context;

        public MesController(MesContext context)
        {
            _context = context;
        }

        // GET: Mes
        public async Task<IActionResult> Index()
        {
            var list = await _context.messages.Include(u => u.user).ToListAsync();

            if (list.Count == 0)
                return Problem("Список пустой!");
            foreach (var m in list)
            {
                m.date = m.Datetime.ToString();
            }
            return View(list);

        }


        // GET: Mes/Details/5
        public async Task<IActionResult> Details(int? id)

        {
            var list = await _context.messages.Include(u => u.user).ToListAsync();
            if (id == null || _context.messages == null)
            {
                return NotFound();
            }

            var message = await _context.messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return PartialView(message);
        }



        // GET: Mes/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Mes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("message,mark,Datetime,UserId")] Messages messages)
        {
            messages.user = await _context.users.FindAsync(messages.UserId);
            if (ModelState.IsValid)
            {
                _context.Add(messages);
                await _context.SaveChangesAsync();
                return PartialView("Success");
            }
            return PartialView("Create");
        }
        //public async Task<IActionResult> Create([Bind("Id,message,mark,Datetime, UserId")] Messages messages)
        //{
        //    // Проверяем, что передан корректный UserId и связываем пользователя с сообщением
        //    messages.user = await _context.users.FindAsync(messages.UserId);

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(messages);
        //        await _context.SaveChangesAsync();
        //        return PartialView("Success");
        //    }

        //    // Если модель не валидна, возвращаем представление с сообщением об ошибке
        //    return PartialView("Create", messages);
        // }
        //public async Task<IActionResult> AddMes(Messages mes)
        //{
        //    mes.user = await _context.users.FindAsync(mes.UserId);
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(mes);
        //        await _context.SaveChangesAsync();
        //        string response = "Отзыв успешно добавлен!";
        //        return Json(response);
        //    }
        //    return Problem("Не получается добавить отзыв!");
        //}


        // GET: Mes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var mes = await _context.messages.FindAsync(id);
            if (mes == null || _context.messages == null)
            {
                return NotFound();
            }

            return PartialView("Edit", mes); ;
        }

        // POST: Mes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,message,mark,Datetime, UserId")] Messages mes)
        {
            mes.user = await _context.users.FindAsync(mes.UserId);
            if (id != mes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMes = await _context.messages.FirstOrDefaultAsync(m => m.Id == id);
                    if (existingMes != null)
                    {

                        existingMes.message = mes.message;
                        existingMes.mark = mes.mark;
                        existingMes.Datetime = mes.Datetime;
                        var user = await _context.users.FindAsync(mes.UserId);
                        if (user == null)
                        {
                            return NotFound("User not found");
                        }
                        existingMes.user = mes.user;
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessagesExists(mes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return PartialView("Success");
            }
            return PartialView("Edit", mes);
        }

        // GET: Mes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.messages == null)
            {
                return NotFound();
            }

            var message = await _context.messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }


            return PartialView("Delete", message);
        }

        // POST: Mes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var messages = await _context.messages.FindAsync(id);
            if (messages != null)
            {
                _context.messages.Remove(messages);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessagesExists(int id)
        {
            return _context.messages.Any(e => e.Id == id);
        }
    }
}
