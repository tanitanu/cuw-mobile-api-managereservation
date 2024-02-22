using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBEmpRelationship
    {
        private string relationship;
        private string relationshipDescription;
        private string relationshipSubType;
        private string relationshipSubTypeDescription;

        public string Relationship
        {
            get { return this.relationship; }
            set { this.relationship = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string RelationshipDescription
        {
            get { return this.relationshipDescription; }
            set { this.relationshipDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string RelationshipSubType
        {
            get { return this.relationshipSubType; }
            set { this.relationshipSubType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string RelationshipSubTypeDescription
        {
            get { return this.relationshipSubTypeDescription; }
            set { this.relationshipSubTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
