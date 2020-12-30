using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MonolithToMicroservices
{
    public interface IPostService
    {
        IList<Post> List(int threshold, int skip = 0, int take = 0);
    }

    public class DbPostService : IPostService
    {
        private readonly BloggingContext _db;

        public DbPostService(BloggingContext db)
        {
            _db = db;
        }

        public IList<Post> List(int threshold, int skip = 0, int take = 0)
        {
            var query = _db.Posts.Where(post => post.PostId > threshold);

            // And yes, you could abstract the next few lines into a QueryPager

            if (skip > 0)
                query = query.Skip(skip);

            if (take > 0)
                query = query.Take(take);

            return query.ToList();
        }
    }
}