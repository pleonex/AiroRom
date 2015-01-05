//
//  IntegrityFrame.Designer.cs
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
	public partial class IntegrityFrame : Table
	{
		SpinButton hashSizeBtn;
		CheckBox   isBrokenCheck;
		TextEntry  keyTxt;

		void CreateComponents()
		{
			Margin = 10;

			isBrokenCheck = new CheckBox("Is broken?");
			keyTxt = new TextEntry {
				MultiLine = true,
				HeightRequest = 50
			};

			hashSizeBtn = new SpinButton {
				Digits = 0,
				IncrementValue = 64,
				MaximumValue = 10000
			};

			Add(new Label("Hash size:"), 0, 0, hexpand: false);
			Add(hashSizeBtn, 1, 0, hexpand: false, hpos: WidgetPlacement.Start);

			Add(isBrokenCheck, 2, 0, hexpand: true, hpos: WidgetPlacement.End);

			Add(new Label("Key:"), 0, 1);
			Add(keyTxt, 0, 2, colspan: 3, vexpand: true);
		}
	}
}

