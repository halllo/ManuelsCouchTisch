using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace ManuelsCouchTisch
{
	public class GastGedaechtnis
	{
		Dictionary<long, TagManagement.Data> _tags;
		Dictionary<string, Brush> _allColors;

		public GastGedaechtnis(Dictionary<long, TagManagement.Data> tags, Dictionary<string, Brush> allColors)
		{
			_tags = tags;
			_allColors = allColors;
		}

		public string AsTagDump()
		{
			var tagDump = "gäste;\n" + string.Join("\n", _tags.Select(t => "" + t.Key + ";" + t.Value.Name + ";" + _allColors.First(c => c.Value == t.Value.Color).Key));
			return tagDump;
		}

		public void Store()
		{
			Properties.Settings.Default.Guests = AsTagDump();
			Properties.Settings.Default.Save();
		}

		public void Restore()
		{
			try
			{
				var tagDump = Properties.Settings.Default.Guests;

				var tags = Split(tagDump, "\n").Skip(1)
					.Select(l => Split(l, ";")).Select(l => new { key = long.Parse(l[0]), name = l[1], color = l[2] })
					.ToDictionary(t => t.key);

				foreach (var tag in _tags.Where(tag => tags.ContainsKey(tag.Key)))
				{
					var storedTag = tags[tag.Key];
					tag.Value.Name = storedTag.name;
					tag.Value.Color = _allColors[storedTag.color];
				}
			}
			catch
			{
			}
		}

		private static string[] Split(string value, string tokenizer)
		{
			return value.Split(new string[] { tokenizer }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
