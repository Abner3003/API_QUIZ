using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Aplicacao.API.Dtos
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
