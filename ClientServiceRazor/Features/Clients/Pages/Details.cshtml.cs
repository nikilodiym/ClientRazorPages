using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Clients.Pages;

public class Details : PageModel
{
    private readonly AppDbContext _dbContext;

    public Details(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public ClientViewModel NewClient { get; set; } = new();

    [BindProperty(Name = "PhoneForm")]
    public PhoneViewModel NewPhone { get; set; } = new();

    public List<Phone> Phones { get; set; } = new();

    [BindProperty]
    public bool ShowPhoneForm { get; set; }

    public void OnGet(int id)
    {
        NewClient = _dbContext.Clients
            .Where(c => c.Id == id)
            .Select(c => new ClientViewModel
            {
                Surname = c.Surname,
                FirstName = c.FirstName,
                Patronymic = c.Patronymic,
                Email = c.Email,
                BirthDate = c.BirthDate
            })
            .FirstOrDefault() ?? new ClientViewModel();

        Phones = _dbContext.Phones
            .Where(p => p.ClientId == id)
            .ToList();
    }

    public IActionResult OnPostShowPhoneForm(int id)
    {
        Console.WriteLine("OnPostShowPhoneForm!");
        OnGet(id);
        ShowPhoneForm = true;
        return Page();
    }

    public IActionResult OnPostAddPhone(int id)
    {
        // var sn = ms.SelectMany(kvp => kvp.Value.Errors.Select(e => $"Key: {kvp.Key}, Error: {e.ErrorMessage}"));
        // Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
        Console.WriteLine($"ID: {id}");
        ModelState.Remove("Email");
        ModelState.Remove("BirthDate");
        ModelState.Remove("Surname");
        ModelState.Remove("FirstName");
        ModelState.Remove("Patronymic");
        var ms = ModelState;
        var nc = NewClient;
        var np = NewPhone;
        var tv = TryValidateModel(NewClient);
        if (!ModelState.IsValid)
        {
            Console.WriteLine($"Not valid model state: {ModelState}");
            OnGet(id);
            return Page();
        }

        var phone = new Phone
        {
            Number = NewPhone.Number,
            CountryCode = NewPhone.CountryCode,
            ClientId = id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Phones.Add(phone);
        _dbContext.SaveChanges();

        return RedirectToPage(new { id });
    }

    public IActionResult OnPostB(int id)
    {
        Console.WriteLine("Add2Phone");
        Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
        Console.WriteLine($"Phone Number: {NewPhone.Number}");
        Console.WriteLine($"CountryCode: {NewPhone.CountryCode}");
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Not valid model state");
            OnGet(id);
            return Page();
        }

        var client = _dbContext.Clients
            .Include(c => c.Phones)
            .FirstOrDefault(client => client.Id == id);
        if (client == null)
        {
            return NotFound();
        }

        var phone = new Phone
        {
            Number = NewPhone.Number,
            CountryCode = NewPhone.CountryCode,
            ClientId = id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        client.Phones.Add(phone);
        _dbContext.SaveChanges();
        return RedirectToPage("./Details", new { id });
    }
}