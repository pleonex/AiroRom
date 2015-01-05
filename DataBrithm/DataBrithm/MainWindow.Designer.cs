﻿//
//  MainWindow.Designer.cs
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
	public partial class MainWindow : Window
	{
		TreeView algorithmList;
		GameInfoView gameInfo;
		AlgorithmView algorithmView;

		void CreateComponents()
		{
			Title  = "DataBrithm - Game Algorithms DB";
			Width  = 800;
			Height = 600;

			var hpaned = new HPaned();
			hpaned.BackgroundColor = Color.FromBytes(149, 167, 185);

			// In the left we have the list of algorithms
			algorithmList = new TreeView();
			algorithmList.WidthRequest = 150;
			hpaned.Panel1.Content = algorithmList;

			// In the right the info about the algorithm
			var infoControlBox = new HBox();
			hpaned.Panel2.Content = infoControlBox;

			var infoBox = new VBox();
			infoControlBox.PackStart(infoBox, true);

			// ... first general game info
			gameInfo = new GameInfoView();

			var gameInfoFrame = new Frame();
			gameInfoFrame.Label   = "Game information";
			gameInfoFrame.Content = gameInfo;
			infoBox.PackStart(gameInfoFrame, false);

			// ... then the algorithm info
			algorithmView = new AlgorithmView();

			var algorithmInfoFrame = new Frame();
			algorithmInfoFrame.Label   = "Algorithm information";
			algorithmInfoFrame.Content = algorithmView;
			infoBox.PackStart(algorithmInfoFrame, true);

			// Create panel buttons bar
			var buttonBar = new VBox();
			infoControlBox.PackStart(buttonBar, false);

			buttonBar.MarginRight  = 5;
			buttonBar.MarginTop    = 5;
			buttonBar.MarginBottom = 5;
			buttonBar.PackEnd(new Button(StockIcons.Add));
			buttonBar.PackEnd(new Button(StockIcons.Information));
			buttonBar.PackEnd(new Button(StockIcons.Remove));

			// Set window content
			Padding = new WidgetSpacing();
			Content = hpaned;

			CloseRequested += HandleCloseRequested;
		}
	}
}
