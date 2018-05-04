using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace botCongresso.Dialogs
{
    [Serializable]
    public class QnADialog : QnAMakerDialog
    {
        public QnADialog() : base(new QnAMakerService(new QnAMakerAttribute(ConfigurationManager.AppSettings["QnaAppID"],
                                                                            ConfigurationManager.AppSettings["QnAKnowledgeBaseKey"], 
                                                                            "Não encontrei sua resposta", 0.5)))
        {
        }
    }
}