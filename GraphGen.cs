using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GraphGen
{
     public List<string> Create(double[] valores, string[] labelsX, string titulo, string caminhoArquivo)
     {
          if (valores == null || valores.Length == 0)
               throw new ArgumentException("O array de valores não pode estar vazio.");

          if (labelsX == null || labelsX.Length != valores.Length)
               throw new ArgumentException("O tamanho dos labels do eixo X deve ser igual ao tamanho dos valores.");

          var plt = new ScottPlot.Plot();

          // Create a bar plot with positions (0, 1, 2, ...) and values
          var bars = plt.Add.Bars(valores);

          // Manually set X-axis tick positions and labels
          plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
               positions: Enumerable.Range(0, labelsX.Length).Select(x => (double)x).ToArray(),
               labels: labelsX
          );

          // Title and axis labels
          plt.Title(titulo);
          plt.YLabel("Valor");
          plt.XLabel("Data");

          // Adjust margins to prevent label cutoff
          plt.Axes.Margins(bottom: 0.1);

          // Save the image
          plt.SavePng(caminhoArquivo, 800, 600);

          return new List<string> { caminhoArquivo };
     }
}