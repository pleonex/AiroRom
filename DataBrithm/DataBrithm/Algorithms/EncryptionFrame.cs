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
using System.Linq;
using Xwt;

namespace DataBrithm
{
	public partial class EncryptionFrame : IAlgorithmFrame
	{
		EncryptionAlgorithm algorithm;
		
		public EncryptionFrame()
		{
			CreateComponents();
		}

		public void SetAlgorithm(AlgorithmInfo algorithm)
		{
			this.algorithm = algorithm as EncryptionAlgorithm;
			if (this.algorithm == null)
				throw new ArgumentException("Invalid algorithm type");

			symmetricCheck.Active = this.algorithm.IsSymmetric;
			crcName.Text = this.algorithm.CrcName;

			if (this.algorithm.Key != null)
				keyTxt.Text = BitConverter.ToString(this.algorithm.Key).Replace('-', ' ');
		}

		public void UpdateAlgorithm()
		{
			algorithm.IsSymmetric = symmetricCheck.Active;
			algorithm.CrcName = crcName.Text;

			if (keyTxt.Text.Length > 0) {
				string[] key = keyTxt.Text.Split(' ');
				algorithm.Key = key.Select(k => Convert.ToByte(k, 16)).ToArray();
			} else {
				algorithm.Key = null;
			}
		}

		public Widget View {
			get { return view; }
		}
	}
}

