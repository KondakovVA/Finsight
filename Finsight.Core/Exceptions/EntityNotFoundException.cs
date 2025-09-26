using System;

namespace Finsight.Core.Exceptions
{
    public sealed class EntityNotFoundException : FinsightException
    {
        public EntityNotFoundException(string entityName, Guid id)
            : base($"Сущность '{entityName}' с идентификатором '{id}' не найдена.")
        {
            EntityName = entityName;
            EntityId = id;
        }

        public string EntityName { get; }

        public Guid EntityId { get; }
    }
}
