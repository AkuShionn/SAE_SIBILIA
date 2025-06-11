using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_SIBILIA.Classes
{
    public class DataAccess
    {
        private static readonly DataAccess instance = new DataAccess();
        private readonly string connectionString = "Host=srv-peda-new;Port=5433;Username=alvesdjo;Password=NgRpEr;Database=sibilia;Options='-c search_path=sibilia_shema'";
        private NpgsqlConnection connection;

        public static DataAccess Instance
        {
            get
            {
                return instance;
            }
        }

        //  Constructeur privé pour empêcher l'instanciation multiple
        private DataAccess()
        {

            try
            {
                connection = new NpgsqlConnection(connectionString);
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
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                    throw;
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





    }


}
