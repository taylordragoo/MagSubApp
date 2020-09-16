using System;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagSubApp.Models;

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

        // GET: Webapp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Webapp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,SubID")] Webappmaster webappmaster)
        {
            if (ModelState.IsValid && webappmaster.SubID != 0)
            {
                if (_context.Webappmaster.Count(e => e.Email == webappmaster.Email && e.SubID == webappmaster.SubID) == 0)
                {
                    if (IsValidEmail(webappmaster.Email))
                    {
                        _context.Add(webappmaster);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Please enter a valid email address");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email already subscribed to that magazine");
                }
            } else
            {
                ModelState.AddModelError("SubID", "Please select a magazine");
            } 
            
            return View(webappmaster);
        }

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
                // Set and then normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

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
