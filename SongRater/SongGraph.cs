using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongRater
{
	public partial class SongGraph : UserControl
	{
		private readonly Dictionary<Song, List<SongSnapshot>> Snapshots = new Dictionary<Song, List<SongSnapshot>>();
		private int maxGames = 20;
		private float ratingRange = 200;

		public SongGraph()
		{
			InitializeComponent();
		}

		private void SongGraph_Load(object sender, EventArgs e)
		{

		}

		public void AddSnapshot(Song song)
		{
			if (!Snapshots.ContainsKey(song))
			{
				Snapshots[song] = new List<SongSnapshot>();
			}

			Snapshots[song].Add(new SongSnapshot(song));
			Snapshots[song].Sort();

			maxGames = Math.Max(10+10*(song.NumGames/10), maxGames);
			ratingRange = (float)Math.Max(200 + 100 * Math.Truncate(Math.Abs(song.Rating)/100), ratingRange);
		}

		public void ClearSnapshots()
		{
			Snapshots.Clear();
			maxGames = 20;
			ratingRange = 200;
		}

		private void SongGraph_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rect = e.ClipRectangle;
			Graphics g = e.Graphics;

			g.SmoothingMode = SmoothingMode.HighQuality;

			using (var pen = new Pen(Color.Aquamarine))
				g.DrawLine(pen, rect.Left, rect.Y + rect.Height * 0.5f, rect.Right, rect.Y + rect.Height * 0.5f);

			Song[] songs = Snapshots.Keys.ToArray();
			int count = songs.Length;

			for (int i = 0; i < count; i++)
			{
				float perc = count == 1 ? 0 : i / (float)(count - 1);
				Color color = ColorFromHSB(360f * perc, 0.8f, 0.8f);

				PointF[] points = Snapshots[songs[i]]
					.Select(s => s.ToPointF(rect, maxGames, ratingRange))
					.ToArray();

				if (points.Length == 0)
					continue;

				using (var pen = new Pen(color, 1))
				{
					if (points.Length == 1)
						g.DrawEllipse(pen, points[0].X, points[0].Y, 1, 1);
					else
						g.DrawCurve(pen, points);
				}
			}
		}

		private static Color ColorFromHSB(float hue, float sat, float bri, int a = 255)
		{
			if (sat == 0)
				return Color.FromArgb(a, (int) (bri * 255), (int) (bri * 255), (int) (bri * 255));

			float fMax, fMid, fMin;

			if (bri > 0.5f)
			{
				fMax = bri - bri * sat + sat;
				fMin = bri + bri * sat - sat;
			}
			else
			{
				fMax = bri + bri * sat;
				fMin = bri - bri * sat;
			}

			var iSextant = (int) Math.Floor(hue / 60f);
			if (300f <= hue)
			{
				hue -= 360f;
			}
			hue /= 60f;
			hue -= 2f * (float) Math.Floor((iSextant + 1f) % 6f / 2f);

			if (iSextant % 2 == 0)
				fMid = hue * (fMax - fMin) + fMin;
			else
				fMid = fMin - hue * (fMax - fMin);

			var iMax = (int)(fMax * 255);
			var iMid = (int)(fMid * 255);
			var iMin = (int)(fMin * 255);

			switch (iSextant)
			{
				case 1:
					return Color.FromArgb(a, iMid, iMax, iMin);
				case 2:
					return Color.FromArgb(a, iMin, iMax, iMid);
				case 3:
					return Color.FromArgb(a, iMin, iMid, iMax);
				case 4:
					return Color.FromArgb(a, iMid, iMin, iMax);
				case 5:
					return Color.FromArgb(a, iMax, iMin, iMid);
				default:
					return Color.FromArgb(a, iMax, iMid, iMin);
			}
		}

		public struct SongSnapshot : IComparable<SongSnapshot>
		{
			public readonly float Rating;
			public readonly int NumGames;

			public SongSnapshot(Song source)
			{
				Rating = source.Rating;
				NumGames = source.NumGames;
			}

			public int CompareTo(SongSnapshot other)
			{
				return NumGames.CompareTo(other.NumGames);
			}

			public PointF ToPointF(Rectangle rect, int maxNumGames, float ratingRange)
			{
				return new PointF(
					x: rect.Left + rect.Width * (float)NumGames / maxNumGames,
					y: rect.Top + 0.5f * (rect.Height - rect.Height * Rating / ratingRange));
			}
		}
	}
}
