using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SdaemonTestCrud.Models;
using System.Text;

namespace SdaemonTestCrud.Controllers
{
    public class ToDoController : Controller
    {
        // Base URL of the Web API
        private readonly string url = "https://localhost:7292/api/ToDo/";

        // HttpClient used to send requests to the API
        private readonly HttpClient client = new HttpClient();

        // GET: ToDo/Index
        // Fetches all tasks from the API and displays them in the Index view
        [HttpGet]
        public IActionResult Index()
        {
            List<TaskItem> task = new List<TaskItem>();

            // Sending GET request to API
            HttpResponseMessage response = client.GetAsync(url).Result;

            // If the response is successful, deserialize and return the list of tasks
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                task = JsonConvert.DeserializeObject<List<TaskItem>>(result);
            }

            return View(task); // Send the list to the view
        }

        // GET: ToDo/Create
        // Returns the form view for creating a new task
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDo/Create
        // Sends the new task to the API for saving
        [HttpPost]
        public async Task<IActionResult> Create(TaskItem item)
        {
            // Serialize TaskItem object into JSON format
            string data = JsonConvert.SerializeObject(item);

            // Prepare the content with correct encoding and MIME type
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            // Send POST request to API to create new task
            HttpResponseMessage response = await client.PostAsync(url, content);

            // On success, redirect to Index page with success message
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Task saved successfully!";
                return RedirectToAction("Index");
            }

            // If there's an error, return the view with existing data
            return View(item);
        }

        // GET: ToDo/Details/{id}
        // Retrieves a single task by ID and displays it
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await client.GetAsync(url + id);

            // If task exists, return it to the view
            if (response.IsSuccessStatusCode)
            {
                var task = await response.Content.ReadFromJsonAsync<TaskItem>();
                return View(task);
            }

            // If task not found, return 404
            return NotFound();
        }

        // GET: ToDo/Edit/{id}
        // Retrieves a task by ID and loads it in the edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await client.GetAsync(url + id);

            if (response.IsSuccessStatusCode)
            {
                var task = await response.Content.ReadFromJsonAsync<TaskItem>();
                return View(task);
            }

            return NotFound(); // Return 404 if task doesn't exist
        }

        // POST: ToDo/Edit
        // Sends the updated task to the API
        [HttpPost]
        public async Task<IActionResult> Edit(TaskItem task)
        {
            // Convert updated task to JSON
            string data = JsonConvert.SerializeObject(task);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            // Send PUT request to update the task
            HttpResponseMessage response = await client.PutAsync(url + task.Id, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Task updated successfully!";
                return RedirectToAction("Index");
            }

            // Return the same view if update fails
            return View(task);
        }

        // GET: ToDo/Delete/{id}
        // Shows the confirmation page for deleting a task
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await client.GetAsync(url + id);

            if (response.IsSuccessStatusCode)
            {
                var task = await response.Content.ReadFromJsonAsync<TaskItem>();
                return View(task);
            }

            return NotFound(); // Task not found
        }

        // POST: ToDo/Delete
        // Sends a DELETE request to remove the task from the API
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + id);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Task deleted successfully!";
                return RedirectToAction("Index");
            }

            return View(); // Return to delete view in case of failure
        }
    }
}
