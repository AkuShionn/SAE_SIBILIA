using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

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
            get => numPlat;
            set => numPlat = value;
        }

        public string NomPlat
        {
            get => nomPlat;
            set => nomPlat = value;
        }

        public double PrixUnitaire
        {
            get => prixUnitaire;
            set => prixUnitaire = value;
        }

        public int DelaiPreparation
        {
            get => delaiPreparation;
            set => delaiPreparation = value;
        }

        public int NbPersonnes
        {
            get => nbPersonnes;
            set => nbPersonnes = value;
        }

        public Periode Disponiple
        {
            get => disponible;
            set => disponible = value;
        }

        public Categorie Categorie
        {
            get => categorie;
            set => categorie = value;
        }

        public SousCategorie SousCategorie
        {
            get => sousCategorie;
            set => sousCategorie = value;
        }

        public string NomCategorie
        {
            get => nomCategorie;
            set => nomCategorie = value;
        }

        public string NomSousCategorie
        {
            get => nomSousCategorie;
            set => nomSousCategorie = value;
        }

        public string LibellePeriode
        {
            get => libellePeriode;
            set => libellePeriode = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            string query = @"
                INSERT INTO plat (nomplat, prixunitaire, delaipreparation, nbpersonnes, numsouscategorie, numperiode)
                VALUES (@nomplat, @prixunitaire, @delaipreparation, @nbpersonnes, @numsouscategorie, @numperiode)
                RETURNING numplat;";
            using var cmd = new NpgsqlCommand(query);
            cmd.Parameters.AddWithValue("@nomplat", NomPlat);
            cmd.Parameters.AddWithValue("@prixunitaire", PrixUnitaire);
            cmd.Parameters.AddWithValue("@delaipreparation", DelaiPreparation);
            cmd.Parameters.AddWithValue("@nbpersonnes", NbPersonnes);
            cmd.Parameters.AddWithValue("@numsouscategorie", SousCategorie.NumSousCategorie);
            cmd.Parameters.AddWithValue("@numperiode", Disponiple.NumPeriode);
            return DataAccess.Instance.ExecuteInsert(cmd);
        }

        public int Delete()
        {
            string query = "DELETE FROM plat WHERE numplat = @id";
            using var cmd = new NpgsqlCommand(query, DataAccess.Instance.GetConnection());
            cmd.Parameters.AddWithValue("@id", NumPlat);
            return cmd.ExecuteNonQuery();
        }

        public bool Equals(Plat? other)
        {
            if (other is null) return false;
            return this.NumPlat == other.NumPlat;
        }

        public void Read()
        {
            // Peut être implémenté pour charger les données du plat actuel depuis la BDD si besoin
        }

        public int Update()
        {
            string query = @"
                UPDATE plat
                SET nomplat = @nomplat,
                    prixunitaire = @prixunitaire,
                    delaipreparation = @delaipreparation,
                    nbpersonnes = @nbpersonnes,
                    numsouscategorie = @numsouscategorie,
                    numperiode = @numperiode
                WHERE numplat = @numplat";

            using var cmd = new NpgsqlCommand(query);
            cmd.Parameters.AddWithValue("@nomplat", NomPlat);
            cmd.Parameters.AddWithValue("@prixunitaire", PrixUnitaire);
            cmd.Parameters.AddWithValue("@delaipreparation", DelaiPreparation);
            cmd.Parameters.AddWithValue("@nbpersonnes", NbPersonnes);
            cmd.Parameters.AddWithValue("@numsouscategorie", SousCategorie.NumSousCategorie);
            cmd.Parameters.AddWithValue("@numperiode", Disponiple.NumPeriode);
            cmd.Parameters.AddWithValue("@numplat", NumPlat);

            return DataAccess.Instance.ExecuteSet(cmd);
        }

        public List<Plat> FindAll()
        {
            List<Plat> plats = new();

            string query = @"
        SELECT 
            p.numplat, p.nomplat, p.prixunitaire, p.delaipreparation, p.nbpersonnes,
            c.numcategorie, c.nomcategorie,
            sc.numsouscategorie, sc.nomsouscategorie,
            pe.numperiode, pe.libelleperiode
        FROM
            plat p
        JOIN souscategorie sc ON p.numsouscategorie = sc.numsouscategorie
        JOIN categorie c ON sc.numcategorie = c.numcategorie
        JOIN periode pe ON p.numperiode = pe.numperiode
        ORDER BY
            p.nomplat;";

            using var cmd = new NpgsqlCommand(query);
            var dt = DataAccess.Instance.ExecuteSelect(cmd);

            foreach (DataRow dr in dt.Rows)
            {
                plats.Add(new Plat
                {
                    NumPlat = Convert.ToInt32(dr["numplat"]),
                    NomPlat = (string)dr["nomplat"],
                    PrixUnitaire = Convert.ToDouble(dr["prixunitaire"]),
                    DelaiPreparation = Convert.ToInt32(dr["delaipreparation"]),
                    NbPersonnes = Convert.ToInt32(dr["nbpersonnes"]),
                    NomCategorie = (string)dr["nomcategorie"],
                    NomSousCategorie = (string)dr["nomsouscategorie"],
                    LibellePeriode = (string)dr["libelleperiode"]
                });
            }

            return plats;
        }

        public List<Plat> FindBySelection(string criteres)
        {
            List<Plat> plats = new();

            string query = @"
                SELECT p.numplat, p.nomplat, p.prixunitaire, p.delaipreparation, p.nbpersonnes,
                       c.nomcategorie, sc.nomsouscategorie, pe.libelleperiode
                FROM plat p
                JOIN souscategorie sc ON p.numsouscategorie = sc.numsouscategorie
                JOIN categorie c ON sc.numcategorie = c.numcategorie
                JOIN periode pe ON p.numperiode = pe.numperiode
                WHERE p.nomplat ILIKE @critere
                   OR c.nomcategorie ILIKE @critere
                   OR sc.nomsouscategorie ILIKE @critere
                ORDER BY p.nomplat;";

            using var cmd = new NpgsqlCommand(query);
            cmd.Parameters.AddWithValue("@critere", $"%{criteres}%");
            var dt = DataAccess.Instance.ExecuteSelect(cmd);

            foreach (DataRow dr in dt.Rows)
            {
                plats.Add(new Plat
                {
                    NumPlat = Convert.ToInt32(dr["numplat"]),
                    NomPlat = (string)dr["nomplat"],
                    PrixUnitaire = Convert.ToDouble(dr["prixunitaire"]),
                    DelaiPreparation = Convert.ToInt32(dr["delaipreparation"]),
                    NbPersonnes = Convert.ToInt32(dr["nbpersonnes"]),
                    NomCategorie = (string)dr["nomcategorie"],
                    NomSousCategorie = (string)dr["nomsouscategorie"],
                    LibellePeriode = (string)dr["libelleperiode"]
                });
            }

            return plats;
        }
    }
}
