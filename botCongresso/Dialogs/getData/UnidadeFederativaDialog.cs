using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using botCongresso.Model.LUIS;
using botCongresso.Utils.LUIS;
using botCongresso.Utils.Messages;

namespace botCongresso.Dialogs.getData
{
    [Serializable]
    public class UnidadeFederativaDialog : IDialog<String>
    {
        private int tentativas = 2;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Para qual estado você está pesquisando? Ex: 'RS', 'Paraná'.");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if(tentativas != 0)
            {
                var message = await result;

                LuisResultModel luisResult = await LuisClassification.luisRequest(message.Text);

                if ((message.Text != null) && (message.Text.Trim().Length > 1) && (luisResult.entities.Length > 0) )
                {
                    if(luisResult.entities[0].type == "unidade_federativa")
                    {
                        context.Done(message.Text);
                    }
                    else
                    {
                        await context.PostAsync("Me desculpe, não consegui entender sua resposta. Qual o nome do estado que você está pesquisando? (ex: 'RS', 'Minas Gerais').");
                        tentativas--;
                        context.Wait(this.MessageReceivedAsync);
                    }
                }
                else
                {
                    await context.PostAsync("Me desculpe, não consegui entender sua resposta. Qual o nome do estado que você está pesquisando? (ex: 'RS', 'Minas Gerais').");
                    tentativas--;
                    context.Wait(this.MessageReceivedAsync);
                }
            }
            else
            {
                DialogsMessages.chatStartOver(context);
                context.Done<object>(null);
            }
        }
    }
}