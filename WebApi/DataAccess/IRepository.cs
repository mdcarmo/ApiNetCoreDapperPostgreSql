using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Update(T item);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T FindByID(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> FindAll();
    }
}
