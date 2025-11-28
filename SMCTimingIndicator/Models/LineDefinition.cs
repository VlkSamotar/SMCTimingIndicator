using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMCTimingIndicator.Enums;

namespace SMCTimingIndicator.Models
{
	public class LineDefinition
	{
		public int Id { get; }
		public string TimeString { get; }
		public MyLineColor Color { get; }
		public MyLineStyle Style { get; }
		public MyLineThickness Thickness { get; }

		public LineDefinition(int id, string timeString, MyLineColor color, MyLineStyle style, MyLineThickness thickness)
		{
			Id = id;
			TimeString = timeString;
			Color = color;
			Style = style;
			Thickness = thickness;
		}

		/// <summary>
		/// Bezpečně převede string na TimeSpan. Pokud selže, vrátí default 08:00.
		/// </summary>
		public TimeSpan GetTimeSpan()
		{
			if (TimeSpan.TryParse(TimeString, out var ts))
				return ts;

			// fallback – defaultní čas
			return new TimeSpan(8, 0, 0);
		}

		public string GetTag(DateTime dayUtc, bool overwrite)
		{
			return overwrite ? $"SMC_Line_{Id}" : $"SMC_Line_{dayUtc:yyyyMMdd}_{Id}";
		}
	}
}
