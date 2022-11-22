namespace CommonLibrary.Core.Models.PostalAddress
{
    public class PostalAddressFormatter : IPostalAddressFormatter
    {
        public static PostalAddressFormat Empty = GetPostalAddressFormat();
        static readonly IDictionary<string, IPostalAddressFormatter> _postalAddressFormatters = GetPPostalAddressFormatters();
        static PostalAddressFormat GetPostalAddressFormat() {
            return new PostalAddressFormat();
        }
        static IDictionary<string, IPostalAddressFormatter> GetPPostalAddressFormatters()
        {
            var postalAddressFormatters = new Dictionary<string, IPostalAddressFormatter>();
            postalAddressFormatters.Add("urban", new UrbanPostalAddressFormatter());
            postalAddressFormatters.Add("box", new BoxPostalAddressFormatter());
            postalAddressFormatters.Add("rural", new RuralPostalAddressFormatter());
            return postalAddressFormatters;
        }
        public PostalAddressFormat Format(PostalAddress postalAddress)
        {
            if (_postalAddressFormatters.ContainsKey(postalAddress.AddressType.ToLowerInvariant()))
            {
                return _postalAddressFormatters[postalAddress.AddressType.ToLowerInvariant()].Format(postalAddress);
            }
            return Empty;
        }
    }
}
