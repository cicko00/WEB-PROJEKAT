using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Grupni_Trening
    {



        public string Naziv { get; set; } 
        public TIP_TRENINGA TipTreninga { get; set; }
        public Fitnes_Centar FitnesCentar { get; set; }
        public int TrajanjeTreninga { get; set; }
        public DateTime DatumiVreme { get; set; }
        public int MaksBrojPosetilaca { get; set; }
        public List<Korisnik> PrijavljeniPosetioci { get; set; }
        public string Izbrisan { get; set; }
        public string nazivFitnesCentra { get; set; }
        public string naziviPrijavljenihPosetioca{get;set;}


        public Grupni_Trening(string naziv, TIP_TRENINGA tipTreninga, Fitnes_Centar fitnesCentar, int trajanjeTreninga, DateTime datumiVreme, int maksBrojPosetilaca)
        {
            Naziv = naziv;
            TipTreninga = tipTreninga; 
            FitnesCentar = fitnesCentar;
            TrajanjeTreninga = trajanjeTreninga;
            DatumiVreme = datumiVreme;
            MaksBrojPosetilaca = maksBrojPosetilaca;
            PrijavljeniPosetioci = new List<Korisnik>() ;
        }
         
        public Grupni_Trening() 
        {
            PrijavljeniPosetioci = new List<Korisnik>();
            Izbrisan = "NE";
            nazivFitnesCentra="";
            naziviPrijavljenihPosetioca = "";
        }


    }
}