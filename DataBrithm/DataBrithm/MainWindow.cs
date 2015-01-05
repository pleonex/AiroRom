//
//  MainWindow.cs
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
	public class MainWindow : Window
	{
		TreeView algorithmList;
		GameInfoWidget gameInfo;
		AlgorithmView algorithmView;

		public MainWindow()
		{
			CreateComponents();
		}

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
			var vbox = new VBox();
			hpaned.Panel2.Content = vbox;

			// ... first general game info
			gameInfo = new GameInfoWidget();

			var gameInfoFrame = new Frame();
			gameInfoFrame.Label   = "Game information";
			gameInfoFrame.Content = gameInfo;
			vbox.PackStart(gameInfoFrame, false);

			// ... then the algorithm info
			algorithmView = new AlgorithmView();

			var algorithmInfoFrame = new Frame();
			algorithmInfoFrame.Label   = "Algorithm information";
			algorithmInfoFrame.Content = algorithmView;
			vbox.PackStart(algorithmInfoFrame, true);

			// Set window content
			Padding = new WidgetSpacing();
			Content = hpaned;

			CloseRequested += HandleCloseRequested;
		}

		void HandleCloseRequested(object sender, CloseRequestedEventArgs e)
		{
			Application.Exit();
		}
	}
}

