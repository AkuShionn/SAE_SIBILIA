using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class Plat : ICrud<Plat>, INotifyPropertyChanged, IEquatable<Client?>
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public bool Equals(Client? other)
        {
            throw new NotImplementedException();
        }

        public List<Plat> FindAll()
        {
            List<Plat> plats = new List<Plat>();

            // Requête SQL pour récupérer les plats
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(@"
             SELECT numsouscategorie, numperiode, nomplat, prixunitaire, delaipreparation, nbpersonnes
             FROM Plat"))
            {
                // Exécution de la requête et récupération des résultats
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    // Création d'un objet Plat avec les valeurs de chaque ligne
                    plats.Add(new Plat
                    {
                        NomPlat = dr["nomplat"].ToString(),
                        PrixUnitaire = Convert.ToDouble(dr["prixunitaire"]), // Convertir le prix en double
                        DelaiPreparation = Convert.ToInt32(dr["delaipreparation"]), // Convertir le délai de préparation en int
                        NbPersonnes = Convert.ToInt32(dr["nbpersonnes"]) // Convertir le nombre de personnes en int
                    });
                }
            }

            return plats;
        }




        public List<Plat> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
        
    }
}
