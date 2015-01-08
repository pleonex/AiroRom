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
using System;
using Xwt;
using Xwt.Drawing;

namespace DataBrithm
{
	public partial class MainWindow
	{
		TreeStore store;
		DataField<Image> iconCol = new DataField<Image>();
		DataField<string> nameCol = new DataField<string>();
		DataField<AlgorithmInfo> infoCol = new DataField<AlgorithmInfo>();

		public MainWindow()
		{
			CreateComponents();

			store = new TreeStore(iconCol, nameCol, infoCol);
			algorithmTree.DataSource = store;
			algorithmTree.Columns.Add("Algorithms", iconCol, nameCol);

			viewMode.SelectionChanged += UpdateList;
			algorithmTree.SelectionChanged += AlgorithmSelected;
		}

		void UpdateList(object sender, EventArgs e)
		{
			if (viewMode.SelectedIndex == 0) {
				foreach (AlgorithmInfo info in AlgorithmManager.Instance.AlgorithmList)
					store.AddNode()
						.SetValue(iconCol, info.Icon)
						.SetValue(nameCol, info.Name)
						.SetValue(infoCol, info);
			}
		}

		void AlgorithmSelected (object sender, EventArgs e)
		{
			AlgorithmInfo info = store.GetNavigatorAt(algorithmTree.SelectedRow).GetValue(infoCol);
			if (info != null) {
				algorithmView.SetAlgorithm(info);
				gameInfo.SetGame(info.Device, info.GameId);
			}
		}

		void HandleCloseRequested(object sender, CloseRequestedEventArgs e)
		{
			gameInfo.Dispose();
			algorithmView.Dispose();
			
			Application.Exit();
		}
	}
}

