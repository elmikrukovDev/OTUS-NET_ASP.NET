using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly SqliteContext _context;

        public DatabaseInitializer(SqliteContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Инициализировать базу данных. <br/>
        /// <br/>
        /// Примечание: <br/>
        /// Пересоздает базу данных если нет миграций. 
        /// При появлении Initial миграции удаляет базу и создает с применением миграции.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
            if (!appliedMigrations.Any())
            {
                await _context.Database.EnsureDeletedAsync();
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await _context.Database.MigrateAsync();
                }
                else
                {
                    await _context.Database.EnsureCreatedAsync();
                }
                await SeedDataAsync();
            }
        }

        private async Task SeedDataAsync()
        {
            var roles = FakeDataFactory.Roles;
            await _context.Roles.AddRangeAsync(roles);

            var employees = FakeDataFactory.Employees;
            await _context.Employees.AddRangeAsync(employees);

            var preferences = FakeDataFactory.Preferences;
            await _context.Preferences.AddRangeAsync(preferences);

            var customers = FakeDataFactory.Customers;
            await _context.Customers.AddRangeAsync(customers);

            await _context.SaveChangesAsync();
        }
    }
}
