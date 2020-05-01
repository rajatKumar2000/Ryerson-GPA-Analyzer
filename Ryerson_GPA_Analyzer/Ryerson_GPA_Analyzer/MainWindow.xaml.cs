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

namespace Ryerson_GPA_Analyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Course> allCourses = new List<Course>();
        private List<Semester> allSemesters = new List<Semester>();

        public MainWindow()
        {
            InitializeComponent();

            ReadCoursesFile();
            setCGPA();
            dgGrades.ItemsSource = allSemesters;

            String temp = "";
            foreach (Semester item in allSemesters)
            {
                temp += item.SemesterName + " " + item.CGPA + " \n";
            }

            lblTemp.Content = temp;
        }

        /// <summary>
        ///  Reads the courses.txt file, and parses the data, creating all the required Courses and Semester objects 
        /// </summary>
        private void ReadCoursesFile() 
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
        /// Sets the CGPA for all Semesters
        /// </summary>
        private void setCGPA() 
        {
            double totalGradePoints = 0;
            double totalWeights = 0;

            for (int i = allSemesters.Count; i > 0; i--) //For loop counts down, cause latest semester appears first in list
            {
                foreach (Course course in allSemesters[i-1].SemesterCourses)
                {
                    if (course.GPA != -1) //-1 means the grade is not from A+ to F. (Ex. PSD)
                    {
                        totalGradePoints += (course.GPA * course.Weight);
                        totalWeights += course.Weight;
                    }
                }

                allSemesters[i-1].CGPA = Math.Round(totalGradePoints / totalWeights, 2);
            }
        }
    }
}
