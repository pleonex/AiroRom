//
//  AlgorithmManager.cs
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
using System.Collections.Generic;
using System.Xml.Linq;

namespace DataBrithm
{
	public class AlgorithmManager
	{
		static AlgorithmManager CurrentInstance;
		static readonly string FileName = "Algorithms.xml";

		private AlgorithmManager()
		{
			ReadXml();
		}

		public static AlgorithmManager Instance {
			get {
				if (CurrentInstance == null)
					CurrentInstance = new AlgorithmManager();

				return CurrentInstance;
			}
		}

		public IEnumerable<AlgorithmInfo> AlgorithmList { get; private set; }

		void ReadXml()
		{
			XDocument doc = XDocument.Load(FileName);

			var algorithms = new List<AlgorithmInfo>();
			foreach (XElement element in doc.Root.Element("AlgorithmList").Elements())
				algorithms.Add(AlgorithmInfoFactory.FromXml(element));

			AlgorithmList = algorithms;
		}
	}
}

