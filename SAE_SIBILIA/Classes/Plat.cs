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
    public class Plat : ICrud<Plat>, INotifyPropertyChanged, IEquatable<Plat?>
    {
        private int numPlat;
        private string nomPlat;
        private double prixUnitaire;
        private int delaiPreparation;
        private int nbPersonnes;
        private string nomCategorie;
        private string nomSousCategorie;
        private string libellePeriode;

        private Periode disponible;
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

        public Periode Disponiple
        {
            get
            {
                return disponible;
            }

            set
            {
                disponible = value;
            }
        }

        public Categorie Categorie
        {
            get
            {
                return categorie;
            }

            set
            {
                categorie = value;
            }
        }

        public SousCategorie SousCategorie
        {
            get
            {
                return this.sousCategorie;
            }

            set
            {
                this.sousCategorie = value;
            }
        }

        public string NomCategorie
        {
            get
            {
                return nomCategorie;
            }

            set
            {
                nomCategorie = value;
            }
        }

        public string NomSousCategorie
        {
            get
            {
                return nomSousCategorie;
            }

            set
            {
                nomSousCategorie = value;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            // ... (code de la méthode Create que nous avons fait précédemment)
            string query = @"
                INSERT INTO plat (nomplat, prixunitaire, delaipreparation, nbpersonnes, numsouscategorie, numperiode)
                VALUES (@nomplat, @prixunitaire, @delaipreparation, @nbpersonnes, @numsouscategorie, @numperiode)
                RETURNING numplat;";
            using (var cmd = new NpgsqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@nomplat", this.NomPlat);
                cmd.Parameters.AddWithValue("@prixunitaire", this.PrixUnitaire);
                cmd.Parameters.AddWithValue("@delaipreparation", this.DelaiPreparation);
                cmd.Parameters.AddWithValue("@nbpersonnes", this.NbPersonnes);
                cmd.Parameters.AddWithValue("@numsouscategorie", this.SousCategorie.NumSousCategorie);
                cmd.Parameters.AddWithValue("@numperiode", this.Disponiple.NumPeriode);
                return DataAccess.Instance.ExecuteInsert(cmd);
            }
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

        public bool Equals(Plat? other)
        {
            throw new NotImplementedException();
        }

        public List<Plat> FindAll()
        {
            List<Plat> plats = new List<Plat>();

            // On modifie la requête pour joindre les autres tables et récupérer les noms
            string query = @"
                SELECT 
                    p.numplat, p.nomplat, p.prixunitaire, p.delaipreparation, p.nbpersonnes,
                    c.nomcategorie,
                    sc.nomsouscategorie,
                    pe.libelleperiode
                FROM
                    plat p
                JOIN souscategorie sc ON p.numsouscategorie = sc.numsouscategorie
                JOIN categorie c ON sc.numcategorie = c.numcategorie
                JOIN periode pe ON p.numperiode = pe.numperiode
                ORDER BY
                    p.nomplat;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(query))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    plats.Add(new Plat
                    {
                        // On remplit les propriétés de base
                        NumPlat = Convert.ToInt32(dr["numplat"]),
                        NomPlat = (string)dr["nomplat"],
                        PrixUnitaire = Convert.ToDouble(dr["prixunitaire"]),
                        DelaiPreparation = Convert.ToInt32(dr["delaipreparation"]),
                        NbPersonnes = Convert.ToInt32(dr["nbpersonnes"]),

                        // ON REMPLIT LES NOUVELLES PROPRIÉTÉS
                        NomCategorie = (string)dr["nomcategorie"],
                        NomSousCategorie = (string)dr["nomsouscategorie"],
                        LibellePeriode = (string)dr["libelleperiode"]
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
