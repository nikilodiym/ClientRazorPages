using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientServiceRazor.Features.Clients.Pages;

public class Index : PageModel
{
    private readonly AppDbContext _dbContext;

    public Index(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //Для форми
    [BindProperty]
    public ClientViewModel NewClient { get; set; } = new();

    //Для таблиці
    public List<Client> Clients { get; set; } = new();

    public void OnGet()
    {
        // Clients = _dbContext.Clients.ToList();
        Clients = _dbContext.Clients.OrderByDescending(c => c.CreatedAt).ToList();
        //TODO
        Console.WriteLine("OnGet!");
        Console.WriteLine($"Clients: {Clients.Count}");
    }

    public IActionResult OnPost()
    {
        Console.WriteLine("OnPost!");
        Console.WriteLine(NewClient.Surname);
        if (!ModelState.IsValid)
        {
            //Якщо форма не валідна, знову показуємо таблицю
            Clients = _dbContext.Clients.OrderByDescending(c => c.CreatedAt).ToList();
            //TODO
            Console.WriteLine("Invalid Form!");
            Console.WriteLine($"Clients: {Clients.Count}");
            return Page();
        }

        //Якщо форма валідна
        var client = new Client
        {
            Surname = NewClient.Surname,
            FirstName = NewClient.FirstName,
            Patronymic = NewClient.Patronymic,
            Email = NewClient.Email,
            //TODO
            BirthDate = NewClient.BirthDate,
            // BirthDate = DateOnly.FromDateTime(NewClient.BirthDate),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        //TODO
        Console.WriteLine("Valid Form!");
        Console.WriteLine(client.BirthDate);
        Console.WriteLine(client.CreatedAt);
        Console.WriteLine(client.UpdatedAt);
        //Збереження в базу даних
        _dbContext.Clients.Add(client);
        //Збереження змін
        _dbContext.SaveChanges();
        //Перезавантаження сторінки
        return RedirectToPage();
    }
}