using System;
using System.Collections.Generic;
using System.Text;

namespace Structure.Domain.Queries
{
    public interface IFilteredQueryInput
    {
        string FilterText { get; set; }
        string[] Fields { get; set; }
    }
}
