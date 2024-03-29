﻿using Blog.Models;

namespace Blog.ViewModels.Posts;

public class ListPostsViewModel
{
    public int Id { get; set; } 
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Slug { get; set; }
    public Category Category { get; set; }
    public User Author { get; set; }
}