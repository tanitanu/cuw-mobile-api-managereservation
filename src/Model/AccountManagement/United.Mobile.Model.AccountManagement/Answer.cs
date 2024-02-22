using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class Answer
    {
        public int answerID { get; set; }
        
        public string answerKey;
        public string AnswerKey
        {
            get { return answerKey; }
            set { answerKey = value; }
        }

        public string questionKey;
        public string QuestionKey
        {
            get { return questionKey; }
            set { questionKey = value; }
        }
        public int questionID { get; set; }
        
        private string answerText;
        public string AnswerText
        {
            get { return answerText; }
            set { answerText = value; }
        }
    }
}
