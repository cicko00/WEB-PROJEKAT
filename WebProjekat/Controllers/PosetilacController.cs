using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class PosetilacController : Controller
    {
        public ActionResult Index()
        {
             

            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];


            List<Fitnes_Centar> FitnesCentri = (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"];



            HttpContext.Application["FitnesCentri"] = FitnesCentri;
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

       

        public ActionResult Prikaz(string Naziv)
        {
            List<Korisnik> Korisnici = (List<Korisnik>)HttpContext.Application["Korisnici"];
            List<Grupni_Trening> GrupniTreninzi = (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"];
            List<Fitnes_Centar> FitnesCentri = (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"];
            List<Komentar> Komentari = (List<Komentar>)HttpContext.Application["Komentari"];
           
            HttpContext.Application["GrupniTreninzi"] = GrupniTreninzi;
            ViewData["GrupniTreninzi"] = GrupniTreninzi;
            ViewBag.Komentari = Komentari;
            ViewBag.UlogovaniKorisnik = (Korisnik)Session["PrijavljeniKorisnik"];

            foreach (Fitnes_Centar f in FitnesCentri)
            {
                if (f.Naziv == Naziv)
                {
                    ViewBag.FitnesCentar = f;
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

        public ActionResult IzmeniProfil()
        {
            if (Session["PrijavljeniKorisnik"] == null)
                
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.KorisnikZaIzmenu = (Korisnik)Session["PrijavljeniKorisnik"];
            
            return View();
        }

        [HttpPost]
        public ActionResult Izmena(string korime, string lozinka, string ponlozinka, string ime, string prezime, string pol, string email)
        {
            if (korime.Trim() == "" || lozinka.Trim() == "" || ponlozinka.Trim() == "" || ime.Trim() == "" || prezime.Trim() == "" || pol.Trim() == "" || email.Trim() == "")
            {
                TempData["Greska"] = "Niste uneli sva polja";
                return RedirectToAction("IzmeniProfil", "Posetilac");
            }

            if (lozinka != ponlozinka)
            {

                TempData["Greska"] = "Lozinka i ponovljen unos lozinke se ne poklapaju";

                return RedirectToAction("IzmeniProfil", "Posetilac");
            }
            Korisnik tempKorisnik = (Korisnik)Session["PrijavljeniKorisnik"];
            if(korime.Trim() !=tempKorisnik.KorIme && lozinka.Trim() != tempKorisnik.Lozinka )
            {

                TempData["Greska"] = "Nije moguce istovremeno promeniti i lozinku i korisnicko ime!";

                return RedirectToAction("IzmeniProfil", "Posetilac");
            }

            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == korime && korime !=tempKorisnik.KorIme )
                {

                    TempData["Greska"] = "Korisnicko ime vec postoji";

                    return RedirectToAction("IzmeniProfil", "Posetilac");
                }
                else if (k.Lozinka == lozinka && lozinka !=tempKorisnik.Lozinka)
                {
                    TempData["Greska"] = "Lozinka vec postoji";
                    return RedirectToAction("IzmeniProfil", "Posetilac");
                }
            }
               
                foreach (Korisnik kk in (List<Korisnik>)HttpContext.Application["Korisnici"])
                {
                    if (kk.KorIme == tempKorisnik.KorIme)
                    {
                        kk.KorIme = korime;
                        kk.Lozinka = lozinka;
                        kk.Pol = (POL)Enum.Parse(typeof(POL), pol);
                        kk.Email = email;
                        kk.Ime = ime;
                        kk.Prezime = prezime;
                        Session["PrijavljeniKorisnik"]=kk;
                    BazaPodataka.IzmeniKorisnike(kk);
                        break;
                    }
                }
            return RedirectToAction("PrikaziProfil","Posetilac");
        }
        public ActionResult PrikaziProfil()
        {
            if (Session["PrijavljeniKorisnik"] == null)

            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.KorisnikZaPrikaz = (Korisnik)Session["PrijavljeniKorisnik"];
            return View();
        }
        [HttpPost]
        public ActionResult PrijavaNaTrening(string treningZaPrijavu)
        {
            var temp = treningZaPrijavu.Split('/');
            
            foreach(Grupni_Trening gt in (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"])
            {
                if (gt.Naziv == temp[1].ToString())
                {
                    Grupni_Trening tempgrupnitrening = gt;
                    if (gt.PrijavljeniPosetioci.Count == gt.MaksBrojPosetilaca)
                    {
                        TempData["Greska"] = "Nema mesta u odabranom grupnom trening.Prijava nije uspela";
                        return RedirectToAction("Index","Posetilac");
                    }
                    else if (gt.PrijavljeniPosetioci.Contains((Korisnik)Session["PrijavljeniKorisnik"]))
                    {
                        TempData["Greska"] = "Vec ste prijavljeni za ucesce u odabranom treningu";
                        return RedirectToAction("Index", "Posetilac");
                    }
                    else
                    {
                      foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
                        {
                            if (k.KorIme == ((Korisnik)Session["PrijavljeniKorisnik"]).KorIme)
                            {
                                k.grupniTreninziPosetilac.Add(gt);
                                if (k.naziviGrupnihTreningaPosetilac.Trim() != string.Empty && k.naziviGrupnihTreningaPosetilac.Trim() != null)
                                {
                                   
                                    k.naziviGrupnihTreningaPosetilac += "/" + gt.Naziv;
                                    
                                   
                                }
                                else
                                {
                                    k.naziviGrupnihTreningaPosetilac = gt.Naziv;
                                }
                                BazaPodataka.IzmeniKorisnike(k);

                                if (!((Korisnik)Session["PrijavljeniKorisnik"]).grupniTreninziPosetilac.Contains(gt))
                                {
                                    ((Korisnik)Session["PrijavljeniKorisnik"]).grupniTreninziPosetilac.Add(gt);
                                }
                                gt.PrijavljeniPosetioci.Add((Korisnik)Session["PrijavljeniKorisnik"]);
                                if(gt.naziviPrijavljenihPosetioca.Trim() !=null && gt.naziviPrijavljenihPosetioca.Trim() != string.Empty)
                                {
                                    gt.naziviPrijavljenihPosetioca = ((Korisnik)Session["PrijavljeniKorisnik"]).KorIme + "/";
                                }
                                else
                                {
                                    gt.naziviPrijavljenihPosetioca += ((Korisnik)Session["PrijavljeniKorisnik"]).KorIme + "/";
                                }
                                
                                BazaPodataka.IzmeniTreninge(gt, tempgrupnitrening);
                            }
                        }
                       
                        
                    }
                }
            }

            return RedirectToAction("Index","Posetilac");

        }

      public ActionResult Istorija()
        {
            Korisnik k = (Korisnik)Session["PrijavljeniKorisnik"];

            

            List<Grupni_Trening> grupnitreninzi = new List<Grupni_Trening>();
            foreach (Grupni_Trening tr in k.grupniTreninziPosetilac)
            {
                if (DateTime.Compare(tr.DatumiVreme, DateTime.Now) < 0 )
                {
                    grupnitreninzi.Add(tr);
                }
            }

            Session["IstorijaTreninga"] = grupnitreninzi;

            if (Session["IstorijaSortirana"] != null)
            {
                List<Grupni_Trening> istorijaSortirana;
                istorijaSortirana = (List<Grupni_Trening>)Session["IstorijaSortirana"];
                
                ViewBag.IstorijaTreninga = istorijaSortirana;
                Session["IstorijaSortirana"] = null;
                return View();

            }
            else
            {
                
                ViewBag.IstorijaTreninga =grupnitreninzi ;
                return View();
            }

        }

        public ActionResult IstorijaSortiranje(string naziv,string tip,string nazivfitnescentra,string sortiranjePo,string sortiranjeRedosled)
        {
            Session["IstorijaSortirana"] = new List<Grupni_Trening>();
            List<Grupni_Trening>temp= (List<Grupni_Trening>)Session["IstorijaTreninga"];
            
            List<Grupni_Trening> istorijaTreninga = new List<Grupni_Trening>();
            istorijaTreninga =temp;
           

            if (naziv.Trim() != String.Empty)
            {
                List<Grupni_Trening> zaBrisanje = new List<Grupni_Trening>();
                foreach (Grupni_Trening gt in istorijaTreninga)
                {
                    if (!gt.Naziv.Contains(naziv.Trim()))
                        zaBrisanje.Add(gt);
                }
                foreach (Grupni_Trening gt in zaBrisanje)
                {
                    istorijaTreninga.Remove(gt);
                }
            }

            if (tip.Trim() != String.Empty)
            {
                List<Grupni_Trening> zaBrisanje = new List<Grupni_Trening>();
                foreach (Grupni_Trening gt in istorijaTreninga)
                {
                    if (gt.TipTreninga!=(TIP_TRENINGA)Enum.Parse(typeof(TIP_TRENINGA),tip))
                        zaBrisanje.Add(gt);
                }
                foreach (Grupni_Trening gt in zaBrisanje)
                {
                    istorijaTreninga.Remove(gt);
                }
            }

            if (nazivfitnescentra.Trim() != String.Empty)
            {
                List<Grupni_Trening> zaBrisanje = new List<Grupni_Trening>();
                foreach (Grupni_Trening gt in istorijaTreninga)
                {
                    if (!gt.FitnesCentar.Naziv.Contains(nazivfitnescentra.Trim()))
                        zaBrisanje.Add(gt);
                }
                foreach (Grupni_Trening gt in zaBrisanje)
                {
                    istorijaTreninga.Remove(gt);
                }
            }

            if (sortiranjePo != String.Empty)
            {
                if (sortiranjePo.Trim() == "NAZIV")
                {
                    if (sortiranjeRedosled.Trim() == "OPADAJUCE")
                    {
                        istorijaTreninga = istorijaTreninga.OrderByDescending(o => o.Naziv).ToList();
                    }
                    else if (sortiranjeRedosled.Trim() == "RASTUCE")
                    {
                        istorijaTreninga = istorijaTreninga.OrderBy(o => o.Naziv).ToList();
                    }
                }
                else if (sortiranjePo.Trim() == "TIP TRENINGA")
                {
                    if (sortiranjeRedosled.Trim() == "OPADAJUCE")
                    {
                        istorijaTreninga = istorijaTreninga.OrderByDescending(o => o.TipTreninga).ToList();
                    }
                    else if (sortiranjeRedosled.Trim() == "RASTUCE")
                    {
                        istorijaTreninga = istorijaTreninga.OrderBy(o => o.TipTreninga).ToList();
                    }
                }

                else if (sortiranjePo.Trim() == "DATUM I VREME")
                {
                    if (sortiranjeRedosled.Trim() == "OPADAJUCE")
                    {
                        istorijaTreninga = istorijaTreninga.OrderByDescending(o => o.DatumiVreme).ToList();
                    }
                    else if (sortiranjeRedosled.Trim() == "RASTUCE")
                    {
                        istorijaTreninga = istorijaTreninga.OrderBy(o => o.DatumiVreme).ToList();
                    }
                }
            }
            Session["IstorijaSortirana"] = istorijaTreninga;
            return RedirectToAction("Istorija", "Posetilac");
        }

        [HttpPost]
        public ActionResult OstaviKomentar(string komentar,string ocena,string fitnescentar)
        {
            Fitnes_Centar temp=null;
            Korisnik k = (Korisnik)Session["PrijavljeniKorisnik"];
                foreach(Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
                {
                   if (fc.Naziv == fitnescentar)
                   {

                      temp = fc;
                      break;
                   }
                }

            Komentar kom = new Komentar();
            kom.Ocena =Int32.Parse( ocena);
            kom.Tekst = komentar;
            kom.Posetilac = k;
            kom.FitnesCentar = temp;
            kom.nazivPosetioca = k.KorIme.Trim();
            kom.nazivFitnesCentra = temp.Naziv.Trim();

            ((List<Komentar>)HttpContext.Application["Komentari"]).Add(kom);
            BazaPodataka.IzmeniKomentare(kom, null);
            return RedirectToAction("Index", "Posetilac");
        }
    }

}
