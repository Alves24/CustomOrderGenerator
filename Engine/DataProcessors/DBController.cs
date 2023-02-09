using Dapper;
using Library;
using Library.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Engine.DataProcessors
{
    public static class DBController
    {
        public static List<Producto> LoadProductos()
        {
            string querryText =
                "SELECT Productos.id , Productos.nombre , Colores.color , Categorias.categoria " +
                "From Productos " +
                "LEFT JOIN Colores ON Colores.id = Productos.color_id " +
                "LEFT JOIN Categorias ON Categorias.id = Productos.categoria_id " +
                "ORDER BY categoria DESC";


            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Producto>(querryText, new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<Cliente> LoadClientes()
        {
            int id;
            string nombre;
            var clientes = new List<Cliente>();
            string querryText = "SELECT * FROM Clientes";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var reader = cnn.ExecuteReader(querryText);
                
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader[0]);
                    nombre = Convert.ToString(reader[1]);

                    clientes.Add(new Cliente(id, nombre));
                }

                return clientes;
            }
        }

        public static UserPreferences LoadPreferences()
        {
            var userPref = new UserPreferences();

            string querryText = "SELECT UserPreferences.valor " +
                                "FROM UserPreferences ";
            string where =      "WHERE (id = 'print')";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                userPref.print = bool.Parse(cnn.QueryFirst<string>(querryText + where));

                where = "WHERE (id = 'lastSeller')";

                userPref.lastSeller = cnn.QueryFirst<string>(querryText + where);

                return userPref;
            }
        }

        public static void UpdateUserPreferences(UserPreferences Preferences)
        {
            string querryText = $"UPDATE UserPreferences " +
                                $"SET valor = '{Preferences.lastSeller}' " +
                                $"WHERE id = 'lastSeller';";
            

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(querryText);
            }
        }

        public static List<int> GetIdsFromCategory(string cat)
        {
            string querryText =
                "SELECT id FROM Productos " +
                "WHERE categoria_id = (SELECT id " +
                                        "FROM Categorias "+
                                        $"WHERE categoria = '{cat}')";


            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var id = cnn.Query<int>(querryText, new DynamicParameters());
                return id.ToList();
            }
        }

        public static int WriteClientAndGetID(string NombreCliente)
        {
            string querryText = $"INSERT INTO Clientes (nombre) VALUES ('{NombreCliente}')";
            int id;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(querryText);

                querryText = $"SELECT Clientes.id FROM Clientes WHERE (nombre = '{NombreCliente}')";

                id = cnn.QueryFirst<int>(querryText);
            }

            return id;
        }
        
        public static List<String> CustomQuerry(string statement, string table, string column, string where = null)
        {
            string querryText = $"{statement} {column} " +
                                $"FROM {table} ";

            if (where != null) querryText += "WHERE " + where;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<String>(querryText, new DynamicParameters());
                return output.ToList();
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }

}