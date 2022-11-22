
namespace CommonLibrary.Core.Models.PostalAddress
{
    interface IPostalAddressFormatter
    {
        PostalAddressFormat Format(PostalAddress postalAddress);
    }
}
