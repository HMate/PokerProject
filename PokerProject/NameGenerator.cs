using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject
{
    static class NameGenerator
    {
        private static List<string> names;
        private static Random randomGenerator;

        static NameGenerator()
        {
            names = new List<string>();
            randomGenerator = new Random();

            names.Add("Máté");
            names.Add("András");
            names.Add("Béla");
            names.Add("Aladár");
            names.Add("Károly");
            names.Add("Borbála");
            names.Add("Ferenc");
            names.Add("Mária");
            names.Add("Teréz");
            names.Add("Tibor");
            names.Add("Ilona");
            names.Add("Kinga");
            names.Add("Soma");
            names.Add("Sándor");
            names.Add("Virág");
            names.Add("Balázs");
            names.Add("Csaba");
            names.Add("Péter");
            names.Add("Győző");
            names.Add("Ádám");
            names.Add("Áron");
            names.Add("Ábel");
            names.Add("Anett");
            names.Add("Rihárd");
            names.Add("Vince");
            names.Add("Krisztián");
            names.Add("Elemér");
            names.Add("Zsolt");
            names.Add("Petra");
            names.Add("Gergő");
            names.Add("Teodóra");
        }

        public static string GenerateName()
        {
            return names[(randomGenerator.Next(names.Count))];
        }
    }
}
