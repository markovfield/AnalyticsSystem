using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultivariateStatistics
{
    public struct DescriptiveStat
    {
        public double max, min, sum, avg, var, stddev, median;
        public string range;
    }
    public static class Mathtool
    {

        public static DescriptiveStat GetDescriptiveStat(List<double> data)
        {
            DescriptiveStat result = new DescriptiveStat();
            double sum = 0, sqSum = 0, max = data[0], min = data[0];
            foreach (double x in data)
            {
                sum += x;
                sqSum += Math.Pow(x, 2);
                if (x > max)
                    max = x;
                if (x < min)
                    min = x;
            }
            int n = data.Count;
            result.sum = sum;
            result.max = max;
            result.min = min;
            result.avg = sum / n;
            result.var = (sqSum - n * Math.Pow(result.avg, 2)) / n;
            result.stddev = Math.Sqrt(result.var);
            result.range = "(" + min + ", " + max + ")";
            data.Sort();
            if (data.Count%2==0)
            {
                result.median = (data[data.Count / 2 - 1] + data[data.Count / 2]) / 2;
            }
            else
            {
                result.median = data[data.Count / 2];
            }
            return result;
        }

        public static double GetCoefficient(double[][] data, int i, int j)
        {
            double sumX = 0, sumY = 0, sumXY = 0, sqSumX = 0, sqSumY = 0;
            int h = data.Length;
            int w = data[0].Length;
            if (i >= w || j >= w)
                throw new Exception("Variables are incorrect");
            for (int k = 0; k < h; k++)
            {
                sumX += data[k][i];
                sqSumX += Math.Pow(data[k][i], 2);
                sumY += data[k][j];
                sqSumY += Math.Pow(data[k][j], 2);
                sumXY += data[k][i] * data[k][j];
            }
            return (h * sumXY - sumX * sumY) / Math.Sqrt(h * sqSumX - Math.Pow(sumX, 2)) * Math.Sqrt(h * sqSumY - Math.Pow(sumY, 2));
        }
    }
}
