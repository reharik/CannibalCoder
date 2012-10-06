namespace CC.Core.Domain
{
    public interface IPersistableObject
    {
        bool IsDeleted { get; set; }
    }

    public interface ILookupType
    {
        int EntityId { get; set; }
        string Name { get; set; }
    }
}