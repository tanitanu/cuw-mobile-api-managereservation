using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class Barcode
    {
        public string BarcodeType { get; set; } = string.Empty;

        public string Base64EncodedImage { get; set; } = string.Empty;

        public string Height { get; set; } = string.Empty;

        public string Width { get; set; } = string.Empty;
    }
}
