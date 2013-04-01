namespace CC.Core.CoreViewModelAndDTOs
{
    public class PhotoDto
    {
        // fine it's cuz i'm using the imageId as an htmlAttr as well.
        public int imageId { get; set; }
        public string FileUrl { get; set; }
        public string FileUrl_Thumb { get { return FileUrl.AddImageSizeToName("Thumb"); } }
        public string FileUrl_Large { get { return FileUrl.AddImageSizeToName("Large"); } }
    }
}