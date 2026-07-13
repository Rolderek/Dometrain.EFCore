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

    //kapcsolat:
    public Genre? Genre { get; set; }
    public int GenreId { get; set; } //az EF tudja hogy ez a kettő összetartozik
    //public int MainGenreId { get; set; } //így már nem tudja összekötni, a név miatt
}

public class MovieTitle
{
    public int Id { get; set; }
    public string? Title { get; set; }
}