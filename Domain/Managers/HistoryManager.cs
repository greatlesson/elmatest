using Domain.Models;
using DomainModel.Repositories;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Managers
{
    public class HistoryManager : IHistoryManager
    {
        public void Add(Models.History item)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(item);
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<Models.History> List()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(History));

                //criteria.Add(Restrictions.Ge("X", 5));

                var docs = criteria.List<History>();

                return docs;
            }
        }

        public IEnumerable<History> List(string search)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(History));

                if (!string.IsNullOrWhiteSpace(search))
                {
                    criteria.Add(Restrictions.Like("Operation", search, MatchMode.Anywhere));
                }
                var docs = criteria.List<History>();

                return docs;
            }
        }
    }
}
