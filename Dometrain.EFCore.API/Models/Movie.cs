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
    public AgeRating AgeRating { get; set; }

    //mi történik a migrációval ha hozzáadunk egy paramétert:
    public decimal InternetRating { get; set; }

    public Person Director { get; set; }
    public ICollection<Person> Actors { get; set; }


    //kapcsolat:
    public required Genre Genre { get; set; }
    //public int GenreId { get; set; } //az EF tudja hogy ez a kettő összetartozik
    //public int MainGenreId { get; set; } //így már nem tudja összekötni, a név miatt
    public required string MainGenreName { get; set;  } //a Genre.Name -> alternate key lett

    //Actor many-many relationship:
    //public List<Actor> Actors { get; set; } //nekem már van egy az előző leckékből

}

public enum AgeRating
{
    All = 0,
    ElementerySchool = 6,
    HighScool = 12,
    Adolescent = 16,
    Adult = 18
}

public class MovieTitle
{
    public int Id { get; set; }
    public string? Title { get; set; }
}

//A movie osztály abstract lett a származtatás miatt, ezek a belőle leszármazott osztályok:
/*
public class CinemaMovie : Movie
{
    public required decimal GrossRevenue { get; set;  }
}

public class TelevisionMovie : Movie 
{
    public required string ChannelFirstAiredOn { get; set; }
}
*/