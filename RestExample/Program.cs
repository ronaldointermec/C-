using System;
using System.Net.Http;
using System.Net.Http.Json;
//using System.Text.Json;
//using Newtonsoft.Json;
using System.Threading.Tasks;



namespace RestExample
{
    internal class Program
    {

        private static readonly HttpClient client = new HttpClient();
        public static async Task Main(string[] args)
        {
            await GetPostAsync();
        }

        public static async Task GetPostAsync()
        {
            try
            {
                string url = "http://localhost:3000/GWS/user";

                // Send GET request 
                Post post = await client.GetFromJsonAsync<Post>(url);

                // Check if the post is not null and print the details
                if (post != null)
                {
                    Console.WriteLine($"UserId: {post.UserId}");
                    Console.WriteLine($"Id: {post.Id}");
                    Console.WriteLine($"Title: {post.Title}");
                    Console.WriteLine($"Body: {post.Body}");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("No post found.");
                }

            
            }
            catch (HttpRequestException e)
            {

                Console.WriteLine($" Request erro: {e.Message}");

            }
        }
    }
}


public class Post
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}