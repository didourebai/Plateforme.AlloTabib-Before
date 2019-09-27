using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;

namespace Plateforme.AlloTabib.InfrastructureLayer.BaseContext
{
    public class Repository<T> : IRepository<T> where T : BasicEntity
    {
        #region Private Attributes
        /// <summary>
        /// Define the current session factory.
        /// </summary>
        //private readonly ISessionFactory _sessionFactory;
        private readonly ISessionFactoriesManager _sessionFactoriesManager;

        private ISessionFactory _sessionFactory
        {
            get
            {
                return _sessionFactoriesManager.GetEntitySessionFactory<T>();
            }
        }

        #endregion

        #region Constructors

        public Repository(ISessionFactoriesManager sessionFactoriesManager)
        {
            if (sessionFactoriesManager == null)
                throw new ArgumentNullException("sessionFactoriesManager");

            //_sessionFactory = sessionFactory;
            _sessionFactoriesManager = sessionFactoriesManager;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return an entity T by its id.
        /// </summary>
        /// <param name="id">the object id.</param>
        /// <returns>entity of type T.</returns>
        public T Get(object id)
        {

            return _sessionFactory.GetCurrentSession().Get<T>(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projections"></param>
        /// <returns></returns>
        public T Get(object id, ProjectionList projections)
        {

            return _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projections)
                .UniqueResult<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="id"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public Q Get<Q>(object id, IProjection projection)// where Q : class
        {

            return _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projection)
                .UniqueResult<Q>();
        }

        /// <summary>
        /// return a list of entities of type T.
        /// </summary>
        /// <returns>list of entities of type T.</returns>
        public IEnumerable<T> GetAll()
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T)).List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Order order)
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T))
                .AddOrder(order)
                .List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(ICriterion restrictions)
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T))
                .Add(restrictions)
                .List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Order order, ICriterion restrictions)
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T))
                .AddOrder(order)
                .Add(restrictions)
                .List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projections"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(ProjectionList projections)
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projections)
                .List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="projections"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Order order, ProjectionList projections)
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projections)
                .AddOrder(order)
                .List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restrictions"></param>
        /// <param name="projections"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(ICriterion restrictions, ProjectionList projections)
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projections)
                .Add(restrictions)
                .List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="restrictions"></param>
        /// <param name="projections"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Order order, ICriterion restrictions, ProjectionList projections)
        {

            return _sessionFactory.GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projections)
                .Add(restrictions)
                .AddOrder(order)
                .List<T>();
        }

        public IEnumerable<Q> GetAll<Q>(IProjection projection) //where Q : class
        {

            return _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projection)
                .Future<Q>();
        }

        public IEnumerable<Q> GetAll<Q>(IProjection projection, ICriterion restrictions) //where Q : class
        {

            return _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .Add(restrictions)
                .SetProjection(projection)
                .Future<Q>();
        }

        public IEnumerable<I> GetAllForStruct<I>(IProjection projection)// where I : struct
        {

            return _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .SetProjection(projection)
                .Future<I>();
        }

        public IEnumerable<I> GetAllForStruct<I>(IProjection projection, ICriterion restrictions)// where I : struct
        {

            return _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .Add(restrictions)
                .SetProjection(projection)
                .Future<I>();
        }

        /// <summary>
        /// Add new entity of type T.
        /// </summary>
        /// <param name="item">the entity to add.</param>
        /// <returns>True if ok, otherwise false</returns>
        public bool Add(T item)
        {

            if (item == null)
                throw new ArgumentNullException();

            //LogDbAction("Adding", item);

            item.CreationDate = DateTime.Now;
            item.LastModificationDate = DateTime.Now;
            _sessionFactory.GetCurrentSession().SaveOrUpdate(item);
            return true;
        }

        public bool AddAndCommit(T item)
        {

            if (item == null)
                throw new ArgumentNullException();

            //LogDbAction("Adding", item);

            item.CreationDate = DateTime.Now;
            item.LastModificationDate = DateTime.Now;
            _sessionFactory.GetCurrentSession().SaveOrUpdate(item);

            var transaction = _sessionFactory.GetCurrentSession().Transaction;
            if (transaction != null || transaction.IsActive)
            {
                transaction.Commit();
                _sessionFactory.GetCurrentSession().BeginTransaction();
            }
            return true;
        }

        /// <summary>
        /// Update an existent entity of type T.
        /// </summary>
        /// <param name="item">the entity to update.</param>
        public void Update(T item)
        {

            if (item == null)
                throw new ArgumentNullException();
            //LogDbAction("Updating", item);

            item.LastModificationDate = DateTime.Now;
            _sessionFactory.GetCurrentSession().SaveOrUpdate(item);
        }

        /// <summary>
        /// Update an existent entity of type T.
        /// </summary>
        /// <param name="item">the entity to update.</param>
        public void UpdateAndCommit(T item)
        {

            if (item == null)
                throw new ArgumentNullException();

            item.LastModificationDate = DateTime.Now;
            _sessionFactory.GetCurrentSession().SaveOrUpdate(item);
            var transaction = _sessionFactory.GetCurrentSession().Transaction;
            if (transaction != null || transaction.IsActive)
            {
                transaction.Commit();
                _sessionFactory.GetCurrentSession().BeginTransaction();
            }
        }

        /// <summary>
        /// Delete an entity by its id.
        /// </summary>
        /// <param name="id">The id of the entity to delete.</param>
        public void Delete(T item)
        {

            //var item = Get(id);

            //if (item == null) return;

            //LogDbAction("Deleting", item);
            _sessionFactory.GetCurrentSession().Delete(item);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void DeleteAndCommit(T item)
        {

            //var item = Get(id);

            //if (item == null) return;

            //LogDbAction("Deleting", item);
            _sessionFactory.GetCurrentSession().Delete(item);
            var transaction = _sessionFactory.GetCurrentSession().Transaction;
            if (transaction != null || transaction.IsActive)
            {
                transaction.Commit();
                _sessionFactory.GetCurrentSession().BeginTransaction();
            }
        }

        /// <summary>
        /// Delete an entity by its id.
        /// </summary>
        /// <param name="id">The id of the entity to delete.</param>
        public void Delete(object id)
        {

            var item = Get(id);

            if (item == null) return;

            //LogDbAction("Deleting", item);
            _sessionFactory.GetCurrentSession().Delete(item);
        }

        /// <summary>
        /// Delete an entity by its id.
        /// </summary>
        /// <param name="id">The id of the entity to delete.</param>
        public void DeleteAndCommit(object id)
        {

            var item = Get(id);

            if (item == null) return;

            //LogDbAction("Deleting", item);
            _sessionFactory.GetCurrentSession().Delete(id);
            var transaction = _sessionFactory.GetCurrentSession().Transaction;
            if (transaction != null || transaction.IsActive)
            {
                transaction.Commit();
                _sessionFactory.GetCurrentSession().BeginTransaction();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {

            var criteria =
                _sessionFactory.GetCurrentSession().CreateCriteria(typeof(T)).SetProjection(Projections.RowCount());
            return (int)criteria.UniqueResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public int GetCount(ICriterion restrictions)
        {

            var criteria =
                _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .Add(restrictions)
                .SetProjection(Projections.RowCount());
            return (int)criteria.UniqueResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restrictions"></param>
        /// <param name="distinctPropertyName"></param>
        /// <returns></returns>
        public int GetDistinctCount(ICriterion restrictions, string distinctPropertyName)
        {

            var criteria =
                _sessionFactory
                .GetCurrentSession()
                .CreateCriteria(typeof(T))
                .Add(restrictions)
                .SetProjection(Projections.CountDistinct(distinctPropertyName));
            return (int)criteria.UniqueResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSkipAndTake(int skip, int take)
        {

            return take <= 0 ?
                    skip <= 0 ?
                        _sessionFactory
                        .GetCurrentSession()
                        .CreateCriteria(typeof(T))
                        .SetFirstResult(skip)
                        .Future<T>()
                    :
                        new List<T>()
                :
                    _sessionFactory
                    .GetCurrentSession()
                    .CreateCriteria(typeof(T))
                    .SetFirstResult(skip)
                    .SetMaxResults(take)
                    .Future<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSkipAndTake(int skip, int take, ICriterion restrictions)
        {

            return take <= 0
                ? skip <= 0
                    ? _sessionFactory
                        .GetCurrentSession().CreateCriteria(typeof(T))
                        .Add(restrictions)
                        .SetFirstResult(skip)
                        .Future<T>()
                    :
                        new List<T>()
                : _sessionFactory
                    .GetCurrentSession().CreateCriteria(typeof(T))
                    .Add(restrictions)
                    .SetFirstResult(skip)
                    .SetMaxResults(take)
                    .Future<T>();
        }

        public IEnumerable<Q> GetSkipAndTake<Q>(int skip, int take, IProjection projection)// where Q : class
        {

            return take <= 0
                ? skip <= 0
                    ? _sessionFactory
                        .GetCurrentSession()
                        .CreateCriteria(typeof(T))
                        .SetProjection(projection)
                        .SetFirstResult(skip)
                        .Future<Q>()
                    :
                        new List<Q>()
                : _sessionFactory
                    .GetCurrentSession()
                    .CreateCriteria(typeof(T))
                    .SetFirstResult(skip)
                    .SetMaxResults(take)
                    .SetProjection(projection)
                    .Future<Q>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="projection"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public IEnumerable<Q> GetSkipAndTake<Q>(int skip, int take, IProjection projection, ICriterion restrictions)
        //where Q : class
        {

            return take <= 0
                ? skip <= 0
                    ? _sessionFactory
                        .GetCurrentSession()
                        .CreateCriteria(typeof(T))
                        .Add(restrictions)
                        .SetProjection(projection)
                        .SetFirstResult(skip)
                        .Future<Q>()
                    : new List<Q>()
                : _sessionFactory
                    .GetCurrentSession()
                    .CreateCriteria(typeof(T))
                    .Add(restrictions)
                    .SetFirstResult(skip)
                    .SetMaxResults(take)
                    .SetProjection(projection)
                    .Future<Q>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSkipAndTakeOrderDesc(int skip, int take, string orderPropertyName, ICriterion restrictions)
        {

            return take <= 0 ?
                    skip <= 0 ?
                        _sessionFactory.GetCurrentSession().CreateCriteria(typeof(T))
                        .Add(restrictions)
                        .AddOrder(Order.Desc(orderPropertyName))
                        .SetFirstResult(skip)
                        .Future<T>()
                    :
                        new List<T>()
                :
                        _sessionFactory
                        .GetCurrentSession().CreateCriteria(typeof(T))
                        .Add(restrictions)
                        .AddOrder(Order.Desc(orderPropertyName))
                        .SetFirstResult(skip)
                        .SetMaxResults(take)
                        .Future<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSkipAndTakeOrderAsc(int skip, int take, string orderPropertyName, ICriterion restrictions)
        {

            return take <= 0 ?
                    skip <= 0 ?
                        _sessionFactory
                        .GetCurrentSession().CreateCriteria(typeof(T))
                        .Add(restrictions)
                        .AddOrder(Order.Asc(orderPropertyName))
                        .SetFirstResult(skip)
                        .Future<T>()
                    :
                        new List<T>()
                :
                        _sessionFactory
                        .GetCurrentSession().CreateCriteria(typeof(T))
                        .Add(restrictions)
                        .AddOrder(Order.Asc(orderPropertyName))
                        .SetFirstResult(skip)
                        .SetMaxResults(take)
                        .Future<T>();
        }

        public string GetSessionId()
        {

            SessionImpl sessionImpl = (SessionImpl)_sessionFactory.GetCurrentSession();
            var ide = sessionImpl.SessionId;
            return ide.ToString();
        }


        public ISession GetCurrentSession()
        {

            return _sessionFactory
                .GetCurrentSession();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSkipAndTakeOrderDesc(int skip, int take, string orderPropertyName)
        {

            return take <= 0 ?
                    skip <= 0 ?
                        _sessionFactory.GetCurrentSession().CreateCriteria(typeof(T))
                        .AddOrder(Order.Desc(orderPropertyName))
                        .SetFirstResult(skip)
                        .Future<T>()
                    :
                        new List<T>()
                :
                        _sessionFactory
                        .GetCurrentSession().CreateCriteria(typeof(T))
                        .AddOrder(Order.Desc(orderPropertyName))
                        .SetFirstResult(skip)
                        .SetMaxResults(take)
                        .Future<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSkipAndTakeOrderAsc(int skip, int take, string orderPropertyName)
        {

            return take <= 0 ?
                    skip <= 0 ?
                        _sessionFactory
                        .GetCurrentSession().CreateCriteria(typeof(T))
                        .AddOrder(Order.Asc(orderPropertyName))
                        .SetFirstResult(skip)
                        .Future<T>()
                    :
                        new List<T>()
                :
                        _sessionFactory
                        .GetCurrentSession().CreateCriteria(typeof(T))
                        .AddOrder(Order.Asc(orderPropertyName))
                        .SetFirstResult(skip)
                        .SetMaxResults(take)
                        .Future<T>();
        }

        #endregion Public Methods
    }
}