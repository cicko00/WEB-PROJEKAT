using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           
            /*
                          VLASNICI:
                   KorIme:Perapera   lozinka:PERA123
                   KorIme:jovajova   lozinka:JOVA123
                   

                           TRENERI: 
                   KorIme:DULE       lozinka:DULEDULE
                   KorIme:MARKO      lozinka:marko

                           POSETIOCI:
                   KorIme:Marija      lozinka:mmarija
                   KorIme:Luka        lozinka:lule123
                   KorIme:cile        lozinka:cile   
                   KorIme:petar       lozinka:petar
             
             
             
             */



            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];


            List<Fitnes_Centar> FitnesCentri = new List<Fitnes_Centar>();  
            foreach(Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if(fc.Izbrisan == "NE")
                {
                    FitnesCentri.Add(fc);
                }
            }
            

               
            if(HttpContext.Session["fitnesCentri"] != null) 
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

        public ActionResult Registracija()
        {
            

            return View();
        }

       
        
        public ActionResult Prijava()
        {
           
            

            return View();
        }

        [HttpPost]
        public ActionResult PrijaviKorisnika(string korime,string lozinka)
        {
            foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if(k.KorIme==korime && k.Lozinka == lozinka)
                {
                    if (k.Uloga == ULOGA.POSETILAC)
                    {
                        
                        Session["PrijavljeniKorisnikIme"] = k.KorIme;
                        Session["PrijavljeniKorisnikLozinka"] = k.Lozinka;
                        return RedirectToAction("Index", "Posetilac");
                    }
                    else if (k.Uloga == ULOGA.TRENER)
                    {

                        Session["PrijavljeniKorisnikIme"] = k.KorIme;
                        Session["PrijavljeniKorisnikLozinka"] = k.Lozinka;
                        return RedirectToAction("Index", "Trener");
                    }
                    else if (k.Uloga == ULOGA.VLASNIK)
                    {

                        Session["PrijavljeniKorisnikIme"] = k.KorIme;
                        Session["PrijavljeniKorisnikLozinka"] = k.Lozinka;
                        return RedirectToAction("Index", "Vlasnik");
                    }
                }
                
            }
            TempData["Greska"] = "Neispravno korisnicko ime ili lozinka.Pokusajte ponovo!";
            return RedirectToAction("Prijava", "Home");

        }
        
        public ActionResult Prikaz(string Naziv)
        {
            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];
            List<Grupni_Trening> GrupniTreninzi = (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"];
            List<Fitnes_Centar> FitnesCentri = (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"];
            List<Komentar> Komentari =  (List<Komentar>)HttpContext.Application["Komentari"];
             
            

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
        public ActionResult Sortiranje(string godinaOd,string godinaDo,string nazivPretraga,string adresaPretraga,string sortiranjePo,string sortiranjeRedosled)
        {
           HttpContext.Session["fitnesCentri"] = new List<Fitnes_Centar>();
            
            List<Fitnes_Centar> sviCentri = new List<Fitnes_Centar>();
            foreach( Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if(fc.Izbrisan == "NE")
                {
                    Fitnes_Centar f = fc;
                    sviCentri.Add(f);
                }
                
                
            }
            if (godinaOd.Trim() != String.Empty && Int32.TryParse(godinaOd.Trim(),out int a)==true)
            {

                List<Fitnes_Centar> zaBrisanje = new List<Fitnes_Centar>();
                foreach(Fitnes_Centar fc in sviCentri)
                {
                    if (fc.GodinaOtvaranja < a)
                        zaBrisanje.Add(fc);
                }
                foreach(Fitnes_Centar fc in zaBrisanje)
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

            if(sortiranjePo != String.Empty)
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
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult DodajNovogKorisnika(string korime,string lozinka,string ponlozinka,string ime,string prezime,string pol,string email,string datumrodjenja)
        {
            if(korime.Trim()=="" || lozinka.Trim() == "" || ponlozinka.Trim() == ""|| ime.Trim() == "" || prezime.Trim() == "" || pol.Trim() == "" || email.Trim() == "" || datumrodjenja.Trim() == "")
            {
                TempData["Greska"] = "Niste uneli sva polja";
                return RedirectToAction("Registracija", "Home");
            }
            
            if (lozinka != ponlozinka)
            {
                
                TempData["Greska"] = "Lozinka i ponovljen unos lozinke se ne poklapaju";

                return RedirectToAction( "Registracija", "Home");
            }

            foreach(Korisnik k in (List < Korisnik >) HttpContext.Application["Korisnici"])
            {
                if(k.KorIme==korime )
                {
                    
                    TempData["Greska"] = "Korisnicko ime vec postoji";

                    return RedirectToAction("Registracija", "Home");
                }
                else if(k.Lozinka == lozinka)
                {
                    TempData["Greska"] = "Lozinka vec postoji";
                    return RedirectToAction("Registracija", "Home");
                }
            }

            Korisnik kor = new Korisnik(korime, lozinka, ime, prezime, (POL)Enum.Parse(typeof(POL), pol), email, DateTime.Parse(datumrodjenja), ULOGA.POSETILAC);
            ((List<Korisnik>)HttpContext.Application["Korisnici"]).Add(kor);
            BazaPodataka.IzmeniKorisnike(kor);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Odjava()
        {
            Session["PrijavljeniKorisnikIme"] = null;
            Session["PrijavljeniKorisnikLozinka"] = null;
            return RedirectToAction("Index","Home");
        }
    }
}