using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
        [Serializable]
        public class MOBGamificationPromotionResponse : MOBResponse
        {
            private string version;
            private string status;
            private string gameType;
            private MOBGamificationContents contents;
            private MOBGamificationActions actions;
            private string mileagePlusNumber = string.Empty;
            private string custID = string.Empty;
            private string promoCode = string.Empty;
            private string memberPromotionID = string.Empty;
            private string altRefID1 = string.Empty;
            private bool? isRegistered = false;
            private string state = string.Empty;
            private DateTime expirationDate;
            private DateTime registrationTimestamp;
            private string registrationText;

            public string Version
            {
                get { return version; }
                set { version = value; }
            }
            public string Status
            {
                get { return status; }
                set { status = value; }
            }
            public string GameType
            {
                get { return gameType; }
                set { gameType = value; }
            }
            public MOBGamificationContents Contents
            {
                get { return contents; }
                set { contents = value; }
            }
            public MOBGamificationActions Actions
            {
                get { return actions; }
                set { actions = value; }
            }

            public string MileagePlusNumber
            {
                get { return mileagePlusNumber; }
                set { this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
            }

            public string CustID
            {
                get
                {
                    return this.custID;
                }
                set
                {
                    this.custID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
                }
            }

            public string PromoCode
            {
                get { return promoCode; }
                set { promoCode = value; }
            }

            public string MemberPromotionID
            {
                get
                {
                    return this.memberPromotionID;
                }
                set
                {
                    this.memberPromotionID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
                }
            }

            public string AltRefID1
            {
                get
                {
                    return this.altRefID1;
                }
                set
                {
                    this.altRefID1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
                }
            }

            public bool? IsRegistered
            {
                get
                {
                    return this.isRegistered;
                }
                set
                {
                    this.isRegistered = value;
                }
            }

            public string State
            {
                get
                {
                    return this.state;
                }
                set
                {
                    this.state = value;
                }
            }

            public DateTime ExpirationDate
            {
                get
                {
                    return this.expirationDate;
                }
                set
                {
                    this.expirationDate = value;
                }
            }

            public DateTime RegistrationTimestamp
            {
                get
                {
                    return this.registrationTimestamp;
                }
                set
                {
                    this.registrationTimestamp = value;
                }
            }

            public string RegistrationText
            {
                get
                {
                    return this.registrationText;
                }
                set
                {
                    this.registrationText = value;
                }
            }
        }

        public class MOBGamificationContent
        {
            private string key;
            private string _value;

            public string Key
            {
                get { return key; }
                set { key = value; }
            }
            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        public class MOBGamificationContents
        {
            private MOBGamificationContent[] content;
            [JsonConverter(typeof(SingleOrArrayConverter<MOBGamificationContent>))]
            public MOBGamificationContent[] Content
            {
                get { return content; }
                set { content = value; }
            }
        }

        public class MOBGamificationAction
        {
            private string name;
            private string icon;
            private string text;
            private string rewards;
            private string challengeText;
            private string challengeDesc;
            private string btnText;
            private string statusIcon;
            private int stepOrder;
            private int displayOrder;
            private string appLink;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public string Icon
            {
                get { return icon; }
                set { icon = value; }
            }
            public string Text
            {
                get { return text; }
                set { text = value; }
            }
            public string Rewards
            {
                get { return rewards; }
                set { rewards = value; }
            }
            public string ChallengeText
            {
                get { return challengeText; }
                set { challengeText = value; }
            }
            public string ChallengeDesc
            {
                get { return challengeDesc; }
                set { challengeDesc = value; }
            }
            public string BtnText
            {
                get { return btnText; }
                set { btnText = value; }
            }
            public string StatusIcon
            {
                get { return statusIcon; }
                set { statusIcon = value; }
            }
            public int StepOrder
            {
                get { return stepOrder; }
                set { stepOrder = value; }
            }
            public int DisplayOrder
            {
                get { return displayOrder; }
                set { displayOrder = value; }
            }
            public string AppLink
            {
                get { return appLink; }
                set { appLink = value; }
            }
        }


        public class MOBGamificationActions
        {
            private MOBGamificationAction[] action;
            [JsonConverter(typeof(SingleOrArrayConverter<MOBGamificationAction>))]
            public MOBGamificationAction[] Action
            {
                get { return action; }
                set { action = value; }
            }
        }

        public class Promotion
        {
            private string promotionId;
            private DateTime effectiveDate;
            private DateTime expirationDate;
            private DateTime registrationEffectiveDate;
            private DateTime registrationExpirationDate;
            private DateTime qualificationEffectiveDate;
            private DateTime qualificationExpirationDate;
            private DateTime displayEffectiveDate;
            private DateTime displayExpirationDate;
            private bool isTargeted;
            private string url;
            private string vanityUrl;
            private string description;
            private string xmlMetaData;
            private dynamic metaData;

            [DataMember]
            public string PromotionId
            {
                get { return promotionId; }
                set { promotionId = value; }
            }
            [DataMember]
            public DateTime EffectiveDate
            {
                get { return effectiveDate; }
                set { effectiveDate = value; }
            }
            [DataMember]
            public DateTime ExpirationDate
            {
                get { return expirationDate; }
                set { expirationDate = value; }
            }
            [DataMember]
            public DateTime RegistrationEffectiveDate
            {
                get { return registrationEffectiveDate; }
                set { registrationEffectiveDate = value; }
            }
            [DataMember]
            public DateTime RegistrationExpirationDate
            {
                get { return registrationExpirationDate; }
                set { registrationExpirationDate = value; }
            }
            [DataMember]
            public DateTime QualificationEffectiveDate
            {
                get { return qualificationEffectiveDate; }
                set { qualificationEffectiveDate = value; }
            }
            [DataMember]
            public DateTime QualificationExpirationDate
            {
                get { return qualificationExpirationDate; }
                set { qualificationExpirationDate = value; }
            }
            [DataMember]
            public DateTime DisplayEffectiveDate
            {
                get { return displayEffectiveDate; }
                set { displayEffectiveDate = value; }
            }
            [DataMember]
            public DateTime DisplayExpirationDate
            {
                get { return displayExpirationDate; }
                set { displayExpirationDate = value; }
            }
            [DataMember]
            public bool IsTargeted
            {
                get { return isTargeted; }
                set { isTargeted = value; }
            }
            public string Url
            {
                get { return url; }
                set { url = value; }
            }
            [DataMember]
            public string VanityUrl
            {
                get { return vanityUrl; }
                set { vanityUrl = value; }
            }
            [DataMember]
            public string Description
            {
                get { return description; }
                set { description = value; }
            }
            [DataMember]
            public string XmlMetaData
            {
                get { return xmlMetaData; }
                set { xmlMetaData = value; }
            }
            [DataMember]
            public dynamic MetaData
            {
                get { return metaData; }
                set { metaData = value; }
            }
        }
        [Serializable]
        public class MemberPromotion<TMetaData>
        {
            public Guid memberPromotionId;
            public string mpId;
            public string altRefId1;
            public string altRefId2;
            public string promotionId;
            public Promotion promotion;
            public string displayName;
            public DateTime effectiveDate;
            public DateTime expirationDate;
            public bool isActive;
            public string emailId;
            public bool isRegistered;
            public DateTime? registrationTimestamp;
            public bool isRegisteredInIms;
            public DateTime? imsRegistrationTimestamp;
            public DateTime insertTimestamp;
            public string insertId;
            public DateTime? updateTimestamp;
            public string updateId;
            public TMetaData metaData;
            public string xmlMetaData;
            public string state;
            public string imsPromotionId;
            public string channelRegistrationId;

            [DataMember]
            public Guid MemberPromotionId
            {
                get { return memberPromotionId; }
                set { memberPromotionId = value; }
            }
            [DataMember]
            public string MpId
            {
                get { return mpId; }
                set { mpId = value; }
            }
            [DataMember]
            public string AltRefId1
            {
                get { return altRefId1; }
                set { altRefId1 = value; }
            }
            [DataMember]
            public string AltRefId2
            {
                get { return altRefId2; }
                set { altRefId2 = value; }
            }
            [DataMember]
            public string PromotionId
            {
                get { return promotionId; }
                set { promotionId = value; }
            }
            [DataMember]
            public Promotion Promotion
            {
                get { return promotion; }
                set { promotion = value; }
            }
            [DataMember]
            public string DisplayName
            {
                get { return displayName; }
                set { displayName = value; }
            }
            [DataMember]
            public DateTime EffectiveDate
            {
                get { return effectiveDate; }
                set { effectiveDate = value; }
            }
            [DataMember]
            public DateTime ExpirationDate
            {
                get { return expirationDate; }
                set { expirationDate = value; }
            }
            [DataMember]
            public bool IsActive
            {
                get { return isActive; }
                set { isActive = value; }
            }
            [DataMember]
            public string EmailId
            {
                get { return emailId; }
                set { emailId = value; }
            }
            [DataMember]
            public bool IsRegistered
            {
                get { return isRegistered; }
                set { isRegistered = value; }
            }
            [DataMember]
            public DateTime? RegistrationTimestamp
            {
                get { return registrationTimestamp; }
                set { registrationTimestamp = value; }
            }
            [DataMember]
            public bool IsRegisteredInIms
            {
                get { return isRegisteredInIms; }
                set { isRegisteredInIms = value; }
            }
            [DataMember]
            public DateTime? ImsRegistrationTimestamp
            {
                get { return imsRegistrationTimestamp; }
                set { imsRegistrationTimestamp = value; }
            }
            [DataMember]
            public DateTime InsertTimestamp
            {
                get { return insertTimestamp; }
                set { insertTimestamp = value; }
            }
            [DataMember]
            public string InsertId
            {
                get { return insertId; }
                set { insertId = value; }
            }
            [DataMember]
            public DateTime? UpdateTimestamp
            {
                get { return updateTimestamp; }
                set { updateTimestamp = value; }
            }
            [DataMember]
            public string UpdateId
            {
                get { return updateId; }
                set { updateId = value; }
            }
            [DataMember]
            public TMetaData MetaData
            {
                get { return metaData; }
                set { metaData = value; }
            }
            [DataMember]
            public string XmlMetaData
            {
                get { return xmlMetaData; }
                set { xmlMetaData = value; }
            }
            [DataMember]
            public string State
            {
                get { return state; }
                set { state = value; }
            }
            [DataMember]
            public string ImsPromotionId
            {
                get { return imsPromotionId; }
                set { imsPromotionId = value; }
            }
            [DataMember]
            public string ChannelRegistrationId
            {
                get { return channelRegistrationId; }
                set { channelRegistrationId = value; }
            }
        }

        public class SingleOrArrayConverter<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(T[]));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JToken.Load(reader);
                if (token.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                {
                    return token.ToObject<T[]>();
                }
                return new T[] { token.ToObject<T>() };
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    
}
