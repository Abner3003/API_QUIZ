using System;
using System.Collections.Generic;
using System.Text;
using Quiz.Dominio.API.DataBase.Contexto;
using Quiz.Dominio.API.DataBase.Models;

namespace Quiz.Dominio.API.Domain
{
   public class tempAddUsers
    {
        public static void AdicionarDadosTeste(QuizContext context)
        {
            var testeUsuario1 = new Usuario
            {
                ID = 1,
                DS_NOME = "Abner",
                DS_EMAIL = "abnerc3003@gmail.com"
            };
            context.Usuarios.Add(testeUsuario1);

            context.SaveChanges();
        }
    }
}
