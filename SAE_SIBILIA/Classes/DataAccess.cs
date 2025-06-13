using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE_SIBILIA.Classes
{
    public class DataAccess
    {
        private static DataAccess instance;
        public static string connectionString;
        private NpgsqlConnection connection;

        public static DataAccess Instance
        {
            get
            {
                return instance;
            }
        }

        //  Constructeur privé pour empêcher l'instanciation multiple
        public DataAccess(string user, string password)
        {
            connectionString = $"Host=srv-peda-new;Port=5433;Username={user};Password={password};Database=sibilia;Options='-c search_path=sibilia_shema'";
            try
            {
                connection = new NpgsqlConnection(connectionString);
                instance = this;
                GetConnection();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                throw;
            }
        }


        // pour récupérer la connexion (et l'ouvrir si nécessaire)
        public NpgsqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    MessageBox.Show("Mauvais mot de passe ou nom d'utilisateur", "Probleme de connection");
                }
            }
            return connection;
        }

        //  pour requêtes SELECT et retourne un DataTable ( table de données en mémoire)
        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL");
                throw;
            }
            return dataTable;
        }

        //   pour requêtes INSERT et renvoie l'ID généré

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                throw;
            }
            return nb;
        }




        //  pour requêtes UPDATE, DELETE
        public int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
            return nb;

        }

        // pour requêtes avec une seule valeur retour  (ex : COUNT, SUM) 
        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                cmd.Connection = GetConnection();
                res = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                throw;
            }
            return res;

        }

        //  Fermer la connexion 
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public int AjouterClient(Client client)
        {
            int result = 0;
            string query = "INSERT INTO Client (nomclient, prenomclient, tel, adresserue, adressecp, adresseville) " +
                           "VALUES (@nomclient, @prenomclient, @tel, @adresserue, @adressecp, @adresseville)";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, GetConnection()))
                {
                    // Ajouter les paramètres à la requête SQL pour éviter les injections SQL
                    cmd.Parameters.AddWithValue("@nomclient", client.NomClient);
                    cmd.Parameters.AddWithValue("@prenomclient", client.PrenomClient);
                    cmd.Parameters.AddWithValue("@tel", client.TelClient);
                    cmd.Parameters.AddWithValue("@adresserue", client.AdresseRue);
                    cmd.Parameters.AddWithValue("@adressecp", client.AdresseCP);
                    cmd.Parameters.AddWithValue("@adresseville", client.AdresseVille);

                    // Exécuter la requête INSERT
                    result = cmd.ExecuteNonQuery();  // Si un enregistrement est ajouté, result sera > 0
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de l'insertion du client");
                throw new Exception("Problème d'insertion dans la base de données");
            }
            return result;
        }


        public List<Categorie> GetCategories()
        {
            var list = new List<Categorie>();
            var cmd = new NpgsqlCommand("SELECT numcategorie, nomcategorie FROM CATEGORIE", GetConnection());
            var table = ExecuteSelect(cmd);

            foreach (DataRow row in table.Rows)
            {
                list.Add(new Categorie
                {
                    NumCategorie = Convert.ToInt32(row["numcategorie"]),
                    NomCategorie = row["nomcategorie"].ToString()
                });
            }
            return list;
        }

        public List<SousCategorie> GetSousCategoriesByCategorie(int numCategorie)
        {
            var list = new List<SousCategorie>();
            var cmd = new NpgsqlCommand("SELECT numsouscategorie, nomsouscategorie, numcategorie FROM SOUSCATEGORIE WHERE numcategorie = @id", GetConnection());
            cmd.Parameters.AddWithValue("@id", numCategorie);
            var table = ExecuteSelect(cmd);

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SousCategorie
                {
                    NumSousCategorie = Convert.ToInt32(row["numsouscategorie"]),
                    NomSousCategorie = row["nomsouscategorie"].ToString(),
                
                });
            }
            return list;
        }

        public List<Periode> GetPeriodes()
        {
            var periodes = new List<Periode>();
            var cmd = new NpgsqlCommand("SELECT numperiode, libelleperiode FROM PERIODE", GetConnection());
            var table = ExecuteSelect(cmd);

            foreach (DataRow row in table.Rows)
            {
                periodes.Add(new Periode
                {
                    NumPeriode = Convert.ToInt32(row["numperiode"]),
                    LibellePeriode = row["libelleperiode"].ToString()
                });
            }
            return periodes;
        }


        public int AjouterPlat(Plat plat)
        {
            int result = 0;

            string query = @"
        INSERT INTO PLAT (nomplat, prixunitaire, delaipreparation, nbpersonnes, numperiode, numsouscategorie)
        VALUES (@nomplat, @prix, @delai, @nb, @periode, @souscat)";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, GetConnection()))
                {
                    if (plat.Disponiple == null || plat.SousCategorie == null)
                        throw new Exception("La période ou la sous-catégorie n’est pas définie.");

                    cmd.Parameters.AddWithValue("@nomplat", plat.NomPlat);
                    cmd.Parameters.AddWithValue("@prix", plat.PrixUnitaire);
                    cmd.Parameters.AddWithValue("@delai", plat.DelaiPreparation);
                    cmd.Parameters.AddWithValue("@nb", plat.NbPersonnes);
                    cmd.Parameters.AddWithValue("@periode", plat.Disponiple.NumPeriode); // OK
                    cmd.Parameters.AddWithValue("@souscat", plat.SousCategorie.NumSousCategorie); // OK

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de l'insertion du plat");
                throw new Exception("Erreur à l'insertion : " + ex.Message);
            }

            return result;
        }



    }


}
