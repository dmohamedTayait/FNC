using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TayaIT.Enterprise.EMadbatah.Config;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public class PagingProvider
    {
        /// <summary>
        /// Creates a list of page numbers to be enumerated in a paging control.
        /// </summary>
        /// <remarks>
        /// Paging is 1-based, meaning that the first page is called page 1.
        /// </remarks>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalItems">The total items.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        public static IEnumerable<PagingItem> CreatePages(int pageSize, int totalItems, int currentPage)
        {
            List<PagingItem> pages = new List<PagingItem>();
            int totalPages = (totalItems / pageSize) + 1;
            int startIndex = 0;
            int endIndex = totalPages;

            if (totalPages > 10)
            {
                startIndex = currentPage - 5;
                endIndex = currentPage + 5;
                if (startIndex < 0)
                {
                    startIndex = 0;
                    endIndex = startIndex + 10;
                }
                if (endIndex > totalPages)
                {
                    endIndex = totalPages;
                    startIndex = totalPages - 10;
                }
            }
            pages.Add(new PagingItem { Title = "««", PageNum = "1", CurrentPage = false, URL=GetSearchURL(1, pageSize) });
            if (currentPage == 1)
                pages.Add(new PagingItem { Title = "«", PageNum = (currentPage).ToString(), CurrentPage = false, URL = GetSearchURL(currentPage, pageSize) });
            else
                pages.Add(new PagingItem { Title = "«", PageNum = (currentPage - 1).ToString(), CurrentPage = false, URL = GetSearchURL(currentPage -1, pageSize) });
            for (int i = startIndex; i < endIndex; i++)
            {
                PagingItem page = new PagingItem { Title = (i + 1).ToString(), PageNum = (i + 1).ToString(), CurrentPage = i == (currentPage - 1), URL = GetSearchURL(i+1, pageSize) };
                pages.Add(page);
            }
            if (currentPage == totalPages)
                pages.Add(new PagingItem { Title = "»", PageNum = (currentPage).ToString(), CurrentPage = false, URL = GetSearchURL(currentPage, pageSize) });
            else
                pages.Add(new PagingItem { Title = "»", PageNum = (currentPage + 1).ToString(), CurrentPage = false, URL = GetSearchURL(currentPage + 1, pageSize) });
            pages.Add(new PagingItem { Title = "»»", PageNum = totalPages.ToString(), CurrentPage = false, URL = GetSearchURL(totalPages, pageSize) });
            return pages;

            
        }

        public static string GetSearchURL(int pageNo, int resPerPage)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(Constants.PageNames.FINAL_APPROVED_SESSION);
            sb.Append("?").Append(Constants.QSKeyNames.PAGE_NO).Append("=").Append(pageNo);

            sb.Append("&").Append(Constants.QSKeyNames.ITEMS_PER_PAGE).Append("=").Append(resPerPage);
            return sb.ToString();
        }

    }
}

