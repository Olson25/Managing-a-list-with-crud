using Demo_NMM.Basic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult ShowTable()
        {
            List<Brewery> breweries = (List<Brewery>)Session["Breweries"];

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