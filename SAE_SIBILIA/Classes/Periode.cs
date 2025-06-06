using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class Periode
    {
        private int numPeriode;
        private string libellePeriode;

        public int NumPeriode
        {
            get
            {
                return numPeriode;
            }

            set
            {
                numPeriode = value;
            }
        }

        public string LibellePeriode
        {
            get
            {
                return this.libellePeriode;
            }

            set
            {
                this.libellePeriode = value;
            }
        }
    }
}
