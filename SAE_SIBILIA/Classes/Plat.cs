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

        private Periode disponiple;
        private Categorie categorie;
        private SousCategorie sousCategorie;

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

        public Periode Disponiple { get => disponiple; set => disponiple = value; }
        public Categorie Categorie { get => categorie; set => categorie = value; }
        public SousCategorie SousCategorie { get => sousCategorie; set => sousCategorie = value; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            string query = "DELETE FROM plat WHERE numplat = @id";

            using (var cmd = new NpgsqlCommand(query, DataAccess.Instance.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@id", this.NumPlat);
                return cmd.ExecuteNonQuery(); // retourne le nombre de lignes supprimées
            }
        }


        public bool Equals(Client? other)
        {
            throw new NotImplementedException();
        }

        public List<Plat> FindAll()
        {
            List<Plat> plats = new List<Plat>();

            // ÉTAPE 1 : On ajoute "numplat" à la requête SELECT
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(@"
        SELECT numplat, numsouscategorie, numperiode, nomplat, prixunitaire, delaipreparation, nbpersonnes
        FROM plat")) // J'ai aussi mis le nom de la table en minuscules pour la cohérence
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    plats.Add(new Plat
                    {
                        // ÉTAPE 2 : On assigne le NumPlat à l'objet
                        NumPlat = Convert.ToInt32(dr["numplat"]),

                        NomPlat = dr["nomplat"].ToString(),
                        PrixUnitaire = Convert.ToDouble(dr["prixunitaire"]),
                        DelaiPreparation = Convert.ToInt32(dr["delaipreparation"]),
                        NbPersonnes = Convert.ToInt32(dr["nbpersonnes"])

                        // Pour charger les objets Categorie, SousCategorie, etc.
                        // il faudrait faire des requêtes supplémentaires ici, mais pour la suppression,
                        // l'ID est la seule chose qui nous manquait.
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
