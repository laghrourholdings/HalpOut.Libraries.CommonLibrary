
namespace CommonLibrary.Core.PostalAddress
{
    public class PostalAddressFormat
    {
        public string AddressType { get; set; }
        public string AddressOneLine { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public override string ToString()
        {
            return AddressOneLine;
        }
    }
}
