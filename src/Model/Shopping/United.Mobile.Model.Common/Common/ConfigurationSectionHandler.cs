using System.Configuration;
using System.Xml;

namespace United.Mobile.Model.Shopping
{
    /// <summary>
    /// Custom section 
    /// </summary>
    public class RewardSection : ConfigurationSection
    {
        [ConfigurationProperty("rewardTypes", IsDefaultCollection = true, IsRequired = true)]
        public RewardTypeElementCollection RewardTypes
        {
            get
            {
                return ((RewardTypeElementCollection)base["rewardTypes"]);
            }
        }
    }


    /// <summary>
    /// TypeElement
    /// </summary>
    public class RewardTypeElement : ConfigurationElement
    {
        internal string _ElementName = "rewardType";


        public RewardTypeElement()
        {
        }


        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return ((string)base["key"]);
            }

            set
            {
                base["key"] = value;
            }
        }


        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return ((string)base["type"]);
            }

            set
            {
                base["type"] = value;
            }
        }


        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get
            {
                return ((string)base["description"]);
            }

            set
            {
                base["description"] = value;
            }
        }

        [ConfigurationProperty("productID", IsRequired = true)]
        public string ProductID
        {
            get
            {
                return ((string)base["productID"]);
            }

            set
            {
                base["productID"] = value;
            }
        }

        protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
        {
            bool retVal = false;

            if (!base.SerializeElement(null, false))
                return false;

            if (writer != null)
            {
                writer.WriteStartElement(this._ElementName);

                retVal |= base.SerializeElement(writer, false);

                writer.WriteEndElement();

                return retVal;
            }

            return (retVal | base.SerializeElement(writer, false));
        }
    }


    /// <summary>
    /// TypeElementCollection represents a collection of TypeElement
    /// </summary>
    public class RewardTypeElementCollection : ConfigurationElementCollection
    {
        public RewardTypeElement this[int index]
        {
            get
            {
                return (RewardTypeElement)base.BaseGet(index);
            }

            set
            {
                if (base.BaseGet(index) != null)

                    base.BaseRemoveAt(index);

                this.BaseAdd(index, value);
            }
        }


        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }


        protected override bool IsElementName(string elementName)
        {

            if ((string.IsNullOrEmpty(elementName)) || (elementName != "rewardType"))

                return false;

            return true;
        }


        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RewardTypeElement)element)._ElementName;
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new RewardTypeElement();
        }
    }
}
