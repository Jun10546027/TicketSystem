using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketSystemDesign.Models;

namespace TicketSystemDesign.Controllers
{
    public class UserInfoesController : Controller
    {
        private readonly TickerSystemContext _context;

        public UserInfoesController(TickerSystemContext context)
        {
            _context = context;
        }

        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public async Task<IActionResult> Index()
        {
            var tickerSystemContext = _context.UserInfo.Include(u => u.StatusNavigation);
            return View(await tickerSystemContext.ToListAsync());
        }

        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo
                .Include(u => u.StatusNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public IActionResult Create()
        {
            ViewData["Status"] = new SelectList(_context.UserRole, "Status", "Role");
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Status,Pwd,Salt")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                userInfo.Salt = CommonHelper.RandomString(64);
                userInfo.Pwd = CommonHelper.GetHastText(userInfo.Pwd, userInfo.Salt);

                _context.Add(userInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Status"] = new SelectList(_context.UserRole, "Status", "Role", userInfo.Status);
            return View(userInfo);
        }

        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserInfo userInfo = await _context.UserInfo.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            // Clean the Password
            userInfo.Pwd = "";

            ViewData["Status"] = new SelectList(_context.UserRole, "Status", "Role", userInfo.Status);
            return View(userInfo);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,UserName,Status,Pwd,Salt")] UserInfo userInfo)
        {
            if (id != userInfo.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userInfo.Salt = CommonHelper.RandomString(64);
                    userInfo.Pwd = CommonHelper.GetHastText(userInfo.Pwd, userInfo.Salt);

                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Status"] = new SelectList(_context.UserRole, "Status", "Role", userInfo.Status);
            return View(userInfo);
        }


        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo
                .Include(u => u.StatusNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var userInfo = await _context.UserInfo.FindAsync(id);
            _context.UserInfo.Remove(userInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]
        [ClaimRequirement("RoleStatus", "4")]
        private bool UserInfoExists(long id)
        {
            return _context.UserInfo.Any(e => e.UserId == id);
        }
    }
}
