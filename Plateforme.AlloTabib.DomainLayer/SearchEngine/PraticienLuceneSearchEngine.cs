using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Mapping;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using Directory = Lucene.Net.Store.Directory;

namespace Plateforme.AlloTabib.DomainLayer.SearchEngine
{
    public class PraticienLuceneSearchEngine : ILuceneSearchEngine<PraticienToIndexModel>
    {
        private readonly IRepository<Praticien> _praticienRepository;

        public PraticienLuceneSearchEngine(IRepository<Praticien> praticienRepository)
        {
            _praticienRepository = praticienRepository;
        }

        public void Index(Directory directory)
        {

            IndexWriter.Unlock(directory);

            var provider = new LuceneDataProvider(directory, Lucene.Net.Util.Version.LUCENE_30);
            var praticiens = _praticienRepository.GetAll().Where(a => !a.IsIndexed).ToList();
            var itemsToIndex = new List<PraticienToIndexModel>();

            praticiens.ForEach(a =>
            {
                var item = PraticienWrapper.PraticienEntityToPraticienToIndexModel(a);

                itemsToIndex.Add(item);
            });

            var indexedIds = new List<string>();

            using (var session = provider.OpenSession<PraticienToIndexModel>())
            {
                foreach (var praticienToIndex in itemsToIndex)
                {
                    session.Add(praticienToIndex);
                    indexedIds.Add(praticienToIndex.Cin);
                    var indexedpraticien =
                        praticiens.First(
                            a =>
                            a.Cin.ToString().Equals(praticienToIndex.Cin, StringComparison.InvariantCultureIgnoreCase));
                    indexedpraticien.IsIndexed = true;
                    _praticienRepository.Update(indexedpraticien);
                }
                session.Commit();
            }
        }

        public List<PraticienToIndexModel> Search(string q)
        {
            q = Uri.UnescapeDataString(q);

            var path = ConfigurationManager.AppSettings["LuceneDirectory"];

            //get path
            var chemin = HttpContext.Current.Server.MapPath("~");
            path = Path.Combine(chemin, path);
            var result = new List<PraticienToIndexModel>();

            var analyzer = new KeywordAnalyzer();

            var indexDirInfo = new DirectoryInfo(path);

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            Directory directory = FSDirectory.Open(indexDirInfo,
                                                                    new SimpleFSLockFactory(indexDirInfo));
            Index(directory);
            var searcher = new IndexSearcher(directory);

            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "All", analyzer)
            {
                AllowLeadingWildcard = true,
                LowercaseExpandedTerms = false
            };
            var queryString = string.Format("{0} OR *{0}* OR *{0} OR {0}*", q);

            var query = parser.Parse(queryString);
            var maxResult = 100;
            var hits = searcher.Search(query, maxResult);

            var mapper = new ReflectionDocumentMapper<PraticienToIndexModel>(Lucene.Net.Util.Version.LUCENE_30);

            maxResult = (hits.TotalHits < maxResult) ? hits.TotalHits : maxResult;

            for (var i = 0; i < maxResult; i++)
            {
                var praticien = new PraticienToIndexModel();

                var doc = searcher.Doc(hits.ScoreDocs[i].Doc);

                mapper.ToObject(doc, null, praticien);
                if (!result.Any(r => r.Cin.Equals(praticien.Cin)))
                    result.Add(praticien);
            }

            searcher.Dispose();
            return result;
        }

        public void DeleteLuceneIndexRecord(string cin)
        {
            var path = ConfigurationManager.AppSettings["LuceneDirectory"];
            var indexDirInfo = new DirectoryInfo(path);

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            Directory directory = FSDirectory.Open(indexDirInfo,
                                                                    new SimpleFSLockFactory(indexDirInfo));
            IndexWriter.Unlock(directory);
            // init lucene
            var analyzer = new KeywordAnalyzer();
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Cin", cin));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }
    }
}
