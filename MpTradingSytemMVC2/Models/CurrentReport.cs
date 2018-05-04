using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MpTradingSytemMVC2.Models
{
    public class CurrentReport
    {   //both property contain data from a two different table    
        // public IEnumerable<dynamic> oCampiTabella { get; set; } //for webgrid header
        public List<WebGridColumn> oCampiTabella { get; set; }
        public List<dynamic> oDatiTabella { get; set; } //for grid data
    }
}