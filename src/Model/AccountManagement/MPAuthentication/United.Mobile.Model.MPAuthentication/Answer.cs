using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class Answer
    {
        private int answerID;
        public int AnswerId
        {
            get { return answerID; }
            set { answerID = value; }
        }
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
        private int questionID;
        public int QuestionId
        {
            get { return questionID; }
            set { questionID = value; }
        }
        private string answerText;
        public string AnswerText
        {
            get { return answerText; }
            set { answerText = value; }
        }
    }
}
