   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI9    3VSI.EDGEAXConnector.UI.UserControlls.EmailTemplates   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        
�������.��5	      �   � RowDefinitions  5
   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� -   5        $��	btnAddNew��5      $��Add New��6      BlueButtonStyle#���   6@   ���#ffffff$��Right=�6�   $	��100��6�   $��32��6�   $��	10,0,10,0q�6�   5      �� 5   
   $��1��6   $��Email Templates��6    $��5,10,5,5q�69   ���	#FF123A70$��Medium"�6b   .��5      �� $��Visible=�6   $��Visible=�6C   .��5      &   System.Windows.Controls.DataGrid  -   5        $��dgEmailTemplates��5   4   
������    CanUserAddRows .  $	��Top=�6c   $��Center=�6{   $
��Left=�6�      	RowHeight$ 30Q�6�      RowDetailsVisibilityMode$ 	Collapsed=�6�      AutoGenerateColumns .     CanUserSortColumns . $��0,5,0,0q�6#  "   VerticalScrollBarVisibility$
 Auto=�64  $   HorizontalScrollBarVisibility$
 Auto=�6W  	 � 	Resources	 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader
 �� 
TargetType �� Property �� Value��%E  '        �� 5      "
  6    .�5      ��  � �� �#1E5C706;   �� 5       � ��
 �����6;   �� 5       � ��$
 Bold"�6;   �� 5       � ��$ 32��67   �� 5       � �� �#A7D8E86<   �� 5       � ��$ 0,0,1,0q�6@   �� 5       � ��$ 10,0,0,0q�68   5      5          Columns 5!      4  .System.Windows.Controls.DataGridTemplateColumn  5"      -   5        	colId��,  &System.Windows.Controls.DataGridColumn   Header$ Id��5"   @      Width5  /System.Windows.Controls.DataGridLengthConverter$ 75 6L      CanUserSort .    SortMemberPath$ Id��6j      SortDirection$ 	AscendingX�6~      CellTemplate 5#      �� 5$   "   .R�5%   &   �� $��1q�6-   ._�5&   *   �� .+�5'   .   �� $��11(�6M   ��69   �� *Id+6]   5(   +   5)   '   5*   #   5,        5-      -   5        colName��$
 Name��5-   B   $	 200 6P    . $
 Name��6o   $ 	AscendingX�6�    5.      �� 5/   "   .R�50   &   �� .+�51   *   �� $��11(�6K   ��65   �� *Name+6[   52   '   53   #   55        56      -   5        
colSubject��$ Subject��56   E   $	 250 6V    . $ Subject��6u   $ 	AscendingX�6�    57      �� 58   "   .R�59   &   �� .+�5:   *   �� $��11(�6N   ��65   �� *	Subject+6^   5;   '   5<   #   5>        5?      -   5        colBody��$
 Body��5?   B   $	 250 6P    . $
 Body��6o   $ 	AscendingX�6�    5@      �� 5A   "   .R�5B   &   �� .+�5C   *   �� $��11(�6K   ��65   �� *Body+6[   5D   '   5E   #   5G        5H      -   5        	colFooter��$ Footer��5H   D   $	 250 6T    . $ Footer��6s   $ 	AscendingX�6�    5I      �� 5J   "   .R�5K   &   �� .+�5L   *   �� $��11(�6M   ��65   �� *Footer+6]   5M   '   5N   #   5P        5Q      -	   5        colIsActive��$ Active��5Q   F   $	 120 6V    . $ IsActive��6v   $ 	AscendingX�6�    5R      �� 5S   "   .R�5T   &   �� .+�5U   *   �� $��11(�6O   ��65   �� *
IsActive+6_   5V   '   5W   #   5Y        5Z      -
   5        
colOptions��$  ��5Z   E   $	 120 6O    . $ IsActive��6o   $ 	AscendingX�6�    5[      �� 5\   "   .R�5]   &   �� $ 
Horizontal=�61   .+�5^   *   ��  ��Name$ btn_Edit��61   -   5         J� IsManipulationEnabled . $��0,0,10,0q�5^   �   
 � Tag 6Z   �� ��5_   .   �� 5`   2    �� 
TargetType" ��6B   .R�5a   6   �� 5b   3   .��5d   .   ��  � Cursor$
 Hand��68    �� Padding$ 0,0,0,0q�6F   $��14(�6X    �� TextAlignment$ Center=�6f   $��Center=�6}   $��Center=�6�   $��24��6�   $��0q�6�   $��24��6�   ��5e   2   �� 5f   6    �� ImageSource$/ )..\Contents\ConnectorImages\edit_icon.png��6A    j� Stretch$ Uniform=�6y    j� 
AlignmentX$ Right=�6�    j� 
AlignmentY$ Center=�6�   5h   /   5i   +   �� 5j   *   $ 
btn_Delete��61   -   5         . $��0,0,10,0q�5j   �    6^   �� ��5k   .   �� 5l   2   " ��6B   .R�5m   6   �� 5n   3   .��5p   .   �� $
 Hand��68   $ 0,0,0,0q�6F   $��14(�6X   $ Center=�6f   $��Center=�6}   $��Center=�6�   $��24��6�   $��0q�6�   $��24��6�   ��5q   2   �� 5r   6   $1 +..\Contents\ConnectorImages\delete_icon.png��6A   $
 Fill=�6{   $ Right=�6�   $ Center=�6�   5t   /   5u   +   5v   '   5w   #   5y      5{      5|      5}      5~      5      