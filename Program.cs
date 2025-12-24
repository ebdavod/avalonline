using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Services;
using AvvalOnline.Shop.Api.Services.Implementations;
using AvvalOnline.Shop.Api.Services.Implementations.Payments;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Database
    var cs = builder.Configuration.GetConnectionString("Default");
    if (string.IsNullOrEmpty(cs))
        throw new Exception("Connection string 'Default' not found in configuration");

    builder.Services.AddDbContext<ShopDbContext>(op => op.UseSqlServer(cs));

    // JWT
    var jwtConfigs = builder.Configuration.GetSection("JWT").Get<JWTConfigModel>();
    if (jwtConfigs == null)
        throw new Exception("JWT configuration section not found or invalid");

    builder.Services.AddScoped<JWTConfigModel>(_ => jwtConfigs);
    builder.Services.AddScoped<IJwtUtils, JwtUtils>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IDiscountService, DiscountService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IHomeService, HomeService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();

    // Payment Configs
    var zarinpalConfig = builder.Configuration.GetSection("PaymentGateways:ZarinPal");
    var merchantId = zarinpalConfig.GetValue<string>("MerchantId");
    var sandboxMode = zarinpalConfig.GetValue<bool>("SandboxMode");

    // Register PaymentService + Gateways
    builder.Services.AddScoped<IPaymentService, PaymentService>();
    builder.Services.AddHttpClient<ZarinPalGateway>();
    builder.Services.AddScoped<IPaymentGateway>(sp =>
    {
        var httpClient = sp.GetRequiredService<HttpClient>();
        return new ZarinPalGateway(httpClient, merchantId, sandboxMode);
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(x =>
    {
        x.AllowAnyHeader();
        x.AllowAnyMethod();
        x.AllowAnyOrigin();
    });

    app.UseAuthorization();

    // Ensure the ProductPictures directory exists
    var productPicturesPath = Path.Combine(Directory.GetCurrentDirectory(), "ProductPictures");
    if (!Directory.Exists(productPicturesPath))
        Directory.CreateDirectory(productPicturesPath);

    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(productPicturesPath),
        RequestPath = new PathString("/ProductPictures")
    });

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    var logDirectory = "logs";
    if (!Directory.Exists(logDirectory))
        Directory.CreateDirectory(logDirectory);

    var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}{Environment.NewLine}Stack Trace: {ex.StackTrace}{Environment.NewLine}{new string('-', 80)}{Environment.NewLine}";
    var path = Path.Combine(logDirectory, "startup_exceptions.txt");
    File.AppendAllText(path, logMessage);

    throw;
}
