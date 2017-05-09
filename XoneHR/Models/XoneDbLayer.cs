using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Diagnostics;

namespace XoneHR.Models
{
    public class XoneDbLayer
    {

       // private IDbConnection Con;
        private SqlConnection Con;
        private SqlCommand Command;
        public XoneDbLayer()
        {
            Command = new SqlCommand();
            Con = new SqlConnection(ConfigurationManager.ConnectionStrings["XonedbContext"].ConnectionString);
        }



        public List<T> DapperToList<T>(string SqlQuery, CommandType? CommandType = null) where T : class
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, CommandType).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }

          
        }

        public List<T> DapperToList<T>(string SqlQuery, DynamicParameters Parameter, CommandType? CommandType = null) where T : class
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, Parameter, null, true, null, CommandType).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }
        }

        public IEnumerable<T> DapperIEnumerable<T>(string SqlQuery, CommandType? CommandType = null) where T : class
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, CommandType).AsEnumerable();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }
        }

        public IEnumerable<T> DapperIEnumerable<T>(string SqlQuery, DynamicParameters Parameter, CommandType? CommandType = null) where T : class
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, Parameter, null, true, null, CommandType).AsEnumerable();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }

        }

        public IQueryable<T> DapperIQueryable<T>(string SqlQuery, CommandType? CommandType = null) where T : class
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, CommandType).AsQueryable();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }
        }

        public IQueryable<T> DapperIQueryable<T>(string SqlQuery, DynamicParameters Parameter, CommandType? CommandType = null) where T : class
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, Parameter, null, true, null, CommandType).AsQueryable();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }
        }


        public T DapperFirst<T>(string SqlQuery, DynamicParameters Parameter, CommandType? CommandType = null) where T : class
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, Parameter, null, true, null, CommandType).First();
            }

            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }
        }

        public string DapperSingle(string SqlQuery, DynamicParameters Parameter, CommandType? CommandType = null)
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<string>(SqlQuery, Parameter, null, true, null, CommandType).Single();
            }

            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Con.Close();
            }
        }


        public int DapperExecute(string SqlQuery, DynamicParameters Parameter, CommandType? CommandType = null)
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Execute(SqlQuery, Parameter, null, null, CommandType);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                Con.Close();
            }
        }

        public T DapperEntityExecute<T>(string SqlQuery, DynamicParameters Parameter, CommandType? CommandType = null)
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<T>(SqlQuery, Parameter, null, true, null, CommandType).First();
            }
            catch (Exception ex)
            {
                return (dynamic)null;
            }
            finally
            {
                Con.Close();
            }
        }

        public SqlMapper.GridReader DapperMultiResults(string SqlQuery, DynamicParameters Parameter=null, CommandType? CommandType = null)
        {
            try
            {
               
                return this.Con.QueryMultiple(SqlQuery, Parameter, null, null, CommandType);
            }
            catch (Exception ex)
            {
                return (dynamic)null;
            }
            finally
            {
                
            }

        }


        public int ExecuteWithDataTable(string ProcedureName, SqlParameter[] Parameter, CommandType CommandType)
        {

            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();

                Command.Connection = Con;              
                Command.CommandType = CommandType;
                Command.CommandText = ProcedureName;
                foreach (SqlParameter p in Parameter)
                {
                    if (p.Direction == ParameterDirection.Output)
                    {
                        Command.Parameters.Add(p);
                    }
                    else
                    {
                        Command.Parameters.Add(p.ParameterName, p.SqlDbType).Value = p.Value;
                    }
                }

                return Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return (dynamic)null;
            }
            finally
            {
                Con.Close();
            }
            


        }

        public bool DapperBoolean(string SqlQuery, DynamicParameters Parameter)
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
                Con.Open();
                return this.Con.Query<bool>(SqlQuery, Parameter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Con.Close();
            }
        }


    }
}