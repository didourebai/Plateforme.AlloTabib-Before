using System.Collections.Generic;
using Lucene.Net.Store;

namespace Plateforme.AlloTabib.DomainLayer.SearchEngine
{
    public interface ILuceneSearchEngine<T>
    {
        void Index(Directory directory);

        List<T> Search(string query);

        void DeleteLuceneIndexRecord(string id);

    }
}