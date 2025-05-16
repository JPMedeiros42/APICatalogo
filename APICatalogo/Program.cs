using APICatalogo.Context;
using APICatalogo.DTO.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //Adicionando o filtro de exce��o global
    options.Filters.Add(typeof(APIExceptionFilter));
})
.AddJsonOptions(options =>
{
    // Configurando o JsonSerializer para ignorar refer�ncias circulares
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Autoriza��o e Autentica��o JWT
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

/*------------------------------------------------------------------------------------------*/
//Banco de Dados SQL Server
//Configurar a string de conex�o com o banco de dados SQL Server
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Checar se a string de conex�o � nula ou vazia
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
// Registrar o DBContext com o SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
/*------------------------------------------------------------------------------------------*/

/*------------------------------------------------------------------------------------------*/
// Configura a Autentica��o e Autoriza��o

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"] 
                    ?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Autenticar o esquema de autentica��o padr�o
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Desafiar o esquema de autentica��o padr�o
}).AddJwtBearer(options =>
{
    options.SaveToken = true; // Salvar o token no cookie de autentica��o
    options.RequireHttpsMetadata = false; // Desabilitar HTTPS para desenvolvimento
    options.TokenValidationParameters = new TokenValidationParameters() // Configura��o dos par�metros de valida��o do token
    {
        ValidateIssuer = true, // Validar o emissor do token
        ValidateAudience = true, // Validar o p�blico do token
        ValidateLifetime = true, // Validar o tempo de vida do token
        ValidateIssuerSigningKey = true, // Validar a chave de assinatura do token
        ClockSkew = TimeSpan.Zero, // N�o permitir atraso na valida��o do token
        ValidAudience = builder.Configuration["JWT:Audience"], // P�blico do token
        ValidIssuer = builder.Configuration["JWT:Issuer"], // Emissor do token
        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey)) // Chave de assinatura do token
    };
});

/*------------------------------------------------------------------------------------------*/

builder.Services.AddTransient<IMeuServico, MeuServico>();

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
