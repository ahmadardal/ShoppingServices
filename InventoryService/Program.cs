using System.Text;
using InventoryService;
using InventoryService.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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

});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


// Detta API får endast användas av administratörer

app.MapPost("/product", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")] async (Product product, InventoryContext db) =>
{

    product.id = Guid.NewGuid();

    await db.Products.AddAsync(product);

    await db.SaveChangesAsync();

    return Results.Created($"/product/{product.id}", "Successfully added product!");
});

app.MapGet("/product/{id}", async (string id, InventoryContext db) =>
{

    var product = db.Products.Where((x) => x.id.ToString() == id);

    if (product == null)
    {
        return Results.NotFound("Product not found!");
    }

    return Results.Ok(product);

    // return product != null ? Results.Ok(product) : Results.NotFound("Product not found");

});


app.MapGet("/products", async (InventoryContext db) =>
{
    return await db.Products.ToListAsync();
});

app.MapGet("/products/{category}", async (string category, InventoryContext db) =>
{
    return db.Products.Where((x) => x.category == category);
});


app.Run();
