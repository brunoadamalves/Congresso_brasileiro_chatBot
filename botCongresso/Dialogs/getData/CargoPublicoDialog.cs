using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Cognitive.LUIS;
using botCongresso.Utils.LUIS;
using botCongresso.Model.LUIS;
using botCongresso.Utils.Messages;

namespace botCongresso.Dialogs.getData
{
    [Serializable]
    public class CargoPublicoDialog : IDialog<string>
    {
        private int tentativas = 2;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Qual cargo público você está pesquisando? Ex: 'Deputado', 'Senador'.");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if(tentativas != 0)
            {
                var message = await result;

                LuisResultModel luisResult = await LuisClassification.luisRequest(message.Text);

                if ((message.Text != null) && (message.Text.Trim().Length <= 8) && (luisResult.entities.Length > 0) )
                {
                    if(luisResult.entities[0].entity.Contains("senador") || luisResult.entities[0].entity.Contains("deputad"))
                    {
                        context.Done(luisResult.entities[0].entity);
                    }
                    else
                    {
                        await context.PostAsync("Me desculpe, no momento só posso realizar pesquisas para os cargos de *Senador e *Deputado.");
                        context.Wait(this.MessageReceivedAsync);
                        tentativas--;
                    }
                }
                else
                {
                    await context.PostAsync("Me desculpe, não consegui entender sua resposta. Qual o nome do cargo que você está pesquisando? (ex: 'Deputado', 'Senador').");
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