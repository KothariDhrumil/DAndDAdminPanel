
using AuthPermissions.BaseCode.CommonCode;
using SharedKernel;

namespace Domain.Todos;

public sealed class TodoItem : Entity, IDataKeyFilterReadWrite, IDataKeyFilterReadOnly
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Labels { get; set; } = [];
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Priority Priority { get; set; }

    /// <summary>
    /// This contains the datakey from the RetailOutlet
    /// </summary>
    public string DataKey { get; set; }
}
