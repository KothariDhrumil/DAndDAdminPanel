using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {

        public string? UserImage { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [ForeignKey("DesignationId")]
        public Field? Designation { get; set; }
        public int? DesignationId { get; set; }
        public bool IsActive { get; set; } // use for whether its active or not
        public InactiveContactStatus? InactiveContactStatus { get; set; }
        public string? InActiveReason { get; set; }
        public bool IsMainContact { get; set; }
        public bool IsApproved { get; set; } // use for whether its pending or approved
        public bool ReRegistered { get; set; }
        public bool ShowPhonumber { get; set; }
        public bool NotificationEnabled { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? DeviceId { get; set; }
        public string? GeoInformation { get; set; }
        public string? MACAddress { get; set; }
        public bool IsAndroid { get; set; }
    }
}
