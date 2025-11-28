using cAlgo.API;
using System;
using System.Collections.Generic;
using SMCTimingIndicator.Enums;
using SMCTimingIndicator.Models;
using SMCTimingIndicator.Services;

namespace SMCTimingIndicator
{
	[Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
	public class SMCTimingIndicator : Indicator
	{
		[Parameter("Line 1 Time (HH:mm)", DefaultValue = "08:00")]
		public string Line1Time { get; set; }
		[Parameter("Line 1 Color", DefaultValue = MyLineColor.Blue)]
		public MyLineColor Line1Color { get; set; }
		[Parameter("Line 1 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line1Style { get; set; }
		[Parameter("Line 1 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line1Thickness { get; set; }


		[Parameter("Line 2 Time (HH:mm)", DefaultValue = "09:00")]
		public string Line2Time { get; set; }
		[Parameter("Line 2 Color", DefaultValue = MyLineColor.Orange)]
		public MyLineColor Line2Color { get; set; }
		[Parameter("Line 2 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line2Style { get; set; }
		[Parameter("Line 2 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line2Thickness { get; set; }


		[Parameter("Line 3 Time (HH:mm)", DefaultValue = "10:30")]
		public string Line3Time { get; set; }
		[Parameter("Line 3 Color", DefaultValue = MyLineColor.Grey)]
		public MyLineColor Line3Color { get; set; }
		[Parameter("Line 3 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line3Style { get; set; }
		[Parameter("Line 3 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line3Thickness { get; set; }


		[Parameter("Line 4 Time (HH:mm)", DefaultValue = "12:30")]
		public string Line4Time { get; set; }
		[Parameter("Line 4 Color", DefaultValue = MyLineColor.Grey)]
		public MyLineColor Line4Color { get; set; }
		[Parameter("Line 4 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line4Style { get; set; }
		[Parameter("Line 4 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line4Thickness { get; set; }


		[Parameter("Line 5 Time (HH:mm)", DefaultValue = "14:00")]
		public string Line5Time { get; set; }
		[Parameter("Line 5 Color", DefaultValue = MyLineColor.Red)]
		public MyLineColor Line5Color { get; set; }
		[Parameter("Line 5 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line5Style { get; set; }
		[Parameter("Line 5 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line5Thickness { get; set; }


		[Parameter("Line 6 Time (HH:mm)", DefaultValue = "15:00")]
		public string Line6Time { get; set; }
		[Parameter("Line 6 Color", DefaultValue = MyLineColor.Red)]
		public MyLineColor Line6Color { get; set; }
		[Parameter("Line 6 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line6Style { get; set; }
		[Parameter("Line 6 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line6Thickness { get; set; }


		[Parameter("Line 7 Time (HH:mm)", DefaultValue = "15:30")]
		public string Line7Time { get; set; }
		[Parameter("Line 7 Color", DefaultValue = MyLineColor.Red)]
		public MyLineColor Line7Color { get; set; }
		[Parameter("Line 7 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line7Style { get; set; }
		[Parameter("Line 7 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line7Thickness { get; set; }


		[Parameter("Line 8 Time (HH:mm)", DefaultValue = "17:00")]
		public string Line8Time { get; set; }
		[Parameter("Line 8 Color", DefaultValue = MyLineColor.Purple)]
		public MyLineColor Line8Color { get; set; }
		[Parameter("Line 8 Style", DefaultValue = MyLineStyle.Dashed)]
		public MyLineStyle Line8Style { get; set; }
		[Parameter("Line 8 Thickness", DefaultValue = MyLineThickness.One)]
		public MyLineThickness Line8Thickness { get; set; }


		[Parameter("Show Historical Lines?", DefaultValue = false)]
		public bool ShowHistory { get; set; }

		[Parameter("UTC Offset (minutes)", DefaultValue = 60)]
		public int TimezoneOffsetMinutes { get; set; }


		private DateTime lastDrawnDay;
		private LineDrawer drawer;
		private List<LineDefinition> lines;

		protected override void Initialize()
		{
			lastDrawnDay = DateTime.MinValue;
			drawer = new LineDrawer(Chart, TimezoneOffsetMinutes);

			// Inicializace definic čar
			lines = new List<LineDefinition>
			{
				new LineDefinition(1, Line1Time, Line1Color, Line1Style, Line1Thickness),
				new LineDefinition(2, Line2Time, Line2Color, Line2Style, Line2Thickness),
				new LineDefinition(3, Line3Time, Line3Color, Line3Style, Line3Thickness),
				new LineDefinition(4, Line4Time, Line4Color, Line4Style, Line4Thickness),	
				new LineDefinition(5, Line5Time, Line5Color, Line5Style, Line5Thickness),
				new LineDefinition(6, Line6Time, Line6Color, Line6Style, Line6Thickness),
				new LineDefinition(7, Line7Time, Line7Color, Line7Style, Line7Thickness),
				new LineDefinition(8, Line8Time, Line8Color, Line8Style, Line8Thickness)
			};

			var todayUtc = Bars.OpenTimes.LastValue.Date;

			if (ShowHistory)
				DrawHistoricalLines();
			else
			{
				DrawLinesForDay(todayUtc, overwrite: true);
				lastDrawnDay = todayUtc;
			}
		}

		public override void Calculate(int index)
		{
			if (index != Bars.Count - 1) return;

			var todayUtc = Bars.OpenTimes[index].Date;

			if (todayUtc > lastDrawnDay &&
				todayUtc.DayOfWeek != DayOfWeek.Saturday &&
				todayUtc.DayOfWeek != DayOfWeek.Sunday)
			{
				DrawLinesForDay(todayUtc, overwrite: !ShowHistory);
				lastDrawnDay = todayUtc;
			}
		}

		private void DrawLinesForDay(DateTime dayUtc, bool overwrite)
		{
			foreach (var line in lines)
				drawer.Draw(line, dayUtc, overwrite);
		}

		private void DrawHistoricalLines()
		{
			DateTime lastDay = DateTime.MinValue;

			foreach (var barTime in Bars.OpenTimes)
			{
				var dayUtc = barTime.Date;
				if (dayUtc == lastDay) continue;

				if (dayUtc.DayOfWeek == DayOfWeek.Saturday || dayUtc.DayOfWeek == DayOfWeek.Sunday)
					continue;

				DrawLinesForDay(dayUtc, overwrite: false);
				lastDay = dayUtc;
			}
		}
	}
}
