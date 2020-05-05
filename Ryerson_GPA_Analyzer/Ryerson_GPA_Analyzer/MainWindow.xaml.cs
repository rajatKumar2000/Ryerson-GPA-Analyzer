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

            while ((line = file.ReadLine()) != null) //Fills out the relevant information from the courses.txt file
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
                else if (count == 5) //6th line of file is garbage ("TakenTaken"), so we use this spot to create any Semester/Course objects
                {
                    if (!oldSemester.Equals(semester)) //if true, creates a new Semester
                    {
                        if (semesterCount > 0)
                        {
                            allSemesters[semesterCount - 1].updateTGPA();
                        }

                        allSemesters.Add(new Semester(semester));
                        oldSemester = semester;
                        semesterCount++;
                    }

                    //Creates new Course, and links the relevant Course and Semester objects
                    Course newCourse = new Course(courseCode, courseName, allSemesters[semesterCount - 1], grade, weight);
                    allSemesters[semesterCount - 1].addCourse(newCourse);
                    allCourses.Add(newCourse);

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

            for (int i = 0; i < allSemesters.Count; i++) 
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

        /// <summary>
        /// Event occurs when user clicks a new Semester cell from datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgGrades_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Semester selectedSemester = (Semester) dgGrades.SelectedItem;
            displaySemesterInfo(selectedSemester);
        }

        /// <summary>
        /// Displays a single Semester's courses. Specifically it shows the course code,
        /// the letter grade achieved, and the grades credit value
        /// </summary>
        private void displaySemesterInfo(Semester selectedSemester) 
        {
            stkCourseInfoArea.Children.Clear();
            String[] letterGrades = { "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F", "PSD", "CR", "NCR" };

            foreach (Course course in selectedSemester.SemesterCourses)
            {
                //Course information is displayed with the following elements
                StackPanel coursePanel = new StackPanel();
                ComboBox cboCourseGrade = new ComboBox();
                Label lblCourseCode = new Label();
                Label lblCourseGPA = new Label();

                //Set the children for the Stackpanel
                coursePanel.Children.Add(lblCourseCode);
                coursePanel.Children.Add(cboCourseGrade);
                coursePanel.Children.Add(lblCourseGPA);

                //Set any useful tags
                cboCourseGrade.Tag = coursePanel;
                coursePanel.Tag = course;

                //Set properties for label displaying a course's GPA
                lblCourseGPA.HorizontalContentAlignment = HorizontalAlignment.Center;
                lblCourseGPA.HorizontalAlignment = HorizontalAlignment.Center;
                lblCourseGPA.VerticalAlignment = VerticalAlignment.Bottom;
                lblCourseGPA.FontWeight = FontWeights.Bold;
                lblCourseGPA.Margin = new Thickness(3);

                //Set properties for label displaying a course's credit
                lblCourseCode.HorizontalContentAlignment = HorizontalAlignment.Center;
                lblCourseCode.HorizontalAlignment = HorizontalAlignment.Center;
                lblCourseCode.VerticalAlignment = VerticalAlignment.Top;
                lblCourseCode.Content = course.CourseCode;
                lblCourseCode.FontWeight = FontWeights.Bold;
                lblCourseCode.Margin = new Thickness(3);

                //Set properties for combobox that allows user to select a course's grade
                cboCourseGrade.HorizontalAlignment = HorizontalAlignment.Center;
                cboCourseGrade.VerticalAlignment = VerticalAlignment.Center;
                cboCourseGrade.ItemsSource = letterGrades;
                cboCourseGrade.SelectedItem = course.Grade;
                cboCourseGrade.Margin = new Thickness(3);
                cboCourseGrade.SelectionChanged += cboCourseGrade_SelectionChanged;

                //Set properties of the border of the StackPanel
                Border courseBorder = new Border();
                courseBorder.CornerRadius = new CornerRadius(4);
                courseBorder.Margin = new Thickness(5);
                courseBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                courseBorder.BorderThickness = new Thickness(1);
                courseBorder.Child = coursePanel;

                setDynamicCoursePanelInfo(course, lblCourseGPA, coursePanel);

                //Adds the border (and hence all its children), to the course info area
                stkCourseInfoArea.Children.Add(courseBorder);
            }
        }

        /// <summary>
        /// Event occurs when user changes the selection of a course's grade from the course info area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboCourseGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Gets relevant information from sender and tags
            ComboBox cboThis = (ComboBox)sender;
            StackPanel coursePanel = (StackPanel)cboThis.Tag;
            Course course = (Course)coursePanel.Tag;

            //Change's grade and gpa of course
            course.Grade = (string)cboThis.SelectedValue;
            course.setGPA();

            Label lblCourseGPA = (Label)coursePanel.Children[2];
            setDynamicCoursePanelInfo(course, lblCourseGPA, coursePanel);
            
            course.Semester.updateTGPA();
            setCGPA();
            dgGrades.Items.Refresh();
            chartManager.updateGraphSeries(allSemesters);
        }

        private void setDynamicCoursePanelInfo(Course course, Label lblCourseGPA, StackPanel coursePanel) 
        {
            //Set's content (text) for label displaying the course's GPA
            if (course.GPA != -1)
                lblCourseGPA.Content = course.GPA;
            else
                lblCourseGPA.Content = "N/A";

            //Set's the color of the StackPanel in accordance to the grade value of the associated course
            if (course.Grade.StartsWith("A"))
                coursePanel.Background = new SolidColorBrush(Colors.LightGreen);
            else if (course.Grade.StartsWith("B"))
                coursePanel.Background = new SolidColorBrush(Colors.LightSteelBlue);
            else if (course.Grade.StartsWith("C"))
                coursePanel.Background = new SolidColorBrush(Colors.LightPink);
            else if (course.Grade.StartsWith("D"))
                coursePanel.Background = new SolidColorBrush(Colors.LightSalmon);
        }
    }
}
