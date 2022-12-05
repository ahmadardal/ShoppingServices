using System.Text;
using PaymentService;
using PaymentService.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PaymentContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<InventoryClient>(client => {
    client.BaseAddress = new Uri("http://inventoryservice");
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.SaveToken = true;


});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/payment/{productId}", async (string productId, PaymentContext db, InventoryClient inventoryClient, HttpContext http) => {

    var product = await inventoryClient.GetProductAsync(productId);

    if (product == null) {
        return Results.NotFound("Product was not found!");
    }

    var userId = http.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

    if (userId == null) {
        return Results.BadRequest("Bad token!");
    }

    var payment = new Payment();

    payment.id = Guid.NewGuid();
    payment.userId = userId;
    payment.total = product.price;
    payment.date = DateTime.UtcNow;

    await db.Payments.AddAsync(payment);
    await db.SaveChangesAsync();

    return Results.Created($"/payment/{payment.id}", "Thanks for your payment!");

});

app.Run();
