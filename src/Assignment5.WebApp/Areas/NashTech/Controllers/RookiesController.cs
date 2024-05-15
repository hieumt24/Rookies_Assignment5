using Assignment5.Application.Interfaces;
using Assignment5.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Assignment5.WebApp.Areas.NashTech.Controllers
{
    public class RookiesController : Controller
    {
        private readonly IPersonService _personService;

        public RookiesController(IPersonService personService)
        {
            _personService = personService;
        }

        public const int ITEMS_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }

        public int countPages { get; set; }

        public IActionResult Index(string nameFilter)
        {
            var query = _personService.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(p => p.FirstName.ToLower().Contains(nameFilter.ToLower()) || p.LastName.ToLower().Contains(nameFilter.ToLower()));
            }

            var totalPersons = query.Count();
            countPages = (int)Math.Ceiling((double)totalPersons / ITEMS_PER_PAGE);

            if (currentPage < 1)
            {
                currentPage = 1;
            }
            if (currentPage > countPages)
            {
                currentPage = countPages;
            }

            query = query.OrderBy(p => p.FirstName)
                         .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                         .Take(ITEMS_PER_PAGE);

            ViewBag.TotalPerson = totalPersons;
            ViewBag.CurrentPage = currentPage;
            ViewBag.CountPages = countPages;
            ViewBag.NameFilter = nameFilter;

            return View("Index", query);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _personService.Create(person);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error" + ex.Message);
                }
            }
            return View(person);
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                _personService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex.Message);
            }
            return View();
        }

        public IActionResult Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            var person = _personService.GetPersonById(id.Value);
            if (person == null)
            {
                return RedirectToAction("Index");
            }
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _personService.Update(person);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error" + ex.Message);
                }
            }
            return View(person);
        }

        public IActionResult Details(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            var person = _personService.GetPersonById(id.Value);
            if (person == null)
            {
                return RedirectToAction("Index");
            }
            return View(person);
        }

        public IActionResult GetMaleMember()
        {
            var listPerson = _personService.GetMaleMembers();
            if (listPerson == null)
            {
                return RedirectToAction("Index");
            }
            return View(listPerson);
        }

        public IActionResult GetOldestMember()
        {
            var oldestPerson = _personService.GetOldestMember();
            if (oldestPerson == null)
            {
                return RedirectToAction("Index");
            }
            return View("Details", oldestPerson);
        }

        public IActionResult GetMemberFullNames()
        {
            var memberFullNames = _personService.GetMemberFullNames();
            if (memberFullNames == null)
            {
                return RedirectToAction("Index");
            }
            return View("FullNames", memberFullNames);
        }

        public IActionResult FilterMembersByBirthYear(int year, string comparisonType)
        {
            var listPerson = _personService.FilterMembersByBirthYear(year, comparisonType);
            if (listPerson == null)
            {
                return RedirectToAction("Index");
            }
            return View("Index", listPerson);
        }

        public IActionResult ExportToExcel()
        {
            var file = _personService.GetMembersAsExcelFile();
            if (file == null)
            {
                return RedirectToAction("Index");
            }
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Members.xlsx");
        }
    }
}