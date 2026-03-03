using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class FinanceAccountTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateFinanceAccount()
    {
        using var context = GetDbContext();
        var acc = new FinanceAccount
        {
            Balance = 123.45m
        };
        context.FinanceAccounts.Add(acc);
        context.SaveChanges();

        var saved = context.FinanceAccounts.First();
        Assert.Equal(123.45m, saved.Balance);
        Assert.True(saved.Id > 0);
    }
}
