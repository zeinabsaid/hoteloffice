using HotelOffice.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HotelOffice
{
    public static class DatabaseHelper
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider services)
        {
            // اطلب نسخة من DbContext من حاوية الخدمات
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                // هذا الأمر سيقوم بتطبيق أي Migrations معلقة على قاعدة البيانات
                // وإذا لم تكن قاعدة البيانات موجودة، فسيقوم بإنشائها أولاً.
                await dbContext.Database.MigrateAsync();

                Console.WriteLine("Database migration successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during database migration: {ex.Message}");
                // يمكنك إضافة معالجة أكثر تفصيلاً للخطأ هنا إذا أردت
            }
        }
    }
}