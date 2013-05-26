﻿namespace CC.UI.Helpers.Tags
{
    public class TagProfileExpression
    {

        public TagProfileExpression()
        {
            Profile = new TagProfile("default");
            Labels = new TagFactoryExpression(Profile.Label);
            Editors = new TagFactoryExpression(Profile.Label);
            Displays = new TagFactoryExpression(Profile.Label);
        }

        public TagProfile Profile { get; set; }

        public TagFactoryExpression Labels { get; private set; }

        public TagFactoryExpression Editors { get; private set; }

        public TagFactoryExpression Displays { get; private set; }
    }
}