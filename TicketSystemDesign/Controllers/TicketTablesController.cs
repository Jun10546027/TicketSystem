using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketSystemDesign.Models;

namespace TicketSystemDesign.Controllers
{
    public class TicketTablesController : Controller
    {
        private readonly TickerSystemContext _context;

        public TicketTablesController(TickerSystemContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimRequirement("RoleStatus", "1,2,4")]
        public JsonResult Resolve(long? id)
        {
            if (id == null)
            {
                return Json("Error");
            }

            var ticketTable = _context.TicketTable.Find(id);
            if (ticketTable == null)
            {
                return Json("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticketTable.Resolved = true;
                    _context.Update(ticketTable);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketTableExists(ticketTable.TicketId))
                    {
                        return Json(false);
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json("Correct");
            }

            return Json("Error");
        }

        [ClaimRequirement("RoleStatus", "1,3,4")]
        public IActionResult Create()
        {
            ViewData["TicketTypeId"] = new SelectList(GetDropDownListByRole(HttpContext.User.Claims.FirstOrDefault(m => m.Type == "RoleStatus").Value),
                                                      "TicketTypeId", "TicketType");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimRequirement("RoleStatus", "1,3,4")]
        public async Task<IActionResult> Create([Bind("Summary,Description,Severity,Priority,TicketTypeId,UserId,Resolved,TicketId")] TicketTable ticketTable)
        {
            if (ModelState.IsValid)
            {
                ticketTable = ModelChange(ticketTable);
                _context.Add(ticketTable);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index" , "Home");
            }

            ViewData["TicketTypeId"] = new SelectList(GetDropDownListByRole(HttpContext.User.Claims.FirstOrDefault(m => m.Type == "RoleStatus").Value),
                                         "TicketTypeId", "TicketType");
            return View(ticketTable);
        }

        [ClaimRequirement("RoleStatus", "1,3,4")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketTable = await _context.TicketTable.FindAsync(id);
            if (ticketTable == null)
            {
                return NotFound();
            }

            TicketTableModel ticketTableModel = new TicketTableModel();
            ticketTableModel.UserId = ticketTable.UserId;
            ticketTableModel.TicketTypeId = ticketTable.TicketTypeId;
            ticketTableModel.TicketId = ticketTable.TicketId;
            ticketTableModel.Summary = ticketTable.Summary;
            ticketTableModel.Description = ticketTable.Description;
            ticketTableModel.Severity = ticketTable.Severity;
            ticketTableModel.Priority = ticketTable.Priority;
            
            ViewData["TicketTypeId"] = new SelectList(GetDropDownListByRole(HttpContext.User.Claims.FirstOrDefault(m => m.Type == "RoleStatus").Value),
                                                     "TicketTypeId", "TicketType" , ticketTable.TicketTypeId);
            return View(ticketTableModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimRequirement("RoleStatus", "1,3,4")]
        public async Task<IActionResult> Edit(long id, [Bind("Summary,Description,Severity,Priority,TicketTypeId,UserId,TicketId")] TicketTable ticketTable)
        {
            if (id != ticketTable.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticketTable = ModelChange(ticketTable);
                    _context.Update(ticketTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketTableExists(ticketTable.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }

            ViewData["TicketTypeId"] = new SelectList(GetDropDownListByRole(HttpContext.User.Claims.FirstOrDefault(m => m.Type == "RoleStatus").Value),
                                                     "TicketTypeId", "TicketType", ticketTable.TicketTypeId);
            return View(ticketTable);
        }

        [ClaimRequirement("RoleStatus", "1,3,4")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketTable = await _context.TicketTable
                .Include(t => t.TicketType)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticketTable == null)
            {
                return NotFound();
            }

            TicketTableModel ticketTableModel = new TicketTableModel();
            ticketTableModel.UserId = ticketTable.UserId;
            ticketTableModel.TicketTypeId = ticketTable.TicketTypeId;
            ticketTableModel.TicketId = ticketTable.TicketId;
            ticketTableModel.Summary = ticketTable.Summary;
            ticketTableModel.Description = ticketTable.Description;
            ticketTableModel.Severity = ticketTable.Severity;
            ticketTableModel.Priority = ticketTable.Priority;
            ticketTableModel.Resolved = (bool)ticketTable.Resolved;
            ticketTableModel.TicketType = ticketTable.TicketType.ToString();

            return View(ticketTableModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ClaimRequirement("RoleStatus", "1,3,4")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ticketTable = await _context.TicketTable.FindAsync(id);
            _context.TicketTable.Remove(ticketTable);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool TicketTableExists(long id)
        {
            return _context.TicketTable.Any(e => e.TicketId == id);
        }

        /// <summary>
        /// Get the DropDownList value by role status
        /// </summary>
        /// <param name="curRole"></param>
        /// <returns></returns>
        private List<TicketProp> GetDropDownListByRole(string curRole)
        {
            List<TicketProp> ticketProps = new List<TicketProp>();

            foreach (TicketProp ticketProp in _context.TicketProp)
            {
                string[] rolePermissions = ticketProp.RolePermission.Split(',');
                if (rolePermissions.Contains(curRole))
                {
                    ticketProps.Add(ticketProp);
                }
            }

            return ticketProps;
        }

        /// <summary>
        /// Adding the parameter when model need to change 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private TicketTable ModelChange(TicketTable model)
        {
            TicketTable tmpModel = model;
            tmpModel.UserId = Int64.Parse(HttpContext.User.Claims.FirstOrDefault(m => m.Type == "UserId").Value);
            tmpModel.Resolved = false;
            tmpModel.DateTime = DateTime.Now;

            return tmpModel;
        }
    }
}
