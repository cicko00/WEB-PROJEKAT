using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Fitnes_Centar
    {
        


       public string Naziv { get; set; }
       public string Adresa { get; set; }
       public int GodinaOtvaranja { get; set; }
       public Korisnik Vlasnik { get; set; }
      public  double CenaMesecneClanarine { get; set; } 
       public double CenaGodisnjeClanarine { get; set; }
       public double CenaJednogTreninga { get; set; }
       public double CenaGrupnogTreninga { get; set; }
       public double CenaTreningaSaTrenerom { get; set; }
        public string Izbrisan { get; set; }

        public string nazivVlasnika { get; set; }





        public Fitnes_Centar(string naziv, string adresa, int godinaOtvaranja, Korisnik vlasnik, double cenaMesecneClanarine, double cenaGodisnjeClanarine, double cenaJednogTreninga, double cenaGrupnogTreninga, double cenaTreningaSaTrenerom)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            Vlasnik = vlasnik;
            CenaMesecneClanarine = cenaMesecneClanarine;
            CenaGodisnjeClanarine = cenaGodisnjeClanarine;
            CenaJednogTreninga = cenaJednogTreninga;
            CenaGrupnogTreninga = cenaGrupnogTreninga;
            CenaTreningaSaTrenerom = cenaTreningaSaTrenerom;
            Izbrisan = "NE";
        }

        public Fitnes_Centar()
        {
            Izbrisan = "NE";
            nazivVlasnika = "";
        }
    }
}