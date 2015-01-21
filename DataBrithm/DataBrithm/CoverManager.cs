//
//  CoverManager.cs
//
//  Author:
//       Benito Palacios Sánchez <benito356@gmail.com>
//
//  Copyright (c) 2015 Benito Palacios Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

namespace DataBrithm
{
	public delegate void OpenCoverCompletedEventHandler(bool success, Stream stream);

	public class CoverManager
	{
		static readonly string FileDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		static readonly CoverManager instance = new CoverManager();

		readonly Dictionary<Device, string> coverUrls = new Dictionary<Device, string> {
			{ Device.NintendoDS, "http://www.advanscene.com/offline/imgs/ADVANsCEne_NDS/{0}-{1}/{2}a.png" },
			{ Device.PSP, "http://www.advanscene.com/offline/imgs/ADVANsCEne_PSN/{0}-{1}/{2}a.png" }
		};

		CoverManager()
		{
			string baseDir = Path.Combine(FileDir, "covers");
			foreach (var device in Enum.GetValues(typeof(Device)))
				Directory.CreateDirectory(Path.Combine(baseDir, device.ToString()));
		}

		public static CoverManager Instage {
			get { return instance; }
		}

		public void GetCover(Device device, int releaseId, OpenCoverCompletedEventHandler handler)
		{
			string cache = GetCoverFile(device, releaseId);
			if (!File.Exists(cache)) {
				string url = GetCoverUrl(device, releaseId);
				Download(url, new DownloadArgs(handler, cache));
			} else {
				var fileStream = new FileStream(cache, FileMode.Open, FileAccess.Read);
				handler.Invoke(false, fileStream);
			}
		}

		string GetCoverFile(Device device, int releaseId)
		{
			string relativePath = Path.Combine("covers", device.ToString());
			string filename = releaseId.ToString() + ".png";
			string fullPath = Path.Combine(FileDir, relativePath, filename);

			return fullPath;
		}

		string GetCoverUrl(Device device, int releaseId)
		{
			// Gets the URL
			int minId = releaseId - (releaseId % 500) + 1;	// In steps of 500
			int maxId = minId + 499;
			return string.Format(coverUrls[device], minId, maxId, releaseId);
		}

		void Download(string url, DownloadArgs args)
		{
			var client = new WebClient();
			client.OpenReadCompleted += HandleOpenReadCompleted;;
			client.OpenReadAsync(new Uri(url), args);
		}

		void HandleOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			var args = (DownloadArgs)e.UserState;
			bool error = !e.Cancelled && e.Error != null;

			Stream stream = null;
			if (!error) {
				stream = new FileStream(args.FilePath, FileMode.Create, FileAccess.ReadWrite);
				e.Result.CopyTo(stream);

				stream.Seek(0, SeekOrigin.Begin);
				e.Result.Close();
			} else {
				stream = null;
			}

			args.Handler.Invoke(error, stream);
		}

		class DownloadArgs
		{
			public DownloadArgs(OpenCoverCompletedEventHandler handler, string filePath)
			{
				this.Handler = handler;
				this.FilePath = filePath;
			}

			public OpenCoverCompletedEventHandler Handler { get; private set; }
			public string FilePath { get; private set; }
		}
	}
}

