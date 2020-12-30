using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;

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

    public class WebApiPostService : IPostService
    {
        HttpClient _httpClient = new HttpClient();

        public IList<Post> List(int threshold, int skip = 0, int take = 0)
        {
            // Yes, this should all be async, but if we're just dropping in, the previous implementation
            // wasn't async either...

            var result = _httpClient.GetAsync($"https://yourpostservice.url/posts?threshold={threshold}&skip={skip}&take={take}").Result;
            return JsonConvert.DeserializeObject<List<Post>>(result.Content.ReadAsStringAsync().Result);
        }
    }
}