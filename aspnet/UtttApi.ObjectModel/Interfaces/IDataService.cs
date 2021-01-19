using System.Collections.Generic;
using System.Threading.Tasks;

namespace UtttApi.ObjectModel.Interfaces
{
    /// <summary>
    /// A generic service to provide CRUD methods for storing data in a mongo database.
    /// Type must be an implementation of AEntity, and all methods are virtual.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDataService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Delete a document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Insert a document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity document);

        /// <summary>
        /// Select a document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> SelectAsync(string id);

        /// <summary>
        /// Select all documents in collection
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> SelectAsync();

        /// <summary>
        /// Update a document by replacing the whole document in db
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity document);
    }
}