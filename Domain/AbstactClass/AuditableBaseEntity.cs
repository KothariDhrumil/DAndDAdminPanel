using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;

namespace Domain.AbstactClass
{
    public abstract class AuditableBaseEntity : IDataKeyFilterReadWrite, IDataKeyFilterReadOnly
    {
        [Key]
        public virtual int Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Multi-tenant filter key populated automatically from the current user context
        /// </summary>
        public string DataKey { get; set; } = default!;
        public bool IsActive { get; set; } = true;
    }

}
