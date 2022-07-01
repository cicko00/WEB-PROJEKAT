using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class VlasnikController : Controller
    {
        public ActionResult Index()
        {

            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];


            List<Fitnes_Centar> FitnesCentri = (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"];



            if (Session["FitnesCentri"] != null)
            {
                List<Fitnes_Centar> tempFitnesCentar = (List<Fitnes_Centar>)Session["FitnesCentri"];
                ViewBag.FitnesCentri = tempFitnesCentar;
                Session["FitnesCentri"] = null;
                tempFitnesCentar = null;
                return View();
            }
            ViewBag.FitnesCentri = FitnesCentri;
            return View();
        }

        public ActionResult RegistracijaTrenera()
        {


            return View();
        }



       

       

        public ActionResult Prikaz(string Naziv)
        {
            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];
            List<Grupni_Trening> GrupniTreninzi = (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"];
            List<Fitnes_Centar> FitnesCentri = (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"];
            List<Komentar> Komentari = (List<Komentar>)HttpContext.Application["Komentari"];

            HttpContext.Application["GrupniTreninzi"] = GrupniTreninzi;
            HttpContext.Application["Komentari"] = Komentari;

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
            Session["FitnesCentri"] = new List<Fitnes_Centar>();

            List<Fitnes_Centar> sviCentri = new List<Fitnes_Centar>();
            sviCentri = (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"];
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
            Session["FitnesCentri"] = sviCentri;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult DodajNovogTrenera(string korime, string lozinka, string ponlozinka, string ime, string prezime, string pol, string email, string datumrodjenja, string uloga)
        {
            if (korime.Trim() == "" || lozinka.Trim() == "" || ponlozinka.Trim() == "" || ime.Trim() == "" || prezime.Trim() == "" || pol.Trim() == "" || email.Trim() == "" || datumrodjenja.Trim() == "" || uloga.Trim() == "")
            {
                TempData["Greska"] = "Niste uneli sva polja";
                return RedirectToAction("Registracija", "Home");
            }

            if (lozinka != ponlozinka)
            {

                TempData["Greska"] = "Lozinka i ponovljen unos lozinke se ne poklapaju";

                return RedirectToAction("Registracija", "Home");
            }

            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == korime)
                {

                    TempData["Greska"] = "Korisnicko ime vec postoji";

                    return RedirectToAction("Registracija", "Home");
                }
                else if (k.Lozinka == lozinka)
                {
                    TempData["Greska"] = "Lozinka vec postoji";
                    return RedirectToAction("Registracija", "Home");
                }
            }

            Korisnik kor = new Korisnik(korime, lozinka, ime, prezime, (POL)Enum.Parse(typeof(POL), pol), email, DateTime.Parse(datumrodjenja), (ULOGA)Enum.Parse(typeof(ULOGA), uloga));
            ((List<Korisnik>)HttpContext.Application["Korisnici"]).Add(kor);
            return RedirectToAction("Index", "Home");
        }
    }
}