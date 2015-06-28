//
//  IntegrityAlgorithm.cs
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
	public class IntegrityAlgorithm : AlgorithmInfo
	{
		public IntegrityAlgorithm()
		{
			Type = AlgorithmType.Integrity;
		}

		public int  HashSize { get; set; }
		public bool IsBroken { get; set; }
		public byte[] Key { get; set; }

		public override Xwt.Drawing.Image Icon {
			get {
				return Xwt.Drawing.Image.FromResource("DataBrithm.res.pencil.png");
			}
		}

		protected override double SpecificQuality {
			get {
				int keyPoints = (Key == null) ? 0 : Key.Length * 2;
				return System.Math.Sqrt(HashSize) * (IsBroken ? 0 : 1) + keyPoints;
			}
		}
	}
}

