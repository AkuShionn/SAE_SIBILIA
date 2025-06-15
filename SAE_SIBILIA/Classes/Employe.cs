using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class Employe
    {
        private int numero;
        private int numrole;
        private string nom;
        private string prenom;
        private string motDePasse;
        private string login;

        public Employe()
        {
        }

        public Employe(int numero, int numerole, string nom, string prenom, string motDePasse, string login)
        {
            this.Numero = numero;
            this.Numrole = numerole;
            this.Nom = nom;
            this.Prenom = prenom;
            this.MotDePasse = motDePasse;
            this.Login = login;
        }

        public int Numero
        {
            get
            {
                return numero;
            }

            set
            {
                numero = value;
            }
        }

        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public string Prenom
        {
            get
            {
                return prenom;
            }

            set
            {
                prenom = value;
            }
        }

        public string MotDePasse
        {
            get
            {
                return motDePasse;
            }

            set
            {
                motDePasse = value;
            }
        }

        public string Login
        {
            get
            {
                return this.login;
            }

            set
            {
                this.login = value;
            }
        }

        public int Numrole
        {
            get
            {
                return this.numrole;
            }

            set
            {
                this.numrole = value;
            }
        }
        public void Read()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sibilia_shema.employe WHERE numemploye = @numemploye"))
            {
                cmd.Parameters.AddWithValue("numemploye", this.Numero);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.Nom = (string)row["nomemploye"];
                    this.Prenom = (string)row["prenomemploye"];
                    this.Login = (string)row["login"];
                    this.MotDePasse = (string)row["password"];

                    int idRole = (int)row["numrole"];
                    this.Numrole = idRole;
                }
            }
        }
        public List<Employe> FindAll()
        {
            List<Employe> lesEmployes = new List<Employe>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM sibilia_shema.employe"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numEmploye = (int)dr["numemploye"];
                    int numRole = (int)dr["numrole"];
                    string nom = (string)dr["nomemploye"];
                    string prenom = (string)dr["prenomemploye"];
                    string login = (string)dr["login"];
                    string mdp = (string)dr["password"];

                    // Créer l'objet Role correctement
                    int role = numRole;

                    lesEmployes.Add(new Employe(numEmploye, role, nom, prenom, login, mdp));
                }
            }
            return lesEmployes;
        }
        public static Employe LoginEtMDP(string login, string password)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sibilia_shema.employe WHERE login = @login AND password = @password"))
            {
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("password", password);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int numEmploye = (int)row["numemploye"];
                    int numRole = (int)row["numrole"];
                    string nom = (string)row["nomemploye"];
                    string prenom = (string)row["prenomemploye"];
                    string loginEmp = (string)row["login"];
                    string mdp = (string)row["password"];

                    int role = numRole;

                    return new Employe(numEmploye, role, nom, prenom, loginEmp, mdp);
                }
                return null;
            }
        }
    }
}
