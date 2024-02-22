using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]

    public class DependentInfo
    {
        private MOBName dependentName;
        private string dependentId;
        private string dateOfBirth;
        private int age;
        private MOBEmpRelationship relationship;

        public MOBName DependentName
        {
            get
            {
                return this.dependentName;
            }
            set
            {
                this.dependentName = value;
            }
        }
        public string DependentId
        {
            get
            {
                return this.dependentId;
            }
            set
            {
                this.dependentId = value;
            }
        }

        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }
            set
            {
                this.dateOfBirth = value;
            }
        }
        public MOBEmpRelationship Relationship
        {
            get
            {
                return this.relationship;
            }
            set
            {
                this.relationship = value;
            }
        }
        public int Age
        {
            get
            {
                return this.age;
            }
            set
            {
                this.age = value;
            }
        }

    }
}
