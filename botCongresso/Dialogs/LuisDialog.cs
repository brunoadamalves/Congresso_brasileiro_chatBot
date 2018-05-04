using botCongresso.Dialogs.Cards;
using botCongresso.Model;
using botCongresso.Model.MeuCongressoModel;
using botCongresso.Utils.Messages;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace botCongresso.Dialogs
{
    [Serializable]
    [LuisModel("","")]
    public class LuisDialog : LuisDialog<object>
    {
        //public LuisDialog(ILuisService service) : base(service) { }

        private string name;
        
        /// <summary>
        /// Caso a intenção não seja reconhecida
        /// </summary>
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            String message = "Desculpe, não consegui entender. Digite 'ajuda' se precisar de alguma assistência :)";

            await context.PostAsync(message);
            
            context.Wait(this.MessageReceived);
            //context.Done<string>(null);
        }

        /// <summary>
        /// Caso a intenção seja um saudação
        /// </summary>
        [LuisIntent("Saudar")]
        public async Task SaudarAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá, o que você gostaria de saber?");
            await context.PostAsync("Gostaria de avisar que momento só estou podendo fazer buscas para Deputados Federais e Senadores, além de responder dúvidas relacionadas aos dois cargos :)");
            
            context.Done<string>(null);
        }

        /// <summary>
        /// Busca inforções de uma pessoa que exerce um cargo público
        /// </summary>
        [LuisIntent("Info_pessoa_cargo_publico")]
        public async Task PessoaCargoPublicoAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Aguarde um momento :)");

            context.Call(new InfoPessoaCargoPublico(result), this.PessoaCargoPublicoResumeAfter);
        }

        private async Task PessoaCargoPublicoResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var dataResult = await result as PessoaCargoPublicoModel;
            mcnApiPessoaPublicaModel pessoa = null;

            //Busca dados do serviço
            if(dataResult.cargo.Contains("senador") || dataResult.cargo.Contains("deputad"))
            {
                pessoa = Utils.MeuCongressoNacional.getPessoaPublica(dataResult, dataResult.cargo);
            }

            if (pessoa != null)
            {
                if(!String.IsNullOrEmpty(pessoa.id))
                {
                    context.UserData.Clear();
                    if(pessoa.cargo.ToLower().Contains("deputad"))
                    {
                        await context.PostAsync("No momento não estou conseguindo carregar os links para acesso ao perfil e controle de gastos de deputados :/");

                        //await context.PostAsync("https://www25.senado.leg.br/web/senadores/senador/-/perfil/"+pessoa.id);
                        context.UserData.SetValue<mcnApiPessoaPublicaModel>("AttachmentsModel", pessoa);
                        await context.Forward(new ProfileCard(), this.ProfileDialogResumeAfter, context.Activity, CancellationToken.None);
                    }
                    else
                    {
                        context.UserData.SetValue<mcnApiPessoaPublicaModel>("AttachmentsModel", pessoa);
                        await context.Forward(new ProfileCard(), this.ProfileDialogResumeAfter, context.Activity, CancellationToken.None);
                    }
                }
                else
                {
                    await context.PostAsync("Aguarde um momento :)");
                    DialogsMessages.chatCanNotCompleteAction(context);
                }
            }
            else
            {
                await context.PostAsync("Desculpe, mas não encontrei a pessoa que você está procurando; por favor revise os dados informados e tente novamente :)");
            }

            //context.Done(null);
            context.Done<object>(null);
        }

        private async Task ProfileDialogResumeAfter(IDialogContext context, IAwaitable<ProfileCard> result)
        {
            context.Done(result);
        }

        private async Task NameDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            this.name = await result;

            context.Done<object>(null);
        }

        private async Task testePegaUF(IDialogContext context, IAwaitable<IMessageActivity> value)
        {
            var codigoNomeUF = await value;
            var reply = "Me diga o estado que está pesquisando";

            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }

        /// <summary>
        /// Utiliza QnA maker; É utilizado em casos de perguntas mais gerais sobre cargos públicos
        /// </summary>
        [LuisIntent("QnA_cargo_publico")]
        public async Task qnaCargoPublicoAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Aguarde :)");
            
            var userQuestion = (context.Activity as Activity).Text;
            
            await context.Forward(new QnADialog(), ResumeAfterQnA, context.Activity, CancellationToken.None);

        }

        /// <summary>
        /// Busca inforções de uma pessoa que exerce um cargo público
        /// </summary>
        [LuisIntent("Info_uf_cargo_publico")]
        public async Task InfoUfCargoPublicoAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Entrando em pesquisa por perfil de pessoa");

            context.Call(new InfoPessoaCargoPublico(result), this.PessoaCargoPublicoResumeAfter);

        }

        private async Task ResumeAfterQnA(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
    }
}