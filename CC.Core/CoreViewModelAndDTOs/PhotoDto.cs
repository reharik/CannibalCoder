namespace CC.Core.CoreViewModelAndDTOs
{
    public class PhotoDto
    {
        public string FileUrl { get; set; }
        public string FileUrl_Thumb { get { return FileUrl.AddImageSizeToName("Thumb"); } }
        public string FileUrl_Large { get { return FileUrl.AddImageSizeToName("Large"); } }
    }
}