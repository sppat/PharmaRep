namespace Appointments.Domain.ValueObjects;

public record UserId
{
    public Guid Value { get; }

    private UserId(Guid value)
    {
        Value = value;
    }

    public static bool TryCreate(Guid id, out UserId userId)
    {
        if (id == Guid.Empty)
        {
            userId = null;
            return false;
        }
        
        userId = new UserId(id);
        return true;
    }
}