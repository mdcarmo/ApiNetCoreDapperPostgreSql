using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi.Model;

namespace WebApi.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class TodoRepository : IRepository<TodoItem>
    {
        private string connectionString;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public TodoRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("TodoContext");
        }

        /// <summary>
        /// 
        /// </summary>
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(TodoItem item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO todo (\"Name\",\"IsComplete\") VALUES(@Name,@IsComplete)", item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TodoItem> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<TodoItem>("SELECT * FROM todo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TodoItem FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<TodoItem>("SELECT \"Name\", \"IsComplete\", \"Id\" FROM public.todo where \"Id\" = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM todo WHERE \"Id\"= @Id", new { Id = id });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Update(TodoItem item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE todo SET \"Name\" = @Name, \"IsComplete\" = @IsComplete WHERE \"Id\" = @Id", item);
            }
        }
    }
}
