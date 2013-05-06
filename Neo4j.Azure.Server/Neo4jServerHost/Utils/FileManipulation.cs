using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Neo4j.Azure.Server.Utils
{
	public class FileManipulation : IFileManipulation
	{
		public void ReplaceConfigLine(string fileName, params Replacement[] replacements)
		{
			Trace.TraceInformation(String.Format("Reading config file '{0}'.", fileName));

			if (!File.Exists(fileName))
			{
				Trace.Fail(string.Format("The file to modify does not exis '{0}'.", fileName));
				return;
			}

			var lines = new List<string>();

			using (var file = new StreamReader(fileName))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();

					if (line == null) continue;

					var replacement = replacements.Where(rep => line.Contains(rep.PatternToFind)).SingleOrDefault();
					if (replacement != null)
					{
						Trace.TraceInformation(String.Format("Found match for <{0}> on line #{1}. The line was <{2}> and was replaced with <{3}>.",
						                                     replacement.PatternToFind, 
						                                     lines.Count + 1, 
						                                     line, 
						                                     replacement.LineToInsert));
						line = replacement.LineToInsert;
					}

					lines.Add(line);

					//Trace.TraceInformation(String.Format("config file line read: <{0}>", line));
				}
			}
			//Trace.TraceInformation(String.Format("Writing config file '{0}'.", fileName));

			using (var file = new StreamWriter(fileName, false))
			{
				foreach (var line in lines)
				{
					file.WriteLine(line);
					//Trace.TraceInformation(String.Format("config file line written: <{0}>", line));
				}
			}

			Trace.TraceInformation(String.Format("Config file '{0}' written.", fileName));
		}
	}
}