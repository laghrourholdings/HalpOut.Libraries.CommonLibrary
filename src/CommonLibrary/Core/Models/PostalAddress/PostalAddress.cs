using CommonLibrary.Logging;

namespace CommonLibrary.Core.PostalAddress
{
    public class PostalAddress : IBusinessObject, IDeletable, ISuspendable
    {
        public Guid LogHandleId { get; set; }

        public DateTimeOffset CreationDate { get; set; }
        public string? Descriptor { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset DeletedDate { get; set; }
        public Guid DeletedBy { get; set; }
        public bool IsSuspended { get; set; }
        public DateTimeOffset SuspendedDate { get; set; }
        public Guid SuspendedBy { get; set; }

        /// <summary>
        /// Indiciates the type of delivery point
        /// Urban, Rural, Bag, Box, Counter, CMB Urban, CMB Rural
        /// </summary>
        public string AddressType { get; set; }
        /// <summary>
        /// The Name of the NZPost outlet or Agency 
        /// </summary>
        public string BoxBagLobbyName { get; set; }
        /// <summary>
        /// PO Box / Private Bag Number 
        /// </summary>
        public string BoxBagNumber { get; set; }
        /// <summary>
        /// Building / Property name 
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// The Type of Delivery Service
        /// PO Box, Private Bag, Counter, CMB
        /// </summary>
        public string DeliveryServiceType { get; set; }
        /// <summary>
        /// Floor Type and Identifier within a Building or Complex
        /// Level 1, Floor 3, Basement
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// Unique reference number, Delivery Point Identifier
        /// </summary>
        

        /// <summary>
        /// The 4 digit code defined by NZPost for the sorting of mail
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// Rural Delivery route number
        /// </summary>
        public string RDNumber { get; set; }
        /// <summary>
        /// Holds the Alpha component of a Street Number
        /// A - Z
        /// </summary>
        public string StreetAlpha { get; set; }
        /// <summary>
        /// Indicates where a road may be split or extended
        /// North, West, Extension Character
        /// </summary>
        public string StreetDirection { get; set; }
        /// <summary>
        /// Holds the Name of the Street
        /// </summary>
        public string StreetName { get; set; }
        /// <summary>
        /// Holds the base street number
        /// 1 - 100000
        /// </summary>
        public string StreetNumber { get; set; }
        /// <summary>
        /// The Street Type that follows the Street Name
        /// Road, Crescent, Lane Character
        /// </summary>
        public string StreetType { get; set; }
        public string SuburbName { get; set; }
        /// <summary>
        /// The Name of the Town / Mailtown associated with the Delivery point
        /// </summary>
        public string TownCityMailTown { get; set; }
        /// <summary>
        /// Sub-Dwellling Identifier
        /// 32, BB2, Top, Penthouse, W506A
        /// </summary>
        public string UnitId { get; set; }
        /// <summary>
        /// Describes the category of a sub-dwelling - used in conjunction with the Unit_ID
        /// Apartment, Flat, Unit, Shop, Suite
        /// </summary>
        public string UnitType { get; set; }
        public string AddressLine { get; set; }

        
    }
}
