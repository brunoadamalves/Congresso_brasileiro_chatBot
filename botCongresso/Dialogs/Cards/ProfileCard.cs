using botCongresso.Model.MeuCongressoModel;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace botCongresso.Dialogs.Cards
{
    [Serializable]
    public class ProfileCard : IDialog<ProfileCard>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Este é o resultado mais próximo que encontrei :)");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            var message = activity.CreateReply();

            mcnApiPessoaPublicaModel profile;
            context.UserData.TryGetValue<mcnApiPessoaPublicaModel>("AttachmentsModel", out profile);

            List<CardAction> cardButtons = new List<CardAction>();

            if(!profile.cargo.ToLower().Contains("deputad"))
            {
                CardAction plButtonInfo = new CardAction()
                {
                    Value = "https://www25.senado.leg.br/web/senadores/senador/-/perfil/"+profile.id,
                    Type = "openUrl",
                    Title = "Informações"
                };

                CardAction plButtonGastos = new CardAction()
                {
                    Value = "http://meucongressonacional.com/senador/"+profile.id,
                    Type = "openUrl",
                    Title = "Gastos"
                };

                cardButtons.Add(plButtonInfo);
                cardButtons.Add(plButtonGastos);
            }

            var hero = new HeroCard();
            hero.Title = profile.nomeCompleto;
            hero.Subtitle = profile.cargo;
            hero.Buttons = cardButtons;
            hero.Subtitle = "Partido: " + profile.partido;

            hero.Images = new List<CardImage>
            {
                new CardImage(profile.fotoURL, profile.fotoURL)
            };

            message.Attachments.Add(hero.ToAttachment());

            await context.PostAsync(message);
            context.Done<object>(null);
        }
    }
}