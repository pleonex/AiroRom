//
//  EncryptionAlgorithm.cs
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
namespace DataBrithm
{
	public class EncryptionAlgorithm : AlgorithmInfo
	{
		public EncryptionAlgorithm()
		{
			Type = AlgorithmType.Encryption;
		}

		public bool IsSymmetric { get; set; }
		public byte[] Key { get; set; }

		public string CrcName { get; set; }

		public override Xwt.Drawing.Image Icon {
			get {
				return Xwt.Drawing.Image.FromResource("DataBrithm.res.key.png");
			}
		}

		protected override double SpecificQuality {
			get {
				double crcPoints = string.IsNullOrEmpty(CrcName) ? 0 : 20;
				double symmetricPoints = IsSymmetric ? 0 : 20;
				double keyPoints = (Key == null) ? 0 : System.Math.Sqrt(Key.Length);
				return crcPoints + symmetricPoints + keyPoints;
			}
		}
	}
}

