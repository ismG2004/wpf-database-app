   M S B A M L   `   `   `  ����  	Login_WPF    Login_WPF.Form2   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Form2��5   	   $	��450��6   $	��800��6$   .��5      � .+�5	   
   �� -   5         ��Name$ stringComboBox��5	      $
��Left=�6+   $	��Top=�6F   $��112,124,0,0q�6^    �� DisplayMemberPath$ .��5
       �� SelectedValuePath$ .��6)   $	��144��6h   $��24��6t   ��6?   �� *MyDictionaryKeys+6t   �� 5   
   -   5        $��textId��5   N   $��18��6   $
��Left=�6    $��	44,48,0,0q�6;   $��ID��6\   $	��Top=�6f   $��67��6~   �� 5   
   -   5        $��	textBoxId��5   �   ${�False��6   
������� �� 
IsReadOnly$
 True��66   $��23��6H   $
��Left=�6T   $��
112,45,0,0q�6o   $	��Top=�6�   $	��144��6�   �� 5   
   -   5         ��Name$ textName��5   N   $��18��6   $
��Left=�6    $��	44,87,0,0q�6;   $��	Ad, soyad��6`   $	��Top=�6q   $��67��6�   �� 5   
   -   5         ��Name$ textBoxName��5   �   ${�False��6   
�������$
 True��66   $��23��6H   $
��Left=�6T   $��
112,84,0,0q�6o   $	��Top=�6�   $	��144��6�   �� 5   
   -   5        $ textDate��5   O   $��18��6   $
��Left=�6    $��
326,51,0,0q�6;   $
��Date��6a   $	��Top=�6m   $��67��6�   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ 
datePicker��5         SelectedDateFormat$ Short=�6)   	   IsTodayHighlighted$
	 True��5      
   FirstDayOfWeek$
 Monday=�5      $��379,43,177,338q�6-   $
��Left=�5      $	��Top=�60   5      �� 5   
   -   5        $ textType��5   O   $��18��6   $
��Left=�6    $��
44,127,0,0q�6;   $��Əmr növü��6a   $	��Top=�6q   $��67��6�   �� 5!   
   -	   5        $��Generate��5!   `   $��Generate��6   $��59��6$   $
��Left=�60   $��654,127,0,0q�6K   $	��Top=�6p   $	��105��6�   6�   &   System.Windows.Controls.DataGrid  5#   
   -
   5          Name$ dataGrid��5#   %      
IsReadOnly$
 True��6      AutoGenerateColumns$
 True��67   $��
0,208,0,10q�6�   �� 5%   
   -   5        $��Refresh��5%      $
��Left=�6#   $	��Top=�6>   $��
654,55,0,0q�6V   $��41��6�   $	��105��6�   5'      5(      