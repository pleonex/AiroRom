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
using System;
using System.Net;
using System.IO;

namespace DataBrithm
{
	public partial class GameInfoView
	{
		public GameInfoView()
		{
			CreateComponents();
		}

		public void SetGame(Device dev, int releaseNum)
		{
			var info = GameInfoManager.Instance.GetGameInfo(dev, releaseNum);
			if (info == null) {
				Reset();
				return;
			}

			title.Text   = info.Title;
			company.Text = info.Company;
			region.Text  = info.Region.ToString();
			releaseNumber.Text = info.ReleaseId.ToString();
			language.Text = info.Language.ToString();
			saveType.Text = info.SaveType;
			gameCoverView.Image = null;

			if (info.Size >= (1 << 30))
				size.Text = (info.Size / (1 << 30)).ToString() + " GB";
			else if (info.Size >= (1 << 20))
				size.Text = (info.Size / (1 << 20)).ToString() + " MB";
			else if (info.Size >= (1 << 10))
				size.Text = (info.Size / (1 << 10)).ToString() + " KB";
			else
				size.Text = info.Size + " B";

			CoverManager.Instage.GetCover(info.Device, info.ReleaseId, HandleOpenCoverCompleted);
		}

		void Reset()
		{
			title.Text   = string.Empty;
			company.Text = string.Empty;
			region.Text  = string.Empty;
			releaseNumber.Text = string.Empty;
			language.Text = string.Empty;
			saveType.Text = string.Empty;
			size.Text = string.Empty;
			gameCoverView.Image = null;
		}

		void HandleOpenCoverCompleted(bool error, Stream stream)
		{
			Action action = () => {
				gameCoverView.Image = Image.FromStream(stream);
				stream.Close();
			};

			if (!error)
				Application.Invoke(action);
			else
				Application.Invoke(() => MessageDialog.ShowError("Error downloading cover"));
		}
	}
}

