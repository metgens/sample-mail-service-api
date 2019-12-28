﻿namespace MailService.Common.Pagination
{
    public class PagedResult<U> where U : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int PageCount { get; set; }
        public object Results { get; set; }
    }
}