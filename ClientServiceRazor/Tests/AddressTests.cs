using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class AddressTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateAddress_WithClientNavigation()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surn",
            FirstName = "Fn",
            Email = "e@e",
            Patronymic = "P",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        var address = new Address
        {
            Country = "UA",
            Region = "R",
            Area = "A",
            City = "C",
            Street = "St",
            Building = "1",
            Apartment = "2",
            Entrance = "E",
            Room = "101",
            Client = client
        };
        context.Addresses.Add(address);
        context.SaveChanges();

        Assert.Equal(1, context.Addresses.Count());
        var saved = context.Addresses.Include(a => a.Client).FirstOrDefault();
        Assert.NotNull(saved);
        Assert.NotNull(saved.Client);
        Assert.Equal("UA", saved.Country);
        Assert.Equal(client.Surname, saved.Client.Surname);
    }

    [Fact]
    public void CreateAddress_WithClientIdOnly()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surn2",
            FirstName = "Fn2",
            Email = "e2@e",
            Patronymic = "P2",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        context.Clients.Add(client);
        context.SaveChanges();

        var address = new Address
        {
            Country = "UA",
            Region = "R2",
            Area = "A2",
            City = "C2",
            Street = "St2",
            Building = "10",
            Apartment = "20",
            Entrance = "E2",
            Room = "202",
            ClientId = client.Id
        };
        context.Addresses.Add(address);
        context.SaveChanges();

        var saved = context.Addresses.Include(a => a.Client).First();
        Assert.Equal(client.Id, saved.ClientId);
        Assert.Equal(client.Surname, saved.Client.Surname);
    }
}
