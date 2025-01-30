using GameAPI.ConsumeAPI;
using GameAPI.Models;
using GameAPI.Models.DTO;


namespace GameAPI.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var apiUrlGames = "https://game-api-rest.azurewebsites.net/api/Games";
            var apiUrlGenres = "https://game-api-rest.azurewebsites.net/api/Genres";

            var newGame = new CreateGameDTO
            {
                Name = "Zelda BOTW",
                Description = "The Legend of Zelda: Breath of the Wild is a 2017 action-adventure game developed and published by Nintendo for the Nintendo Switch and Wii U.",
                ReleaseDate = new DateTime(2017, 3, 17),
                GenreId = 6,
                DeveloperId = 4,
                PlatformIds = new List<int> { 5 }
            };

            try
            {
                var createdGame = Crud<CreateGameDTO>.Create(apiUrlGames, newGame);
                if (createdGame != null)
                {
                    Console.WriteLine($"Game created successfully: {createdGame.Name}");
                }
                else
                {
                    Console.WriteLine("Game creation returned null");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating game: {ex.Message}");
            }

            //try
            //{
            //    var createdGame = Crud<CreateGameDTO, GameResponseDTO>.Create(apiUrlGames, newGame);
            //    if (createdGame != null)
            //    {
            //        Console.WriteLine($"Game created successfully: {createdGame.Name}");
            //        Console.WriteLine($"Genre: {createdGame.Genre.Name}");
            //        Console.WriteLine($"Developer: {createdGame.Developer.Name}");
            //        Console.WriteLine($"Platforms: {string.Join(", ", createdGame.Platforms.Select(p => p.Name))}");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Game creation returned null");
            //    }
            //}
            //catch (HttpRequestException ex)
            //{
            //    Console.WriteLine($"API Error: {ex.Message}");

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating game: {ex.Message}");
            //}

            //var newGenre = new CreateGenreDTO
            //{
            //    Name = "Adventure"
            //};

            //try
            //{
            //    var createdGenre = Crud<CreateGenreDTO>.Create(apiUrlGenres, newGenre);
            //    Console.WriteLine($"Genre created successfully: {createdGenre.Name}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating genre: {ex.Message}");
            //}

        }
    }
}
