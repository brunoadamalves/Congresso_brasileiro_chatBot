using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace botCongresso.Model.MeuCongressoModel
{
    public class mcnApiPessoaPublicaModel
    {
        public string id { get; set; }
        public string nomeParlamentar { get; set; }
        public string nomeCompleto { get; set; }
        public string cargo { get; set; }
        public string partido { get; set; }
        public string mandato { get; set; }
        public string sexo { get; set; }
        public string uf { get; set; }
        public object telefone { get; set; }
        public object email { get; set; }
        public string nascimento { get; set; }
        public string fotoURL { get; set; }
        public float gastoTotal { get; set; }
        public float gastoPorDia { get; set; }
    }

}