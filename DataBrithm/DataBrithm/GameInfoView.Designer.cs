//
//  GameInfoView.Designer.cs
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
	public partial class GameInfoView : HBox
	{
		ImageView gameCoverView;

		void CreateComponents()
		{
			HeightRequest = 192;

			var infoEntries = new Table();
			infoEntries.DefaultRowSpacing += 2;
			PackStart(infoEntries, true, WidgetPlacement.Center, margin: 10);

			infoEntries.Add(new Label("Title:"), 0, 0);
			infoEntries.Add(new InfoText("Metroid Prime Hunters - First Hunt (Demo)"),
				1, 0, hexpand: true);	// First column should have the horizontal expand

			infoEntries.Add(new Label("Company:"), 0, 1);
			infoEntries.Add(new InfoText("Nintendo"), 1, 1);

			infoEntries.Add(new Label("Region:"), 0, 2);
			infoEntries.Add(new InfoText("USA"), 1, 2);

			infoEntries.Add(new Label("Size:"), 0, 3);
			infoEntries.Add(new InfoText("16 MB"), 1, 3);

			infoEntries.Add(new Label("ROM Numer:"), 0, 4);
			infoEntries.Add(new InfoText("1"), 1, 4);

			infoEntries.Add(new Label("Language:"), 0, 5);
			infoEntries.Add(new InfoText("English (UK)"), 1, 5);

			infoEntries.Add(new Label("Save type:"), 0, 6);
			infoEntries.Add(new InfoText("None"), 1, 6);

			// Set a test cover
			gameCoverView = new ImageView();
			gameCoverView.HeightRequest = 192;	// Cut image to get only front cover
			gameCoverView.BackgroundColor = Colors.Black;	// Must be set to cut image
			gameCoverView.MarginRight = 10;
			UpdateCover(1);
			PackEnd(gameCoverView);
		}

		class InfoText : Label
		{
			public InfoText(string text)
			{
				Text = text;
				Ellipsize = EllipsizeMode.End;
				MinWidth  = 80;
			}
		}
	}
}

