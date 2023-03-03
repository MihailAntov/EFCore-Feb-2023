namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportSongsAboveDuration(context, 4)); 
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    Name = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Price = a.Price,
                    Songs = a.Songs.Select(s => new
                    {
                        s.Name,
                        s.Price,
                        SongWriterName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.Name)
                    .ThenBy(s => s.SongWriterName)
                    .ToList()
                }).ToList();

            StringBuilder sb = new StringBuilder();
            foreach(var album in albums.OrderByDescending(a=>a.Price))
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");
                int i = 1;
                foreach(var song in album.Songs)
                {
                    sb.AppendLine($"---#{i}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.SongWriterName}");

                    i++;
                }
                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }
            
            return sb.ToString().TrimEnd();
                
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(s => s.Duration.Seconds + 60 * s.Duration.Minutes > duration)
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                    .Select(sp => new
                    {
                        PerformerFullName = $"{sp.Performer.FirstName} {sp.Performer.LastName}"
                    }).ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToList();

            int counter = 0;
            StringBuilder sb = new StringBuilder();

            foreach(var song in songs)
            {
                sb.AppendLine($"-Song #{++counter}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                foreach (var performer in song.Performers.OrderBy(p=>p.PerformerFullName))
                {
                    sb.AppendLine($"---Performer: {performer.PerformerFullName}");
                }
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
