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
		TreeView algoList;

		public MainWindow()
		{
			CreateComponents();
		}

		void CreateComponents()
		{
			Title = "DataBrithm - Game Algorithms DB";
			Width = 740;
			Height = 600;
	
			var hpaned = new HPaned();
			hpaned.BackgroundColor = Colors.LightSteelBlue;

			algoList = new TreeView();
			hpaned.Panel1.Content = algoList;

			var vbox = new VBox();

			var gameInfo = new Frame();
			gameInfo.Label = "Game information";
			gameInfo.HeightRequest = 250;
			vbox.PackStart(gameInfo);

			var algoInfo = new Frame();
			algoInfo.Label = "Algorithm information";
			vbox.PackStart(algoInfo);

			hpaned.Panel2.Content = vbox;

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

