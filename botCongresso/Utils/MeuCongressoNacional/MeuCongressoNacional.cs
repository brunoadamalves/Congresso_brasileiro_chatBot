using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using botCongresso.Model;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using botCongresso.Model.MeuCongressoModel;
using System.Text;

namespace botCongresso.Utils
{
    public static class MeuCongressoNacional
    {
        private static Model.MeuCongressoModel.mcnApiPessoaPublicaModel senador;
        private static readonly string senadorUrl = "http://meucongressonacional.com/api/001/senador";
        private static readonly string deputadoUrl = "http://meucongressonacional.com/api/001/deputado";

        public static mcnApiPessoaPublicaModel getPessoaPublica(PessoaCargoPublicoModel pessoaCP, string cargo)
        {
            string uri = string.Empty;
            if(cargo.ToLower().Contains("deputad"))
            {
                uri = deputadoUrl;
            }
            else
            {
                uri = senadorUrl;
            }

            using (var webClient = new System.Net.WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var json = webClient.DownloadString(uri);
                List<Model.MeuCongressoModel.mcnApiPessoaPublicaModel> result = new JavaScriptSerializer().Deserialize<List<Model.MeuCongressoModel.mcnApiPessoaPublicaModel>>(json);

                if(pessoaCP.unidade_federativa.Length > 2)
                {
                    pessoaCP.unidade_federativa = setUF(pessoaCP.unidade_federativa);
                }

                mcnApiPessoaPublicaModel pessoa = result.FirstOrDefault(x => (x.nomeCompleto.ToLower().Contains(pessoaCP.nomePessoa.ToLower()) 
                                                                                    || x.nomeParlamentar.ToLower().Contains(pessoaCP.nomePessoa.ToLower())) 
                                                                        && x.uf.ToLower().Equals(pessoaCP.unidade_federativa.ToLower()));

                return pessoa;
            }
        }

        private static string setUF(string unidadeFederativa)
        {
            string result = string.Empty;

            switch (unidadeFederativa.ToLower())
            {
                case "acre":
                    result = "ca";
                    break;
                case "alagoas":
                    result = "al";
                    break;
                case "amazonas":
                    result = "am";
                    break;
                case "amapá":
                    result = "ap";
                    break;
                case "amapa":
                    result = "ba";
                    break;
                case "bahia":
                    result = "ba";
                    break;
                case "ceará":
                    result = "ce";
                    break;
                case "ceara":
                    result = "ce";
                    break;
                case "distrito federal":
                    result = "df";
                    break;
                case "espírito santo":
                    result = "es";
                    break;
                case "espirito santo":
                    result = "es";
                    break;
                case "goias":
                    result = "go";
                    break;
                case "goiás":
                    result = "go";
                    break;
                case "maranhao":
                    result = "ma";
                    break;
                case "maranhão":
                    result = "ma";
                    break;
                case "minas gerais":
                    result = "mg";
                    break;
                case "mato grosso do sul":
                    result = "ms";
                    break;
                case "mato grosso":
                    result = "mt";
                    break;
                case "pará":
                    result = "pa";
                    break;
                case "para":
                    result = "pa";
                    break;
                case "paraíba":
                    result = "pb";
                    break;
                case "pernambuco":
                    result = "pe";
                    break;
                case "paraná":
                    result = "pr";
                    break;
                case "parana":
                    result = "pr";
                    break;
                case "rio de janeiro":
                    result = "rj";
                    break;
                case "rio grande do norte":
                    result = "rn";
                    break;
                case "rondônia":
                    result = "ro";
                    break;
                case "roraima":
                    result = "rr";
                    break;
                case "rio grande do sul":
                    result = "rs";
                    break;
                case "santa catarina":
                    result = "sc";
                    break;
                case "sergipe":
                    result = "se";
                    break;
                case "são paulo":
                    result = "sp";
                    break;
                case "sao paulo":
                    result = "sp";
                    break;
                case "tocantins":
                    result = "to";
                    break;
                default:
                    result = unidadeFederativa;
                    break;
            }
                return result;
        }
    }
}