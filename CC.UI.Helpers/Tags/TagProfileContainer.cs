namespace CC.UI.Helpers.Tags
{
    public interface ITagProfileContainer
    {
        TagProfile Profile { get; set; }
    }

    public class TagProfileContainer : ITagProfileContainer
    {
        public TagProfile Profile { get; set; } 
    }
}