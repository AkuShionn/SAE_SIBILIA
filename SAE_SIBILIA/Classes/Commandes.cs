using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class Commande
    {
        private int numCommande;
        private DateTime dateCommande;
        private DateTime dateRetraitPrevue;
        private bool payee;
        private bool retiree;
        private double prixTotal;

        public int NumCommande
        {
            get
            {
                return numCommande;
            }

            set
            {
                numCommande = value;
            }
        }

        public DateTime DateCommande
        {
            get
            {
                return dateCommande;
            }

            set
            {
                dateCommande = value;
            }
        }

        public DateTime DateRetraitPrevue
        {
            get
            {
                return dateRetraitPrevue;
            }

            set
            {
                dateRetraitPrevue = value;
            }
        }

        public bool Payee
        {
            get
            {
                return payee;
            }

            set
            {
                payee = value;
            }
        }

        public bool Retiree
        {
            get
            {
                return retiree;
            }

            set
            {
                retiree = value;
            }
        }

        public double PrixTotal
        {
            get
            {
                return this.prixTotal;
            }

            set
            {
                this.prixTotal = value;
            }
        }
    }
}
