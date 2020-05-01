using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts;
using LiveCharts.Wpf;

namespace Ryerson_GPA_Analyzer
{
    class ChartManager
    {
        public void createLineGraph(List<Semester> allSemesters, LiveCharts.Wpf.CartesianChart graph) 
        {
            List<double> tgpaPoints = new List<double>();
            List<double> cgpaPoints = new List<double>();
            List<string> semDates = new List<string>();

            foreach (Semester sem in allSemesters)
            {
                tgpaPoints.Add(sem.TGPA);
                cgpaPoints.Add(sem.CGPA);
                semDates.Add(sem.SemesterName);
            }

            SeriesCollection series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "TGPA",
                    Values = new ChartValues<double>(tgpaPoints)
                },
                new LineSeries 
                {
                    Title = "CGPA",
                    Values = new ChartValues<double>(cgpaPoints)
                }
            };

            List<double> y_axis_values = new List<double> { 0, 0.67, 1, 1.33, 1.67, 2, 2.33, 2.67, 3, 3.33, 3.67, 4, 4.33 };
            Func<double, string> y_axis = new Func<double, string>(y_axis_values => y_axis_values.ToString("0.##"));
           
            graph.Series = series;

            graph.AxisX.Add(new Axis { Title = "Semesters", Labels = semDates, });
            graph.AxisY.Add(new Axis { Title = "GPA", LabelFormatter = y_axis});


            
        }
    }
}
