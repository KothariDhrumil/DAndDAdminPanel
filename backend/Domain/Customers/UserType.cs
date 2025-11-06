using Domain.AbstactClass;
using System.ComponentModel.DataAnnotations;

namespace Domain.Customers;

public class UserType : AuditableEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    

}