using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace botCongresso.Model
{
    public class PessoaCargoPublicoModel
    {
        public string nomePessoa;
        public string cargo;
        public string unidade_federativa;

        public PessoaCargoPublicoModel(string nomePessoa, string cargo, string unidade_federativa)
        {
            this.nomePessoa = nomePessoa;
            this.cargo = cargo;
            this.unidade_federativa = unidade_federativa;
        }

        public void getPerfilPessoaCargoPublico(PessoaCargoPublicoModel pessoa)
        {

        }
    }
}