using System;
using EFTutorial.Models;
using System.Linq;
using Castle.Core.Internal;

namespace EFTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            bool blogInput = false;
            while (true)
            {
                Console.WriteLine("Enter your selection:");
                Console.WriteLine("1) Display all blogs");
                Console.WriteLine("2) Add Blog");
                Console.WriteLine("3) Display Post");
                Console.WriteLine("4) Add Posts");
                Console.WriteLine("Enter q to quit");

                var selectionInput = Console.ReadLine();

                if (selectionInput == "1")
                {
                    // 1. Read blogs from database
                    using (var context = new BlogContext())
                    {
                        var blogs = context.Blogs.ToList();
                        if (blogs.Count == 0)
                        {
                            Console.WriteLine($"\n{blogs.Count} Blogs returned\n");
                        }
                        else
                        {
                            Console.WriteLine($"\n{blogs.Count} Blogs returned");
                            Console.WriteLine("Id) Blog Name");
                            foreach (var myBlog in context.Blogs)
                            {
                                Console.WriteLine($"{myBlog.BlogId}, {myBlog.Name}");
                            }
                            Console.WriteLine();
                        }

                    }
                }
                else if (selectionInput == "2")
                {
                    blogInput = true;
                    // 2. Add Blog to database
                    while (blogInput)
                    {
                        Console.Write("Enter a name for a new Blog: ");
                        var blogName = Console.ReadLine();
                        if (blogName != "")
                        {
                            var blog = new Blog()
                            {
                                Name = blogName
                            };

                            using (var context = new BlogContext())
                            {
                                context.Blogs.Add(blog);
                                context.SaveChanges();
                            }
                            blogInput = false;

                        }
                        else
                        {
                            Console.WriteLine("|ERROR|Blog name cannot be null");
                        }
                    }

                }
                else if (selectionInput == "3")
                {
                    //3. Read post from blog
                    using (var context = new BlogContext())
                    {
                        Console.WriteLine("Select the blog whose posts you want to view");
                        foreach (var myBlog in context.Blogs)
                        {
                            Console.WriteLine($"{myBlog.BlogId}) {myBlog.Name}");
                        }
                        var blogId = Console.ReadLine();
                        if (int.TryParse(blogId, out int id))
                        {
                            var selectedBlog = context.Blogs.FirstOrDefault(blog => blog.BlogId == id);

                            if (selectedBlog != null)
                            {
                                Console.WriteLine($"Blog with ID {id} exists in the database.");

                                Console.WriteLine("The posts for the selected blog are:");
                                foreach (var post in selectedBlog.Posts)
                                {
                                    Console.WriteLine($"{post.PostId}) {post.Title}");
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine($"Blog with ID {id} does not exist in the database.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }

                    }
                }
                else if (selectionInput == "4")
                {
                    // 4. Add post to blog
                    using (var context = new BlogContext())
                    {
                        Console.WriteLine("Select the blog you would like to post to:");
                        foreach (var myBlog in context.Blogs)
                        {
                            Console.WriteLine($"{myBlog.BlogId}) {myBlog.Name}");
                        }

                        var blogId = Console.ReadLine();
                        if (int.TryParse(blogId, out int id))
                        {
                            var selectedBlog = context.Blogs.FirstOrDefault(blog => blog.BlogId == id);

                            if (selectedBlog != null)
                            {
                                Console.WriteLine($"Blog with ID {id} exists in the database.");

                                Console.WriteLine("The posts for the selected blog are:");
                                foreach (var post in selectedBlog.Posts)
                                {
                                    Console.WriteLine(post.Title);
                                }

                                Console.WriteLine("Enter a new post title:");
                                var postTitle = Console.ReadLine();

                                var newPost = new Post()
                                {
                                    Title = postTitle,
                                    BlogId = selectedBlog.BlogId
                                };

                                context.Posts.Add(newPost);
                                context.SaveChanges();
                                Console.WriteLine("New post added to the selected blog.\n");

                            }
                            else
                            {
                                Console.WriteLine($"Blog with ID {id} does not exist in the database.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid integer for the Blog ID.");
                        }
                    }
                }
                else if (selectionInput.ToLower() == "q")
                {
                    Console.WriteLine("Closing Program...");
                    break;
                }
                else
                {
                    Console.WriteLine("That is not a valid input, try again");
                }
            }
        }
    }
}
