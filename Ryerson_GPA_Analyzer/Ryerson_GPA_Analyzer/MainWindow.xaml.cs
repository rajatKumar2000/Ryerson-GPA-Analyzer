using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace Ryerson_GPA_Analyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Course> allCourses = new List<Course>();
        private List<Semester> allSemesters = new List<Semester>();
        private ChartManager chartManager;

        public MainWindow()
        {
            InitializeComponent();

            chartManager = new ChartManager();

            readCoursesFile();
            allSemesters.Reverse(); //So allSemesters[0] is first sem, instead of your last sem
            setCGPA();
            dgGrades.ItemsSource = allSemesters;
            dgGrades.SelectedIndex = 0;
            chartManager.createLineGraph(allSemesters, lvcGraph); //Creates the visual graph
        }

        /// <summary>
        ///  Reads the courses.txt file, and parses the data, creating all the required Courses and Semester objects 
        /// </summary>
        private void readCoursesFile() 
        {
            StreamReader file = new StreamReader("courses.txt");
            String line;
            int count = 0;
            int semesterCount = 0;
            String oldSemester = "";
            
            String courseCode = "";
            String courseName = "";
            String semester = "";
            String grade = "";
            double weight = 0;

            while ((line = file.ReadLine()) != null)
            {
                if (count == 0)
                    courseCode = line;
                else if (count == 1)
                    courseName = line;
                else if (count == 2)
                    semester = line;
                else if (count == 3)
                    grade = line;
                else if (count == 4)
                    weight = Double.Parse(line);
                else if (count == 5) 
                {
                    Course newCourse = new Course(courseCode, courseName, semester, grade, weight);
                    allCourses.Add(newCourse);

                    if (!oldSemester.Equals(semester))
                    {
                        if (semesterCount > 0)
                        {
                            allSemesters[semesterCount - 1].updateTGPA();
                        }

                        allSemesters.Add(new Semester(semester));
                        allSemesters[semesterCount].addCourse(newCourse);
                        oldSemester = semester;
                        semesterCount++;
                    }
                    else
                        allSemesters[semesterCount - 1].addCourse(newCourse);

                    count = -1;
                }
                count++;
            }

            //Have to update the last semester GPA's, as it skips inside while loop
            allSemesters[semesterCount - 1].updateTGPA(); 
        }

        /// <summary>
        /// Calculates and sets the CGPA for all Semesters
        /// </summary>
        private void setCGPA() 
        {
            double totalGradePoints = 0;
            double totalWeights = 0;

            for (int i = 0; i < allSemesters.Count; i++) //For loop counts down, cause latest semester appears first in list
            {
                foreach (Course course in allSemesters[i].SemesterCourses)
                {
                    if (course.GPA != -1) //-1 means the grade is not from A+ to F. (Ex. PSD)
                    {
                        totalGradePoints += (course.GPA * course.Weight);
                        totalWeights += course.Weight;
                    }
                }

                allSemesters[i].CGPA = Math.Round(totalGradePoints / totalWeights, 2);
            }
        }

        private void dgGrades_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Semester selectedSemester = (Semester) dgGrades.SelectedItem;
            displaySemesterInfo(selectedSemester);
        }

        /// <summary>
        /// Displays a single Semester's courses. Specifically it shows the course code, the letter grade achieved, and the grades credit value
        /// </summary>
        private void displaySemesterInfo(Semester selectedSemester) 
        {
            stkCourseInfoArea.Children.Clear();

            foreach (Course course in selectedSemester.SemesterCourses)
            {
                StackPanel coursePanel = new StackPanel();
                Label lblCourseCode = new Label();
                Label lblCourseGrade = new Label();
                Label lblCourseGPA = new Label();

                lblCourseCode.Content = course.CourseCode;
                lblCourseGrade.Content = course.Grade;

                lblCourseCode.FontWeight = FontWeights.Bold;
                lblCourseGrade.Foreground = new SolidColorBrush(Colors.Green);

                if (course.Grade.StartsWith("A"))
                    lblCourseGrade.Foreground = new SolidColorBrush(Colors.DarkGreen);
                else if (course.Grade.StartsWith("B"))
                    lblCourseGrade.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
                else if (course.Grade.StartsWith("C"))
                    lblCourseGrade.Foreground = new SolidColorBrush(Colors.DarkOrange);
                else if (course.Grade.StartsWith("D"))
                    lblCourseGrade.Foreground = new SolidColorBrush(Colors.DarkRed);
                else
                    lblCourseGrade.Foreground = new SolidColorBrush(Colors.Black);

                //(Color)ColorConverter.ConvertFromString("")
                if (course.GPA != -1)
                    lblCourseGPA.Content = course.GPA;
                else
                    lblCourseGPA.Content = "N/A";

                coursePanel.Children.Add(lblCourseCode);
                coursePanel.Children.Add(lblCourseGrade);
                coursePanel.Children.Add(lblCourseGPA);

                coursePanel.Margin = new Thickness(5);

                stkCourseInfoArea.Children.Add(coursePanel);
            }
        }
    }
}
