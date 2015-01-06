//
//  AlgorithmView.Designer.cs
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

namespace DataBrithm
{
	public partial class AlgorithmView : Table
	{
		SpinButton idBtn;
		SpinButton gameIdBtn;
		TextEntry  nameTxt;
		TextEntry  companyTxt;
		ComboBox   deviceCombo;
		ComboBox   typeCombo;

		CheckBox   detectableCheck;
		SpinButton instructionsBtn;
		TextEntry  basedOnTxt;
		SpinButton filesBtn;
		ComboBox   filesCombo;
		SpinButton filesFreqBtn;

		ComboBox   bestAlgorithmCombo;
		SpinButton qualityBtn;
		TextEntry  detailsTxt;

		Frame algorithmSpecific;

		void CreateComponents()
		{
			Margin = 10;

			// Create components
			algorithmSpecific = new Frame { HeightRequest = 120 };

			idBtn = new SpinButton {
				Digits = 0,
				IncrementValue = 1,
				MaximumValue = 10000
			};

			gameIdBtn = new SpinButton {
				Digits = 0,
				IncrementValue = 1,
				MaximumValue = 10000
			};

			nameTxt    = new TextEntry();
			companyTxt = new TextEntry();

			deviceCombo = new ComboBox();
			foreach (string dev in Enum.GetNames(typeof(Device)))
				deviceCombo.Items.Add(dev);

			typeCombo = new ComboBox();
			typeCombo.Sensitive = false;	// Forbidden change algorithm type
			foreach (string type in Enum.GetNames(typeof(AlgorithmType)))
				typeCombo.Items.Add(type);

			detectableCheck = new CheckBox("Autodetectable?");
			instructionsBtn = new SpinButton {
				Digits = 0,
				IncrementValue = 1,
				MaximumValue = 10000
			};

			filesBtn = new SpinButton {
				Digits = 0,
				IncrementValue = 1,
				MaximumValue = 10000
			};

			filesFreqBtn = new SpinButton {
				Digits = 2,
				IncrementValue = 0.25,
				MaximumValue = 1
			};

			basedOnTxt = new TextEntry();
			filesCombo = new ComboBox();
			foreach (string type in Enum.GetNames(typeof(FileType)))
				filesCombo.Items.Add(type);

			qualityBtn = new SpinButton {
				Digits = 2,
				IncrementValue = 5,
				MaximumValue = 100
			};

			bestAlgorithmCombo = new ComboBox();
			detailsTxt = new TextEntry { MultiLine = true, MinHeight = 60 };

			// Set them
			Add(new Label("ID:"), 0, 0, hexpand: false);
			Add(idBtn, 1, 0, hpos: WidgetPlacement.Start, hexpand: false);

			Add(new Label("Name:"), 2, 0, hexpand: false);
			Add(nameTxt, 3, 0, hexpand: true);

			Add(new Label("Game ID:"), 0, 1);
			Add(gameIdBtn, 1, 1, hpos: WidgetPlacement.Start);

			Add(new Label("Company:"), 2, 1);
			Add(companyTxt, 3, 1);

			Add(new Label("Device:"), 0, 2);
			Add(deviceCombo, 1, 2);

			Add(new Label("Type:"), 2, 2);
			Add(typeCombo, 3, 2, hpos: WidgetPlacement.Start);

			Add(detectableCheck, 0, 3, colspan: 2);

			Add(new Label("# Instructions:"), 2, 3);
			Add(instructionsBtn, 3, 3, hpos: WidgetPlacement.Start);

			Add(new Label("# Files:"), 0, 4);
			Add(filesBtn, 1, 4, hpos: WidgetPlacement.Start);

			Add(new Label("Files type:"), 2, 4);
			Add(filesCombo, 3, 4, hpos: WidgetPlacement.Start);

			Add(new Label("File frequency:"), 0, 5);
			Add(filesFreqBtn, 1, 5, hpos: WidgetPlacement.Start);

			Add(new Label("Based on:"), 2, 5);
			Add(basedOnTxt, 3, 5);

			Add(algorithmSpecific, 0, 6, colspan: 4);

			Add(new Label("Best algorithm:"), 0, 7);
			Add(bestAlgorithmCombo, 1, 7);

			Add(new Label("Quality:"), 2, 7);
			Add(qualityBtn, 3, 7, hpos: WidgetPlacement.Start);

			Add(new Label("Details"), 0, 8);
			Add(detailsTxt, 0, 9, colspan: 4, vexpand: true);
		}
	}
}

