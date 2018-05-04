using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace botCongresso.Utils.Messages
{
    public static class DialogsMessages
    {
        public static async void chatStartOver(IDialogContext context)
        {
            await context.PostAsync("Desulpe, não estou conseguindo lhe entender; vamos começar novamente :)");
            context.Done<object>(null);
        }

        public static async void chatCanNotCompleteAction(IDialogContext context)
        {
            await context.PostAsync("Desculpe, não pude concluir sua solicitação :/");
            context.Done<object>(null);
        }
    }
}