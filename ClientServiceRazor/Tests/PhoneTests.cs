using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class PhoneTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreatePhone_WithClientNavigation()
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
        var phone = new Phone
        {
            Number = "+380111111111",
            CountryCode = CountryCode.UA,
            Client = client
        };
        context.Phones.Add(phone);
        context.SaveChanges();

        Assert.Equal(1, context.Phones.Count());
        var saved = context.Phones.Include(p => p.Client).First();
        Assert.NotNull(saved.Client);
        Assert.Equal(CountryCode.UA, saved.CountryCode);
        Assert.Equal("+380111111111", saved.Number);
    }

    [Fact]
    public void CreatePhone_WithClientIdOnly()
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

        var phone = new Phone
        {
            Number = "+380222222222",
            CountryCode = CountryCode.UA,
            ClientId = client.Id
        };
        context.Phones.Add(phone);
        context.SaveChanges();

        var saved = context.Phones.Include(p => p.Client).First();
        Assert.Equal(client.Id, saved.ClientId);
        Assert.Equal("+380222222222", saved.Number);
    }
}
