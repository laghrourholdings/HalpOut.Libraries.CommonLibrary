namespace CommonLibrary.Core.Models.PostalAddress
{
    public class BagPostalAddressFormatter : IPostalAddressFormatter
    {
        public PostalAddressFormat Format(PostalAddress postalAddress)
        {
            if (postalAddress == null)
                throw new ArgumentNullException("postalAddress", "postalAddress is null.");
            if (postalAddress.AddressType.ToLowerInvariant() != "bag")
            {
                throw new ArgumentOutOfRangeException("postalAddress", string.Format("Invalid AddressType. Expected 'bag' but was '{0}'", postalAddress.AddressType.ToLowerInvariant()));
            }
            PostalAddressFormat format = new PostalAddressFormat();
            format.AddressType = postalAddress.AddressType;
            format.AddressLine1 = GetAddressLine1(postalAddress);
            format.AddressLine2 = postalAddress.BoxBagLobbyName;
            format.AddressLine3 = string.Empty;
            format.Suburb = string.Empty;
            format.City = postalAddress.TownCityMailTown;
            format.PostCode = postalAddress.PostCode;
            format.AddressOneLine = string.Join(", ", new string[] { format.AddressLine1, format.Suburb, format.City, format.PostCode });
            return format;
        }
        private static string GetAddressLine1(PostalAddress postalAddress)
        {
            return string.Format("{0} {1}", postalAddress.DeliveryServiceType, postalAddress.BoxBagNumber);
        }
    }
}
