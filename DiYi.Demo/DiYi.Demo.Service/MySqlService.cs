using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DiYi.Demo.EntityDto.Extend;
using MySql.Data.MySqlClient;

namespace DiYi.Demo.Service
{
    public class MySqlService
    {
        private readonly string mySql = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
        protected virtual MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(mySql);
            //connection.Open();

            return connection;
        }

        protected bool Execute(string sql, object parameter = null)
        {
            int count = 0;
            using (var connection = GetConnection())
            {
                count = connection.Execute(sql, parameter);
            }
            return count > 0;

        }

        protected T ExecuteScalar<T>(string sql, object parameter = null)
        {
            using (var connection = GetConnection())
            {
                return connection.ExecuteScalar<T>(sql, parameter);
            }
        }

        protected T QuerySingle<T>(string sql, object parameter = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Query<T>(sql, parameter).SingleOrDefault();
            }
        }

        protected List<T> QueryList<T>(string sql, object parameter = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Query<T>(sql, parameter).ToList();
            }
        }

        public bool Add<T>(T t)
        {
            string sql = "insert into {0}({1}) values({2})";

            string tableName = GetDbTableName<T>();
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = typeof(T).Name;
            }

            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                bool isDbColumn = IsDbColumn<T>(pi.Name) || IsDbKeyColumn<T>(pi.Name);
                if (isDbColumn)
                {
                    list1.Add(pi.Name);
                    list2.Add("@" + pi.Name);
                }
            }

            sql = string.Format(sql, tableName, string.Join(",", list1), string.Join(",", list2));

            return Execute(sql, t);
        }

        public int AddReturnId<T>(T t)
        {
            string sql = "insert into {0}({1}) values({2});select @@identity";

            string tableName = GetDbTableName<T>();
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = typeof(T).Name;
            }

            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in properties)
            {
                bool isDbColumn = IsDbColumn<T>(pi.Name) || IsDbKeyColumn<T>(pi.Name);
                if (isDbColumn)
                {
                    list1.Add(pi.Name);
                    list2.Add("@" + pi.Name);
                }
            }

            sql = string.Format(sql, tableName, string.Join(",", list1), string.Join(",", list2));

            return ExecuteScalar<int>(sql, t);
        }

        public T Get<T>(int id)
        {
            string tableName = GetDbTableName<T>();
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = typeof(T).Name;
            }

            string sql = $"select * from {tableName} where IsDeleted=0 and Id={id}";

            return QuerySingle<T>(sql);
        }

        private string GetDbTableName<T>()
        {
            string title = string.Empty;

            var attribute = typeof(T).GetCustomAttributes(typeof(DbTableAttribute), false).FirstOrDefault();

            if (attribute != null)
            {
                title = ((DbTableAttribute)attribute).Title;
            }

            return title;
        }

        private bool IsDbColumn<T>(string propertyName)
        {
            bool result = false;

            var propertyInfo = typeof(T).GetProperty(propertyName);
            var attribute = propertyInfo.GetCustomAttributes(typeof(DbColumnAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                result = true;
            }

            return result;
        }

        private bool IsDbKeyColumn<T>(string propertyName)
        {
            bool result = false;

            var propertyInfo = typeof(T).GetProperty(propertyName);
            var attribute = propertyInfo.GetCustomAttributes(typeof(DbKeyColumnAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                result = true;
            }

            return result;
        }


    }
}
