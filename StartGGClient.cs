using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using Models;

public class StartGGClient 
{

  private readonly GraphQLHttpClient _client; 

  public StartGGClient(IConfiguration config)
  {
      _client = new GraphQLHttpClient(
            "https://api.start.gg/gql/alpha", 
            new NewtonsoftJsonSerializer());

      var authToken = config["AuthToken"];
      _client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
  }

    public async Task<IEnumerable<Set>> GetEntrantsSetsForEvent(string eventName)
    {
        List<Set> allSets = new List<Set>();
        PaginatedSets sets = null;
        int pageNum = 0;
        int perPage=100;

        do {
          pageNum++;

          var request = new GraphQLRequest
          {
              Query = GetEventSetsQuery,
              Variables = new {
                  slug = eventName,
                  pageNum = pageNum,
                  perPage = perPage
              }
          };
    
          var results = await _client.SendQueryAsync<BaseEvent>(request);
          sets = results?.Data?.Event?.sets;

          if(sets != null)
            allSets.AddRange(sets.nodes.Where(n => !n.displayScore.Equals("DQ", StringComparison.InvariantCultureIgnoreCase)));

        } while(sets != null && sets.pageInfo.totalPages != sets.pageInfo.page);
        
        return allSets;
    }

        const string GetEventSetsQuery = """
query getEventSets($slug: String, $perPage: Int, $pageNum: Int) {
  event(slug: $slug) {
    id
    name
    sets(page: $pageNum, perPage:$perPage, sortType: RECENT, filters:{
      
    }) {
        nodes {
          winnerId
          id
          fullRoundText
          displayScore
          completedAt
          winnerId
          slots(includeByes: false) {
            entrant {
              name
              id
            }
          }
        }
        pageInfo {
          total
          totalPages
          page
          perPage
        }
  		}
	}
}
""";
}