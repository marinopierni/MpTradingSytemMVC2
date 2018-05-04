using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MpTradingSytemMVC2.Models;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace MpTradingSytemMVC2.Controllers
{
    public class PrezziController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prezzi
        public ActionResult Index()
        {
            //int nRow = 0;
            //DbManager db = new DbManager();

            //DataTable dt = db.ExecSql("Select * from Prezzi where idTitolo=2").Tables[0];

            //CurrentReport c = new CurrentReport();

            ////  List<dynamic> li= db.ConvertDtToList(dt);
            //c = db.getDataTableForGridView(null, dt);
            //nRow = dt.Rows.Count;

            //Prezzi prezzi = new Prezzi();

            //prezzi.ColsPrezzi = c.oCampiTabella;
            //prezzi.DatiPrezzi = c.oDatiTabella;
            //prezzi.totalRowsPrezzi = nRow;

            // var prezzis = db.Prezzis.Include(p => p.Titoli);
            Prezzi prezzi = new Prezzi();
            return View(prezzi);
        }


        public ActionResult CaricaPrezzi(Prezzi prezzi)
        {
            DbManager dbM = new DbManager();

            DataSet dsTitoli = new DataSet();

            HttpWebRequest  webRequest = null;
            HttpWebResponse webResponse = null;
            StreamWriter streamWriter = null;
            String json = "";

            dsTitoli = dbM.ExecSql("Select IdTitolo, NomeTitolo, Simbolo from [dbo].[Titoli]");


            if (dsTitoli != null)
            {
                DataTable dtTitoli = dsTitoli.Tables[0];

                foreach (DataRow titolo in dtTitoli.Rows)
                {
                    try
                    {
                        String req = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + titolo["Simbolo"].ToString().TrimEnd().TrimStart() + "&outputsize=compact&apikey=8CKQ7AYKGKKB5UPF";
                        //webRequest = (HttpWebRequest)System.Net.WebRequest.Create(req);
                        //webRequest.ContentType = "application/json";
                        //webRequest.Method = "GET";
                        //streamWriter = new System.IO.StreamWriter(webRequest.GetRequestStream());
                        //streamWriter.Write(json);
                        //streamWriter.Close();

                        using (var client = new WebClient())
                        {
                            var json2 = client.DownloadString(req);
                            var serializer = new JavaScriptSerializer();
                            //    SomeModel model = serializer.Deserialize<SomeModel>(json);
                            // TODO: do something with the model
                            JObject j = JObject.Parse(json2); //   .SerializeObject(json2);

                            JToken tok = j.First;
                            JToken tok2 = tok.First;
                            JToken tok3 = tok2.First;

                            JToken tokk = j.Last;
                           IEnumerable<JToken> tokk2 = tokk.SelectTokens("");



                           // DataTable tester = (DataTable)JsonConvert.DeserializeObject(json2, (typeof(DataTable)));

                            XmlDocument xd1 = null;
                            xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(json2, "Time Series (Daily)");
                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            AzioniLoader azioni = new AzioniLoaderAlpha();
            azioni.getData();

            return View();

        }

        // GET: Prezzi/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prezzi prezzi = db.Prezzis.Find(id);
            if (prezzi == null)
            {
                return HttpNotFound();
            }
            return View(prezzi);
        }

        // GET: Prezzi/Create
        public ActionResult Create()
        {
            ViewBag.IdTitolo = new SelectList(db.Titolis, "IdTitolo", "NomeTitolo");
            return View();
        }

        // POST: Prezzi/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPrezzo,Data,Apertura,Chiusura,Massimo,Minimo,Volume,SimboloTitolo,IdTitolo,Segnalazioni")] Prezzi prezzi)
        {
            if (ModelState.IsValid)
            {
                db.Prezzis.Add(prezzi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTitolo = new SelectList(db.Titolis, "IdTitolo", "NomeTitolo", prezzi.IdTitolo);
            return View(prezzi);
        }

        // GET: Prezzi/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prezzi prezzi = db.Prezzis.Find(id);
            if (prezzi == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTitolo = new SelectList(db.Titolis, "IdTitolo", "NomeTitolo", prezzi.IdTitolo);
            return View(prezzi);
        }

        // POST: Prezzi/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPrezzo,Data,Apertura,Chiusura,Massimo,Minimo,Volume,SimboloTitolo,IdTitolo,Segnalazioni")] Prezzi prezzi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prezzi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTitolo = new SelectList(db.Titolis, "IdTitolo", "NomeTitolo", prezzi.IdTitolo);
            return View(prezzi);
        }

        // GET: Prezzi/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prezzi prezzi = db.Prezzis.Find(id);
            if (prezzi == null)
            {
                return HttpNotFound();
            }
            return View(prezzi);
        }

        // POST: Prezzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Prezzi prezzi = db.Prezzis.Find(id);
            db.Prezzis.Remove(prezzi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
