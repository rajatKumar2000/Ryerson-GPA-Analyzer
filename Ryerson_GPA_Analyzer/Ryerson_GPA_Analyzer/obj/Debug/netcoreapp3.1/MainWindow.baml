   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	    Ryerson_GPA_Analyzer%   Ryerson_GPA_Analyzer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   
�������  � Title$  Ryerson GPA Analyzer��5	   	   $	��470��6&   $	��930��63   .��5
      �  � ColumnDefinitions 5   
   �� 5      $	��15* �6   �� 5      $��7 �6   �� 5      $��8* �6    � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $��7 �6   �� 5      $��* �6   .+�5   
   �� $
��Auto=�6   $
��True��6;   $��0��6S   $��0��6c   .��5      ��  �� CornerRadius$ 7��6   $��3q�6'   
���   �$��2q�6F   ._�5      #   LiveCharts.Wpf.CartesianChart  -   5        $��lvcGraph��5   %   $��Stretch=�65   $��Stretch=�6S   $��5q�6o   &    LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Right=�6z   5      5      �� 5   
   $��1��6   $��0��6'   $��3��64   $��Stretch=�6E   $��0,0,0,0q�6c   6v   �� 5    
   $
��Auto=�6   $
��True��6:   $��2��6R   $��0��6b   .��5!      �� $ 7��6   $��3q�6'   
���   �$��2q�6F   ._�5"      �� -   5        $��stkTableArea��5"      .+�5#      &   System.Windows.Controls.DataGrid  -   5        $��dgGrades��5#         AutoGenerateColumns$ False��6/      
IsReadOnly$
 True��6K   $��4,6,4,4q�6]      Columns 5$      0  *System.Windows.Controls.DataGridTextColumn  5%      ,  &System.Windows.Controls.DataGridColumn   Header$ Semester��61   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 3* 6d   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6C   �� *SemesterName+6d     5&      $
 TGPA��61   $	 2* 6X   
 6?   �� *TGPA+6X     5'      $
 CGPA��61   $	 2* 6X   
 6?   �� *CGPA+6X   5)      5*      5+      5,      �� 5.   
   $
��Auto=�6   $
��True��6:   $��2��6R   $��2��6b   .��5/      �� $ 7��6   $��3q�6'   
���   �$��2q�6F   ._�50      �  52      �� 53      $��* �6+   �� 54      $��* �6+    57      �� 58      $��* �6(   �� 59      $��* �6(   �� 5:      $��* �6(   .+�5=      �� -   5        $��btnAddCourse��5=   E   $��0��6   $��0��6-   $��5q�6:   $��
Add Course��6Y   6�   �� 5>      -   5        $��btnDeleteCourse��5>   E   $��0��6   $��1��6-   $��5q�6:   $��Delete Course��6\   6�   �� 5?      -   5        $��btnAddSemester��5?   E   $��1��6   $��0��6-   $��5q�6:   $��Add Semester��6[   6�   �� 5@      -   5        $��btnDeleteSemester��5@   E   $��1��6   $��1��6-   $��5q�6:   $��Delete Semester��6^   6�   �� 5A      -   5        $��chkRealDeletion��5A      $��0��66   $��2��6F   $��2��6S   $��	3,10,3,10q�6g   $-��'Allow deletion of real Courses/Semester��6z   6�   5B      5C      5D      �� 5F   
   $��0��6   $��1��6'   $��3��64   $��Stretch=�6H   $��0,0,0,0q�6f   6y   �� 5H   
   $
��Auto=�6   $��Disabled=�6=   $
��True��6d   $��0��6|   $��2��6�   .��5I      �� $ 7��6   $��3q�6&   
���   �$��2q�6E   ._�5J      �� -	   5        $��stkCourseInfoArea��5J       �� Orientation$ 
Horizontal=�66   5K      5L      5M      5N      5O      