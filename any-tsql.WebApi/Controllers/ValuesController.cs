using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using any_tsql.WebApi.Models;
using any_tsql.WebApi.Services;

namespace any_tsql.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public async Task<object> Get(string server, string database, string userId, string password, int timeout, string queryString)
        {
            var requestInfo = new SqlRequest()
            {
                ConnectionString = $"Data Source={server};Initial Catalog={database};User ID={userId};Password={password};Connect Timeout={timeout};Column Encryption Setting=Enabled;",
                QueryString = queryString,
                Timeout = timeout
            };

            object result = await SqlHelper.GetQueryResult(requestInfo);
            return result;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
