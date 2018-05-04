using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using botCongresso.Dialogs;

namespace botCongresso
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    //await Conversation.SendAsync(activity, () => new LuisDialog(service));
                    await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                    break;
                case ActivityTypes.ConversationUpdate:
                    if (activity.MembersAdded.Any(o => o.Id == activity.Recipient.Id))
                    {
                        var reply = activity.CreateReply();
                        reply.Text = "Olá, tudo bem? Você pode conversar comigo para saber informações sobre senadores e deputados federais, para isso basta me dizer o *Nome, *Cargo e *Estado que você pretende pesquisar :)";

                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                    break;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private Activity HandleSystemMessage(Activity message)
        
            {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}