using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace Words
{
    class Diagrams : MainWindow
    {
        public static void RoundDiagram(WpfPlot diagram)
        { 
            diagram.Reset();
            double[] values = new double[ListLog.Count];
            string[] word = new string[ListLog.Count];
            for (int i = 0; i < ListLog.Count; i++)
            {
                values[i] = ListLog[i].CountWord;
                word[i] = ListLog[i].Word;
            }

            var pie = diagram.Plot.AddPie(values);
            pie.Explode = true;
            pie.SliceLabels = word;
            pie.ShowLabels = true;
            pie.ShowPercentages = true;
            pie.ShowValues = true;
            diagram.Plot.Legend(true);
            diagram.Refresh();
        }
        
        public static void ColumnDiagram(WpfPlot diagram)
        {
            diagram.Reset();
            double[] values = new double[ListLog.Count];
            double[] positions = new double[ListLog.Count];
            string[] labels = new string[ListLog.Count];
            for (int i = 0; i < ListLog.Count; i++)
            {
                values[i] = ListLog[i].CountWord;
                positions[i] = i;
                labels[i] = ListLog[i].Word;
            }

            var plt = diagram.Plot.AddBar(values, positions);
            diagram.Plot.XTicks(positions, labels);
            diagram.Plot.SetAxisLimits(yMin: 0);
            plt.ShowValuesAboveBars = true;
            diagram.Plot.Legend(true);
            diagram.Refresh();
        }

        public static void RadialDiagram(WpfPlot diagram)
        {
            diagram.Reset();
            diagram.Plot.Palette = ScottPlot.Palette.Nord;
            double[] values = new double[ListLog.Count];
            double[] positions = new double[ListLog.Count];
            string[] labels = new string[ListLog.Count];
            for (int i = 0; i < ListLog.Count; i++)
            {
                values[i] = ListLog[i].CountWord;
                positions[i] = i;
                labels[i] = ListLog[i].Word;
            }

            var plt = diagram.Plot.AddRadialGauge(values); ;
            plt.Labels = labels;
            plt.LabelPositionFraction = 0;
            plt.GaugeMode = RadialGaugeMode.Sequential;
            plt.OrderInsideOut = false;
            diagram.Plot.Legend(true);
            diagram.Refresh();
        }
       
    }
}
