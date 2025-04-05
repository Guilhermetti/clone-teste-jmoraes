#region assembly Microsoft.AspNetCore.Mvc.NewtonsoftJson, Version=5.0.14.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// C:\Users\isaias.gomes\.nuget\packages\microsoft.aspnetcore.mvc.newtonsoftjson\5.0.14\lib\net5.0\Microsoft.AspNetCore.Mvc.NewtonsoftJson.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinhaApiComSQLite.Data;
using MinhaApiComSQLite.Data.Repositories;
using MinhaApiComSQLite.Data.Repositories.Interfaces;
using MinhaApiComSQLite.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace MinhaApiComSQLite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Método chamado pela ASP.NET Core para adicionar serviços ao container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurar banco de dados SQLite
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // Configurar o Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Minha API com SQLite",
                    Version = "v1",
                    Description = "API para gerenciar produtos com SQLite",
                    Contact = new OpenApiContact
                    {
                        Name = "Matheus Guilhermetti",
                        Email = "matheusguilhermetti59@gmail.com",
                        Url = new Uri("https://github.com/guilhermetti")
                    }
                });

                // JWT Bearer
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT no formato: Bearer {seu token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Injeção de Depêndência
            services.AddTransient<TokenService>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();

            // Configurar controladores e endpoints
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // JWT
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]!);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
        }

        // Método chamado pela ASP.NET Core para configurar o pipeline HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Configurar Swagger apenas em desenvolvimento
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API com SQLite v1");
                    options.RoutePrefix = ""; // Swagger abre na URL raiz
                });
            }

            app.UseHttpsRedirection(); // Força HTTPS
            app.UseRouting(); // Habilita o roteamento
            app.UseAuthentication(); // Habilita a autenticação
            app.UseAuthorization(); // Habilita a autorização
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Mapeia controladores
            });
        }
    }
}
