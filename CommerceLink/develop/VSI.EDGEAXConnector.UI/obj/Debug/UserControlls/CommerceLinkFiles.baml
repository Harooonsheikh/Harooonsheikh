   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI<    6VSI.EDGEAXConnector.UI.UserControlls.CommerceLinkFiles   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      
�������.��5      �   � RowDefinitions  5	   
   �� 5
      $��40 �6   �� 5      $��* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� $��0��6   $
��Left=�6)    �� Orientation$ 
Horizontal=�6D   .+�5      �� $��Select Entity��6   
���   �$��12(�6D   $��Center=�6R   $
��Bold"�6o   $��Center=�6�   $��	10,0,10,0q�6�   �� 5      -   5        $��	cmbEntity��5       �� SelectedValuePath$ Content��6,   $��5q�6H   $��32��6T   $	��120��6`   
������ ���#4caab5$��2q�6�   $��14(�6�   
���   �.@�5      �� $��3q�6#    �� 
IsSelected . .��6A   
SalesOrder6M   5      5      �� 5      $��1��6   $��Right=�6)   $ 
Horizontal=�6E   5      5      � 5   
   $��1��6    5      �� 5      $��* �6#   �� 5       $	��120 �6#   �� 5!      $��* �6#   .+�5#      �� $��0��6   $��Failed��6'   $��10q�67   ���	#FF123A70$��Medium"�6Z   .��5$      �� $��Disabled=�6   $��Visible=�6H   .��5%      &   System.Windows.Controls.DataGrid  -   5        $��dgFailed��5%   8   
������    SelectionMode$ Extended=�6H      CanUserAddRows .  $	��Top=�6y   $��Center=�6�      	RowHeight$ 30Q�6�   $
��Left=�6�      RowDetailsVisibilityMode$ 	Collapsed=�6�   	   AutoGenerateColumns	 .  
   CanUserSortColumns
 . $��0,5,0,0q�69  "   VerticalScrollBarVisibility$
 Auto=�6J  $   HorizontalScrollBarVisibility$ Hidden=�6m   � 	Resources 5&      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value��%E  '        �� 5'      "  6$   .�5(   "   ��  � �� �#1E5C706?   �� 5)   "    � ��
 �����6?   �� 5*   "    � ��$
 Bold"�6?   �� 5+   "    � ��$ 32��6;   �� 5,   "    � �� �#A7D8E86@   �� 5-   "    � ��$ 0,0,1,0q�6D   �� 5.   "    � ��$ 10,0,0,0q�6<   5/      50         Columns 51      4  .System.Windows.Controls.DataGridTemplateColumn  52      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��65      Width5  /System.Windows.Controls.DataGridLengthConverter$
 200* 6C      CanUserSort .    SortMemberPath$
 Name��6c      SortDirection$ 	AscendingX�6y      CellTemplate 53   "   �� 54   &   .R�55   *   �� .+�56   .   �� $��11(�6O   ��69   �� *Name+6_   57   +   58   '   5:        5;      $ DateTime��65   $
 120* 6G    . $ DateTime��6h   $ 	AscendingX�6�    5<   "   �� 5=   &   .R�5>   *   �� .+�5?   .   �� $��11(�6S   ��69   �� *
DateTime+6c   5@   +   5A   '   5C      5E      5F      5G      �� 5H      $��1��6   $��Center=�6)   .+�5I      �� -   5        $��btnMoveSelected��5I   �   $��0��6   $��Move Selected��6'      BlueButtonStyle#���   6]   ���#ffffff$��Right=�6�   $	��120��6�   $��32��6�   $��0q�6�   �� 5J      -   5        $��
btnMoveAll��5J   �   $��0��6   $��Move All >>��6'     RedButtonStyle#���  6V   ���#ffffff$��Center=�6�   $	��120��6�   $��32��6�   $��0,10,0,0q�6�   �� 5K      -   5        $��
btnRefresh��5K   �   $��0��6   $��Refresh��6'     GreenButtonStyle#���  6R   ���#ffffff$��Center=�6�   $	��120��6�   $��32��6�   $��0,10,0,0q�6�   5L      �� 5M      $��2��6   $��
Processing��6'   $��10q�6;   ���	#FF123A70$��Medium"�6^   .��5N      �� $��Disabled=�6   $��Visible=�6H   .��5O        -   5        $��dgProcessed��5O   8   
������ $ Extended=�6K    .  $	��Top=�6|   $��Center=�6�   $ 30Q�6�   $
��Left=�6�   $ 	Collapsed=�6�   	 .  
 . $��0,5,0,0q�6<  $
 Auto=�6M  $ Hidden=�6p   5P      ��%E  '        �� 5Q      "  6$   .�5R   "   ��  � �� �#1E5C706?   �� 5S   "    � ��
 �����6?   �� 5T   "    � ��$
 Bold"�6?   �� 5U   "    � ��$ 32��6;   �� 5V   "    � �� �#A7D8E86@   �� 5W   "    � ��$ 0,0,1,0q�6D   �� 5X   "    � ��$ 10,0,0,0q�6<   5Y      5Z       5[        5\      $
 Name��65   $
 200* 6C    . $
 Name��6c   $ 	AscendingX�6y    5]   "   �� 5^   &   .R�5_   *   �� .+�5`   .   �� $��11(�6O   ��69   �� *Name+6_   5a   +   5b   '   5d        5e      $ DateTime��65   $
 120* 6G    . $ DateTime��6h   $ 	AscendingX�6�    5f   "   �� 5g   &   .R�5h   *   �� .+�5i   .   �� $��11(�6S   ��69   �� *
DateTime+6c   5j   +   5k   '   5m      5o      5p      5q      5r      5s      5t      