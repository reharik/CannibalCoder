namespace CC.UI.Helpers.Tags
{
    public interface ITagProfileContainer
    {
        TagProfile Profile { get; set; }
    }

    public class TagProfileContainer : ITagProfileContainer
    {
        private TagProfile _profile;
        public TagProfile Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
    }
}