using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WebProjekat.Models
{
    public class BazaPodataka
    {

        public List<Korisnik> KORISNICI;
        public List<Fitnes_Centar>FITNESCENTRI ;
        public List<Grupni_Trening> GRUPNITRENINZI;
        public List<Komentar> KOMENTARI;

    public BazaPodataka()
        {
            Ucitaj(out KORISNICI,out FITNESCENTRI,out GRUPNITRENINZI,out KOMENTARI);
        }

        public void Ucitaj(out List<Korisnik> KORISNICI,out List<Fitnes_Centar> FITNESCENTRI,out List<Grupni_Trening> GRUPNITRENINZI,out List<Komentar> KOMENTARI)
        {
            KORISNICI = new List<Korisnik>();
            FITNESCENTRI = new List<Fitnes_Centar>();
            GRUPNITRENINZI = new List<Grupni_Trening>();
            KOMENTARI = new List<Komentar>();
            foreach(Korisnik k in sviKorisnici())
            {
                if (k.Uloga == ULOGA.POSETILAC)
                {
                    var temp = k.naziviGrupnihTreningaPosetilac.Split('/');
                    foreach(string s in temp)
                    {

                        //var temp2 = s.Split('$');
                        foreach(Grupni_Trening gt in sviTreninzi())
                        {
                            if (gt.Naziv.Trim() == s.Trim())
                            {
                                k.grupniTreninziPosetilac.Add(gt);
                                
                                
                            }
                        }
                    }
                }

                else if (k.Uloga == ULOGA.TRENER)
                {
                    var temp = k.naziviGrupnihTreningaTrener.Split('/');
                    foreach (string s in temp)
                    {
                        
                        foreach (Grupni_Trening gt in sviTreninzi())
                        {
                            if (gt.Naziv.Trim() == s.Trim() && gt.nazivFitnesCentra == k.fitnesCentarNaziv)
                            {
                                k.grupniTreninziTrener.Add(gt);
                                
                            }
                        }
                    }

                    foreach(Fitnes_Centar fc in sviFitnesCentri())
                    {
                        if (fc.Naziv.Trim() == k.fitnesCentarNaziv.Trim())
                            k.FitnesCentar = fc;
                        break;

                    }
                }


                else if (k.Uloga == ULOGA.VLASNIK)
                {
                    var temp = k.NazivifitnesCentaraVlasnik.Split('/');
                    foreach (string s in temp)
                    {

                        foreach (Fitnes_Centar fs in sviFitnesCentri())
                        {
                            if (fs.Naziv.Trim() == s.Trim() )
                            {
                                k.FitnesCentri.Add(fs);

                            }
                        }
                    }
                }

                KORISNICI.Add(k);
            }

            foreach(Fitnes_Centar fc in sviFitnesCentri())
            {
                foreach(Korisnik k in KORISNICI)
                {
                    if (fc.nazivVlasnika == k.KorIme)
                    {
                        fc.Vlasnik = k;
                        break;
                    }
                }
                FITNESCENTRI.Add(fc);
            }

            foreach(Grupni_Trening gt in sviTreninzi())
            {
                foreach(Fitnes_Centar fc in FITNESCENTRI)
                {
                    if (gt.nazivFitnesCentra.Trim() == fc.Naziv.Trim())
                    {
                        gt.FitnesCentar = fc;
                        break;
                    }
                }
                var temp = gt.naziviPrijavljenihPosetioca.Split('/');
                foreach(string s in temp)
                {
                    foreach(Korisnik k in KORISNICI)
                    {
                        if (k.KorIme == s)
                        {
                            gt.PrijavljeniPosetioci.Add(k);
                            break;
                        }
                    }
                }
                GRUPNITRENINZI.Add(gt);
            }

            foreach(Komentar kom in sviKomentari())
            {
                  foreach(Korisnik k in KORISNICI)
                  {
                    if (k.KorIme == kom.nazivPosetioca)
                    {
                        kom.Posetilac = k;
                        break;
                    }
                  }

                  foreach(Fitnes_Centar fc in FITNESCENTRI)
                  {
                    if (fc.Naziv == kom.nazivFitnesCentra)
                    {
                        kom.FitnesCentar = fc;
                        break;
                    }
                  }
                KOMENTARI.Add(kom);
            }

            foreach(Korisnik k in KORISNICI)
            {

                if (k.Uloga == ULOGA.POSETILAC)
                {
                    var temp = k.naziviGrupnihTreningaPosetilac.Split('/');
                    k.grupniTreninziPosetilac = new List<Grupni_Trening>();
                    foreach (string s in temp)
                    {
                        
                        //var temp2 = s.Split('$');
                        foreach (Grupni_Trening gt in GRUPNITRENINZI)
                        {
                            if (gt.Naziv.Trim() == s.Trim())
                            {
                                k.grupniTreninziPosetilac.Add(gt);


                            }
                        }
                    }
                }

                else if (k.Uloga == ULOGA.TRENER)
                {
                    var temp = k.naziviGrupnihTreningaTrener.Split('/');
                    k.grupniTreninziTrener = new List<Grupni_Trening>();
                    foreach (string s in temp)
                    {
                        
                        foreach (Grupni_Trening gt in GRUPNITRENINZI)
                        {
                            if (gt.Naziv.Trim() == s.Trim() && gt.nazivFitnesCentra.Trim() == k.fitnesCentarNaziv.Trim())
                            {
                                k.grupniTreninziTrener.Add(gt);

                            }
                        }
                    }

                    foreach (Fitnes_Centar fc in FITNESCENTRI)
                    {
                        if (fc.Naziv.Trim() == k.fitnesCentarNaziv.Trim())
                        {
                            k.FitnesCentar = fc;
                            break;
                        }
                            

                    }
                }


                else if (k.Uloga == ULOGA.VLASNIK)
                {
                    var temp = k.NazivifitnesCentaraVlasnik.Split('/');
                    k.FitnesCentri = new List<Fitnes_Centar>();
                    foreach (string s in temp)
                    {
                        
                        foreach (Fitnes_Centar fs in FITNESCENTRI)
                        {
                            if (fs.Naziv == s)
                            {
                                k.FitnesCentri.Add(fs);

                            }
                        }
                    }
                }

                



            }

        }

        public   List<Korisnik> sviKorisnici()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Vlasnici.xml");
            var root = xml.Root;
           foreach(var element in root.Elements())
            {
                Korisnik k = new Korisnik();
               
                k.KorIme = element.Element("KorIme").Value;   
                k.Blokiran = element.Element("Blokiran").Value;
                k.Lozinka = element.Element("Lozinka").Value;
                k.Ime = element.Element("Ime").Value;
                k.Prezime = element.Element("Prezime").Value;
                k.Pol =(POL)Enum.Parse(typeof(POL), element.Element("Pol").Value);
                k.Email = element.Element("Email").Value;
                k.DatumRodjenja = DateTime.Parse(element.Element("DatumRodjenja").Value);
                k.Uloga = (ULOGA)Enum.Parse(typeof(ULOGA), element.Element("Uloga").Value);
                k.NazivifitnesCentaraVlasnik = element.Element("NazivifitnesCentaraVlasnik").Value;
                k.fitnesCentarNaziv = element.Element("fitnesCentarNaziv").Value;
                k.naziviGrupnihTreningaTrener = element.Element("naziviGrupnihTreningaTrener").Value;
                k.naziviGrupnihTreningaPosetilac = element.Element("naziviGrupnihTreningaPosetilac").Value;
                korisnici.Add(k);


              
            }
            return korisnici;

        }

        public List<Fitnes_Centar> sviFitnesCentri()
        {
            List<Fitnes_Centar> fitnescentri = new List<Fitnes_Centar>();
            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Fitnes_Centri.xml");
            var root = xml.Root;
            foreach (var element in root.Elements())
            {
                Fitnes_Centar fc = new Fitnes_Centar();
                fc.Naziv = element.Element("Naziv").Value;
                fc.Adresa = element.Element("Adresa").Value;
                fc.GodinaOtvaranja =Int32.Parse( element.Element("GodinaOtvaranja").Value);
                fc.CenaGodisnjeClanarine =float.Parse( element.Element("CenaGodisnjeClanarine").Value);
                fc.CenaGrupnogTreninga = float.Parse(element.Element("CenaGrupnogTreninga").Value);
                fc.CenaJednogTreninga = float.Parse(element.Element("CenaJednogTreninga").Value);
                fc.CenaMesecneClanarine = float.Parse(element.Element("CenaMesecneClanarine").Value);
                fc.CenaTreningaSaTrenerom = float.Parse(element.Element("CenaTreningaSaTrenerom").Value);
                fc.Izbrisan = element.Element("Izbrisan").Value;
                fc.nazivVlasnika = element.Element("nazivVlasnika").Value;
                fitnescentri.Add(fc);

            }
            return fitnescentri;

        }

        public List<Grupni_Trening> sviTreninzi()
        {
            List<Grupni_Trening> grupnitreninzi = new List<Grupni_Trening>();
            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Grupni_Treninzi.xml");
            var root = xml.Root;
            foreach (var element in root.Elements())
            {
                Grupni_Trening gt = new Grupni_Trening();
                gt.Izbrisan = element.Element("Izbrisan").Value;
                gt.Naziv = element.Element("Naziv").Value;
                gt.DatumiVreme = DateTime.Parse(element.Element("DatumiVreme").Value);
                gt.TipTreninga = (TIP_TRENINGA)Enum.Parse(typeof(TIP_TRENINGA), element.Element("TipTreninga").Value);
                gt.MaksBrojPosetilaca = int.Parse(element.Element("MaksBrojPosetilaca").Value);
                gt.TrajanjeTreninga = int.Parse(element.Element("TrajanjeTreninga").Value);
                gt.nazivFitnesCentra = element.Element("nazivFitnesCentra").Value;
                gt.naziviPrijavljenihPosetioca = element.Element("naziviPrijavljenihPosetioca").Value;
                



                grupnitreninzi.Add(gt);

            }
            return grupnitreninzi;

        }


        public List<Komentar> sviKomentari()
        {
            List<Komentar> komentari = new List<Komentar>();
            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Komentari.xml");
            var root = xml.Root;
            foreach (var element in root.Elements())
            {
                Komentar kom = new Komentar();
                kom.Tekst = element.Element("Tekst").Value;
                kom.Ocena= int.Parse(element.Element("Ocena").Value);
                kom.nazivFitnesCentra = element.Element("nazivFitnesCentra").Value;
                kom.nazivPosetioca= element.Element("nazivPosetioca").Value;
                kom.Odobren = element.Element("Odobren").Value;




                komentari.Add(kom);

            }
            return komentari;

        }

        public static void IzmeniKorisnike(Korisnik k)
        {
            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Vlasnici.xml");
            var root = xml.Root;
            foreach (var element in root.Elements())
            {
                if(element.Element("KorIme").Value.Trim()==k.KorIme || element.Element("Lozinka").Value.Trim()==k.Lozinka)
                {
                    element.Remove();
                    root.Add(new XElement("Korisnik", new XElement("KorIme", k.KorIme), new XElement("Lozinka", k.Lozinka), new XElement("Ime", k.Ime), new XElement("Prezime", k.Prezime), new XElement("Pol", k.Pol.ToString()), new XElement("Email", k.Email), new XElement("DatumRodjenja", k.DatumRodjenja), new XElement("Uloga", k.Uloga.ToString()), new XElement("naziviGrupnihTreningaPosetilac", k.naziviGrupnihTreningaPosetilac), new XElement("naziviGrupnihTreningaTrener", k.naziviGrupnihTreningaTrener), new XElement("fitnesCentarNaziv", k.fitnesCentarNaziv), new XElement("NazivifitnesCentaraVlasnik", k.NazivifitnesCentaraVlasnik),new XElement("Blokiran",k.Blokiran)));
                    xml.Save("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Vlasnici.xml");
                    return;
                }

                       
            } 
            root.Add(new XElement("Korisnik", new XElement("KorIme", k.KorIme), new XElement("Lozinka", k.Lozinka), new XElement("Ime", k.Ime), new XElement("Prezime", k.Prezime), new XElement("Pol", k.Pol.ToString()), new XElement("Email", k.Email), new XElement("DatumRodjenja", k.DatumRodjenja), new XElement("Uloga", k.Uloga.ToString()), new XElement("naziviGrupnihTreningaPosetilac", k.naziviGrupnihTreningaPosetilac), new XElement("naziviGrupnihTreningaTrener", k.naziviGrupnihTreningaTrener), new XElement("fitnesCentarNaziv", k.fitnesCentarNaziv), new XElement("NazivifitnesCentaraVlasnik", k.NazivifitnesCentaraVlasnik),new XElement("Blokiran", k.Blokiran)));
            xml.Save("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Vlasnici.xml");

        }
        public static void IzmeniFitnesCentre(Fitnes_Centar novi,Fitnes_Centar stari)
        {

            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Fitnes_Centri.xml");
            var root = xml.Root;
            if (stari != null)
            {
                foreach (var element in root.Elements())
                {
                    if (element.Element("Naziv").Value.Trim() == stari.Naziv.Trim() && element.Element("Adresa").Value.Trim() == stari.Adresa)
                    {
                        element.Remove();

                    }

                }
            }
            root.Add(new XElement("FitnesCentar", new XElement("Naziv", novi.Naziv), new XElement("Adresa", novi.Adresa), new XElement("GodinaOtvaranja", novi.GodinaOtvaranja), new XElement("CenaMesecneClanarine", novi.CenaMesecneClanarine), new XElement("CenaGodisnjeClanarine", novi.CenaGodisnjeClanarine), new XElement("CenaGrupnogTreninga", novi.CenaGrupnogTreninga), new XElement("CenaJednogTreninga", novi.CenaJednogTreninga), new XElement("CenaTreningaSaTrenerom", novi.CenaTreningaSaTrenerom), new XElement("nazivVlasnika", novi.nazivVlasnika),new XElement("Izbrisan",novi.Izbrisan)));
            xml.Save("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Fitnes_Centri.xml");




        }
        public static void IzmeniTreninge(Grupni_Trening novi,Grupni_Trening stari)
        {


            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Grupni_Treninzi.xml");
            var root = xml.Root;
            if (stari != null)
            {
                foreach (var element in root.Elements())
                {
                    if (element.Element("Naziv").Value.Trim() == stari.Naziv.Trim() && element.Element("nazivFitnesCentra").Value.Trim() == stari.nazivFitnesCentra.Trim())
                    {
                        element.Remove();

                    }

                }
            }
            root.Add(new XElement("GrupniTrening", new XElement("Naziv", novi.Naziv), new XElement("TipTreninga", novi.TipTreninga), new XElement("TrajanjeTreninga", novi.TrajanjeTreninga), new XElement("DatumiVreme", novi.DatumiVreme), new XElement("MaksBrojPosetilaca", novi.MaksBrojPosetilaca), new XElement("nazivFitnesCentra", novi.nazivFitnesCentra), new XElement("naziviPrijavljenihPosetioca", novi.naziviPrijavljenihPosetioca),new XElement("Izbrisan",novi.Izbrisan)));
            xml.Save("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Grupni_Treninzi.xml");




        }
        public static void IzmeniKomentare(Komentar novi,Komentar stari)
        {




            var xml = XDocument.Load("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Komentari.xml");
            var root = xml.Root;
            if (stari != null)
            {
                foreach (var element in root.Elements())
                {
                    if (element.Element("Tekst").Value.Trim() == stari.Tekst && element.Element("nazivPosetioca").Value.Trim() == stari.nazivPosetioca)
                    {
                        element.Remove();

                    }

                }
            }
            root.Add(new XElement("Komentar", new XElement("Tekst", novi.Tekst), new XElement("Ocena", novi.Ocena), new XElement("nazivPosetioca", novi.nazivPosetioca), new XElement("nazivFitnesCentra", novi.nazivFitnesCentra),new XElement("Odobren",novi.Odobren)));
            xml.Save("C:\\Users\\Cvijetin Glisic\\Desktop\\WEB_Projekat\\WebProjekat\\WebProjekat\\Models\\Komentari.xml");





        }


    }
}