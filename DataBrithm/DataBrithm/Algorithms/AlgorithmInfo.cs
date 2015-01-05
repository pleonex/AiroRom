//
//  AlgorithmInfo.cs
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
	public enum Device {
		NintendoDS,
		PS1,
		PSP
	}

	public enum FileType {
		Generic,
		Text,

		Save,
		DLC,
	}

	public enum AlgorithmType {
		Encryption,
		Compression,
		Integrity
	}

	public class AlgorithmInfo
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public int GameId  { get; set; }
		public int Company { get; set; }
		public Device Device { get; set; }

		public AlgorithmType Type { get; set; }
		public bool CanBeDetected { get; set; }
		public int  Instructions  { get; set; }
		public string BasedOn { get; set; }

		public int Files { get; set; }
		public FileType FileType { get; set; }
		public double   FileFrecuencyAccess { get; set; }

		public int BestAlgorithm { get; set; }
		public double Quality { get; set; }
		public string Details { get; set; }
	}
}
