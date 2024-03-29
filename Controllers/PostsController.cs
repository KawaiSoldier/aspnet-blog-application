using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnet_blog_application.Models;
using System.Reflection.Metadata.Ecma335;
using aspnet_blog_application.Models.ViewModels;
using Microsoft.Data.Sqlite;


namespace aspnet_blog_application.Controllers;

public class PostsController : Controller
{
    private readonly ILogger<PostsController> _logger;


    private readonly IConfiguration _configuration;
    public PostsController(ILogger<PostsController> logger , IConfiguration  configuration)
    {
        _logger = logger;

        _configuration = configuration;
    
    }

    public IActionResult Index()
    {
        var postListViewModel = GetAllPosts();

        return View(postListViewModel);
    }  

    internal PostViewModel GetAllPosts()
    {
        List<PostModel> postList = new();
        using (SqliteConnection connection = new SqliteConnection())
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM post"; 
             
                using (var reader = command.ExecuteReader())
                {
                if (reader.HasRows)
                    {
                    while(reader.Read())
                    {
                     postList.Add(
                        new PostModel {
                          Id = reader.GetInt32(0),
                          Title = reader.GetString(1),
                          Body = reader.GetString(2),
                          CreatedAt = reader.GetDateTime(3),
                          UpdatedAt = reader.GetDateTime(4)
                                     }
                                 );
                    }
                    } 
                    else  {
                        return new PostViewModel { PostList = postList};
                    }
                }
            }
        }
     return new PostViewModel
    {
        PostList = postList
    };
 }
    
}
