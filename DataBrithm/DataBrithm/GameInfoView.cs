//
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
			gameCoverView.Image = info.Cover;
			title.Text   = info.Title;
			company.Text = info.Company;
			region.Text  = info.Region.ToString();
			size.Text    = info.Size.ToString();
			releaseNumber.Text = info.ReleaseId.ToString();
			language.Text = info.Language.ToString();
			saveType.Text = info.SaveType;
		}
	}
}

