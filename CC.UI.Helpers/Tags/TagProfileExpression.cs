namespace CC.UI.Helpers.Tags
{
    public class TagProfileExpression
    {

        public TagProfileExpression()
        {
            Labels = new TagFactoryExpression(new TagFactory());
            Editors = new TagFactoryExpression(new TagFactory());
            Displays = new TagFactoryExpression(new TagFactory());
        }


        public TagFactoryExpression Labels { get; private set; }

        public TagFactoryExpression Editors { get; private set; }

        public TagFactoryExpression Displays { get; private set; }
    }
}