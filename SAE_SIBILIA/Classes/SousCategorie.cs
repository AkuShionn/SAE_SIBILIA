using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class SousCategorie
    {
        private int numSousCategorie;
        private string nomSousCategorie;

        public int NumSousCategorie
        {
            get
            {
                return numSousCategorie;
            }

            set
            {
                numSousCategorie = value;
            }
        }

        public string NomSousCategorie
        {
            get
            {
                return this.nomSousCategorie;
            }

            set
            {
                this.nomSousCategorie = value;
            }
        }
    }
