using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System.Threading;

namespace MVP.App.Bot.Dialogs
{
    [Serializable]
    public class AppBaseDialog<T> : LuisDialog<T>
    {
        public async Task<bool> IsIntentValidAsync(string query)
        {
            var tasks = services.Select(service => service.QueryAsync(query, CancellationToken.None)).ToArray();
            var results = await Task.WhenAll(tasks);

            var bestResults = from result in results.Select((value, index) => new {value, index})
                let resultWinner = BestIntentFrom(result.value)
                where resultWinner != null
                select new LuisServiceResult(result.value, resultWinner, this.services[result.index]);

            var bestResult = this.BestResultFrom(bestResults);
            return bestResult != null && bestResult.BestIntent.Intent != "None";
        }
    }
}