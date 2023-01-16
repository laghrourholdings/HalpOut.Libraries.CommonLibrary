
namespace CommonLibrary.Core.PostalAddress
{
    interface IPostalAddressFormatter
    {
        PostalAddressFormat Format(PostalAddress postalAddress);
    }
}
