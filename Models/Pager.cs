namespace AssignmentPRN222.Models
{
    public class Pager
    {
        public int totalItem { get; set; }
        public int totalPage { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int startPage { get; set; }
        public int endPage { get; set; }
        public Pager()
        {

        }
        public Pager(int totalItems, int page, int pageSizes)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSizes);
            int currentPages = page;
            int startPages = currentPages - 4;
            int endPages = currentPages + 5;

            if (startPages <= 0)
            {
                endPages = Math.Min(10, totalPages);
                startPages = 1;
            }

            if (endPages > totalPages)
            {
                endPages = totalPages;
                if (endPages > 10)
                {
                    startPages = endPages - 9;
                }

            }
            this.startPage = startPages;
            this.endPage = endPages;
            this.totalPage = totalPages;
            this.currentPage = currentPages;
            this.pageSize = pageSizes;
            this.totalItem = totalItems;
        }
    }
}
