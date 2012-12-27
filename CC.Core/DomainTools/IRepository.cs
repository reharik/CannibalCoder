using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CC.Core.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace CC.Core.DomainTools
{
    public interface IRepository
    {
        ISession CurrentSession();

        void Save<ENTITY>(ENTITY entity)
            where ENTITY : IPersistableObject;

        ENTITY Load<ENTITY>(int id)
            where ENTITY : IPersistableObject;

        IQueryable<ENTITY> Query<ENTITY>()
            where ENTITY : IPersistableObject;

        IQueryable<T> Query<T>(Expression<Func<T, bool>> where);
        IEnumerable<ENTITY> ExecuteQueryOver<ENTITY>(QueryOver<ENTITY> query) where ENTITY : class, IPersistableObject;

        T FindBy<T>(Expression<Func<T, bool>> where);

        T Find<T>(int id) where T : IPersistableObject;

        IEnumerable<T> FindAll<T>() where T : IPersistableObject;

        void Delete<ENTITY>(ENTITY entity) where ENTITY : IPersistableObject;

        void HardDelete(object target);


        void Commit();
        void Rollback();
        void Initialize();
        IList<ENTITY> ExecuteCriteria<ENTITY>(DetachedCriteria criteria) where ENTITY : IPersistableObject;

        IList<T> GetNamedQuery<T>(string sprocName);
        void DisableFilter(string FilterName);
        void EnableFilter(string FilterName, string field, object value);
        IUnitOfWork UnitOfWork { get; set; }
        IFutureValue<ENTITY> CreateQueryOverFuture<ENTITY>(QueryOver<ENTITY> query) where ENTITY : class, IPersistableObject;
        IEnumerable<ENTITY> ExecuteSproc<ENTITY>();
    }
}
