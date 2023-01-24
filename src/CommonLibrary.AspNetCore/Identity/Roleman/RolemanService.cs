using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommonLibrary.AspNetCore.Identity.Roleman;

public class RolemanService
{
    public class Permission
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Role")]
        public string RoleName { get; set; } = null!;

        public Guid AssignedBy { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;
    }
}