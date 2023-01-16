using System.Text.RegularExpressions;

namespace CommonLibrary.Core.PostalAddress
{
    public class UrbanPostalAddressFormatter : IPostalAddressFormatter
    {
        public PostalAddressFormat Format(PostalAddress postalAddress)
        {
            if (postalAddress == null)
                throw new ArgumentNullException("postalAddress", "postalAddress is null.");
            if (postalAddress.AddressType.ToLowerInvariant() != "urban")
            {
                throw new ArgumentOutOfRangeException("postalAddress", string.Format("Invalid AddressType. Expected 'urban' but was '{0}'", postalAddress.AddressType.ToLowerInvariant()));
            }
            PostalAddressFormat format = new PostalAddressFormat();
            format.AddressType = postalAddress.AddressType;
            format.AddressLine1 = GetAddressLine1(postalAddress);
            format.AddressLine2 = GetAddressLine2(postalAddress);
            format.AddressLine3 = postalAddress.SuburbName != format.AddressLine2 ? postalAddress.SuburbName : string.Empty;
            format.Suburb = postalAddress.SuburbName;
            format.City = postalAddress.TownCityMailTown;
            format.PostCode = postalAddress.PostCode;
            format.AddressOneLine = string.Join(", ", string.Join("~", new string[] { format.AddressLine1, format.AddressLine2, format.AddressLine3, string.Format("{0} {1}", format.City, format.PostCode) }).Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray());
            return format;
        }

        private static string GetAddressLine2(PostalAddress postalAddress)
        {
            string unitType = string.IsNullOrWhiteSpace(postalAddress.UnitType) ? string.Empty : postalAddress.UnitType.ToLowerInvariant();
            if (unitType == "suite" && !string.IsNullOrWhiteSpace(postalAddress.Floor))
            {
                if (IsNumeric(postalAddress.StreetNumber) && IsAlphaOnly(postalAddress.UnitId))
                {
                    return string.Format("{0}{2} {1}", postalAddress.StreetNumber, postalAddress.StreetName, postalAddress.UnitId);
                }
                return string.Format("{0} {1}", postalAddress.StreetNumber, postalAddress.StreetName);
            }
            return postalAddress.SuburbName;
        }
        private static string GetAddressLine1(PostalAddress postalAddress)
        {
            string unitType = string.IsNullOrWhiteSpace(postalAddress.UnitType) ? string.Empty : postalAddress.UnitType.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(unitType))
            {
                return string.Format("{0}{1} {2}", postalAddress.StreetNumber,postalAddress.StreetAlpha, postalAddress.StreetName);
            }
            if (unitType == "suite" && !string.IsNullOrWhiteSpace(postalAddress.Floor))
            {
                return string.Format("Suite {0} {1}", postalAddress.UnitId, postalAddress.Floor);
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
