using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quiz.Dominio.API.DataBase.Contexto;
using Quiz.Dominio.API.Modelos;

using System;
using System.Text;

namespace Quiz.Aplicacao.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            #region Swagger Configuration
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            #endregion


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("Jwt"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            //#region configuracao JWT validation
            ////Em seguida serão invocados os métodos AddAuthentication e AddJwtBearer.A chamada a AddAuthentication especificará 
            ////os schemas utilizados para a autenticação do tipo Bearer. Já em AddJwtBearer serão definidas configurações como a
            ////chave e o algoritmo de criptografia utilizados, a necessidade de analisar se um token ainda é válido e o tempo de 
            ////tolerância para expiração de um token(zero, no caso desta aplicação de testes);

            //var signingConfigurations = new SigningConfigurations();
            //services.AddSingleton(signingConfigurations);




            //services.AddAuthentication(authOptions =>
            //{
            //    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(bearerOptions =>
            //{
            //    var paramsValidation = bearerOptions.TokenValidationParameters;
            //    paramsValidation.IssuerSigningKey = signingConfigurations.Key;
            //    paramsValidation.ValidAudience = tokenConfigurations.Audience;
            //    paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

            //    // Valida a assinatura de um token recebido
            //    paramsValidation.ValidateIssuerSigningKey = true;

            //    // Verifica se um token recebido ainda é válido
            //    paramsValidation.ValidateLifetime = true;

            //    // Tempo de tolerância para a expiração de um token (utilizado
            //    // caso haja problemas de sincronismo de horário entre diferentes
            //    // computadores envolvidos no processo de comunicação)
            //    paramsValidation.ClockSkew = TimeSpan.Zero;
            //});

            //#endregion

            //#region Authorization
            //// A chamada ao método AddAuthorization ativará o uso de tokens com o intuito de autorizar ou não o acesso a recursos da aplicação de testes.            // a recursos deste projeto
            //services.AddAuthorization(auth =>
            //{
            //    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            //        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            //        .RequireAuthenticatedUser().Build());
            //});

            //#endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<QuizContext>(options => options.UseInMemoryDatabase());
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //Precisamos também, informar ao ASP.NET Core para utilizar a autenticação, e isto é feito no método Configure
            app.UseAuthentication();


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
