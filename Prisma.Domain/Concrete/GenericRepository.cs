//using Dapper;
//using Microsoft.Extensions.Configuration;
//using MDFusionLabHaute.Domain.Abstract;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data;

//namespace MDFusionLabHaute.Domain.Concrete
//{
//    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
//    {
//        protected IDbConnection Connection { get; private set; }
//        private readonly IConfiguration _configuration;
//        public GenericRepository(IConfiguration configuration, IDbConnection connection)
//        {
//            _configuration = configuration;
//            Connection = connection;
//        }
//        public async Task<IEnumerable<T>> GetAllAsync()
//        {
//            var sql = $"SELECT * FROM {GetTableName()}";
//            return await Connection.QueryAsync<T>(sql);
//        }

//        public Task<T> GetAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public virtual string GetTableName()
//        {
//            Type t = typeof(T);
//            TableAttribute tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(t, typeof(TableAttribute));
//            if (tableAttribute == null)
//            {
//                return t.Name;
//            }

//            return $"[{tableAttribute.Schema}].[{tableAttribute.Name}]";
//        }
//    }
//}
