namespace CC.Core.Services
{
    public interface IInjectableSiteConfig
    {
        SiteConfigurationBase Settings();
    }

    public class SiteConfigurationBase
    {
        public virtual string Name { get; set; }
        public virtual string Host { get; set; }
        public virtual string LanguageDefault { get; set; }
        public virtual string ScriptsPath { get; set; }
        public virtual string CssPath { get; set; }
        public virtual string ImagesPath { get; set; }
        public virtual string WebSiteRoot { get; set; }
        public virtual string jsApplicationName { get; set; }
    }
}