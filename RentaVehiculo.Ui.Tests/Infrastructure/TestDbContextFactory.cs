using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;

namespace RentaVehiculo.Ui.Tests.Infrastructure;

public static class TestDbContextFactory
{
    public static string NewDatabaseName() => $"RentaVehiculo_TestDb_{Guid.NewGuid():N}";

    public static RentaVehiculosContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<RentaVehiculosContext>()
            .UseInMemoryDatabase(databaseName)
            .EnableSensitiveDataLogging()
            .Options;

        return new RentaVehiculosContext(options);
    }
}

