using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ComCabin
    {
        private int columnCount;
        private string description;
        private string isUpperDeck;
        private string key;
        private string layout;
        private string name;
        private int rowCount;
        private int totalSeat;

        public int ColumnCount
        {
            get
            {
                return this.columnCount;
            }
            set
            {
                this.columnCount = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string IsUpperDeck
        {
            get
            {
                return this.isUpperDeck;
            }
            set
            {
                this.isUpperDeck = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Layout
        {
            get
            {
                return this.layout;
            }
            set
            {
                this.layout = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int RowCount
        {
            get
            {
                return this.rowCount;
            }
            set
            {
                this.rowCount = value;
            }
        }

        public int TotalSeat
        {
            get
            {
                return this.totalSeat;
            }
            set
            {
                this.totalSeat = value;
            }
        }
    }
}
