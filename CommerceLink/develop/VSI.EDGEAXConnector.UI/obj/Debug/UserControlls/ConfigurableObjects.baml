   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.CommonkLclr-namespace:VSI.EDGEAXConnector.Common;assembly=VSI.EDGEAXConnector.CommonVSI.EDGEAXConnector.Common  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5       VSI.EDGEAXConnector.UI>   8VSI.EDGEAXConnector.UI.UserControlls.ConfigurableObjects   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ^localLclr-namespace:VSI.EDGEAXConnector.Common;assembly=VSI.EDGEAXConnector.Common     5      6sys&clr-namespace:System;assembly=mscorlib     5      -   5        
�������.��5      �   � RowDefinitions  5   
   �� 5      $��* �6   �� 5      $��40 �6   .+�5   
   �� $��Configurable Objects��6   $��5,10,5,5q�61   ���	#FF123A70$��Medium"�6Z   .��5      &   System.Windows.Controls.DataGrid  -   5        $��dgConfigurableObjects��5   0   
������ $	��Top=�6N   $��Center=�6f      	RowHeight$ 30Q�6�   $
��Left=�6�      RowDetailsVisibilityMode$ 	Collapsed=�6�      AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns . $��0,5,0,0q�6%   � 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property	 �� Value��%E  '        �� 5      "  6   .�5      ��  � ��	 �#1E5C7067   �� 5       � ��
	 �����67   �� 5       � ��$
	 Bold"�67   �� 5       � ��$	 32��63   �� 5       � ��	 �#A7D8E868   �� 5       � ��$	 0,0,1,0q�6<   �� 5       � ��$	 10,0,0,0q�64   5      5      
   Columns
 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      -   5        	colId��,  &System.Windows.Controls.DataGridColumn   Header$ Id��5   <      Width5  /System.Windows.Controls.DataGridLengthConverter$	 50* 6H      CanUserSort .    SortMemberPath$ Id��6g      SortDirection$ 	AscendingX�6{      CellTemplate 5      �� 5       .R�5!   "   �� $��1q�6)   ._�5"   &   �� .+�5#   *   �� $��11(�6I   ��65   �� *Id+6Y   5$   '   5%   #   5&      5(        5)      -   5        colComValue��$ Commerce Value��5)   B   $
 150* 6Z    5*      �� 5+      .R�5,   "   �� .+�5-   &   �� $��11(�6K   ��61   �� *
ComValue+6[   5.   #   5/         CellEditingTemplate 51      �� 52      .R�53   "   �� $��ComValue��6*   ��6:   �� *
ComValue+ �� UpdateSourceTrigger$ PropertyChanged=�54      56        57      -   5        colErpValue��$ 	ERP Value��57   B   $
 150* 6U    58      �� 59      .R�5:   "   �� .+�5;   &   �� $��11(�6K   ��61   �� *
ErpValue+6[   5<   #   5=       5?      �� 5@      .R�5A   "   �� $��ErpValue��6*   ��6:   �� *
ErpValue+$ PropertyChanged=�5B      5D        5W      -   5        
colOptions��$  ��5W   A   $	 120 6K    5X      �� 5Y      .R�5Z   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5[   &   ��  ��Name$ btnSave��6-   -   5         J� IsManipulationEnabled . $��0,0,10,0q�5[   �   
 � Tag 6T   �� ��5\   *   �� 5]   .    �� 
TargetType" ��6>   .R�5^   2   �� 5_   /   .��5a   *   ��  � Cursor$
 Hand��64    �� Padding$ 0,0,0,0q�6B   $��14(�6T    �� TextAlignment$ Center=�6b   $��Center=�6y   $��Center=�6�   $��24��6�   $��0q�6�   $��24��6�   ��5b   .   �� 5c   2    �� ImageSource$/ )..\Contents\ConnectorImages\save_icon.png��6=    j� Stretch$ Uniform=�6u    j� 
AlignmentX$ Right=�6�    j� 
AlignmentY$ Center=�6�   5e   +   5f   '   5g   #   5h      5j      5l      5m      5n      5o      