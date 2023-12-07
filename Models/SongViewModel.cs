using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment_5.Models
{
    public class SongViewModel
    {
        public List<Song>? Songs { get; set; }
        public SelectList? Genres { get; set; }
        public SelectList? Performer { get; set; }
        public string? SongGenre { get; set; }
        public string? SongPerformer { get; set; }
    }
}
