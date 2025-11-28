using cAlgo.API;
using SMCTimingIndicator.Helpers;
using SMCTimingIndicator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMCTimingIndicator.Services
{
	public class LineDrawer
	{
		private readonly Chart chart;
		private readonly int timezoneOffsetMinutes;

		public LineDrawer(Chart chart, int timezoneOffsetMinutes)
		{
			this.chart = chart;
			this.timezoneOffsetMinutes = timezoneOffsetMinutes;
		}

		public void Draw(LineDefinition line, DateTime dayUtc, bool overwrite)
		{
			var localTime = line.GetTimeSpan();
			var utcTime = localTime - TimeSpan.FromMinutes(timezoneOffsetMinutes);
			var targetUtc = dayUtc.Add(utcTime);

			var color = LineVisualMapper.ToColor(line.Color);
			var style = LineVisualMapper.ToLineStyle(line.Style);
			var thickness = LineVisualMapper.ToThickness(line.Thickness);

			var tag = line.GetTag(dayUtc, overwrite);

			chart.DrawVerticalLine(tag, targetUtc, color, thickness, style);
		}
	}
}
