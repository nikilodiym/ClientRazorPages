using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Clients.Pages;

public class Address : PageModel
{
    private readonly AppDbContext _dbContext;

    public Address(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public AddressViewModel AddressVm { get; set; } = new();

    public bool AddressExists { get; set; }

    public IActionResult OnGet(int clientId)
    {
        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == clientId);

        if (client == null)
            return NotFound();

        AddressVm.ClientId = clientId;

        if (client.Address != null)
        {
            AddressExists = true;

            AddressVm.Country = client.Address.Country;
            AddressVm.City = client.Address.City;
            AddressVm.Street = client.Address.Street;
            AddressVm.PostalCode = client.Address.PostalCode;
        }

        return Page();
    }

    public IActionResult OnPost(int clientId)
    {
        if (!ModelState.IsValid)
        {
            AddressExists = _dbContext.Addresses.Any(a => a.ClientId == clientId);
            return Page();
        }

        var existingAddress = _dbContext.Addresses
            .FirstOrDefault(a => a.ClientId == clientId);

        if (existingAddress == null)
        {
            var newAddress = new Models.Address
            {
                Country = AddressVm.Country,
                City = AddressVm.City,
                Street = AddressVm.Street,
                PostalCode = AddressVm.PostalCode,
                ClientId = clientId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Addresses.Add(newAddress);
        }
        else
        {
            existingAddress.Country = AddressVm.Country;
            existingAddress.City = AddressVm.City;
            existingAddress.Street = AddressVm.Street;
            existingAddress.PostalCode = AddressVm.PostalCode;
            existingAddress.UpdatedAt = DateTime.UtcNow;
        }

        _dbContext.SaveChanges();

        return RedirectToPage(new { clientId });
    }

    public IActionResult OnPostDelete(int clientId)
    {
        var address = _dbContext.Addresses
            .FirstOrDefault(a => a.ClientId == clientId);

        if (address != null)
        {
            _dbContext.Addresses.Remove(address);
            _dbContext.SaveChanges();
        }

        return RedirectToPage(new { clientId });
    }
}
