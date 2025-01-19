using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace StartGGHelper
{
    public class GetResultsCSV
    {
        private readonly ILogger<GetResultsCSV> _logger;
        private readonly StartGGClient _startGGClient;

        public GetResultsCSV(ILogger<GetResultsCSV> logger, StartGGClient graphqlClient)
        {
            _logger = logger;
            _startGGClient = graphqlClient;
        }

        [Function("GetResultsCSV")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, string urlSlug)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            if(String.IsNullOrEmpty(urlSlug))
            {
                return new NotFoundResult();
            }

            var startggSets = await _startGGClient.GetEntrantsSetsForEvent(urlSlug);

            var allSets = startggSets
                .DistinctBy(d => d.id)
                .OrderBy(s => s.completedAt);

            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("time completed,winner name,winner score,loser name,loser score");
            foreach (var set in allSets)
            {
                var player1Name = set.slots.First().entrant.name;
                var player2Name = set.slots.Last().entrant.name;

                var scoreResults = set.displayScore
                    .Replace(player1Name, "")
                    .Replace(player2Name, "")
                    .Split("-").Select(s => s.Trim()).ToArray();
                    
                var player1Score = int.Parse(scoreResults[0].Last().ToString());
                var player2Score = int.Parse(scoreResults[1].Last().ToString());

                csvBuilder.Append(set.CompletedAtDateTime);
                if(player1Score > player2Score)
                    csvBuilder.Append($",{player1Name},{player1Score},{player2Name},{player2Score}");
                else
                    csvBuilder.Append($",{player2Name},{player2Score},{player1Name},{player1Score}");

                csvBuilder.AppendLine();
            }

            var strResult = csvBuilder.ToString();

            byte[] filebytes = Encoding.UTF8.GetBytes(strResult);

            return new FileContentResult(filebytes, "application/octet-stream") {
                FileDownloadName = "Export.csv"
            };
        }
    }
}
