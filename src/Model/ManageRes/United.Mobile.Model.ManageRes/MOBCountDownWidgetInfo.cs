using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.ManageRes
{
    [Serializable()]
    public class MOBCountDownWidgetInfo
    {
        private string sectionTitle = string.Empty;
        private string sectionDescription = string.Empty;
        private string instructionLinkText = string.Empty;
        private string instructionPageTitle = string.Empty;
        private string instructionPageContent = string.Empty;

        public string SectionTitle
        {
            get
            {
                return this.sectionTitle;
            }
            set
            {
                this.sectionTitle = value;
            }
        }

        public string SectionDescription
        {
            get
            {
                return this.sectionDescription;
            }
            set
            {
                this.sectionDescription = value;
            }
        }

        public string InstructionLinkText
        {
            get
            {
                return this.instructionLinkText;
            }
            set
            {
                this.instructionLinkText = value;
            }
        }

        public string InstructionPageTitle
        {
            get
            {
                return this.instructionPageTitle;
            }
            set
            {
                this.instructionPageTitle = value;
            }
        }

        public string InstructionPageContent
        {
            get
            {
                return this.instructionPageContent;
            }
            set
            {
                this.instructionPageContent = value;
            }
        }


    }

}
