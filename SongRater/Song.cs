using System.Collections.Generic;

namespace SongRater
{
	public class Song
	{
		public readonly string Path;
		public readonly string Filename;
		public readonly string Title;
		public readonly string Artist;

		private readonly HashSet<Song> playedAgainst = new HashSet<Song>();

		public int NumWins { get; private set; } = 0;
		public int NumLosses { get; private set; } = 0;
		public int NumGames => NumWins + NumLosses;
		public float Rating { get; private set; } = 0;

		public Song(string path)
		{
			Path = path;
			Filename = System.IO.Path.GetFileName(path);

			// Get Title from tag
			using (TagLib.File file = TagLib.File.Create(path))
			{
				Title = file.Tag.Title;
				Artist = string.Join(", ", file.Tag.Performers);
			}

			if (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Artist)) return;

			// Get Title from filename
			Title = System.IO.Path.GetFileNameWithoutExtension(Filename);
			Artist = string.Empty;
		}

		public bool HasPlayedAgainst(Song song)
		{
			return playedAgainst.Contains(song);
		}

		public static void EvaluateRound(Song winner, Song loser)
		{
			winner.playedAgainst.Add(loser);
			winner.NumWins++;
			loser.playedAgainst.Add(winner);
			loser.NumLosses++;

			float winnerRating = winner.Rating;
			float loserRating = loser.Rating;

			winner.UpdateRating(loserRating);
			loser.UpdateRating(winnerRating);
		}

		private void UpdateRating(float opponentRating)
		{
			// https://en.wikipedia.org/wiki/Elo_rating_system#Performance_rating
			Rating = (opponentRating + 400 * (NumWins - NumLosses)) / NumGames;
		}

		public override string ToString()
		{
			return $"[{Rating:F0}]\t" + (string.IsNullOrWhiteSpace(Artist)
				? Title
				: $"{Artist} - {Title}");
		}
	}
}