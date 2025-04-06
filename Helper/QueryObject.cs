using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Helper
{
    public class QueryObject
    {
        public string? Keyword { get; set; } = null;
        public string? Role { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public string? SortDirection { get; set; } = null;
    }
}