using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Ryerson_GPA_Analyzer
{
    class ChartManager
    {
        private LineSeries tgpaLine;
        private LineSeries cgpaLine;
        private List<ObservableValue> tgpaPoints;
        private List<ObservableValue> cgpaPoints;

        public void createLineGraph(List<Semester> allSemesters, LiveCharts.Wpf.CartesianChart graph) 
        {
            List<string> semDates = new List<string>();

            //initializing important graph values
            tgpaPoints = new List<ObservableValue>();
            cgpaPoints = new List<ObservableValue>();
            tgpaLine = new LineSeries {Title = "TGPA", PointGeometry = DefaultGeometries.Square, PointGeometrySize = 8};
            cgpaLine = new LineSeries {Title = "CGPA", PointGeometrySize = 8};

            foreach (Semester sem in allSemesters)
            {
                semDates.Add(sem.SemesterName);  //probably gonna have to move this its own function, when semesters can be added
                addObservablePoints(sem.TGPA, sem.CGPA);
            }

            tgpaLine.Values = new ChartValues<ObservableValue>(tgpaPoints);
            cgpaLine.Values = new ChartValues<ObservableValue>(cgpaPoints);

            SeriesCollection series = new SeriesCollection
            {
                tgpaLine, cgpaLine
            };

            List<double> y_axis_values = new List<double> { 0, 0.67, 1, 1.33, 1.67, 2, 2.33, 2.67, 3, 3.33, 3.67, 4, 4.33 };
            Func<double, string> y_axis = new Func<double, string>(y_axis_values => y_axis_values.ToString("0.##"));
           
            graph.Series = series;

            graph.AxisX.Add(new Axis { Title = "Semesters", Labels = semDates, });
            graph.AxisY.Add(new Axis { Title = "GPA", LabelFormatter = y_axis});
        }

        public void updateGraphSeries(List<Semester> allSemesters) 
        {
            for (int i = 0; i < allSemesters.Count; i++)
            {
                tgpaPoints[i].Value = allSemesters[i].TGPA;
                cgpaPoints[i].Value = allSemesters[i].CGPA;
            }
        }

        public void addObservablePoints(double tgpa, double cgpa) 
        {
            tgpaPoints.Add(new ObservableValue(tgpa));
            cgpaPoints.Add(new ObservableValue(cgpa));
        }
    }
}
