using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DbContexts.Xone
{
    public class DapperLayer
    {
        private IDbConnection Con;
        private IDbCommand Command;

        public DapperLayer()
        {
            //Con = new SqlConnection("Data Source=DESKTOP-FL1E6LD\\SQLEXPRESS; User ID=sa;Password=sqladmin;;Initial Catalog=Xone;MultipleActiveResultSets=True");
            Con = new SqlConnection(ConfigurationManager.ConnectionStrings["XoneContext"].ToString());
            Command = new SqlCommand();
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

        public SqlMapper.GridReader DapperMultiResults(string SqlQuery, DynamicParameters Parameter = null, CommandType? CommandType = null)
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
    }
}