using HotelOffice.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HotelOffice.Business.Services; // تأكد من وجود هذا السطر
using System.Threading.Tasks;          // تأكد من وجود هذا السطر

namespace HotelOffice
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // 1. تحديد مسار قاعدة البيانات
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "HotelOffice.db");

           // 2. تسجيل مصنع DbContext
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}")
);

            // 3. تسجيل خدمات Business Logic
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGuestService, GuestService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            // 4. بناء التطبيق وتهيئة قاعدة البيانات
            var app = builder.Build();

            // استدعاء المساعد لتهيئة قاعدة البيانات عند بدء التشغيل
            _ = Task.Run(() => DatabaseHelper.InitializeDatabaseAsync(app.Services));

            try
            {
                // نستدعي الدالة التي أنشأناها
                Task.Run(() => HotelOffice.Data.DatabaseSeeder.SeedAsync(app.Services)).Wait();
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، نقوم بطباعته لنعرف السبب
                System.Diagnostics.Debug.WriteLine($"An error occurred during database seeding: {ex.Message}");
            }

            return app;
        }
    }
}