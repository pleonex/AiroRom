//
//  AlgorithmView.cs
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
using Xwt;

namespace DataBrithm
{
	public partial class AlgorithmView
	{
		public AlgorithmView()
		{
			CreateComponents();
		}

		public void SetAlgorithm(AlgorithmInfo algorithm)
		{
			idBtn.Value     = algorithm.Id;
			gameIdBtn.Value = algorithm.GameId;
			nameTxt.Text    = algorithm.Name;
			companyTxt.Text = algorithm.Company;
			typeCombo.SelectedText   = algorithm.Type.ToString();
			deviceCombo.SelectedText = algorithm.Device.ToString();
			detectableCheck.Active   = algorithm.CanBeDetected;
			instructionsBtn.Value    = algorithm.Instructions;
			basedOnTxt.Text = algorithm.BasedOn;
			filesBtn.Value  = algorithm.Files;
			filesCombo.SelectedText = algorithm.FileType.ToString();
			filesFreqBtn.Value = algorithm.FileFrecuencyAccess;
			bestAlgorithmCombo.SelectedIndex = algorithm.BestAlgorithm;
			qualityBtn.Value = algorithm.Quality;
			detailsTxt.Text  = algorithm.Details;

			if (algorithmSpecific.Content != null)
				algorithmSpecific.Content.Dispose();

			switch (algorithm.Type) {
			case AlgorithmType.Compression:
				algorithmSpecific.Content = new CompressionFrame((CompressionAlgorithm)algorithm);
				break;

			case AlgorithmType.Encryption:
				algorithmSpecific.Content = new EncryptionFrame((EncryptionAlgorithm)algorithm);
				break;

			case AlgorithmType.Integrity:
				algorithmSpecific.Content = new IntegrityFrame((IntegrityAlgorithm)algorithm);
				break;
			}
		}
	}
}

