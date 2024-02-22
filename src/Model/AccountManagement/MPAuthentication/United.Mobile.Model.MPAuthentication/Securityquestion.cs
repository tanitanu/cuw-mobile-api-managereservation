using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class Securityquestion
    {
        private int questionID;
        public int QuestionId
        {
            get { return questionID; }
            set { questionID = value; }
        }

        public string questionKey;
        public string QuestionKey
        {
            get { return questionKey; }
            set { questionKey = value; }
        }

        private string questionText;
        public string QuestionText
        {
            get { return questionText; }
            set { questionText = value; }
        }

        private bool used;
        public bool Used
        {
            get { return used; }
            set { used = value; }
        }


        private List<Answer> answers;
        public List<Answer> Answers
        {
            get { return answers; }
            set { answers = value; }
        }

    }
}
