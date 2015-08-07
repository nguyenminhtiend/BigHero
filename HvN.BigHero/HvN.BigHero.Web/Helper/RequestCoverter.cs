using System.Collections.Generic;
using System.Web;

namespace HvN.BigHero.Web.Helper
{
    public class RequestCoverter
    {
        public static Dictionary<string, object> GetDataFromRequest(HttpRequestBase request, string columns)
        {
            var data = new Dictionary<string, object>();
            foreach (var column in columns.Split(','))
            {
                data.Add(column, request[column]);
            }
            return data;
        }
    }
}