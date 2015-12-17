using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinoki.Core
{
    /// <summary>
    /// 本文の記法
    /// </summary>
    public enum ArticleFormat
    {
        /// <summary>
        /// HTML
        /// </summary>
        Html = 1,

        /// <summary>
        /// Markdown
        /// </summary>
        Markdown = 2,

        /// <summary>
        /// Org-mode
        /// </summary>
        Org = 3,
    }
}
