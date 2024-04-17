using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MainMenu
{
    class CommonDev
    {
        public static string SN = VogueJewellers.Properties.Settings.Default.SN.ToString();
        public static string UN = VogueJewellers.Properties.Settings.Default.UN.ToString();
        public static string PWD = VogueJewellers.Properties.Settings.Default.PWD.ToString();

        public static string connstr = "Data Source=" + SN + ";Initial Catalog=Limited_DB;User ID=" + UN + ";Password=" + PWD + "";
        public static string connstrHR = "Data Source=" + SN + ";Initial Catalog=HR_Attendance;User ID=" + UN + ";Password=" + PWD + "";
       
        
        public static string loggedUser = string.Empty;
        public static string userPass = string.Empty;
        public static string userAuthority = string.Empty;
        public static string g_branchcode = string.Empty;

        public static int ExecuteStatement(string query, string constr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    int res = 0;
                    using (SqlCommand comm = new SqlCommand())
                    {
                        SqlTransaction dbTrans = default(SqlTransaction);
                        var _with1 = comm;
                        _with1.Connection = conn;
                        _with1.CommandType = System.Data.CommandType.Text;
                        _with1.CommandText = query;
                        try
                        {
                            conn.Open();
                            dbTrans = conn.BeginTransaction();
                            comm.Transaction = dbTrans;
                            res = comm.ExecuteNonQuery();
                            dbTrans.Commit();
                        }
                        catch (SqlException)
                        {
                            dbTrans.Rollback();
                        }
                        finally
                        {
                            conn.Dispose();
                        }
                        return res;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DataTable SelectMultipleString(string query, string constr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                DataTable dt = new DataTable();
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with4 = comm;
                    _with4.Connection = conn;
                    _with4.CommandType = System.Data.CommandType.Text;
                    _with4.CommandText = query;
                    SqlDataAdapter da = new SqlDataAdapter(comm);
                    try
                    {
                        conn.Open();
                        da.Fill(dt);
                    }
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        da.Dispose();
                        conn.Dispose();
                    }
                    return dt;
                }
            }
        }

        public static string SelectSingleString(string query, string constr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlDataReader dr = default(SqlDataReader);
                string res = string.Empty;
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with5 = comm;
                    _with5.Connection = conn;
                    _with5.CommandType = System.Data.CommandType.Text;
                    _with5.CommandText = query;
                    try
                    {
                        conn.Open();
                        dr = comm.ExecuteReader();
                        while ((dr.Read()))
                        {
                            if (dr.IsDBNull(0) == false)
                            {
                                res = Convert.ToString(dr.GetValue(0));
                            }
                        }
                    }
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        conn.Dispose();
                    }
                    return res;
                }
            }
        }

        public static int SelectSingleInteger(string query, string constr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlDataReader dr = default(SqlDataReader);
                int res = 0;
                using (SqlCommand comm = new SqlCommand())
                {
                    var _with6 = comm;
                    _with6.Connection = conn;
                    _with6.CommandType = System.Data.CommandType.Text;
                    _with6.CommandText = query;
                    try
                    {
                        conn.Open();
                        dr = comm.ExecuteReader();
                        while ((dr.Read()))
                        {
                            if (dr.IsDBNull(0) == false)
                            {
                                res = int.Parse(dr.GetValue(0).ToString());
                            }
                        }
                    }
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        conn.Dispose();
                    }
                    return res;
                }
            }
        }

        public static DataSet ReturnDataSet(string query, string constr)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                DataSet dstable = new DataSet();
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    conn.Open();
                    adapter.Fill(dstable);
                }
                catch (SqlException)
                {
                }
                finally
                {
                    conn.Dispose();
                }
                return dstable;
            }
        }
    }
}
