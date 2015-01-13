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
using System;
using System.Linq;
using Xwt;

namespace DataBrithm
{
	public partial class CompressionFrame : IAlgorithmFrame
	{
		CompressionAlgorithm algorithm;

		public CompressionFrame()
		{
			CreateComponents();
		}

		public void SetAlgorithm(AlgorithmInfo algorithm)
		{
			this.algorithm = algorithm as CompressionAlgorithm;
			if (this.algorithm == null)
				throw new ArgumentException("Invalid algorithm type");

			ratioBtn.Value                   = this.algorithm.CompressionRatio;
			ratioBalancedBtn.Value           = this.algorithm.BalancedCompressionRatio;
			supportSubfilesCheck.Active      = this.algorithm.SupportsSubFiles;
			avgSubfilesBtn.Value             = this.algorithm.AverageSubFiles;
			supportDirectAccessCheck.Active  = this.algorithm.SupportsInmediateAccess;
			isHeaderEncryptedCheck.Active    = this.algorithm.IsHeaderEncrypted;
			areSubfilesEncryptedCheck.Active = this.algorithm.AreSubFilesEncrypted;

			string txt = "";
			foreach (int id in this.algorithm.EncryptionAlgorithms)
				txt += id.ToString() + " ";
			algorithmUsedTxt.Text = txt;
		}

		public void UpdateAlgorithm()
		{
			algorithm.CompressionRatio         = ratioBtn.Value;
			algorithm.SupportsSubFiles         = supportSubfilesCheck.Active;
			algorithm.AverageSubFiles          = avgSubfilesBtn.Value;
			algorithm.SupportsInmediateAccess  = supportDirectAccessCheck.Active;
			algorithm.IsHeaderEncrypted        = isHeaderEncryptedCheck.Active;
			algorithm.AreSubFilesEncrypted     = areSubfilesEncryptedCheck.Active;

			string[] ids = algorithmUsedTxt.Text.Split(' ');
			algorithm.EncryptionAlgorithms = ids.Select(id => Convert.ToInt32(id)).ToArray();
		}

		public Widget View {
			get { return view; }
		}
	}
}

