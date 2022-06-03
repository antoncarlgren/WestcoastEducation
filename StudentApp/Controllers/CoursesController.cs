using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    [Route("[controller]")]
    public class CoursesController : Controller
    {
        private readonly CourseServiceModel _coursesServiceModel;

        public CoursesController(IConfiguration config)
        {
            _coursesServiceModel = new CourseServiceModel(config);
        }
        
        // GET: courses/list
        [HttpGet("list")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var courses = await _coursesServiceModel
                    .ListCoursesAsync();
                
                return View("Index", courses);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        // GET: Courses/
        [HttpGet("categories")]
        public async Task<IActionResult> Categories()
        {
            try
            {
                var courseCategories = await _coursesServiceModel
                    .GetCoursesByCategoryAsync();

                return View("Categories", courseCategories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("Error");
            }
        }
        
        // GET: courses/5555
        [HttpGet("{courseNo:int}")]
        public async Task<IActionResult> Details(int courseNo)
        {
            try
            {
                var course = await _coursesServiceModel
                    .GetCourseByCourseNoAsync(courseNo);

                return View("Details", course);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        // GET: courses/byid/asdasdasd
        [HttpGet("byid{id}")]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var course = await _coursesServiceModel
                    .GetCourseByIdAsync(id);

                return View("Details", course);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("Error");
            }
        }
        
        // // GET: Courses/Create
        // public ActionResult Create()
        // {
        //     return View();
        // }
        //
        // // POST: Courses/Create
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Create(IFormCollection collection)
        // {
        //     try
        //     {
        //         // TODO: Add insert logic here
        //
        //         return RedirectToAction(nameof(Index));
        //     }
        //     catch
        //     {
        //         return View();
        //     }
        // }
        //
        // // GET: Courses/Edit/5
        // public ActionResult Edit(int id)
        // {
        //     return View();
        // }
        //
        // // POST: Courses/Edit/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Edit(int id, IFormCollection collection)
        // {
        //     try
        //     {
        //         // TODO: Add update logic here
        //
        //         return RedirectToAction(nameof(Index));
        //     }
        //     catch
        //     {
        //         return View();
        //     }
        // }
        //
        // // GET: Courses/Delete/5
        // public ActionResult Delete(int id)
        // {
        //     return View();
        // }
        //
        // // POST: Courses/Delete/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Delete(int id, IFormCollection collection)
        // {
        //     try
        //     {
        //         // TODO: Add delete logic here
        //
        //         return RedirectToAction(nameof(Index));
        //     }
        //     catch
        //     {
        //         return View();
        //     }
        // }
    }
}