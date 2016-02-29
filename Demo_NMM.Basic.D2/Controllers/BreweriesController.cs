using Demo_NMM.Basic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo_NMM.Basic.DAL;
using System.Web.Mvc;


namespace Demo_NMM.Basic.Controllers
{
    public class BreweriesController : Controller
    {
        // GET: Breweries
        public ActionResult Index()
        {
            return View();
        }
        public void WriteToDb()
        {
            //DAL.Data.InitializeBreweries();uncomment and run when sql needs to be re-initialized
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
            System.Data.SqlClient.SqlConnection sqlConnection1 =
            new System.Data.SqlClient.SqlConnection("Server=tcp:aspnet.database.windows.net,1433;Database=brewery repo;User ID=Yato@aspnet;Password=Dragon5476;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "DELETE FROM Review";
            //cmd.CommandText = "INSERT INTO Table (Id, name, address, city, state, zip, phone) VALUES (1, 'California','Los Angeles', 'California','Los Angeles', 'California','Los Angeles')";
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
            SqlCrud Sql = new SqlCrud();
            int index = 0;
            
            foreach (Brewery brewery in breweries)
            {
                Sql.update(breweries[index].ID.ToString(), breweries[index].Name.ToString(), breweries[index].City.ToString(), breweries[index].State.ToString(), breweries[index].Zip.ToString(), breweries[index].Address.ToString(), breweries[index].Phone.ToString());
                index++;
            }  
        }
        public void ReadFromDb()
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
            int x = 0;
            while (breweries.Count !=0)
                {
                    breweries.RemoveAt(0);
                }
            while (x < 200)
            {
                System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();          
                comm.Connection = new System.Data.SqlClient.SqlConnection(
                   //"Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\olson25\\Desktop\\Persistence\\Demo_NMM.Basic.D2\\App_Data\\ReviewDB2.mdf;Integrated Security=True");
                "Server=tcp:aspnet.database.windows.net,1433;Database=brewery repo;User ID=Yato@aspnet;Password=Dragon5476;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                   String sql = @"SELECT Id, name, address, city, state, zip, phone
                          FROM Review 
                            WHERE Id=" + x.ToString() ;
                comm.CommandText = sql;
                comm.Connection.Open();
                System.Data.SqlClient.SqlDataReader cursor = comm.ExecuteReader();
                
                while (cursor.Read()) 
                { 
                    Brewery newBrewery = new Brewery()
                    {
                        ID = Convert.ToInt16(cursor["id"].ToString()),
                        Name = cursor["name"].ToString(),
                        Address = cursor["address"].ToString(),
                        City = cursor["city"].ToString(),
                        State = (AppEnum.StateAbrv)Enum.Parse(typeof(AppEnum.StateAbrv), cursor["state"].ToString()),
                        Zip = cursor["zip"].ToString(),
                        Phone = cursor["phone"].ToString()
                    };
                breweries.Add(newBrewery);
                }
                x++;
                comm.Connection.Close();
            }
        }
        //public void update(string id, string name, string city, string state, string zip, string address, string phone )
        //{
        //    List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
        //    System.Data.SqlClient.SqlConnection sqlConnection1 =
        //    new System.Data.SqlClient.SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\olson25\\Desktop\\Demo_NMM.Basic-master\\Demo_NMM.Basic.D2\\App_Data\\ReviewDB2.mdf;Integrated Security=True");
        //    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        //    cmd.CommandType = System.Data.CommandType.Text;
        //    cmd.CommandText = "INSERT INTO Review VALUES ("+id + ", '"+ name+ "', '"+ city + "', '" + state + "', '"+ zip+ "', '"+address+ "', '"+phone+"')";
        //    //cmd.CommandText = "INSERT INTO Table (Id, name, address, city, state, zip, phone) VALUES (1, 'California','Los Angeles', 'California','Los Angeles', 'California','Los Angeles')";
        //    cmd.Connection = sqlConnection1;
        //    sqlConnection1.Open();
        //    cmd.ExecuteNonQuery();
        //    sqlConnection1.Close();
        //}
        
        public ActionResult ShowTable()
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
            ReadFromDb();
            //SqlWrite Sql = new SqlWrite();
            //int index = 0;
            //foreach (Brewery brewery in breweries)
            //{
            //    Sql.update(breweries[index].ID.ToString(), breweries[index].Name.ToString(), breweries[index].City.ToString(), breweries[index].State.ToString(), breweries[index].Zip.ToString(), breweries[index].Address.ToString(), breweries[index].Phone.ToString());
            //    index++;
            //}   
            return View(breweries);
            
          
            
        }

        public ActionResult ShowList()
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];

            return View(breweries);
        }

        public ActionResult ShowDetail(int id)
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];

            int index = breweries.FindIndex(a => a.ID == id);

            Brewery brewery = breweries[index];

           
            return View(brewery);
        }


        public ActionResult DeleteBrewery(int id)
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
            Brewery breweryToDelete = null;

            foreach (Brewery brewery in breweries)
            {
                if (brewery.ID == id)
                {
                    breweryToDelete = brewery;
                }
            }
            return View(breweryToDelete);
        }

        [HttpPost]
        public ActionResult DeleteBrewery(FormCollection form)
        {
            if (form["operation"] == "Delete")
            {
                List<Brewery> breweries = (List<Brewery>)Session["Breweries"];

                int index = breweries.FindIndex(a => a.ID == Convert.ToInt32(form["ID"]));

                breweries.RemoveAt(index);
                
                Session["Breweries"] = breweries;
                WriteToDb();

            }
            
            return Redirect("/Breweries/ShowTable");
        }

        public ActionResult CreateBrewery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBrewery(FormCollection form)
        {
            if (form["operation"] == "Add")
            {
                List<Brewery> breweries = (List<Brewery>)Session["Breweries"];

                Brewery newBrewery = new Brewery()
                {
                    ID = GetNextID(),
                    Name = form["name"],
                    Address = form["address"],
                    City = form["city"],
                    State = (AppEnum.StateAbrv)Enum.Parse(typeof(AppEnum.StateAbrv), form["state"]),
                    Zip = form["zip"],
                    Phone = form["phone"]
                };

                breweries.Add(newBrewery);
                 WriteToDb();
                Session["Breweries"] = breweries;
               

            }
            
            return Redirect("/Breweries/ShowTable");
        }

        public ActionResult UpdateBrewery(int id)
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
            Brewery breweryToUpdate = null;

            foreach (Brewery brewery in breweries)
            {
                if (brewery.ID == id)
                {
                    breweryToUpdate = brewery;
                }
            }
            return View(breweryToUpdate);
        }

        [HttpPost]
        public ActionResult UpdateBrewery(FormCollection form)
        {
            if (form["operation"] == "Edit")
            {
                List<Brewery> breweries = (List<Brewery>)Session["Breweries"];

                int index = breweries.FindIndex(a => a.ID == Convert.ToInt32(form["ID"]));

                breweries[index].Name = form["Name"];
                breweries[index].Address = form["Address"];
                breweries[index].City = form["City"];
                breweries[index].State = (AppEnum.StateAbrv)Enum.Parse(typeof(AppEnum.StateAbrv), form["State"]);
                breweries[index].Zip = form["Zip"];
                breweries[index].Phone = form["Phone"];

                Session["Breweries"] = breweries;     
                WriteToDb();

            }
            
            return Redirect("/Breweries/ShowTable");
        }

        public ActionResult ReloadData()
        {
            DAL.Data.InitializeBreweries();

            return Redirect("/Breweries/ShowTable");
        }

        private int GetNextID()
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];
            
            return breweries.Max(x => x.ID) + 1;
        }
    }
}