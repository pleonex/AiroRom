//
//  CompressionFrame.cs
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
	public partial class CompressionFrame
	{
		readonly CompressionAlgorithm algorithm;

		public CompressionFrame(CompressionAlgorithm algorithm)
		{
			this.algorithm = algorithm;
			CreateComponents();
			SetValues();
		}

		void SetValues()
		{
			ratioBtn.Value = algorithm.CompressionRatio;
			ratioBalancedBtn.Value = algorithm.BalancedCompressionRatio;
			supportSubfilesCheck.Active = algorithm.SupportsSubFiles;
			avgSubfilesBtn.Value = algorithm.AverageSubFiles;
			supportDirectAccessCheck.Active = algorithm.SupportsInmediateAccess;
			isHeaderEncryptedCheck.Active = algorithm.IsHeaderEncrypted;
			areSubfilesEncryptedCheck.Active = algorithm.AreSubFilesEncrypted;

			string txt = "";
			foreach (int id in algorithm.EncryptionAlgorithms)
				txt += id.ToString() + " ";
			algorithmUsedTxt.Text = txt;
		}
	}
}

