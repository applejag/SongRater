using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SongRater.ExtensionMethods;

namespace SongRater
{
	public partial class Form1 : Form
	{
		public string FileSearchPattern { get; set; } = "*.mp3";
		private readonly List<Song> songsBacking;
		private readonly BindingList<Song> songs;

		private readonly List<Song> fighters;

		public Form1()
		{
			InitializeComponent();

			songsBacking = new List<Song>();
			songs = new BindingList<Song>(songsBacking);
			fighters = new List<Song>();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			scoreListBox.DataSource = songs;
		}

		private void folderTextBox1_SelectedPathChanged(object sender, EventArgs e)
		{
			ReloadFilesInFolder();
		}

		private void ReloadFilesInFolder()
		{
			if (songs.Count > 0)
			{
				DialogResult result = MessageBox.Show(
					"You are about to refresh the songs list. All your ranking statistics will be reset.\n\nAre you sure?",
					"Reset songs?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (result != DialogResult.Yes)
					return;
			}

			if (folderTextBox1.SelectedPath == null)
			{
				songs.Clear();
				AssignTwoChampions();
				return;
			}

			string[] files = Directory.GetFiles(folderTextBox1.SelectedPath, FileSearchPattern);

			fighters.Clear();
			songs.Clear();
			foreach (string path in files)
				songs.Add(new Song(path));

			AssignTwoChampions();
		}

		private void AssignTwoChampions()
		{
			if (songs == null || songs.Count < 2)
			{
				songPage1.Song = null;
				songPage2.Song = null;
				scoreListBox.Enabled = false;
				button1.Enabled = false;
				button2.Enabled = false;
				return;
			}

			Song fighter1 = GetNextFighter();
			Song fighter2 = GetNextFighter(fighter1);

			if (fighter1 == null || fighter2 == null)
			{
				// Döner.
				songPage1.Song = null;
				songPage2.Song = null;
				button1.Enabled = false;
				button2.Enabled = false;
				return;
			}

			songPage1.Song = fighter1;
			songPage2.Song = fighter2;
			scoreListBox.Enabled = true;
			button1.Enabled = true;
			button2.Enabled = true;
		}

		private Song GetNextFighter(Song other = null)
		{
			const int tries = 10;

			for (int i = 0; i < tries; i++)
			{
				if (songsBacking.Count == 0)
					return null;

				if (fighters.Count == 0)
				{
					fighters.AddRange(songsBacking);
					fighters.Shuffle();
				}

				int index = fighters.Count - 1;
				Song fighter = fighters[index];
				fighters.RemoveAt(index);

				if (other != null && other.HasPlayedAgainst(fighter))
					continue;

				return fighter;
			}

			return null;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Song.EvaluateRound(winner: songPage1.Song, loser: songPage2.Song);
			SortSongs();
			AssignTwoChampions();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Song.EvaluateRound(winner: songPage2.Song, loser: songPage1.Song);
			SortSongs();
			AssignTwoChampions();
		}

		private void SortSongs()
		{
			songsBacking.Sort((a,b) => b.Rating.CompareTo(a.Rating));
			songs.ResetBindings();
		}
	}
}
