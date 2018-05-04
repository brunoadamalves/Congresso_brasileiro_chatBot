using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace botCongresso.Dialogs.getData
{
    [Serializable]
    public class NomePessoaDialog : IDialog<string>
    {

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Me diga o nome da pessoa que você está pesquisando :)");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                context.Done(message.Text);
            }
            else
            {
                await context.PostAsync("Me desculpe, não consegui entender sua resposta. Qual o nome da pessoa que você está pesquisando?");

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}