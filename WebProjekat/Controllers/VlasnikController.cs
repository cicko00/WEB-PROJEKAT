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


          if(  Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index","Home");
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
            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

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
            ViewBag.UlogovanIme = (string)Session["PrijavljeniKorisnikIme"];
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


            Session["FitnesCentri"] = new List<Fitnes_Centar>();



            List<Fitnes_Centar> sviCentri = new List<Fitnes_Centar>();

            foreach (Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fc.Izbrisan == "NE")
                {
                    sviCentri.Add(fc);
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
            Session["FitnesCentri"] = sviCentri;
            return RedirectToAction("Index", "Vlasnik");
        }
        [HttpPost]
        public ActionResult DodajNovogTrenera(string korime, string lozinka, string ponlozinka, string ime, string prezime, string pol, string email, string datumrodjenja, string uloga)
        {
            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


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




        public ActionResult PrikaziProfil()
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
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



        public ActionResult IzmeniProfil()
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
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
                return RedirectToAction("Index", "Home");
            }

            if (korime.Trim() == "" || lozinka.Trim() == "" || ponlozinka.Trim() == "" || ime.Trim() == "" || prezime.Trim() == "" || pol.Trim() == "" || email.Trim() == "")
            {
                TempData["Greska"] = "Niste uneli sva polja";
                return RedirectToAction("IzmeniProfil", "Vlasnik");
            }

            if (lozinka != ponlozinka)
            {

                TempData["Greska"] = "Lozinka i ponovljen unos lozinke se ne poklapaju";

                return RedirectToAction("IzmeniProfil", "Vlasnik");
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

                return RedirectToAction("IzmeniProfil", "Vlasnik");
            }

            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == korime && korime != prijavljen.KorIme)
                {

                    TempData["Greska"] = "Korisnicko ime vec postoji";

                    return RedirectToAction("IzmeniProfil", "Vlasnik");
                }
                else if (k.Lozinka == lozinka && lozinka != prijavljen.Lozinka)
                {
                    TempData["Greska"] = "Lozinka vec postoji";
                    return RedirectToAction("IzmeniProfil", "Vlasnik");
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

                    foreach (Fitnes_Centar gt in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
                    {
                        if (gt.nazivVlasnika.Trim() == imetemp.Trim())
                        {
                            gt.nazivVlasnika = kk.KorIme;
                            BazaPodataka.IzmeniFitnesCentre(gt, gt);
                        }

                    }
                    break;
                }
            }
            return RedirectToAction("PrikaziProfil", "Vlasnik");
        }

        public ActionResult UpravljanjeFitnesCentrima()
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Fitnes_Centar> tempfc = new List<Fitnes_Centar>();

            foreach(Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fc.Vlasnik.KorIme ==(string)Session["PrijavljeniKorisnikIme"] && fc.Izbrisan=="NE")
                {
                    tempfc.Add(fc);
                }
            }
            ViewBag.fitnescentri = tempfc;

            return View();


        }

        public ActionResult IzmeniFitnesCentar(string Naziv)
        {
            foreach(Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fc.Naziv == Naziv)
                {
                    Session["FitnesCentar"] = fc;
                    break;
                }
            }


            ViewBag.fitnescentar = (Fitnes_Centar)Session["FitnesCentar"];
            return View();
        }

        [HttpPost]
        public ActionResult PotvrdiIzmenu(string naziv,string adresa,string godinaotvaranja,string cenamesecneclanarine,string cenagodisnjeclanarine,string cenajednogtreninga,string cenagrupnogtreninga,string cenatreningasatrenerom)
        {

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null || Session["FitnesCentar"]==null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (naziv.Trim() == "" || adresa.Trim() == "" || godinaotvaranja.Trim() == "" || cenamesecneclanarine.Trim() == "" || cenagodisnjeclanarine.Trim() == "" || cenajednogtreninga.Trim() == "" || cenagrupnogtreninga.Trim() == "" || cenatreningasatrenerom.Trim() == "")
            {
                TempData["Greska"] = "Niste uneli sva polja";
                return RedirectToAction("IzmeniFitnesCentar", "Vlasnik");
            }

            if(Int32.TryParse(godinaotvaranja,out int a)==false || float.TryParse(cenamesecneclanarine, out float aa) == false || float.TryParse(cenagodisnjeclanarine, out float aaa) == false || float.TryParse(cenajednogtreninga, out float aaaa) == false || float.TryParse(cenagrupnogtreninga, out float aaaaa) == false || float.TryParse(cenatreningasatrenerom, out float aaaaaaa) == false)
            {
                TempData["Greska"] = "Niste pravilno uneli polja";
                return RedirectToAction("IzmeniFitnesCentar", "Vlasnik");

            }

            foreach (Fitnes_Centar fcc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fcc.Naziv == naziv && naziv != ((Fitnes_Centar)Session["FitnesCentar"]).Naziv)
                {
                    TempData["Greska"] = "Naziv vec postoji u evidenciji";
                    return RedirectToAction("IzmeniFitnesCentar", "Vlasnik");
                }
            }

            foreach (Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if(fc.Naziv== ((Fitnes_Centar)Session["FitnesCentar"]).Naziv )
                {
                    string tempnaziv = fc.Naziv;
                    string tempadresa = fc.Adresa;
                    
                    fc.Naziv = naziv;
                    fc.Adresa = adresa;
                    fc.GodinaOtvaranja = Int32.Parse(godinaotvaranja);
                    fc.CenaMesecneClanarine = float.Parse(cenamesecneclanarine);
                    fc.CenaGodisnjeClanarine = float.Parse(cenagodisnjeclanarine);
                    fc.CenaJednogTreninga = float.Parse(cenajednogtreninga);
                    fc.CenaGrupnogTreninga = float.Parse(cenagrupnogtreninga);
                    fc.CenaTreningaSaTrenerom = float.Parse(cenatreningasatrenerom);

                    Fitnes_Centar temp = new Fitnes_Centar();
                    temp.Naziv = tempnaziv;
                    temp.Adresa = tempadresa;

                    BazaPodataka.IzmeniFitnesCentre(fc, temp);
                    foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
                    {
                        if (k.fitnesCentarNaziv.Trim() == tempnaziv.Trim())
                        {
                            k.fitnesCentarNaziv = fc.Naziv.Trim();
                            BazaPodataka.IzmeniKorisnike(k);
                        }
                        if (k.NazivifitnesCentaraVlasnik.Contains(tempnaziv))
                        {
                           k.NazivifitnesCentaraVlasnik= k.NazivifitnesCentaraVlasnik.Replace(tempnaziv, fc.Naziv);
                            BazaPodataka.IzmeniKorisnike(k);
                        }

                    }
                    foreach(Grupni_Trening gt in (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"])
                    {
                        if (gt.nazivFitnesCentra.Trim() == tempnaziv.Trim())
                        {
                           string naz=gt.Naziv;
                            string nazivfc = gt.nazivFitnesCentra;

                            gt.nazivFitnesCentra = fc.Naziv;

                            Grupni_Trening gttemp = new Grupni_Trening();
                            gttemp.Naziv = naz;
                            gttemp.nazivFitnesCentra = nazivfc;


                            BazaPodataka.IzmeniTreninge(gt, gttemp);
                        }
                    }

                    foreach (Komentar kom in (List<Komentar>)HttpContext.Application["Komentari"])
                    {
                        if (kom.nazivFitnesCentra.Trim() == tempnaziv.Trim())
                        {
                            Session["komtemp"] = kom;
                            kom.nazivFitnesCentra = fc.Naziv;
                            BazaPodataka.IzmeniKomentare(kom, (Komentar)Session["komtemp"]);
                        }
                    }

                }
            }

            return RedirectToAction("UpravljanjeFitnesCentrima","Vlasnik");
        }

        public ActionResult ObrisiFitnesCentar(string Naziv)
        {
            bool zaBrisanje = true;

            foreach(Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fc.Naziv == Naziv)
                {
                    foreach(Grupni_Trening trening in (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"])
                    {
                        if(trening.FitnesCentar.Naziv.Trim()==fc.Naziv.Trim() && DateTime.Compare(trening.DatumiVreme, DateTime.Now) > 0 && trening.Izbrisan =="NE")
                        {
                            zaBrisanje = false;
                        }
                    }

                    if (zaBrisanje == true)
                    {
                        fc.Izbrisan = "DA";
                        foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
                        {
                            if (k.fitnesCentarNaziv.Trim() == fc.Naziv)
                                k.Blokiran = "DA";
                            BazaPodataka.IzmeniKorisnike(k);
                        }
                        BazaPodataka.IzmeniFitnesCentre(fc, fc);
                        break;
                    }
                    else
                    {
                        TempData["Greska"] = "Nije moguce brisanje.Postoje zakazani treninzi!";
                        return RedirectToAction("UpravljanjeFitnesCentrima", "Vlasnik");
                    }

                }
            }
            return RedirectToAction("UpravljanjeFitnesCentrima","Vlasnik");

        }
        [HttpPost]
        public ActionResult OdobriKomentar(string tekst,string ocena)
        {
            foreach(Komentar k in (List<Komentar>)HttpContext.Application["Komentari"])
            {
                if(k.Tekst.Trim()==tekst.Trim() && Int32.Parse(ocena) == k.Ocena)
                {
                    k.Odobren = "DA";
                    TempData["Greska"] = "Komentar odobren!";
                    BazaPodataka.IzmeniKomentare(k, k);
                    break;
                }
            }
            return RedirectToAction("Index","Vlasnik");
        }

        [HttpPost]
        public ActionResult OdbijKomentar(string tekst, string ocena)
        {
            foreach (Komentar k in (List<Komentar>)HttpContext.Application["Komentari"])
            {
                if (k.Tekst.Trim() == tekst.Trim() && Int32.Parse(ocena) == k.Ocena)
                {
                    k.Odobren = "ODBIJEN";
                    BazaPodataka.IzmeniKomentare(k, k);
                    break;
                }
            }
            return RedirectToAction("Index", "Vlasnik");
        }

        public ActionResult DodajFitnesCentar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DodajNoviCentar(string naziv, string adresa, string godinaotvaranja, string cenamesecneclanarine, string cenagodisnjeclanarine, string cenajednogtreninga, string cenagrupnogtreninga, string cenatreningasatrenerom)
        {



            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null )
            {
                return RedirectToAction("Index", "Home");
            }

            if (naziv.Trim() == "" || adresa.Trim() == "" || godinaotvaranja.Trim() == "" || cenamesecneclanarine.Trim() == "" || cenagodisnjeclanarine.Trim() == "" || cenajednogtreninga.Trim() == "" || cenagrupnogtreninga.Trim() == "" || cenatreningasatrenerom.Trim() == "")
            {
                TempData["Greska"] = "Niste uneli sva polja";
                return RedirectToAction("DodajFitnesCentar", "Vlasnik");
            }

            if (Int32.TryParse(godinaotvaranja, out int a) == false || float.TryParse(cenamesecneclanarine, out float aa) == false || float.TryParse(cenagodisnjeclanarine, out float aaa) == false || float.TryParse(cenajednogtreninga, out float aaaa) == false || float.TryParse(cenagrupnogtreninga, out float aaaaa) == false || float.TryParse(cenatreningasatrenerom, out float aaaaaaa) == false)
            {
                TempData["Greska"] = "Niste pravilno uneli polja";
                return RedirectToAction("DodajFitnesCentar", "Vlasnik");

            }

            foreach (Fitnes_Centar fcc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if (fcc.Naziv == naziv)
                {
                    TempData["Greska"] = "Naziv vec postoji u evidenciji";
                    return RedirectToAction("DodajFitnesCentar", "Vlasnik");
                }
            }


                    Fitnes_Centar fc = new Fitnes_Centar();
                    fc.Naziv = naziv;
                    fc.Adresa = adresa;
                    fc.GodinaOtvaranja = Int32.Parse(godinaotvaranja);
                    fc.CenaMesecneClanarine = float.Parse(cenamesecneclanarine);
                    fc.CenaGodisnjeClanarine = float.Parse(cenagodisnjeclanarine);
                    fc.CenaJednogTreninga = float.Parse(cenajednogtreninga);
                    fc.CenaGrupnogTreninga = float.Parse(cenagrupnogtreninga);
                    fc.CenaTreningaSaTrenerom = float.Parse(cenatreningasatrenerom);
                    
                   foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
                   {
                                if (k.KorIme== (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka==(string)Session["PrijavljeniKorisnikLozinka"])
                                {
                                    fc.Vlasnik = k;
                                    fc.nazivVlasnika = k.KorIme;
                                }
    
                   }


                    ((List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"]).Add(fc);
                    BazaPodataka.IzmeniFitnesCentre(fc, null);
                   

            return RedirectToAction("UpravljanjeFitnesCentrima", "Vlasnik");

        }



    }
}