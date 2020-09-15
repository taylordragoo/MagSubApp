using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagSubApp.Models;
//using AspNetCore;

namespace MagSubApp.Controllers
{
    public class WebappController : Controller
    {
        private readonly WebAppContext _context;

        public WebappController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Webapp
        public async Task<IActionResult> Index()
        {
            return View(await _context.Webappmaster.ToListAsync());
        }

        // GET: Webapp/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var webappmaster = await _context.Webappmaster
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (webappmaster == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(webappmaster);
        //}

        // GET: Webapp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Webapp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,MagSub")] Webappmaster webappmaster)
        {
            if (ModelState.IsValid)
            {
                if (_context.Webappmaster.Count(e => e.Email == webappmaster.Email && e.MagSub == webappmaster.MagSub)==0)
                {
                    if(IsValidEmail(webappmaster.Email))
                    {
                        _context.Add(webappmaster);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    } else
                    {
                        ModelState.AddModelError("Email", "Please enter a valid email address");
                    }
                } else
                {
                    ModelState.AddModelError("Email", "Email already subscribed to that magazine");
                }
            }
            return View(webappmaster);
        }

        // GET: Webapp/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var webappmaster = await _context.Webappmaster.FindAsync(id);
        //    if (webappmaster == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(webappmaster);
        //}

        // POST: Webapp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,MagSub")] Webappmaster webappmaster)
        //{
        //    if (id != webappmaster.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(webappmaster);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!WebappmasterExists(webappmaster.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(webappmaster);
        //}

        // GET: Webapp/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var webappmaster = await _context.Webappmaster
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (webappmaster == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(webappmaster);
        //}

        // POST: Webapp/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var webappmaster = await _context.Webappmaster.FindAsync(id);
        //    _context.Webappmaster.Remove(webappmaster);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool WebappmasterExists(int id)
        {
            return _context.Webappmaster.Any(e => e.Id == id);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
