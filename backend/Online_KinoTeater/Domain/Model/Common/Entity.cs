
namespace Domain.Model.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    protected Entity() { }

    public override bool Equals(object? obj) => obj is Entity same && Id == same.Id;

    public override int GetHashCode() => Id.GetHashCode();
}

