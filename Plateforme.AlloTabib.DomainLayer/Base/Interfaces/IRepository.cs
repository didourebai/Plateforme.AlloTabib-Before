using System.Collections.Generic;
using NHibernate.Criterion;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Base.Interfaces
{
    public interface IRepository<T> where T : BasicEntity
    {
        /// <summary>
        /// Get an object T by its id
        /// </summary>
        /// <param name="id">The object id</param>
        /// <returns>Object T</returns>
        T Get(object id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        T Get(object id, ProjectionList projections);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="id"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        Q Get<Q>(object id, IProjection projection);// where Q : class;

        /// <summary>
        /// Get a list of objects of type T.
        /// </summary>
        /// <returns>List of objects</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Order order);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(ICriterion restrictions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Order order, ICriterion restrictions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projections"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(ProjectionList projections);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="projections"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Order order, ProjectionList projections);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restrictions"></param>
        /// <param name="projections"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(ICriterion restrictions, ProjectionList projections);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="restrictions"></param>
        /// <param name="projections"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Order order, ICriterion restrictions, ProjectionList projections);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="projection"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<E> GetAll<E>(IProjection projection, ICriterion restrictions);// where E : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="projection"></param>
        /// <returns></returns>
        IEnumerable<E> GetAll<E>(IProjection projection);// where E : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="projection"></param>
        /// <returns></returns>
        IEnumerable<I> GetAllForStruct<I>(IProjection projection); //where I : struct;


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="projection"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<I> GetAllForStruct<I>(IProjection projection, ICriterion restrictions);// where I : struct;



        /// <summary>
        /// Add new object T.
        /// </summary>
        /// <param name="item">The object to add</param>
        /// <returns>true or false</returns>
        bool Add(T item);

        bool AddAndCommit(T item);

        /// <summary>
        /// Update an object T.
        /// </summary>
        /// <param name="item">The object to update</param>
        void Update(T item);

        void UpdateAndCommit(T item);

        /// <summary>
        /// Delete an object by its id.
        /// </summary>
        /// <param name="id">The object id.</param>
        void Delete(object id);

        /// <summary>
        /// Delete an object.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        void Delete(T item);

        /// <summary>
        /// Delete an object by its id and commits the transaction.
        /// </summary>
        /// <param name="id">The object id.</param>
        void DeleteAndCommit(object id);


        /// <summary>
        /// Deletes and object and commits the transaction
        /// </summary>
        /// <param name="item">Item to delete</param>
        void DeleteAndCommit(T item);

        /// <summary>
        /// Count the number of entries.
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// Count the number of entries for a specific criteria.
        /// </summary>
        /// <param name="restrictions">The criteria (Restriction)</param>
        /// <returns></returns>
        int GetCount(ICriterion restrictions);

        /// <summary>
        /// Count the number of entries for a specific criteria.
        /// </summary>
        /// <param name="restrictions"></param>
        /// <param name="distinctPropertyName"></param>
        /// <returns></returns>
        int GetDistinctCount(ICriterion restrictions, string distinctPropertyName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<T> GetSkipAndTake(int skip, int take);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<T> GetSkipAndTake(int skip, int take, ICriterion restrictions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        IEnumerable<Q> GetSkipAndTake<Q>(int skip, int take, IProjection projection);// where Q : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="projection"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<Q> GetSkipAndTake<Q>(int skip, int take, IProjection projection, ICriterion restrictions);// where Q : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <returns></returns>
        IEnumerable<T> GetSkipAndTakeOrderDesc(int skip, int take, string orderPropertyName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <returns></returns>
        IEnumerable<T> GetSkipAndTakeOrderAsc(int skip, int take, string orderPropertyName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<T> GetSkipAndTakeOrderDesc(int skip, int take, string orderPropertyName, ICriterion restrictions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="orderPropertyName"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        IEnumerable<T> GetSkipAndTakeOrderAsc(int skip, int take, string orderPropertyName, ICriterion restrictions);

   
    }
}
