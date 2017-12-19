using System;
using System.Collections.Generic;

namespace SongRater
{
	public class Song : IComparable<Song>, IComparable
	{
		public readonly string Path;
		public readonly string Filename;
		public readonly string Title;
		public readonly string Artist;

		public string CombinedTitle => string.IsNullOrWhiteSpace(Artist)
			? Title
			: $"{Artist} - {Title}";

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
			return $"[{Rating:F0}]\t{CombinedTitle}";
		}

		public int CompareTo(Song other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			int ratingComparison = Rating.CompareTo(other.Rating);
			if (ratingComparison != 0) return -ratingComparison;
			return string.Compare(CombinedTitle, other.CombinedTitle, StringComparison.CurrentCultureIgnoreCase);
		}

		public int CompareTo(object obj)
		{
			if (ReferenceEquals(null, obj)) return 1;
			if (ReferenceEquals(this, obj)) return 0;
			if (!(obj is Song)) throw new ArgumentException($"Object must be of type {nameof(Song)}");
			return CompareTo((Song) obj);
		}
	}
}