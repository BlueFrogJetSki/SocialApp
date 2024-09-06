namespace SocialApp.DataTransferObject
{
    public class EditProflieDTO
    {
        public string? UserName { get; set; }
        public string? Biography { get; set; }
        //public Major? Major { get; set; }
        //public IList<Interest>? Interests { get; set; }

        public IFormFile? IconImg { get; set; }
        public string? IconURL { get; set; }


    }
}
