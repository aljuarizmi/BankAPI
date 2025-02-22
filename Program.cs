using System.Reflection;
using System.Text;
using BankAPI.Data;
using BankAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();  // Agrega soporte para controladores
builder.Services.AddEndpointsApiExplorer(); // Permite la exploración de API
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //Una ruta absoluta al archivo que contiene comentarios XML
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //Inyectar descripciones fáciles de entender para operaciones, parámetros y esquemas basados ​​en archivos de comentarios XML
    options.IncludeXmlComments(xmlPath);

    // **Agregar definición de seguridad JWT para Swagger** (en rojo)
    //Agregue una o más "securityDefinitions" que describan cómo se protege su API al Swagger generado.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter your token"
    });

    // ✅ **Agregar requerimiento de seguridad para todos los endpoints** (en rojo)
    //Añade un requisito de seguridad global
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
}
); // ✅ Agrega Swagger para documentación

//builder.Services.AddOpenApi();
//Se agrega el dbcontext
builder.Services.AddSqlServer<BankContext>(builder.Configuration.GetConnectionString("BankConnection"));
//Capa de servicios/Service layer
//Esta sentencia nos permite inyectar el servicio
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AccountTypeService>();
builder.Services.AddScoped<LoginService>();
//Registra los servicios requeridos por los servicios de autenticación. 
//DefaultScheme especifica el nombre del esquema que se utilizará de forma predeterminada cuando no se solicita un esquema específico.
//Habilita la autenticación con JWT (Bearer Tokens).
//Especifica que el esquema de autenticación será "Bearer".
//AddJwtBearer(options=>): Configura los parámetros para validar los tokens JWT.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options=>{
        //options.TokenValidationParameters = new TokenValidationParameters: Define las reglas para validar los tokens JWT.
        options.TokenValidationParameters=new TokenValidationParameters{
            //Activa la validación de la firma del token.
            ValidateIssuerSigningKey=true,
            //Especifica la clave secreta (JWT:Key) para firmar y validar los tokens.
            //Usa una clave simétrica (SymmetricSecurityKey), lo que significa que la misma clave se usa para firmar y verificar el token.
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            //Deshabilita la validación del emisor (Issuer) y del público (Audience).
            //Esto significa que el token será aceptado sin importar de dónde provenga.
            ValidateIssuer=false,
            ValidateAudience=false
        };
    }
);
//Agrega servicios de autorización a la IServiceCollection especificada
builder.Services.AddAuthorization(options=>{
    //Agregua una política creada a partir de un delegado con el nombre proporcionado.
    options.AddPolicy("SuperAdmin",policy=>policy.RequireClaim("AdminType","Super"));
});
var app = builder.Build();

// Configurar el pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();es para Minimal APIs, y al usar controladores, esa línea no era necesaria.
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();//Con esto definimos que vamos a usar autentificación
app.UseAuthorization();
app.MapControllers();  //Esto mapea los controladores correctament

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
