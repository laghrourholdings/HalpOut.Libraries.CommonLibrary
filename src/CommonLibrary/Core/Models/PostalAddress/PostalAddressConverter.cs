
namespace CommonLibrary.Core.Models.PostalAddress
{
    public abstract class PostalAddressConverter<T>
    {
        protected PostalAddressConverter(PostalAddress postalAddress)
        {
            PostalAddress = postalAddress;
        }

        protected PostalAddress PostalAddress { get; private set; }

        public abstract T Convert();
    }
}
