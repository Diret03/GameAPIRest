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
            var apiUrlDevelopers = "https://game-api-rest.azurewebsites.net/api/Developers";
            var apiUrlPlatforms = "https://game-api-rest.azurewebsites.net/api/Platforms";

            //var newGame = new CreateGameDTO
            //{
            //    Name = "Zelda BOTW",
            //    Description = "The Legend of Zelda: Breath of the Wild is a 2017 action-adventure game developed and published by Nintendo for the Nintendo Switch and Wii U.",
            //    ReleaseDate = new DateTime(2017, 3, 17),
            //    GenreId = 6,
            //    DeveloperId = 4,
            //    PlatformIds = new List<int> { 5 }
            //};

            //try
            //{
            //    var createdGame = Crud<CreateGameDTO>.Create(apiUrlGames, newGame);
            //    if (createdGame != null)
            //    {
            //        Console.WriteLine($"Game created successfully: {createdGame.Name}");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Game creation returned null");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating game: {ex.Message}");
            //}

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


            // Test Read_All for Games
            //try
            //{
            //    Console.WriteLine("\n=== Games ===");
            //    var games = Crud<GameResponseDTO>.Read_All(apiUrlGames);
            //    foreach (var game in games)
            //    {
            //        Console.WriteLine($"ID: {game.Id}");
            //        Console.WriteLine($"Name: {game.Name}");
            //        Console.WriteLine($"Genre: {game.Genre.Name}");
            //        Console.WriteLine($"Developer: {game.Developer.Name}");
            //        Console.WriteLine($"Platforms: {string.Join(", ", game.Platforms.Select(p => p.Name))}");
            //        Console.WriteLine();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error reading games: {ex.Message}");
            //}

            //// Test Read_All for Genres
            //try
            //{
            //    Console.WriteLine("\n=== Genres ===");
            //    var genres = Crud<GenreDTO>.Read_All(apiUrlGenres);
            //    foreach (var genre in genres)
            //    {
            //        Console.WriteLine($"ID: {genre.Id}");
            //        Console.WriteLine($"Name: {genre.Name}");
            //        Console.WriteLine();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error reading genres: {ex.Message}");
            //}

            //// Test Read_All for Developers
            //try
            //{
            //    Console.WriteLine("\n=== Developers ===");
            //    var developers = Crud<DeveloperResponseDTO>.Read_All(apiUrlDevelopers);
            //    foreach (var developer in developers)
            //    {
            //        Console.WriteLine($"ID: {developer.Id}");
            //        Console.WriteLine($"Name: {developer.Name}");
            //        Console.WriteLine($"Location: {developer.Location}");
            //        Console.WriteLine($"Number of games: {developer.Games.Count}");
            //        Console.WriteLine();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error reading developers: {ex.Message}");
            //}

            //// Test Read_All for Platforms
            //try
            //{
            //    Console.WriteLine("\n=== Platforms ===");
            //    var platforms = Crud<PlatformResponseDTO>.Read_All(apiUrlPlatforms);
            //    foreach (var platform in platforms)
            //    {
            //        Console.WriteLine($"ID: {platform.Id}");
            //        Console.WriteLine($"Name: {platform.Name}");
            //        Console.WriteLine($"Number of games: {platform.Games.Count}");
            //        Console.WriteLine();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error reading platforms: {ex.Message}");
            //}



            //Test Read_ById for Game
            //try
            //    {
            //        Console.WriteLine("\n=== Game Details ===");
            //        var game = Crud<GameResponseDTO>.Read_ById(apiUrlGames, 5);
            //        Console.WriteLine($"ID: {game.Id}");
            //        Console.WriteLine($"Name: {game.Name}");
            //        Console.WriteLine($"Description: {game.Description}");
            //        Console.WriteLine($"Release Date: {game.ReleaseDate:d}");
            //        Console.WriteLine($"Genre: {game.Genre.Name}");
            //        Console.WriteLine($"Developer: {game.Developer.Name}");
            //        Console.WriteLine($"Platforms: {string.Join(", ", game.Platforms.Select(p => p.Name))}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error reading game: {ex.Message}");
            //    }

            // Test Read_ById for Genre 
            try
            {
                Console.WriteLine("\n=== Genre Details ===");
                var genre = Crud<GenreResponseDTO>.Read_ById(apiUrlGenres, 2);
                Console.WriteLine($"ID: {genre.Id}");
                Console.WriteLine($"Name: {genre.Name}");

                // Debug games collection
                Console.WriteLine("\nGames in this genre:");
                if (genre.Games != null && genre.Games.Any())
                {
                    foreach (var game in genre.Games)
                    {
                        Console.WriteLine($"- {game.Name} (Developer: {game.Developer?.Name}, Release Date: {game.ReleaseDate:d})");
                        Console.WriteLine($"  Platforms: {string.Join(", ", game.Platforms?.Select(p => p.Name) ?? new string[] { })}");
                    }
                }
                else
                {
                    Console.WriteLine("No games found or Games collection is null");
                    Console.WriteLine($"Games is null: {genre.Games == null}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading genre: {ex.Message}");
            }

            //// Test Read_ById for Developer 
            //try
            //{
            //    Console.WriteLine("\n=== Developer Details ===");
            //    var developer = Crud<DeveloperResponseDTO>.Read_ById(apiUrlDevelopers, 4);
            //    Console.WriteLine($"ID: {developer.Id}");
            //    Console.WriteLine($"Name: {developer.Name}");
            //    Console.WriteLine($"Location: {developer.Location}");
            //    Console.WriteLine("\nGames by this developer:");
            //    foreach (var game in developer.Games)
            //    {
            //        Console.WriteLine($"- {game.Name} ({game.Genre.Name})");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error reading developer: {ex.Message}");
            //}

            //// Test Read_ById for Platform
            //try
            //{
            //    Console.WriteLine("\n=== Platform Details ===");
            //    var platform = Crud<PlatformResponseDTO>.Read_ById(apiUrlPlatforms, 1);
            //    Console.WriteLine($"ID: {platform.Id}");
            //    Console.WriteLine($"Name: {platform.Name}");
            //    Console.WriteLine("\nGames on this platform:");
            //    foreach (var game in platform.Games)
            //    {
            //        Console.WriteLine($"- {game.Name} by {game.Developer.Name}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error reading platform: {ex.Message}");
            //}




            //// Test Create Game
            //try
            //{
            //    Console.WriteLine("\n=== Creating Game ===");
            //    var newGame = new CreateGameDTO
            //    {
            //        Name = "The Legend of Zelda: Ocarina of Time",
            //        Description = "The Legend of Zelda: Ocarina of Time is a 1998 action-adventure game by Nintendo for the Nintendo 64",
            //        ReleaseDate = new DateTime(1988, 11, 21),
            //        GenreId = 6,  
            //        DeveloperId = 4,  
            //        PlatformIds = new List<int> { 8 }  
            //    };

            //    var createdGame = Crud<CreateGameDTO>.Create(apiUrlGames, newGame);
            //    Console.WriteLine($"Game created successfully: {createdGame.Name}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating game: {ex.Message}");
            //}

            //// Test Create Genre
            //try
            //{
            //    Console.WriteLine("\n=== Creating Genre ===");
            //    var newGenre = new CreateGenreDTO
            //    {
            //        Name = "Strategy"
            //    };

            //    var createdGenre = Crud<CreateGenreDTO>.Create(apiUrlGenres, newGenre);
            //    Console.WriteLine($"Genre created successfully: {createdGenre.Name}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating genre: {ex.Message}");
            //}

            //// Test Create Developer
            //try
            //{
            //    Console.WriteLine("\n=== Creating Developer ===");
            //    var newDeveloper = new CreateDeveloperDTO
            //    {
            //        Name = "Rockstar Games",
            //        Location = "USA"
            //    };

            //    var createdDeveloper = Crud<CreateDeveloperDTO>.Create(apiUrlDevelopers, newDeveloper);
            //    Console.WriteLine($"Developer created successfully: {createdDeveloper.Name}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating developer: {ex.Message}");
            //}

            //// Test Create Platform
            //try
            //{
            //    Console.WriteLine("\n=== Creating Platform ===");
            //    var newPlatform = new InputPlatformDTO
            //    {
            //        Name = "PlayStation 1"
            //    };

            //    var createdPlatform = Crud<InputPlatformDTO>.Create(apiUrlPlatforms, newPlatform);
            //    Console.WriteLine($"Platform created successfully: {createdPlatform.Name}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating platform: {ex.Message}");
            //}

            // Test Update Game
            //try
            //{
            //    Console.WriteLine("\n=== Updating Game ===");
            //    var updateGame = new UpdateGameDTO
            //    {
            //        Name = "Mario Kart 8 Deluxe"
            //    };

            //    bool updateSuccess = Crud<UpdateGameDTO>.Update(apiUrlGames, 8, updateGame);
            //    Console.WriteLine($"Game update {(updateSuccess ? "successful" : "failed")}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error updating game: {ex.Message}");
            //}

            //// Test Update Genre
            //try
            //{
            //    Console.WriteLine("\n=== Updating Genre ===");
            //    var updateGenre = new UpdateGenreDTO
            //    {
            //        Name = "Shooter"
            //    };

            //    bool updateSuccess = Crud<UpdateGenreDTO>.Update(apiUrlGenres, 3, updateGenre);
            //    Console.WriteLine($"Genre update {(updateSuccess ? "successful" : "failed")}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error updating genre: {ex.Message}");
            //}

            //// Test Update Developer
            //try
            //{
            //    Console.WriteLine("\n=== Updating Developer ===");
            //    var updateDeveloper = new UpdateDeveloperDTO
            //    {
            //        Name = "Capcom"
            //    };

            //    bool updateSuccess = Crud<UpdateDeveloperDTO>.Update(apiUrlDevelopers, 7, updateDeveloper);
            //    Console.WriteLine($"Developer update {(updateSuccess ? "successful" : "failed")}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error updating developer: {ex.Message}");
            //}

            //// Test Update Platform
            //try
            //{
            //    Console.WriteLine("\n=== Updating Platform ===");
            //    var updatePlatform = new InputPlatformDTO
            //    {
            //        Name = "PlayStation 5 Pro"
            //    };

            //    bool updateSuccess = Crud<InputPlatformDTO>.Update(apiUrlPlatforms, 2, updatePlatform);
            //    Console.WriteLine($"Platform update {(updateSuccess ? "successful" : "failed")}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error updating platform: {ex.Message}");
            //}

            //// Optional: Verify updates by reading the updated records
            //try
            //{
            //    Console.WriteLine("\n=== Verifying Updates ===");

            //    var updatedGame = Crud<GameResponseDTO>.Read_ById(apiUrlGames, 1);
            //    Console.WriteLine($"Updated Game Name: {updatedGame.Name}");

            //    var updatedGenre = Crud<GenreDTO>.Read_ById(apiUrlGenres, 1);
            //    Console.WriteLine($"Updated Genre Name: {updatedGenre.Name}");

            //    var updatedDeveloper = Crud<DeveloperResponseDTO>.Read_ById(apiUrlDevelopers, 1);
            //    Console.WriteLine($"Updated Developer Name: {updatedDeveloper.Name}");

            //    var updatedPlatform = Crud<PlatformResponseDTO>.Read_ById(apiUrlPlatforms, 1);
            //    Console.WriteLine($"Updated Platform Name: {updatedPlatform.Name}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error verifying updates: {ex.Message}");
            //}

            // Test Delete with valid ID
            //Console.WriteLine("=== Testing Delete with Valid ID ===");
            //try
            //{
            //    int idToDelete = 9; // Use an ID that exists in your database
            //    bool deleteResult = Crud<GenreDTO>.Delete(apiUrlGenres, idToDelete);

            //    if (deleteResult)
            //    {
            //        Console.WriteLine($"Genre with ID {idToDelete} was successfully deleted");

            //        // Verify deletion
            //        try
            //        {
            //            var deletedGenre = Crud<GenreDTO>.Read_ById(apiUrlGenres, idToDelete);
            //            if (deletedGenre == null)
            //            {
            //                Console.WriteLine("Verified: Genre no longer exists");
            //            }
            //            else
            //            {
            //                Console.WriteLine("Warning: Genre still exists after deletion");
            //            }
            //        }
            //        catch
            //        {
            //            Console.WriteLine("Verified: Genre no longer exists (404 received)");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Failed to delete genre with ID {idToDelete}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error during deletion: {ex.Message}");
            //}

        }
    }
}
