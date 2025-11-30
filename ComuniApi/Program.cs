using ComuniApi.BLL.Comunidades;
using ComuniApi.BLL.Conceptos;
using ComuniApi.BLL.EdoCuentas;
using ComuniApi.BLL.Foros;
using ComuniApi.BLL.Mantenimiento;
using ComuniApi.BLL.PagosOnline;
using ComuniApi.BLL.Reportes;
using ComuniApi.BLL.Users;
using ComuniApi.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ComuniApp API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT: Bearer {token}"
    };

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<ComuniContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ComuniDB"),
        new MySqlServerVersion(new Version(8, 0, 43))
        ));
//builder.Services.AddScoped<IPasswordHasher<UsuarioEntity>, PasswordHasher<UsuarioEntity>>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ConceptosService>();
builder.Services.AddScoped<EdoCuentasService>();
builder.Services.AddScoped<ComunidadService>();
builder.Services.AddScoped<IncidenciasService>();
builder.Services.AddScoped<ReportesService>();
builder.Services.AddScoped<ForosService>();
builder.Services.AddScoped<PagoOnlineService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            RoleClaimType = ClaimTypes.Role
        };
    });

var app = builder.Build();
app.UseCors("PermitirTodo");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
