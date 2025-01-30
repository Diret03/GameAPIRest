using GameAPI.ConsumeAPI;
using GameAPI.Models;


namespace GameAPI.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var apiUrlGames = "https://game-api-rest.azurewebsites.net/api/Games";


            Crud<Game>.Create(apiUrlGames, new Game
            {
              
            });

        }
    }
}
