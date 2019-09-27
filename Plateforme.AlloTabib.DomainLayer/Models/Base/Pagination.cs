namespace Plateforme.AlloTabib.DomainLayer.Models.Base
{
    public class Pagination
    {
        public Pagination()
        { }

        public Pagination(int absolutTotalCount, int totalCount, int totalPages)
        {
            AbsoluteTotalCount = absolutTotalCount;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }

        public int AbsoluteTotalCount { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
