using System;
using System.Collections.Generic;
using System.Text;

namespace Ryerson_GPA_Analyzer
{
    class Semester
    {
        public List<Course> SemesterCourses { get; set; }
        public String SemesterName { get; private set; }
        public double TGPA { get; private set; }
        public double CGPA { get; set; }

        public Semester(String semesterName) 
        {
            SemesterName = semesterName;
            SemesterCourses = new List<Course>();
        }

        /// <summary>
        ///  Add's a Course object to this Semester object
        /// </summary>
        /// <param name="newCourse"></param>
        public void addCourse(Course newCourse) 
        {
            SemesterCourses.Add(newCourse);
        }

        /// <summary>
        /// Updates the TGPA (Term Grade Point Average)
        /// </summary>
        public void updateTGPA() 
        {
            double totalGradePoints = 0;
            double totalWeights = 0;

            foreach (Course course in SemesterCourses) 
            {
                if (course.GPA != -1)
                {
                    totalGradePoints += (course.GPA * course.Weight);
                    totalWeights += course.Weight;
                }
            }

            TGPA =  Math.Round(totalGradePoints / totalWeights , 2);
        }
    }
}
