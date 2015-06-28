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
		static readonly string Filename = "Algorithms.xml";
		static AlgorithmManager CurrentInstance;
		static string FileDir;
		string filePath;

		AlgorithmManager()
		{
			if (string.IsNullOrEmpty(FileDir))
				FileDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			filePath = Path.Combine(FileDir, Filename);

			ReadXml();
		}

		public static void SetFilePath(string path)
		{
			if (CurrentInstance == null)
				FileDir = path;
		}

		public static AlgorithmManager Instance {
			get {
				if (CurrentInstance == null)
					CurrentInstance = new AlgorithmManager();

				return CurrentInstance;
			}
		}

		public SortedSet<AlgorithmInfo> AlgorithmList { get; private set; }

		void ReadXml()
		{
			AlgorithmList = new SortedSet<AlgorithmInfo>(new SortById());
			if (!File.Exists(filePath))
				return;

			XDocument doc = XDocument.Load(filePath);
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

			doc.Save(filePath);
		}

		class SortById : IComparer<AlgorithmInfo>
		{
			public int Compare(AlgorithmInfo info1, AlgorithmInfo info2)
			{
				int compId = info1.Id.CompareTo(info2.Id);
				if (compId != 0)
					return compId;

				return info1.Name.CompareTo(info2.Name);
			}
		}
	}
}

