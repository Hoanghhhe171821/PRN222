namespace AssignmentPRN222.Models
{
    public class Pager
    {
        public int totalPage { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; } = 5;
        public int totalItems { get; set; }
        public int startPage { get; set; }
        public int endPage { get; set; }

        public Pager(int totalItems, int page, int pageSize = 5)
        {
            this.totalItems = totalItems;
            this.pageSize = pageSize;
            this.currentPage = page;
            totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            startPage = Math.Max(1, currentPage - 2);
            endPage = Math.Min(totalPage, currentPage + 2);
        }
    }
}
