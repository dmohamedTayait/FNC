using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    /// <summary>
    /// Page class containing the information used to create a paging control
    /// </summary>
    public class PagingItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>The page num.</value>
        public string PageNum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page is the current page.
        /// </summary>
        /// <value><c>true</c> if [current page]; otherwise, <c>false</c>.</value>
        public bool CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the navigating url of this page.
        /// </summary>
        /// <value><c>true</c> if [current page]; otherwise, <c>false</c>.</value>
        public string URL { get; set; }
    }
}