   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI:    4VSI.EDGEAXConnector.UI.UserControlls.EmailSubscriber   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        
�������.��5      �   � RowDefinitions  5	   
   �� 5
      $��40 �6   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� -   5        $��	btnAddNew��5      $��Add New��6      BlueButtonStyle#���   6@   ���#ffffff$��Right=�6�   $	��100��6�   $��32��6�   $��	10,0,10,0q�6�   5      �� 5   
   $��1��6   $��Email Subscribers��6    $��5,10,5,5q�6;   ���	#FF123A70$��Medium"�6d   .��5      �� $��Visible=�6   $��Visible=�6C   .��5      &   System.Windows.Controls.DataGrid  -   5        $��dgEmailSubscribers��5   4   
������    CanUserAddRows .  $	��Top=�6f   $��Center=�6~      	RowHeight$ 30Q�6�   $
��Left=�6�      RowDetailsVisibilityMode$ 	Collapsed=�6�      AutoGenerateColumns .     CanUserSortColumns . $��0,5,0,0q�6&  "   VerticalScrollBarVisibility$
 Auto=�67  $   HorizontalScrollBarVisibility$
 Auto=�6Z  	 � 	Resources	 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader
 �� 
TargetType �� Property �� Value��%E  '        �� 5      "
  6    .�5      ��  � �� �#1E5C706;   �� 5       � ��
 �����6;   �� 5       � ��$
 Bold"�6;   �� 5       � ��$ 32��67   �� 5       � �� �#A7D8E86<   �� 5       � ��$ 0,0,1,0q�6@   �� 5       � ��$ 10,0,0,0q�68   5      5         Columns 5       4  .System.Windows.Controls.DataGridTemplateColumn  5!      -   5        	colId��,  &System.Windows.Controls.DataGridColumn   Header$ Id��5!   @      Width5  /System.Windows.Controls.DataGridLengthConverter$ 75 6L      CanUserSort .    SortMemberPath$ Id��6j      SortDirection$ 	AscendingX�6~      CellTemplate 5"      �� 5#   "   .R�5$   &   �� $��1q�6-   ._�5%   *   �� .+�5&   .   �� $��11(�6M   ��69   �� *Id+6]   5'   +   5(   '   5)   #   5+        5,      -   5        colEmail��$ Email��5,   C   $	 250 6R    . $ Email��6q   $ 	AscendingX�6�    5-      �� 5.   "   .R�5/   &   �� .+�50   *   �� $��11(�6L   ��65   �� *Email+6\   51   '   52   #   54        55      -   5        colName��$
 Name��55   B   $	 200 6P    . $
 Name��6o   $ 	AscendingX�6�    56      �� 57   "   .R�58   &   �� .+�59   *   �� $��11(�6K   ��65   �� *Name+6[   5:   '   5;   #   5=        5>      -   5        colIsActive��$ Active��5>   F   $	 120 6V    . $ IsActive��6v   $ 	AscendingX�6�    5?      �� 5@   "   .R�5A   &   �� .+�5B   *   �� $��11(�6O   ��65   �� *
IsActive+6_   5C   '   5D   #   5F        5G      -   5        
colOptions��$  ��5G   E   $	 120 6O    . $ IsActive��6o   $ 	AscendingX�6�    5H      �� 5I   "   .R�5J   &   �� $ 
Horizontal=�61   .+�5K   *   ��  ��Name$ btn_Edit��61   -	   5         J� IsManipulationEnabled . $��0,0,10,0q�5K   �   
 � Tag 6C   �� ��5L   .   �� 5M   2    �� 
TargetType" ��6B   .R�5N   6   �� 5O   3   .��5Q   .   ��  � Cursor$
 Hand��68    �� Padding$ 0,0,0,0q�6F   $��14(�6X    �� TextAlignment$ Center=�6f   $��Center=�6}   $��Center=�6�   $��24��6�   $��0q�6�   $��24��6�   ��5R   2   �� 5S   6    �� ImageSource$/ )..\Contents\ConnectorImages\edit_icon.png��6A    j� Stretch$ Uniform=�6y    j� 
AlignmentX$ Right=�6�    j� 
AlignmentY$ Center=�6�   5U   /   5V   +   �� 5W   *   $ 
btn_Delete��61   -
   5         . $��0,0,10,0q�5W   �    6E   �� ��5X   .   �� 5Y   2   " ��6B   .R�5Z   6   �� 5[   3   .��5]   .   �� $
 Hand��68   $ 0,0,0,0q�6F   $��14(�6X   $ Center=�6f   $��Center=�6}   $��Center=�6�   $��24��6�   $��0q�6�   $��24��6�   ��5^   2   �� 5_   6   $1 +..\Contents\ConnectorImages\delete_icon.png��6A   $
 Fill=�6{   $ Right=�6�   $ Center=�6�   5a   /   5b   +   5c   '   5d   #   5f      5h      5i      5j      5k      5l      