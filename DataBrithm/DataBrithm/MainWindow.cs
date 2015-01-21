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
			btnEdit.Clicked += EditClicked;
			btnAdd.Clicked += AddClicked;

			viewMode.SelectedIndex = 0;
		}

		void AddClicked (object sender, EventArgs e)
		{
			Dialog dialog = new Dialog();
			dialog.Title = "New algorithm info";

			Table dialogContent = new Table();
			dialog.Content = dialogContent;

			dialogContent.Add(new Label("Type:"), 0, 0);

			var typeCombo = new ComboBox();
			foreach (var t in Enum.GetValues(typeof(AlgorithmType)))
				typeCombo.Items.Add(t);

			dialogContent.Add(typeCombo, 1, 0);

			dialog.Buttons.Add(new DialogButton(Command.Add));
			dialog.Buttons.Add(new DialogButton(Command.Cancel));

			var result = dialog.Run(this);
			if (result == Command.Add) {
				var newAlgorithm = AlgorithmInfoFactory.FromType((AlgorithmType)typeCombo.SelectedItem);
				newAlgorithm.Name = "New Algorithm";
				newAlgorithm.Id = AlgorithmManager.Instance.AlgorithmList.Count;
				AlgorithmManager.Instance.AlgorithmList.Add(newAlgorithm);
				AlgorithmManager.Instance.Save();

				UpdateList(sender, e);
				SelectAlgorithm(newAlgorithm);
			}

			dialog.Dispose();
		}

		void EditClicked (object sender, EventArgs e)
		{
			AlgorithmInfo selected = store.GetNavigatorAt(algorithmTree.SelectedRow).GetValue(infoCol);

			algorithmView.UpdateAlgorithm();
			AlgorithmManager.Instance.Save();

			UpdateList(sender, e);
			SelectAlgorithm(selected);
		}

		void UpdateList(object sender, EventArgs e)
		{
			store.Clear();

			foreach (AlgorithmInfo info in AlgorithmManager.Instance.AlgorithmList) {
				if (viewMode.SelectedIndex == 0)
					InsertAlgorithm(store.AddNode(), info);
				else if (viewMode.SelectedIndex == 1)
					InsertInGameList(info);
			}
		}

		void InsertAlgorithm(TreeNavigator navigator, AlgorithmInfo info)
		{
			navigator
				.SetValue(iconCol, info.Icon)
				.SetValue(nameCol, info.Name)
				.SetValue(infoCol, info);
		}

		void InsertInGameList(AlgorithmInfo info)
		{
			var gameInfo = GameInfoManager.Instance.GetGameInfo(info.Device, info.GameId);
			string title;
			if (gameInfo != null)
				title = string.Format("{0} [{1}]", gameInfo.Title, gameInfo.Device);
			else
				title = "Unknown";

			var navigator = SearchNode(title);
			if (navigator == null)
				navigator = store.AddNode().SetValue(nameCol, title);

			InsertAlgorithm(navigator.AddChild(), info);
		}

		TreeNavigator SearchNode(string name)
		{
			TreeNavigator navigator = store.GetFirstNode();
			if (navigator.CurrentPosition == null)
				return null;

			do {
				if (name == navigator.GetValue(nameCol))
					return navigator;
			} while (navigator.MoveNext());

			return null;
		}

		void SelectAlgorithm(AlgorithmInfo algorithm)
		{
			TreeNavigator nav = store.GetFirstNode();
			do {
				if (nav.GetValue(infoCol) == algorithm) {
					algorithmTree.SelectRow(nav.CurrentPosition);
					break;
				}
			} while (nav.MoveNext());
		}

		void AlgorithmSelected(object sender, EventArgs e)
		{
			if (algorithmTree.SelectedRow == null)
				return;

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

