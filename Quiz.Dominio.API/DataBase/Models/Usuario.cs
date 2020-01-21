using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Dominio.API.DataBase.Models
{
    public class Usuario
    {
        public int ID { get; set; }
        public int CD_PESSOA { get; set; }
        public string DS_NOME { get; set; }
        public string DS_EMAIL { get; set; }
        public string DS_SENHA { get; set; }
       }
}
