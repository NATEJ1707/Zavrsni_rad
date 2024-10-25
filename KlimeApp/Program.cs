using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sgo => {
    var o = new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Upravljanje klimama API",
        Version = "v1",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "matkosanja@email.com",
            Name = "Sanja Boduljak"
        },
        Description = "API za upravljanje klimama",
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "Moja Licenca"
        }
    };
    sgo.SwaggerDoc("v1", o);
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    sgo.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Configure the DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.ConfigObject.AdditionalItems.Add("requestSnippetsEnabled", true);
});


app.UseHttpsRedirection();
app.UseStaticFiles();  // Serve static files from wwwroot folder
app.UseDefaultFiles();  // Allow serving default files like index.html
app.UseCors("CorsPolicy");  // Enable CORS

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");  // Fallback to index.html for any unmatched routes

app.Run();
