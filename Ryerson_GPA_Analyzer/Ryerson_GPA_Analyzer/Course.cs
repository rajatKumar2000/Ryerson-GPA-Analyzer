using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Ryerson_GPA_Analyzer
{
    class Course
    {
        public String CourseCode { get; set; }
        public String CourseName { get; private set; }
        public Semester Semester { get; set; }
        public String Grade { get; set; }
        public double Weight { get; private set; }
        public double GPA { get; private set; }

        public Course(String courseCode, String courseName, Semester semester, String grade, double weight) 
        {
            CourseCode = courseCode;
            CourseName = courseName;
            Semester = semester;
            Grade = grade;
            Weight = weight;

            setGPA();
        }

        public void setGPA() 
        {
            String[] letterGrades = {"A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F" };
            double[] gradeValues = {4.33, 4, 3.67, 3.33, 3, 2.67, 2.33, 2, 1.67, 1.33, 1, 0.67, 0};

            int indexVal = Array.IndexOf(letterGrades, Grade);

            if (indexVal == -1) //So things like PSD/CR/NCR don't affect GPA
                GPA = -1;
            else
                GPA = gradeValues[indexVal];
        }
    }
}
