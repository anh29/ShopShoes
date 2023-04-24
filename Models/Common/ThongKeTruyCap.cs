using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ShopOnline.Models.Common
{
    public class ThongKeTruyCap
    {
        private static string connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public static ThongKeViewModel ThongKe()
        {
            using(var connect = new SqlConnection(connection))
            {
                var item = connect.QueryFirstOrDefault< ThongKeViewModel>("sp_ThongKe", commandType: CommandType.StoredProcedure);
                return item;
            }
        }
    }
}