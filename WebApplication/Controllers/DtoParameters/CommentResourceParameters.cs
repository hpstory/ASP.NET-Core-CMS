namespace WebApplication.Controllers.DtoParameters
{
    public class CommentResourceParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return 10; }
        }
        public string OrderBy { get; set; } = "Date";
    }
}
