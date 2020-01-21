using Microsoft.EntityFrameworkCore;

namespace Quiz.Dominio.API.DataBase.Contexto
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options) : base(options)
        {

        }
    }
}
