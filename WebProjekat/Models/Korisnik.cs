using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebProjekat.Models
{
    public class Korisnik
    {
       public string KorIme { get; set; }
       public string Lozinka { get; set; }
       public string Ime { get; set; }  
       public string Prezime { get; set; }
      public POL Pol { get; set; }
      public string Email { get; set; }
     public  DateTime DatumRodjenja { get; set; }
     public  ULOGA Uloga { get; set; } 
      public List <Grupni_Trening> grupniTreninziPosetilac { get; set; }
      public List<Grupni_Trening> grupniTreninziTrener { get; set; }
        public Fitnes_Centar FitnesCentar { get; set; }
       public List<Fitnes_Centar>FitnesCentri { get; set; }
        public string Blokiran { get; set; } 

      public string NazivifitnesCentaraVlasnik { get; set; }
        public string fitnesCentarNaziv { get; set; }   
       public string naziviGrupnihTreningaTrener { get; set; }    
        public string naziviGrupnihTreningaPosetilac { get; set; } 
         
          

          
        public Korisnik()
        {
            grupniTreninziPosetilac = new List<Grupni_Trening>();
            grupniTreninziTrener = new List<Grupni_Trening>(); 
            FitnesCentri = new List<Fitnes_Centar>();
            Blokiran = "NE";
            NazivifitnesCentaraVlasnik = "";
            fitnesCentarNaziv = "";
            naziviGrupnihTreningaTrener = "";
            naziviGrupnihTreningaPosetilac = "";
        }
       public Korisnik(string korime, string lozinka, string ime,string prezime,POL p,string email,DateTime datumrodjenja,ULOGA u){
            KorIme = korime;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = p;
            Email = email;
            DatumRodjenja = datumrodjenja; 
            Uloga = u;
            grupniTreninziPosetilac = new List<Grupni_Trening>();
            grupniTreninziTrener = new List<Grupni_Trening>();
            FitnesCentri = new List<Fitnes_Centar>();
            Blokiran = "NE";
            NazivifitnesCentaraVlasnik = "";
            fitnesCentarNaziv = "";
            naziviGrupnihTreningaTrener = "";
            naziviGrupnihTreningaPosetilac = "";
        }
    }
}