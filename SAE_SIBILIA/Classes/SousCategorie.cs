using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class SousCategorie : IEquatable<SousCategorie?>
    {
        private int numSousCategorie;
        private string nomSousCategorie;

        public SousCategorie()
        {
        }

        public SousCategorie(int numSousCategorie, string nomSousCategorie)
        {
            this.NumSousCategorie = numSousCategorie;
            this.NomSousCategorie = nomSousCategorie;
        }

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

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as SousCategorie);
        }

        public bool Equals(SousCategorie? other)
        {
            return other is not null &&
                   this.NumSousCategorie == other.NumSousCategorie &&
                   this.NomSousCategorie == other.NomSousCategorie;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.NumSousCategorie, this.NomSousCategorie);
        }
    }
}
