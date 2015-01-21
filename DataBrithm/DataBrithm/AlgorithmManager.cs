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
using System;
using System.Reflection;
using System.IO;

namespace DataBrithm
{
	public class AlgorithmManager
	{
		static AlgorithmManager CurrentInstance;
		static readonly string FileDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		static readonly string FilePath = Path.Combine(FileDir, "Algorithms.xml");

		AlgorithmManager()
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

		public IList<AlgorithmInfo> AlgorithmList { get; private set; }

		void ReadXml()
		{
			XDocument doc = XDocument.Load(FilePath);

			AlgorithmList = new List<AlgorithmInfo>();
			foreach (XElement element in doc.Root.Element("AlgorithmList").Elements())
				AlgorithmList.Add(AlgorithmInfoFactory.FromXml(element));
		}

		public void Save()
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("AlgorithmManager"));

			XElement xmlList = new XElement("AlgorithmList");
			doc.Root.Add(xmlList);

			foreach (AlgorithmInfo info in AlgorithmList)
				xmlList.Add(AlgorithmInfoFactory.ToXml(info));

			doc.Save(FilePath);
		}
	}
}

