using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GraphGen
{
     public List<string> Create(double[] values, string[] dateLabels, string title, string filePath)
     {
          var plt = new Plot();

          int labelStep = Math.Max(1, values.Length / 15);     
          double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();

          var barPlot = plt.Add.Bars(positions, values);

          barPlot.Color = Colors.SteelBlue;

          var ticks = new List<Tick>();
          
          for (int i = 0; i < dateLabels.Length; i += labelStep)
          {
               ticks.Add(new Tick(i, dateLabels[i]));
          }

          plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks.ToArray());

          plt.Axes.Bottom.TickLabelStyle.Rotation = 45;
          plt.Axes.Bottom.TickLabelStyle.Alignment = Alignment.UpperCenter;
          plt.Axes.Margins(bottom: 0.2);
          
          plt.Title(title);
          plt.YLabel("Preço (R$)");
          plt.XLabel("Data");

          plt.SavePng(filePath, 1200, 600);
          
          return new List<string> { filePath };
     }
}