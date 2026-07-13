using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //tároló típis váltásához kell

namespace Dometrain.EFCore.API.Models;

[Table("Pictures")]
public class Movie
{
    
    public int Id { get; set; }
    public string? Title { get; set; } 
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
}

public class MovieTitle
{
    public int Id { get; set; }
    public string? Title { get; set; }
}