using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult Index()
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if(k.KorIme== (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka== (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    if(k.Blokiran != "NE")
                    {
                        TempData["Greska"] = "Nalog kojem ste pokusali pristupiti je blokiran!";
                        
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];


            List<Fitnes_Centar> FitnesCentri = new List<Fitnes_Centar>();
            foreach (Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fc.Izbrisan == "NE")
                {
                    FitnesCentri.Add(fc);
                }
            }



            if (HttpContext.Session["fitnesCentri"] != null)
            {
                List<Fitnes_Centar> tempFitnesCentar = (List<Fitnes_Centar>)HttpContext.Session["fitnesCentri"];
                ViewBag.FitnesCentri = tempFitnesCentar;
                HttpContext.Session["fitnesCentri"] = null;
                tempFitnesCentar = null;
                return View();
            }
            ViewBag.FitnesCentri = FitnesCentri;
            return View();
        }

       

       

        public ActionResult Prikaz(string Naziv)
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];
            List<Grupni_Trening> GrupniTreninzi = (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"];
            List<Fitnes_Centar> FitnesCentri = (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"];
            List<Komentar> Komentari = (List<Komentar>)HttpContext.Application["Komentari"];



            ViewData["GrupniTreninzi"] = GrupniTreninzi;
            ViewBag.Komentari = Komentari;

            foreach (Fitnes_Centar f in FitnesCentri)
            {
                if (f.Naziv == Naziv)
                {
                    ViewBag.Naziv = f.Naziv;
                    ViewBag.Vlasnik = f.Vlasnik;
                    ViewBag.Adresa = f.Adresa;
                    ViewBag.GodinaOtvaranja = f.GodinaOtvaranja;
                    ViewBag.CenaMesecneClanarine = f.CenaMesecneClanarine;
                    ViewBag.CenaGodisnjeClanarine = f.CenaGodisnjeClanarine;
                    ViewBag.CenaJednogTreninga = f.CenaJednogTreninga;
                    ViewBag.CenaGrupnogTreninga = f.CenaGrupnogTreninga;
                    ViewBag.CenaTreningaSaTrenerom = f.CenaTreningaSaTrenerom;
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Sortiranje(string godinaOd, string godinaDo, string nazivPretraga, string adresaPretraga, string sortiranjePo, string sortiranjeRedosled)
        {


            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            HttpContext.Session["fitnesCentri"] = new List<Fitnes_Centar>();

            List<Fitnes_Centar> sviCentri = new List<Fitnes_Centar>();
            foreach (Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fc.Izbrisan == "NE")
                {
                    Fitnes_Centar f = fc;
                    sviCentri.Add(f);
                }


            }
            if (godinaOd.Trim() != String.Empty && Int32.TryParse(godinaOd.Trim(), out int a) == true)
            {

                List<Fitnes_Centar> zaBrisanje = new List<Fitnes_Centar>();
                foreach (Fitnes_Centar fc in sviCentri)
                {
                    if (fc.GodinaOtvaranja < a)
                        zaBrisanje.Add(fc);
                }
                foreach (Fitnes_Centar fc in zaBrisanje)
                {
                    sviCentri.Remove(fc);
                }
            }

            if (godinaDo.Trim() != String.Empty && Int32.TryParse(godinaDo.Trim(), out int aa) == true)
            {
                List<Fitnes_Centar> zaBrisanje = new List<Fitnes_Centar>();
                foreach (Fitnes_Centar fc in sviCentri)
                {
                    if (fc.GodinaOtvaranja > aa)
                        zaBrisanje.Add(fc);
                }
                foreach (Fitnes_Centar fc in zaBrisanje)
                {
                    sviCentri.Remove(fc);
                }
            }

            if (nazivPretraga.Trim() != String.Empty)
            {
                List<Fitnes_Centar> zaBrisanje = new List<Fitnes_Centar>();
                foreach (Fitnes_Centar fc in sviCentri)
                {
                    if (!fc.Naziv.Contains(nazivPretraga.Trim()))
                        zaBrisanje.Add(fc);
                }
                foreach (Fitnes_Centar fc in zaBrisanje)
                {
                    sviCentri.Remove(fc);
                }
            }

            if (adresaPretraga.Trim() != String.Empty)
            {
                List<Fitnes_Centar> zaBrisanje = new List<Fitnes_Centar>();
                foreach (Fitnes_Centar fc in sviCentri)
                {
                    if (!fc.Adresa.Contains(adresaPretraga.Trim()))
                        zaBrisanje.Add(fc);
                }
                foreach (Fitnes_Centar fc in zaBrisanje)
                {
                    sviCentri.Remove(fc);
                }
            }

            if (sortiranjePo != String.Empty)
            {
                if (sortiranjePo.Trim() == "NAZIV")
                {
                    if (sortiranjeRedosled.Trim() == "OPADAJUCE")
                    {
                        sviCentri = sviCentri.OrderByDescending(o => o.Naziv).ToList();
                    }
                    else if (sortiranjeRedosled.Trim() == "RASTUCE")
                    {
                        sviCentri = sviCentri.OrderBy(o => o.Naziv).ToList();
                    }
                }
                else if (sortiranjePo.Trim() == "GODINA OTVARANJA")
                {
                    if (sortiranjeRedosled.Trim() == "OPADAJUCE")
                    {
                        sviCentri = sviCentri.OrderByDescending(o => o.GodinaOtvaranja).ToList();
                    }
                    else if (sortiranjeRedosled.Trim() == "RASTUCE")
                    {
                        sviCentri = sviCentri.OrderBy(o => o.GodinaOtvaranja).ToList();
                    }
                }

                else if (sortiranjePo.Trim() == "ADRESA")
                {
                    if (sortiranjeRedosled.Trim() == "OPADAJUCE")
                    {
                        sviCentri = sviCentri.OrderByDescending(o => o.Adresa).ToList();
                    }
                    else if (sortiranjeRedosled.Trim() == "RASTUCE")
                    {
                        sviCentri = sviCentri.OrderBy(o => o.Adresa).ToList();
                    }
                }
            }
            HttpContext.Session["fitnesCentri"] = sviCentri;
            return RedirectToAction("Index", "Trener");
        }



        public ActionResult IzmeniProfil()
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            if (Session["PrijavljeniKorisnikLozinka"] == null)


            {

                return RedirectToAction("Index", "Home");
            }
            Korisnik prijavljen = null;
            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka == (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    prijavljen = k;
                    break;

                }

            }
            ViewBag.KorisnikZaIzmenu = prijavljen;

            return View();
        }

        [HttpPost]
        public ActionResult Izmena(string korime, string lozinka, string ponlozinka, string ime, string prezime, string pol, string email)
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Trener");
            }

            if (korime.Trim() == "" || lozinka.Trim() == "" || ponlozinka.Trim() == "" || ime.Trim() == "" || prezime.Trim() == "" || pol.Trim() == "" || email.Trim() == "")
            {
                TempData["Greska"] = "Niste uneli sva polja";
                return RedirectToAction("IzmeniProfil", "Trener");
            }

            if (lozinka != ponlozinka)
            {

                TempData["Greska"] = "Lozinka i ponovljen unos lozinke se ne poklapaju";

                return RedirectToAction("IzmeniProfil", "Trener");
            }
            Korisnik prijavljen = null;
            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka == (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    prijavljen = k;
                    break;

                }

            }

            if (korime.Trim() != prijavljen.KorIme && lozinka.Trim() != prijavljen.Lozinka)
            {

                TempData["Greska"] = "Nije moguce istovremeno promeniti i lozinku i korisnicko ime!";

                return RedirectToAction("IzmeniProfil", "Trener");
            }

            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == korime && korime != prijavljen.KorIme)
                {

                    TempData["Greska"] = "Korisnicko ime vec postoji";

                    return RedirectToAction("IzmeniProfil", "Trener");
                }
                else if (k.Lozinka == lozinka && lozinka != prijavljen.Lozinka)
                {
                    TempData["Greska"] = "Lozinka vec postoji";
                    return RedirectToAction("IzmeniProfil", "Trener");
                }
            }

            foreach (Korisnik kk in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (kk.KorIme == prijavljen.KorIme)
                {
                    string imetemp = kk.KorIme;
                    kk.KorIme = korime;
                    kk.Lozinka = lozinka;
                    kk.Pol = (POL)Enum.Parse(typeof(POL), pol);
                    kk.Email = email;
                    kk.Ime = ime;
                    kk.Prezime = prezime;
                    Session["PrijavljeniKorisnikIme"] = kk.KorIme;
                    Session["PrijavljeniKorisnikLozinka"] = kk.Lozinka;


                    BazaPodataka.IzmeniKorisnike(kk);
                    

                   

                    
                    break;
                }
            }
            return RedirectToAction("PrikaziProfil", "Trener");
        }

        public ActionResult PrikaziProfil()
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }



            if (Session["PrijavljeniKorisnikLozinka"] == null)

            {
                return RedirectToAction("Index", "Home");
            }


            Korisnik prijavljen = null;
            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka == (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    prijavljen = k;
                    break;

                }

            }


            ViewBag.KorisnikZaPrikaz = prijavljen;
            return View();
        }

    }
}
     
