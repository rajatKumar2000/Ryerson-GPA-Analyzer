   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf   Ryerson_GPA_Analyzer%   Ryerson_GPA_Analyzer.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     
�������  � Title$  Ryerson GPA Analyzer��$	��470��$	��930��.���  � ColumnDefinitions �� $	��15* ��� $��7 ��� $��8* � � RowDefinitions �� $��2* ��� $��7 ��� $��* �.+��� $
��Auto=�$
��True��$��0��$��0��.����  �� CornerRadius$ 7��$��3q�
���   �$��2q�._�#   LiveCharts.Wpf.CartesianChart  -   $��lvcGraph��$��Stretch=�$��Stretch=�$��5q�&    LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Right=��� $��1��$��0��$��3��$��Stretch=�$��0,0,0,0q��� $
��Auto=�$
��True��$��2��$��0��.���� $ 7��$��3q�
���   �$��2q�._��� -   $��stkTableArea��.+�&   System.Windows.Controls.DataGrid  -   $��dgGrades��   AutoGenerateColumns$ False��   
IsReadOnly$
 True��$��4,6,4,4q�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Semester��	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 3* 1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 �� *SemesterName+  $
 TGPA��$	 2* 
 �� *TGPA+  $
 CGPA��$	 2* 
 �� *CGPA+�� $
��Auto=�$
��True��$��2��$��2��.���� $ 7��$��3q�
���   �$��2q�._��  �� $��* ��� $��* � �� $��* ��� $��* ��� $��* �.+��� -   $��btnAddCourse��$��0��$��0��$��5q�$��
Add Course���� -   $��btnDeleteCourse��$��0��$��1��$��5q�$��Delete Course���� -   $��btnAddSemester��$��1��$��0��$��5q�$��Add Semester���� -   $��btnDeleteSemester��$��1��$��1��$��5q�$��Delete Semester���� -   $��chkRealDeletion��$��0��$��2��$��2��$��	3,10,3,10q�$-��'Allow deletion of real Courses/Semester���� $��0��$��1��$��3��$��Stretch=�$��0,0,0,0q��� $
��Auto=�$��Disabled=�$
��True��$��0��$��2��.���� $ 7��$��3q�
���   �$��2q�._��� -	   $��stkCourseInfoArea�� �� Orientation$ 
Horizontal=�