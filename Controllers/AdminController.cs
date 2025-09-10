using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RestautantMvc.DTOs;
using RestautantMvc.Services;
using RestautantMvc.ViewModels;
using System.Reflection;
using System.Threading.Tasks;

namespace RestautantMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBookingApiServices _bookingApiServices;
        private readonly ITableApiService _tableApiService;
        private readonly IAuthService _authService;
        private readonly IMenuApiService _menuApiService;

        public AdminController(IBookingApiServices bookingApiServices, ITableApiService tableApiService, IAuthService authService, IMenuApiService menuApiService)
        {
            _authService = authService;
            _bookingApiServices = bookingApiServices;
            _tableApiService = tableApiService;
            _menuApiService = menuApiService;
        }

        public async Task<IActionResult> Index(string activeTab = "bookings")
        {
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Index", "Admin") });
            }

            ViewData["ActiveTab"] = activeTab;

            return View();
        }

        // Booking Actions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBookingTable(int bookingId, int tableId, DateTime newDate, int newNumberOfGuests, TimeSpan newTime)
        {
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _bookingApiServices.UpdateBookingTable(bookingId, tableId, newDate, newTime, newNumberOfGuests);

            if (result)
            {
                TempData["SuccessMessage"] = "Booking table updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update booking table.";
            }

            return RedirectToAction("Index", new { activeTab = "bookings" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking(CreateBooking booking)
        {
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input. All the fields are required.";
                return RedirectToAction("Index", new { activeTab = "bookings" });
            }

            var tables = await _tableApiService.GetAllTables();
            var selectedTable = tables.FirstOrDefault(t => t.TableId == booking.TableId);

            if (selectedTable == null)
            {
                TempData["ErrorMessage"] = "Selected table not found.";
                return RedirectToAction("Index", new { activeTab = "bookings" });
            }
            if (selectedTable.Capacity < booking.NumberOfGuests)
            {
                TempData["ErrorMessage"] = $"Table {selectedTable.TableNumber} has capacity for {selectedTable.Capacity} guests, but you're trying to book for {booking.NumberOfGuests} guests.";
                return RedirectToAction("Index", new { activeTab = "bookings" });
            }

            var result = await _bookingApiServices.CreateBooking(booking);

            if (result != null)
            {
                TempData["SuccessMessage"] = $"Booking created successfully for {booking.CustomerName} on {booking.BookingDate:MMM dd, yyyy} at {booking.BookingTime:hh\\:mm}";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create booking.";
            }
            return RedirectToAction("Index", new { activeTab = "bookings" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _bookingApiServices.DeleteBooking(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Booking deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete booking.";
            }

            return RedirectToAction("Index", new { activeTab = "bookings" });
        }

        // Table Actions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTable(CreateTableRequest request)
        {
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input. Table number and capacity are required.";
                return RedirectToAction("Index", new { activeTab = "bookings" });
            }

            var result = await _tableApiService.CreateTable(request);

            if (result != null)
            {
                TempData["SuccessMessage"] = $"Table {result.TableNumber} created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create table. The table number might already exist.";
            }

            return RedirectToAction("Index", new { activeTab = "tables" });
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTable(int id)
        {
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _tableApiService.DeleteTable(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Table deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete table. It might be referenced by existing bookings.";
            }

            return RedirectToAction("Index", new { activeTab = "tables" });
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTable(int id, UpdateTableRequest request)
        {
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input. Table number and capacity are required.";
                return RedirectToAction("Index", new { activeTab = "tables" });
            }

            var result = await _tableApiService.UpdateTable(id, request);

            if (result != null)
            {
                TempData["SuccessMessage"] = $"Table {result.TableNumber} updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update table. The table number might already be in use.";
            }

            return RedirectToAction("Index", new { activeTab = "tables" });
        }

        //menu items
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMenuItem(int id, UpdateMenuItem request)
        {
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"Form key: {key}, Value: {Request.Form[key]}");
            }
            if (!await _authService.IsAuth())
            {
                return RedirectToAction("Login", "Auth");
            }
           
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your input. Make sure the category is right.";
                return RedirectToAction("Index", new { activeTab = "menu" });
            }
            var result = await _menuApiService.UpdateMenuItem(id, request);
            if (result != null)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorMessage"] = $"Validation errors: {string.Join(", ", errors)}";
                TempData["SuccessMessage"] = $"Menu item {result.Name} updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update the menu item. Check your input.";

            }
            return RedirectToAction("Index", new { activeTab = "menu" });
        }

    }
}
