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
        private int selectedCoursePanelIndex = -1;
        private int userAddedCourses;
        private int userAddedSemesters;
        private String[] letterGrades = { "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F", "PSD", "CRD", "NCR" };

        private List<Course> allCourses = new List<Course>();
        private List<Semester> allSemesters = new List<Semester>();
        private ChartManager chartManager;

        public MainWindow()
        {
            InitializeComponent();

            userAddedCourses = 0;
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
                    allSemesters[semesterCount - 1].SemesterCourses.Add(newCourse);
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
            if (selectedSemester != null)
                displaySemesterInfo(selectedSemester);
        }

        /// <summary>
        /// Displays all the courses and their relevant information, for the selected Semester
        /// </summary>
        /// <param name="selectedSemester"></param>
        private void displaySemesterInfo(Semester selectedSemester) 
        {
            stkCourseInfoArea.Tag = selectedSemester;

            stkCourseInfoArea.Children.Clear();
            foreach (Course course in selectedSemester.SemesterCourses)
            {
                createCoursePanel(course);
            }

            selectLastCoursePanel();
        }

        /// <summary>
        ///  Creates a stackpanel that has relevant information about the course parameter
        ///  Adds this stackpanel to the course info area
        /// </summary>
        /// <param name="course"></param>
        private void createCoursePanel(Course course) 
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
            courseBorder.MouseLeftButtonDown += courseBorder_MouseLeftButtonDown;

            setDynamicCoursePanelInfo(course, lblCourseGPA, coursePanel);

            //Adds the border (and hence all its children), to the course info area
            stkCourseInfoArea.Children.Add(courseBorder);
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
            updateVisuals();
        }

        private void courseBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            Border courseBorder = (Border)sender;
            changeSelectedCoursePanel(courseBorder);
        }

        private void changeSelectedCoursePanel(Border courseBorder) 
        {
            if (selectedCoursePanelIndex != -1)
            {
                Border oldSelectedBorder = (Border)stkCourseInfoArea.Children[selectedCoursePanelIndex];
                oldSelectedBorder.BorderThickness = new Thickness(1);
            }

            courseBorder.BorderThickness = new Thickness(3);
            selectedCoursePanelIndex = stkCourseInfoArea.Children.IndexOf(courseBorder);
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
            else if (course.Grade.StartsWith("C") && course.Grade != "CRD")
                coursePanel.Background = new SolidColorBrush(Colors.LightPink);
            else if (course.Grade.StartsWith("D"))
                coursePanel.Background = new SolidColorBrush(Colors.LightSalmon);
            else
                coursePanel.Background = new SolidColorBrush(Colors.White);
        }

        private void updateVisuals() 
        {
            dgGrades.Items.Refresh();
            chartManager.updateGraphSeries(allSemesters);
        }

        private void selectLastCoursePanel(bool isAdd = false) 
        {
            if (isAdd == false)
                selectedCoursePanelIndex = -1;
            Border lastCourseBoder = (Border)stkCourseInfoArea.Children[stkCourseInfoArea.Children.Count - 1];
            changeSelectedCoursePanel(lastCourseBoder);
        }

        private Course createGenericCourse(Semester thisSem) 
        {
            userAddedCourses += 1;
            string courseCode = "RYE " + userAddedCourses.ToString("D3");
            Course newCourse = new Course(courseCode, "Future Course", thisSem, "A+", 1);
            
            return newCourse;
        }

        private void btnAddCourse_Click(object sender, RoutedEventArgs e)
        {
            if (dgGrades.SelectedIndex != -1)
            {
                Semester thisSem = (Semester)dgGrades.SelectedItem;
                Course newCourse = createGenericCourse(thisSem);

                thisSem.SemesterCourses.Add(newCourse);
                createCoursePanel(newCourse);

                thisSem.updateTGPA();
                setCGPA();

                selectLastCoursePanel(true);
                updateVisuals();
            }
        }

        private void btnDeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            Semester selectedSemester = (Semester)stkCourseInfoArea.Tag;
            Course deletedCourse = selectedSemester.SemesterCourses[selectedCoursePanelIndex];

            if (chkRealDeletion.IsChecked == false && !deletedCourse.CourseCode.StartsWith("RYE"))
            {
                MessageBox.Show("Cannot be deleted!\nEnable the checkbox, to allow deletion of a real course","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            else if (stkCourseInfoArea.Children.Count == 1)
            {
                MessageBox.Show("Cannot be deleted.\nA semester must have at least ONE course!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            
            stkCourseInfoArea.Children.RemoveAt(selectedCoursePanelIndex);
            selectedSemester.SemesterCourses.RemoveAt(selectedCoursePanelIndex);

            selectLastCoursePanel();

            selectedSemester.updateTGPA();
            setCGPA();
            updateVisuals();
        }

        private void btnAddSemester_Click(object sender, RoutedEventArgs e)
        {
            userAddedSemesters += 1;
            Semester newSem = new Semester("Sem " + userAddedSemesters.ToString("D3"));
            allSemesters.Add(newSem);

            for (int i = 0; i < 5; i++)
            {
                Course newCourse = createGenericCourse(newSem);
                newSem.SemesterCourses.Add(newCourse);
            }

            newSem.updateTGPA();
            setCGPA();
            chartManager.addObservablePoints(newSem.TGPA, newSem.CGPA, newSem.SemesterName);
            updateVisuals();
            dgGrades.SelectedIndex = allSemesters.Count-1;
        }

        private void btnDeleteSemester_Click(object sender, RoutedEventArgs e)
        {
            if (dgGrades.SelectedIndex != -1)
            {
                Semester deletedSem = (Semester)dgGrades.SelectedValue;

                if (chkRealDeletion.IsChecked == false && !deletedSem.SemesterName.StartsWith("Sem"))
                {
                    MessageBox.Show("Cannot be deleted!\nEnable the checkbox, to allow deletion of a real Semester", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                allSemesters.RemoveAt(dgGrades.SelectedIndex);
                chartManager.removeObservablePoints(dgGrades.SelectedIndex);
                setCGPA();
                updateVisuals();
            }
        }
    }
}
