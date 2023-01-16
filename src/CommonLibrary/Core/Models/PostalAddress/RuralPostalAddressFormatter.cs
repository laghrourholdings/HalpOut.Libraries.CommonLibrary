using System.Text.RegularExpressions;

namespace CommonLibrary.Core.PostalAddress
{
    public class RuralPostalAddressFormatter : IPostalAddressFormatter
    {
        public PostalAddressFormat Format(PostalAddress postalAddress)
        {
            if (postalAddress == null)
                throw new ArgumentNullException("postalAddress", "postalAddress is null.");
            if (postalAddress.AddressType.ToLowerInvariant() != "rural")
            {
                throw new ArgumentOutOfRangeException("postalAddress", string.Format("Invalid AddressType. Expected 'rural' but was '{0}'", postalAddress.AddressType.ToLowerInvariant()));
            }
            PostalAddressFormat format = new PostalAddressFormat();
            format.AddressType = postalAddress.AddressType;
            format.AddressLine1 = GetAddressLine1(postalAddress);
            format.AddressLine2 = GetAddressLine2(postalAddress);
            format.AddressLine3 = string.Empty;
            format.Suburb = format.AddressLine2;
            format.City = postalAddress.TownCityMailTown;
            format.PostCode = postalAddress.PostCode;
            format.AddressOneLine = string.Join(", ", new string[] { format.AddressLine1, format.Suburb, format.City, format.PostCode });
            return format;
        }

        private static string GetAddressLine2(PostalAddress postalAddress)
        {
            return string.Format("RD {0}", postalAddress.RDNumber);
        }
        private static string GetAddressLine1(PostalAddress postalAddress)
        {
            string unitType = string.IsNullOrWhiteSpace(postalAddress.UnitType) ? string.Empty : postalAddress.UnitType.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(unitType))
            {
                return string.Format("{0} {1}", postalAddress.StreetNumber, postalAddress.StreetName);
            }
            if (IsNumeric(postalAddress.StreetNumber) && IsAlphaOnly(postalAddress.UnitId))
            {
                return string.Format("{0}{2} {1}", postalAddress.StreetNumber, postalAddress.StreetName, postalAddress.UnitId);
            }
            return string.Format("{2}/{0} {1}", postalAddress.StreetNumber, postalAddress.StreetName, postalAddress.UnitId);
        }

        private static bool IsAlphaOnly(string p)
        {
            return Regex.IsMatch(p, @"^[a-zA-Z]+$");
        }

        private static bool IsNumeric(string p)
        {
            int check = 0;
            return int.TryParse(p, out check);
        }
    }
}
