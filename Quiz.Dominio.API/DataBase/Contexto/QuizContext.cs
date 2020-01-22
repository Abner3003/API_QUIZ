using Microsoft.EntityFrameworkCore;
using Quiz.Dominio.API.DataBase.Models;

namespace Quiz.Dominio.API.DataBase.Contexto
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
