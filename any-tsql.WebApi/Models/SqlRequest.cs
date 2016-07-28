using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace any_tsql.WebApi.Models
{
    public class SqlRequest
    {
        public string ConnectionString { get; set; }

        public string QueryString { get; set; }

        public int Timeout { get; set; }
    }
}