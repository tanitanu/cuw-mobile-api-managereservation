using System;


namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class AncillaryProductsResponse : MOBResponse
    {
        private PlacePass placePass;

        public PlacePass PlacePass
        {
            get { return placePass; }
            set { this.placePass = value; }
        }
    }

}