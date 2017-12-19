using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongRater
{
	public partial class SongPage : UserControl
	{
		private Song _song;
		public Song Song
		{
			get => _song;
			set
			{
				_song = value;
				OnSongChanged();
			}
		}

		public SongPage()
		{
			InitializeComponent();
		}

		private void OnSongChanged()
		{
			if (_song == null)
			{
				labelFilename.Text = "<none>";
				Enabled = false;
				return;
			}
			
			labelFilename.Text = _song.Filename;
			labelTitle.Text = _song.Title;
			labelArtist.Text = _song.Artist;

			Enabled = true;
		}
	}
}
