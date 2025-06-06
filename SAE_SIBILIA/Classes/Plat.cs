using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class Plat
    {
        private int numPlat;
        private string nomPlat;
        private double prixUnitaire;
        private int delaiPreparation;
        private int nbPersonnes;

        public int NumPlat
        {
            get
            {
                return numPlat;
            }

            set
            {
                numPlat = value;
            }
        }

        public string NomPlat
        {
            get
            {
                return nomPlat;
            }

            set
            {
                nomPlat = value;
            }
        }

        public double PrixUnitaire
        {
            get
            {
                return prixUnitaire;
            }

            set
            {
                prixUnitaire = value;
            }
        }

        public int DelaiPreparation
        {
            get
            {
                return delaiPreparation;
            }

            set
            {
                delaiPreparation = value;
            }
        }

        public int NbPersonnes
        {
            get
            {
                return this.nbPersonnes;
            }

            set
            {
                this.nbPersonnes = value;
            }
        }
    }
}
