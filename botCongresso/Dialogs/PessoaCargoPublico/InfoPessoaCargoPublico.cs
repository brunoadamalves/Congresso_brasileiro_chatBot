using botCongresso.Dialogs;
using botCongresso.Dialogs.Cards;
using botCongresso.Dialogs.getData;
using botCongresso.Model;
using botCongresso.Utils;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace botCongresso.Dialogs
{
    //public class InfoPessoaCargoPublico : LuisDialog<object>
    [Serializable]
    public class InfoPessoaCargoPublico : IDialog<PessoaCargoPublicoModel>
    {
        private string nomePessoa;
        private string cargoPublico;
        private string unidadeFederativa;

        public InfoPessoaCargoPublico(LuisResult result)
        {
            nomePessoa = result.Entities.FirstOrDefault(c => c.Type == "nome_pessoa")?.Entity;
            cargoPublico = result.Entities.FirstOrDefault(c => c.Type == "nome_cargo_publico")?.Entity;
            unidadeFederativa = result.Entities.FirstOrDefault(c => c.Type == "unidade_federativa")?.Entity;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("verificando...");
            
            await this.MessageReceivedAsync(context);
        }

        //obs: Refatorar nomes

        private async Task MessageReceivedAsync(IDialogContext context)
        {
            if(string.IsNullOrEmpty(this.unidadeFederativa))
            {
                context.Call(new UnidadeFederativaDialog(), this.MessageReceivedResumeAfter);
            }
            else if (string.IsNullOrEmpty(this.cargoPublico))
            {
                context.Call(new CargoPublicoDialog(), this.CargoPublicoResumeAfter);
            }
            else if(string.IsNullOrEmpty(this.nomePessoa)) 
            {
                context.Call(new NomePessoaDialog(), this.NomePessoaResumeAfter);
            }
            else
            {
                PessoaCargoPublicoModel result = new PessoaCargoPublicoModel(this.nomePessoa,
                                                                             this.cargoPublico,
                                                                             this.unidadeFederativa);
                context.Done(result);
            }
        }

        private async Task MessageReceivedResumeAfter(IDialogContext context, IAwaitable<String> result)
        {
            if (String.IsNullOrEmpty(this.unidadeFederativa))
            {
                this.unidadeFederativa = await result;
            }

            await this.MessageReceivedAsync(context);
        }

        private async Task CargoPublicoResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            if(String.IsNullOrEmpty(this.cargoPublico))
            {
                this.cargoPublico = await result;
            }

            await this.MessageReceivedAsync(context);
        }

        private async Task NomePessoaResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            if(String.IsNullOrEmpty(this.nomePessoa))
            {
                this.nomePessoa = await result;
            }

            await this.MessageReceivedAsync(context);
        }
    }
}