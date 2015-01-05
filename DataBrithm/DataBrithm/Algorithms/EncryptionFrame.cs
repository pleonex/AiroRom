//
//  EncryptionFrame.cs
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
	public class EncryptionFrame : Table
	{
		EncryptionAlgorithm algorithm;

		CheckBox symmetricCheck;
		TextEntry keyTxt;
		TextEntry crcName;

		public EncryptionFrame(EncryptionAlgorithm algorithm)
		{
			this.algorithm = algorithm;
			CreateComponents();
		}

		void CreateComponents()
		{
			Margin = 10;

			symmetricCheck = new CheckBox("Is symmetryc?");
			symmetricCheck.Active = algorithm.IsSymmetric;
			symmetricCheck.WidthRequest = 205;

			crcName = new TextEntry {
				Text = algorithm.CrcName
			};

			keyTxt = new TextEntry {
				MultiLine = true,
				HeightRequest = 50,
			};
			if (algorithm.Key != null)
				keyTxt.Text = BitConverter.ToString(algorithm.Key).Replace('-', ' ');

			Add(symmetricCheck, 0, 0, hexpand: false);

			Add(new Label("CRC Name:"), 1, 0, hexpand: false);
			Add(crcName, 2, 0, hexpand: true);

			Add(new Label("Key:"), 0, 1);
			Add(keyTxt, 0, 2, colspan: 3, vexpand: true);
		}
	}
}

