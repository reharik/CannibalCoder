namespace CC.Core.Domain
{
    public interface IPersistableObject
    {
        bool Archived { get; set; }
    }

    public interface ILookupType
    {
        int EntityId { get; set; }
        string Name { get; set; }
    }
}