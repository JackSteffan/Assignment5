using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment_5.Models
{
    public class SongGenreView
    {
        public List<Song>? Songs { get; set; }
        public SelectList? Genres { get; set; }
        public string? SongGenre { get; set; }
        public string? SearchString { get; set; }
    }
}
