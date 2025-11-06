using Domain.AbstactClass;
using Microsoft.VisualBasic.FileIO;

namespace Domain
{
    public class Field : AuditableEntity
    {
        public string? Name { get; set; }

        public bool IsActive { get; set; }

        public FieldType FieldType { get; set; }

    }

}
