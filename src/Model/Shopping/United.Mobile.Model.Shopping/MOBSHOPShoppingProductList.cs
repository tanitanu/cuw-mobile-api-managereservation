using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPShoppingProductList : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.MOBSHOPShoppingProductList";
        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        #endregion

        public string SessionId { get; set; }
        public List<MOBSHOPShoppingProduct> Columns { get; set; }
        public string CartId { get; set; }
    }
}
