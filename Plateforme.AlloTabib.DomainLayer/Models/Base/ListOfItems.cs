using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models.Base
{
    public class ListOfItems<T> where T: class
    {
        public ListOfItems()
        { }

        public ListOfItems(Pagination pagination, IList<T> items)
        {
            Pagination = pagination;
            Items = items ?? new List<T>();
        }

        public Pagination Pagination { get; set; }

        public IList<T> Items { get; set; }
    }
}
