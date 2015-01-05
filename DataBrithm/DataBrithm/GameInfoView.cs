﻿//
//  GameInfoWidget.cs
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
using Xwt;
using Xwt.Drawing;
using System.IO;
using System.Net;

namespace DataBrithm
{
	public partial class GameInfoView
	{
		const string CoverUrl = 
			"http://www.advanscene.com/offline/imgs/ADVANsCEne_NDS/{0}-{1}/{2}a.png";

		public GameInfoView()
		{
			CreateComponents();
		}

		void UpdateCover(int gameId)
		{
			// Gets the URL
			int minId = gameId - (gameId % 500) + 1;	// In steps of 500
			int maxId = minId + 499;
			string cover = string.Format(CoverUrl, minId, maxId, gameId);

			// Downloads and sets the cover
			var webClient = new WebClient();
			Stream webCoverStream = webClient.OpenRead(cover);
			gameCoverView.Image = Image.FromStream(webCoverStream);
		}
	}
}
