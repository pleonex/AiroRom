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
		AlgorithmInfo algorithm;

		public AlgorithmView()
		{
			CreateComponents();
		}

		public void SetAlgorithm(AlgorithmInfo algorithm)
		{
			this.algorithm = algorithm;

			idBtn.Value     = algorithm.Id;
			gameIdBtn.Value = algorithm.GameId;
			nameTxt.Text    = algorithm.Name;
			companyTxt.Text = algorithm.Company;
			typeCombo.SelectedItem   = algorithm.Type;
			deviceCombo.SelectedItem = algorithm.Device;
			detectableCheck.Active   = algorithm.CanBeDetected;
			instructionsBtn.Value    = algorithm.Instructions;
			basedOnTxt.Text = algorithm.BasedOn;
			filesBtn.Value  = algorithm.Files;
			filesCombo.SelectedItem = algorithm.FileType;
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

		public void UpdateAlgorithm()
		{
			algorithm.Id = (int)idBtn.Value;
			algorithm.GameId = (int)gameIdBtn.Value;
			algorithm.Name = nameTxt.Text;
			algorithm.Company = companyTxt.Text;
			algorithm.Device = (Device)deviceCombo.SelectedItem;
			algorithm.CanBeDetected = detectableCheck.Active;
			algorithm.Instructions = (int)instructionsBtn.Value;
			algorithm.BasedOn = basedOnTxt.Text;
			algorithm.Files = (int)filesBtn.Value;
			algorithm.FileType = (FileType)filesCombo.SelectedItem;
			algorithm.FileFrecuencyAccess = filesFreqBtn.Value;
			algorithm.BestAlgorithm = bestAlgorithmCombo.SelectedIndex;
			algorithm.Details = detailsTxt.Text;
		}
	}
}

