using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MM.Framework.Data.Entity
{
    public interface IEntityDataContext<TEntity> : IDisposable where TEntity : class
    {
        #region Properties

        /// <summary>
        ///     A read-only reference to the connection string used for this object
        /// </summary>
        string ConnectionString { get; }

        IDbSet<TEntity> EntityCollection { get; set; }

        #endregion

        #region public

        /// <summary>
        ///     Adds a model into the database
        /// </summary>
        /// <param name="model"> The model to add </param>
        void Add(TEntity model);

        /// <summary>
        ///     Adds models into the database
        /// </summary>
        /// <param name="models"> The model to add </param>
        void Add(IEnumerable<TEntity> models);

        /// <summary>
        ///     Attaches a model to the current session
        /// </summary>
        /// <param name="model"> The model to attach </param>
        /// <returns> </returns>
        TEntity Attach(TEntity model);

        /// <summary>
        ///     Deletes the model from the database
        /// </summary>
        /// <param name="model"> The model to delete </param>
        void Delete(TEntity model);

        /// <summary>
        ///     Deletes the models from the database
        /// </summary>
        /// <param name="models"> The model to delete </param>
        void Delete(IEnumerable<TEntity> models);

        /// <summary>
        ///     Gets a model from the database
        /// </summary>
        /// <param name="id"> The ID of the model type to retrieve </param>
        /// <returns> </returns>
        TEntity Get(object id);

        /// <summary>
        ///     Gets models matching the given <paramref name="searchFunction" /> from the database
        /// </summary>
        /// <param name="searchFunction"> </param>
        /// <returns> </returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> searchFunction);

        /// <summary>
        ///     Gets a model from the database
        /// </summary>
        /// <param name="searchFunction"> </param>
        /// <returns> </returns>
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> searchFunction);

        /// <summary>
        ///     Gets models from the database as batched pages
        /// </summary>
        /// <returns> </returns>
        IQueryable<TEntity> GetPaged<TKey>(Expression<Func<TEntity, TKey>> orderingExpression, int take, int skip = 0);

        /// <summary>
        ///     Gets a model from the database
        /// </summary>
        /// <param name="searchFunction"> </param>
        /// <returns> </returns>
        TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> searchFunction);

        /// <summary>
        ///     Saves the existing model to the database
        /// </summary>
        /// <param name="model"> The model to save </param>
        void Save(TEntity model);

        /// <summary>
        ///     Saves the existing models to the database
        /// </summary>
        /// <param name="models"> The model to save </param>
        void Save(IEnumerable<TEntity> models);

        #endregion
    }
}