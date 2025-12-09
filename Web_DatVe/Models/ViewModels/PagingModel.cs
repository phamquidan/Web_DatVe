using System.Collections.Generic;

namespace Web_DatVe.Models.ViewModels
{
    public class PagingModel<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int PageCount { get; set; }
        public List<T> Results { get; set; }

        public PagingModel()
        {
            Results = new List<T>();
        }
    }
}

