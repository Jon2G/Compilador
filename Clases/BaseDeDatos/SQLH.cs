using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//Alias para poder trabajar con mas facilidad
using SqlCommand = System.Data.SQLite.SQLiteCommand;
using SqlConnection = System.Data.SQLite.SQLiteConnection;
using SqlDataReader = System.Data.SQLite.SQLiteDataReader;
using SqlDbType = System.Data.DbType;
using SqlParameter = System.Data.SQLite.SQLiteParameter;

namespace My8086.Clases.BaseDeDatos
{
    public class SQLH
    {
        public string CadenaCon { get; private set; }
        public SQLH(string CadenaCon)
        {
            this.CadenaCon = CadenaCon;
        }
        public SQLH()
        {
            FileInfo file = new FileInfo(AppData.Directorio + @"\db\Compilador.db");
            this.CadenaCon = "Data Source=" + file.FullName + ";Version=3;Pooling=true";
        }
        public class Reader : IDisposable
        {
            private SqlDataReader _Reader { get; set; }
            private SqlCommand Cmd { get; set; }
            private SqlConnection Connection { get; set; }
            public int FieldCount { get => _Reader.FieldCount; }

            internal Reader(SqlCommand Cmd)
            {
                this.Connection = Cmd.Connection;
                this.Cmd = Cmd;
                this._Reader = Cmd.ExecuteReader();
            }
            public void Dispose()
            {
                try
                {
                    if (_Reader?.IsClosed ?? false)
                        _Reader?.Close();
                    Cmd?.Dispose();
                    Connection?.Close();
                    Connection?.Dispose();
                }
                catch (Exception ex)
                {
                    Log.LogMe(ex, "Desechando objetos");
                }
            }

            public bool Read()
            {
                return this._Reader.Read();
            }
            public object this[int index] => _Reader[index];
        }
        private SqlConnection Con()
        {
            return new SqlConnection(this.CadenaCon);
        }
        public void Querry(string sql)
        {
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
        public T Single<T>(string sql)
        {
            T result = default;
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = (
                                typeof(T).IsEnum ?
                                (T)Enum.Parse(typeof(T), reader[0].ToString(), true) :
                            (T)Convert.ChangeType(reader[0], typeof(T)));
                        }
                    }
                }

                con.Close();
            }
            return result;
        }
        public T Single<T>(string sql, params SqlParameter[] parametros)
        {
            T result = default;
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(parametros);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader[0] == DBNull.Value)
                            {
                                return default;
                            }
                            result = (T)Convert.ChangeType(reader[0], typeof(T));
                        }
                    }
                }

                con.Close();
            }
            return result;
        }
        public int EXEC(string procedimiento, params SqlParameter[] parametros)
        {
            int Rows = -1;
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(procedimiento, con))
                {
                    if (parametros.Any(x => x.Value is null))
                    {
                        foreach (SqlParameter t in parametros)
                        {
                            if (t.Value is null)
                            {
                                t.Value = DBNull.Value;
                            }
                            if (!parametros.Any(x => x.Value is null))
                                break;
                        }
                    }
                    cmd.Parameters.AddRange(parametros);
                    try
                    {
                        Rows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Log.LogMe(ex, "Transaccion fallida reportada");
                        Log.LogMe(cmd.CommandText);
                        Rows = -2;
                    }
                    con.Close();
                }
            }
            return Rows;
        }
        public List<T> Lista<T>(string sql)
        {
            List<T> result = new List<T>();
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add((T)Convert.ChangeType(reader[0], typeof(T)));
                        }
                    }
                }
                con.Close();
            }
            return result;
        }
        public List<T> Lista<T>(string sql, int indice = 0, params SqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(parameters);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader[indice] == DBNull.Value)
                            {
                                result.Add(default(T));
                                continue;
                            }
                            result.Add((T)Convert.ChangeType(reader[indice], typeof(T)));
                        }
                    }
                }
                con.Close();
            }
            return result;
        }
        public bool Exists(string sql, params SqlParameter[] parameters)
        {
            using (var reader = Leector(sql, parameters))
            {
                return reader.Read();
            }
        }
        //ListaTupla
        public List<Tuple<T, Q>> ListaTupla<T, Q>(string sql)
        {
            List<Tuple<T, Q>> result = new List<Tuple<T, Q>>();
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Tuple<T, Q>
                                ((T)Convert.ChangeType(reader[0], typeof(T)),
                                (Q)Convert.ChangeType(reader[1], typeof(Q))));
                        }
                    }
                }
                con.Close();
            }
            return result;
        }
        public List<Tuple<T, Q>> ListaTupla<T, Q>(string sql, params SqlParameter[] parameters)
        {
            List<Tuple<T, Q>> result = new List<Tuple<T, Q>>();
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(parameters);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Tuple<T, Q>
                                ((T)Convert.ChangeType(reader[0], typeof(T)),
                                (Q)Convert.ChangeType(reader[1], typeof(Q))));
                        }
                    }
                }
                con.Close();
            }
            return result;
        }
        public DataTable DataTable(string Querry, string TableName = null, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable(TableName);
            using (SqlConnection con = Con())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(Querry, con))
                {
                    cmd.Parameters.AddRange(parameters);
                    result.Load(cmd.ExecuteReader());
                }
                con.Close();
            }
            return result;
        }
        public Reader Leector(string sql, params SqlParameter[] parametros)
        {
            SqlCommand cmd = null;
            try
            {
                using (cmd = new SqlCommand(sql, Con()))
                {
                    cmd.Parameters.AddRange(parametros);
                    cmd.Connection.Open();
                    return new Reader(cmd);
                }
            }
            catch (Exception ex)
            {
                Log.LogMe("Transaccion fallida reportada");
                Log.LogMe(cmd.CommandText);
                cmd?.Dispose();
                return null;
            }
        }
    }
}
