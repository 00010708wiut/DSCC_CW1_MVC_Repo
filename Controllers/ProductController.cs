using _00010708.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _00010708.Controllers
{
    public class ProductController : Controller
    {

        HttpClient client;
        
        public async Task<ActionResult> Index()
        {
            string Baseur1 = "https://localhost:44339/";
            List<Product> ProdInfo = new List<Product>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseur1);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Product");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    ProdInfo = JsonConvert.DeserializeObject<List<Product>>(PrResponse);
                }
                return View(ProdInfo);
            }
        }


        // GET: Product/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            string Baseurl = "https://localhost:44339/";
            Product ProdDetails = new Product();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                //Sending request to find web api REST service resource using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);

                //Checking the response is successful or not which is sent HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details received from web api
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response received from web api and storing the Product 
                    ProdDetails = JsonConvert.DeserializeObject<Product>(PrResponse);


                }

                //returning the Product list to view
                return View(ProdDetails);
            }


        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            client = new HttpClient();
            
            client.BaseAddress = new Uri("https://localhost:44339/");
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "api/Product", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44339/");
            Product product = new Product();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Product/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(data);
            }

            return View("Edit", product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44339/");
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "api/Product/" + product.ID, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44339/");
                var deleteProduct = client.DeleteAsync("api/Product/" + id.ToString());

                var response = deleteProduct.Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        
    }
}
