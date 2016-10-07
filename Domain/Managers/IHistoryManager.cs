using Domain.Models;
using System.Collections.Generic;

namespace Domain.Managers
{
    public interface IHistoryManager
    {
        void Add(History item);
        IEnumerable<History> List();
        IEnumerable<History> List(string search);
    }
}
