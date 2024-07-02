using System.Reflection;
using System.Text;
using backend_tfg;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop.Infrastructure;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);
Entorno entorno = new Entorno(builder.Configuration);


var connectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
var databaseName = builder.Configuration.GetConnectionString("MongoDbDatabaseName");

var produccion = builder.Configuration["PRODUCCION"];

if (entorno.PRODUCCION)
{
    entorno.IP = builder.Configuration["IP_PRODUCCION"];
}else{
    entorno.IP = builder.Configuration["IP_DESARROLLO"];
}
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", builder =>
        builder.WithOrigins(entorno.IP)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true));
});
 

builder.Services.AddContextMongoDB(builder.Configuration,entorno.DB_HOST+":"+entorno.DB_PORT,entorno.DB_NAME);
builder.Services.AddRepositorios();
builder.Services.AddControllers().AddJsonOptions(options=>
{
    options.JsonSerializerOptions.PropertyNamingPolicy=null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        var titulo = "BackEnd "+entorno.APP_NAME+" "+entorno.APP_VERSION;
        c.SwaggerDoc("v1", new OpenApiInfo { Title = titulo, Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "Token JWT",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Pon **_UNICAMENTE_** el token obtenido en el login, nada más",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options=>
        {
            var tokenKey=string.Empty;
            tokenKey=entorno.JWT_SECRET;       
            var tokenAudience=string.Empty;
            tokenAudience=entorno.JWT_AUDIENCE;
            var tokenIssuer=string.Empty;
            tokenIssuer=entorno.JWT_ISSUER;
            if (tokenKey is not null && tokenAudience is not null){
                options.TokenValidationParameters=new()
                {
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(entorno.JWT_SECRET)),
                    ValidIssuer=tokenIssuer,
                    ValidAudience=tokenAudience,
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateLifetime=true,
                    
                };
            }else{
                throw new ArgumentNullException("Debe definir una Token:Key en la configuracion"); 
            }
        }        
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CORS");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WebChat>("/WebChat");
});


app.Run();

