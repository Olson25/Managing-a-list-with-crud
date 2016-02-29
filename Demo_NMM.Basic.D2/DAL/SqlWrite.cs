using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo_NMM.Basic.Models;
using Demo_NMM.Basic.Controllers;


namespace Demo_NMM.Basic.DAL
{

    public class SqlCrud
    {
        public void update(string id, string name, string city, string state, string zip, string address, string phone)
        {
            //List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
            System.Data.SqlClient.SqlConnection sqlConnection1 =
            new System.Data.SqlClient.SqlConnection("Server=tcp:aspnet.database.windows.net,1433;Database=brewery repo;User ID=Yato@aspnet;Password=Dragon5476;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO Review (Id, name, city, state, zip, address, phone) VALUES (" + id + ", '" + name + "', '" + city + "', '" + state + "', '" + zip + "', '" + address + "', '" + phone + "')";
            //cmd.CommandText = "INSERT INTO Table (Id, name, address, city, state, zip, phone) VALUES (1, 'California','Los Angeles', 'California','Los Angeles', 'California','Los Angeles')";
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
        }
        //public string read()
        //{
//            System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
//            comm.Connection = new System.Data.SqlClient.SqlConnection(
//              "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\olson25\\Desktop\\Persistence\\Demo_NMM.Basic.D2\\App_Data\\ReviewDB2.mdf;Integrated Security=True");
//            String sql = @"SELECT Id, name, address, city, state, zip, phone
//                      FROM ReviewDB2";
//            comm.CommandText = sql;
//            comm.Connection.Open();
//            System.Data.SqlClient.SqlDataReader cursor = comm.ExecuteReader();
//            while (cursor.Read())
//                Console.WriteLine(cursor["name"] + "\t" +
//                                  cursor["population"]);
//            comm.Connection.Close();
//            return sql;
        //}
    }
}