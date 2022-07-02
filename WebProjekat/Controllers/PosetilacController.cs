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

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
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

            Korisnik prijavljen = null;
            foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka== (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    prijavljen = k;
                    break;

                }
                
            }
            ViewBag.UlogovaniKorisnik = prijavljen;

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

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            Session["FitnesCentri"] = new List<Fitnes_Centar>();

            List<Fitnes_Centar> sviCentri = new List<Fitnes_Centar>();
             foreach( Fitnes_Centar fc in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
            {
                if(fc.Izbrisan == "NE")
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
            return RedirectToAction("Index", "Posetilac");
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
                return RedirectToAction("Index", "Home");
            }

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
            Korisnik prijavljen = null;
            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka == (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    prijavljen = k;
                    break;

                }

            }
            
            if(korime.Trim() !=prijavljen.KorIme && lozinka.Trim() != prijavljen.Lozinka )
            {

                TempData["Greska"] = "Nije moguce istovremeno promeniti i lozinku i korisnicko ime!";

                return RedirectToAction("IzmeniProfil", "Posetilac");
            }

            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == korime && korime !=prijavljen.KorIme )
                {

                    TempData["Greska"] = "Korisnicko ime vec postoji";

                    return RedirectToAction("IzmeniProfil", "Posetilac");
                }
                else if (k.Lozinka == lozinka && lozinka !=prijavljen.Lozinka)
                {
                    TempData["Greska"] = "Lozinka vec postoji";
                    return RedirectToAction("IzmeniProfil", "Posetilac");
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
                        Session["PrijavljeniKorisnikIme"]=kk.KorIme;
                        Session["PrijavljeniKorisnikLozinka"] = kk.Lozinka;


                         BazaPodataka.IzmeniKorisnike(kk);
                        foreach(Komentar kom in (List<Komentar>) HttpContext.Application["Komentari"])
                        {
                          if (kom.nazivPosetioca == imetemp)
                          {
                            kom.nazivPosetioca = kk.KorIme;

                            Komentar temp = new Komentar();
                            temp.nazivPosetioca = imetemp;
                            temp.Tekst = kom.Tekst;


                            BazaPodataka.IzmeniKomentare(kom, temp);
                          }
                        }

                         foreach (Grupni_Trening gt in (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"])
                         {
                            if (gt.naziviPrijavljenihPosetioca.Contains(imetemp.Trim()))
                            {
                              gt.naziviPrijavljenihPosetioca = gt.naziviPrijavljenihPosetioca.Replace(imetemp, kk.KorIme);
                              BazaPodataka.IzmeniTreninge(gt, gt);
                            }

                         }

                    foreach (Fitnes_Centar gt in (List<Fitnes_Centar>)HttpContext.Application["FitnesCentri"])
                    {
                        if (gt.nazivVlasnika.Trim()==imetemp.Trim())
                        {
                            gt.nazivVlasnika = kk.KorIme;
                            BazaPodataka.IzmeniFitnesCentre(gt, gt);
                        }

                    }

                    break;
                    }
                }
            return RedirectToAction("PrikaziProfil","Posetilac");
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
        [HttpPost]
        public ActionResult PrijavaNaTrening(string treningZaPrijavu)
        {
            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }



            var temp = treningZaPrijavu.Split('/');

            
            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka == (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    Session["Prijavljen"]= k;
                    break;

                }

            }

            foreach (Grupni_Trening gt in (List<Grupni_Trening>)HttpContext.Application["GrupniTreninzi"])
            {
                if (gt.Naziv == temp[1].ToString())
                {
                    Grupni_Trening tempgrupnitrening = gt;
                    if (gt.PrijavljeniPosetioci.Count == gt.MaksBrojPosetilaca)
                    {
                        TempData["Greska"] = "Nema mesta u odabranom grupnom trening.Prijava nije uspela";
                        return RedirectToAction("Index","Posetilac");
                    }
                    else if (gt.PrijavljeniPosetioci.Contains((Korisnik)Session["Prijavljen"]))
                    {
                        TempData["Greska"] = "Vec ste prijavljeni za ucesce u odabranom treningu";
                        return RedirectToAction("Index", "Posetilac");
                    }
                    else if (((Korisnik)Session["Prijavljen"]).grupniTreninziPosetilac.Contains(gt))
                    {
                        TempData["Greska"] = "Vec ste prijavljeni za ucesce u odabranom treningu";
                        return RedirectToAction("Index", "Posetilac");
                    }

                    else
                    {
                      foreach(Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
                        {
                            if (k.KorIme == ((Korisnik)Session["Prijavljen"]).KorIme)
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
                                Session["Prijavljen"] = null;
                                Session["Prijavljen"] = k;
                                BazaPodataka.IzmeniKorisnike(k);

                               
                                gt.PrijavljeniPosetioci.Add((k)); 
                                if(gt.naziviPrijavljenihPosetioca.Trim() !=null && gt.naziviPrijavljenihPosetioca.Trim() != string.Empty)
                                {
                                    gt.naziviPrijavljenihPosetioca += k.KorIme + "/";
                                }
                                else
                                {
                                    gt.naziviPrijavljenihPosetioca = k.KorIme + "/";
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

            List<Grupni_Trening> grupnitreninzi = new List<Grupni_Trening>();
            foreach (Grupni_Trening tr in prijavljen.grupniTreninziPosetilac)
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

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


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

            if (Session["PrijavljeniKorisnikIme"] == null || Session["PrijavljeniKorisnikLozinka"] == null)
            {
                return RedirectToAction("Index", "Home");
            }



            Fitnes_Centar temp=null;

            Korisnik prijavljen = null;
            foreach (Korisnik k in (List<Korisnik>)HttpContext.Application["Korisnici"])
            {
                if (k.KorIme == (string)Session["PrijavljeniKorisnikIme"] && k.Lozinka == (string)Session["PrijavljeniKorisnikLozinka"])
                {
                    prijavljen = k;
                    break;

                }

            }
            
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
            kom.Posetilac = prijavljen;
            kom.FitnesCentar = temp;
            kom.nazivPosetioca = prijavljen.KorIme.Trim();
            kom.nazivFitnesCentra = temp.Naziv.Trim();

            ((List<Komentar>)HttpContext.Application["Komentari"]).Add(kom);
            BazaPodataka.IzmeniKomentare(kom, null);
            return RedirectToAction("Index", "Posetilac");
        }
    }

}
