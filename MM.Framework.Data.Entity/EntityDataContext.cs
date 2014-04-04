using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;

namespace MM.Framework.Data.Entity
{
    public class EntityDataContext<TEntity> : DbContext, IEntityDataContext<TEntity> where TEntity : class
    {
        #region Fields

        protected readonly EntityTypeConfiguration<TEntity> TypeConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityDataContext{TEntity}" />
        /// </summary>
        /// <param name="connectionString"> </param>
        public EntityDataContext(string connectionString) : this(connectionString, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityDataContext{TEntity}" />
        /// </summary>
        /// <param name="connectionString"> </param>
        /// <param name="typeConfiguration"> </param>
        public EntityDataContext(string connectionString, EntityTypeConfiguration<TEntity> typeConfiguration)
            : base(connectionString)
        {
            TypeConfiguration = typeConfiguration;
            ConnectionString = connectionString;
            SetDatabaseInitializer<EntityDataContext<TEntity>>();
        }

        #endregion

        #region IEntityDataContext<TEntity> Members

        /// <summary>
        ///     A read-only reference to the connection string used for this object
        /// </summary>
        public string ConnectionString { get; private set; }

        public IDbSet<TEntity> EntityCollection { get; set; }

        /// <summary>
        ///     Adds a model into the database
        /// </summary>
        /// <param name="model"> The model to add </param>
        public virtual void Add(TEntity model)
        {
            EntityCollection.Add(model);
            SaveChanges();
        }

        /// <summary>
        ///     Adds models into the database
        /// </summary>
        /// <param name="models"> The model to add </param>
        public virtual void Add(IEnumerable<TEntity> models)
        {
            foreach(var model in models)
            {
                EntityCollection.Add(model);
            }
            SaveChanges();
        }

        /// <summary>
        ///     Attaches a model to the current <see cref="ObjectContext" />
        /// </summary>
        /// <param name="model"> The model to attach </param>
        /// <returns> </returns>
        public virtual TEntity Attach(TEntity model)
        {
            return EntityCollection.Attach(model);
        }

        /// <summary>
        ///     Deletes the model from the database
        /// </summary>
        /// <param name="model"> The model to delete </param>
        public virtual void Delete(TEntity model)
        {
            EntityCollection.Remove(model);
            SaveChanges();
        }

        /// <summary>
        ///     Deletes the models from the database
        /// </summary>
        /// <param name="models"> The model to delete </param>
        public virtual void Delete(IEnumerable<TEntity> models)
        {
            foreach(var model in models)
            {
                EntityCollection.Remove(model);
            }
            SaveChanges();
        }

        /// <summary>
        ///     Gets a model from the database
        /// </summary>
        /// <param name="id"> The ID of the model type to retrieve </param>
        /// <returns> </returns>
        public virtual TEntity Get(object id)
        {
            return EntityCollection.Find(id);
        }

        /// <summary>
        ///     Gets models matching the given <paramref name="searchFunction" /> from the database
        /// </summary>
        /// <param name="searchFunction"> </param>
        /// <returns> </returns>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> searchFunction)
        {
            return EntityCollection.Where(searchFunction);
        }

        /// <summary>
        ///     Gets a model from the database
        /// </summary>
        /// <param name="searchFunction"> </param>
        /// <returns> </returns>
        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> searchFunction)
        {
            return EntityCollection.FirstOrDefault(searchFunction);
        }

        /// <summary>
        ///     Gets models from the database as batched pages
        /// </summary>
        /// <returns> </returns>
        public IQueryable<TEntity> GetPaged<TKey>(Expression<Func<TEntity, TKey>> orderingExpression, int take,
            int skip = 0)
        {
            return EntityCollection.OrderBy(orderingExpression).Skip(skip).Take(take);
        }

        /// <summary>
        ///     Gets a model from the database
        /// </summary>
        /// <param name="searchFunction"> </param>
        /// <returns> </returns>
        public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> searchFunction)
        {
            return EntityCollection.SingleOrDefault(searchFunction);
        }

        /// <summary>
        ///     Saves the existing model to the database
        /// </summary>
        /// <param name="model"> The model to save </param>
        public virtual void Save(TEntity model)
        {
            Entry(model).State = EntityState.Modified;
            SaveChanges();
        }

        /// <summary>
        ///     Saves the existing models to the database
        /// </summary>
        /// <param name="models"> The model to save </param>
        public virtual void Save(IEnumerable<TEntity> models)
        {
            foreach(var model in models)
            {
                Entry(model).State = EntityState.Modified;
            }
            SaveChanges();
        }

        #endregion

        #region Static Methods

        protected static void SetDatabaseInitializer<TDbContext>() where TDbContext : DbContext
        {
            Database.SetInitializer<TDbContext>(null);
        }

        #endregion

        #region protected

        /// <summary>
        ///     Setup fluent configurations.
        /// </summary>
        /// <param name="modelBuilder"> </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if(TypeConfiguration != null)
            {
                modelBuilder.Configurations.Add(TypeConfiguration);
            }
            else
            {
                base.OnModelCreating(modelBuilder);
            }
        }

        #endregion
    }
}