using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Komentar
    {
        public Komentar()
        {
            Odobren = "NE";
        }

        public Komentar(Korisnik posetilac, Fitnes_Centar fitnesCentar, string tekst, int ocena)
        {
            Posetilac = posetilac; 
            FitnesCentar = fitnesCentar;
            Tekst = tekst; 
            Ocena = ocena; 
        }
          
         
       public Korisnik Posetilac { get; set; }
        public Fitnes_Centar FitnesCentar { get; set; } 
        public string Tekst { get; set; }
        public int Ocena { get; set; }
        public string Odobren { get; set; }

        public string nazivPosetioca { get; set; }
        public string nazivFitnesCentra { get; set; }
    }
}