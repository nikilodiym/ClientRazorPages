using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class ClientFinanceAccountTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void LinkClientAndFinanceAccount_WithCompositeKey()
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
        var acc = new FinanceAccount
        {
            Balance = 1000m
        };
        context.Clients.Add(client);
        context.FinanceAccounts.Add(acc);
        context.SaveChanges();

        var link = new ClientFinanceAccount
        {
            ClientId = client.Id,
            FinanceAccountId = acc.Id
        };
        context.ClientFinanceAccounts.Add(link);
        context.SaveChanges();

        var saved = context.ClientFinanceAccounts.Include(c => c.Client).Include(c => c.FinanceAccount).First();
        Assert.Equal(client.Id, saved.ClientId);
        Assert.Equal(acc.Id, saved.FinanceAccountId);
        Assert.Equal(1000m, saved.FinanceAccount.Balance);
    }
}
