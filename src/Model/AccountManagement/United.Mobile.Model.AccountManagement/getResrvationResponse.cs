namespace United.Mobile.Model.Common
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ual.com/des/reservation")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ual.com/des/reservation", IsNullable = false)]
    public partial class getResrvationResponse
    {
        /// <remarks/>
        public string Status { get; set; } = string.Empty;
        /// <remarks/>
        public string Msg { get; set; } = string.Empty;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Reservation")]
        public Reservation[] Reservation { get; set; }

    }

    /// <remarks/>
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ual.com/des/reservation")]
    public partial class Reservation
    {

        private string recLocField;

        private string recLocCrtDtField;

        private string nickNameField;

        private Segment[] segmentField;

        private Traveller[] travellerField;

        private bool hasFFCField;

        private bool isCancelledField;

        private bool isFarelockPNRField;

        private string fFCExpirationDtField;

        /// <remarks/>
        public string RecLoc
        {
            get
            {
                return this.recLocField;
            }
            set
            {
                this.recLocField = value;
            }
        }

        /// <remarks/>
        public string RecLocCrtDt
        {
            get
            {
                return this.recLocCrtDtField;
            }
            set
            {
                this.recLocCrtDtField = value;
            }
        }

        /// <remarks/>
        public string NickName
        {
            get
            {
                return this.nickNameField;
            }
            set
            {
                this.nickNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Segment")]
        public Segment[] Segment
        {
            get
            {
                return this.segmentField;
            }
            set
            {
                this.segmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Traveller")]
        public Traveller[] Traveller
        {
            get
            {
                return this.travellerField;
            }
            set
            {
                this.travellerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool hasFFC
        {
            get
            {
                return this.hasFFCField;
            }
            set
            {
                this.hasFFCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isCancelled
        {
            get
            {
                return this.isCancelledField;
            }
            set
            {
                this.isCancelledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFarelockPNR
        {
            get
            {
                return this.isFarelockPNRField;
            }
            set
            {
                this.isFarelockPNRField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FFCExpirationDt
        {
            get
            {
                return this.fFCExpirationDtField;
            }
            set
            {
                this.fFCExpirationDtField = value;
            }
        }
    }

    /// <remarks/>
    //[System.SerializableAttribute()]
    ////[System.ComponentModel.DesignerCategoryAttribute("code")]
    ////[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ual.com/des/reservation")]
    //public partial class Segment
    //{

    //private string opCarrCdField;

    //private string fltNbrField;

    //private string origField;

    //private string destField;

    //private string depDtmLclField;

    //private string depDtmGmtField;

    //private string arrDtmLclField;

    //private string arrDtmGmtField;

    //private string bkngClsField;

    //private string segActnCdField;

    ///// <remarks/>
    //public string OpCarrCd
    //{
    //    get
    //    {
    //        return this.opCarrCdField;
    //    }
    //    set
    //    {
    //        this.opCarrCdField = value;
    //    }
    //}

    ///// <remarks/>
    //public string FltNbr
    //{
    //    get
    //    {
    //        return this.fltNbrField;
    //    }
    //    set
    //    {
    //        this.fltNbrField = value;
    //    }
    //}

    ///// <remarks/>
    //public string Orig
    //{
    //    get
    //    {
    //        return this.origField;
    //    }
    //    set
    //    {
    //        this.origField = value;
    //    }
    //}

    ///// <remarks/>
    //public string Dest
    //{
    //    get
    //    {
    //        return this.destField;
    //    }
    //    set
    //    {
    //        this.destField = value;
    //    }
    //}

    ///// <remarks/>
    //public string DepDtmLcl
    //{
    //    get
    //    {
    //        return this.depDtmLclField;
    //    }
    //    set
    //    {
    //        this.depDtmLclField = value;
    //    }
    //}

    ///// <remarks/>
    //public string DepDtmGmt
    //{
    //    get
    //    {
    //        return this.depDtmGmtField;
    //    }
    //    set
    //    {
    //        this.depDtmGmtField = value;
    //    }
    //}

    ///// <remarks/>
    //public string ArrDtmLcl
    //{
    //    get
    //    {
    //        return this.arrDtmLclField;
    //    }
    //    set
    //    {
    //        this.arrDtmLclField = value;
    //    }
    //}

    ///// <remarks/>
    //public string ArrDtmGmt
    //{
    //    get
    //    {
    //        return this.arrDtmGmtField;
    //    }
    //    set
    //    {
    //        this.arrDtmGmtField = value;
    //    }
    //}

    ///// <remarks/>
    //public string BkngCls
    //{
    //    get
    //    {
    //        return this.bkngClsField;
    //    }
    //    set
    //    {
    //        this.bkngClsField = value;
    //    }
    //}

    ///// <remarks/>
    //public string SegActnCd
    //{
    //    get
    //    {
    //        return this.segActnCdField;
    //    }
    //    set
    //    {
    //        this.segActnCdField = value;
    //    }
    //}
    //}

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.ual.com/des/reservation")]
    public partial class Traveller
    {
        /// <remarks/>
        public string FirstName { get; set; } = string.Empty;
        /// <remarks/>
        public string MiddleName { get; set; } = string.Empty;
        /// <remarks/>
        public string LastName { get; set; } = string.Empty;

    }
}
