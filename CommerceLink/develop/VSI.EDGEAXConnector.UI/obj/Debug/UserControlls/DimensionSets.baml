   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI8    2VSI.EDGEAXConnector.UI.UserControlls.DimensionSets   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        .��5      �   � RowDefinitions  5	   
   �� 5
      $��* �6   �� 5      $��40 �6   .+�5   
   �� $��Dimension Sets��6   $��5,10,5,5q�6+   ���	#FF123A70$��Medium"�6T   .��5      &   System.Windows.Controls.DataGrid  -   5        $��dgDimensionSets��5   0   
������ $	��Top=�6H   $��Center=�6`      	RowHeight$ 30Q�6�   $
��Left=�6�      RowDetailsVisibilityMode$ 	Collapsed=�6�      AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns . $��0,5,0,0q�6   � 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property	 �� Value��%E  '        �� 5      "  6   .�5      ��  � ��	 �#1E5C7067   �� 5       � ��
	 �����67   �� 5       � ��$
	 Bold"�67   �� 5       � ��$	 32��63   �� 5       � ��	 �#A7D8E868   �� 5       � ��$	 0,0,1,0q�6<   �� 5       � ��$	 10,0,0,0q�64   5      5      
   Columns
 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      -   5        ID��,  &System.Windows.Controls.DataGridColumn   Header$ Id��5   9      Width5  /System.Windows.Controls.DataGridLengthConverter$	 50* 6E      CanUserSort .    SortMemberPath$ Id��6d      SortDirection$ 	AscendingX�6x      CellTemplate 5      �� 5      .R�5   "   �� $��1q�6)   ._�5   &   �� .+�5    *   �� $��11(�6I   ��65   �� *Id+6Y   5!   '   5"   #   5#      5%        5&      -   5        colErpValue��$ 	ERP Value��5&   B   $
 150* 6U    5'      �� 5(      .R�5)   "   �� .+�5*   &   �� $��11(�6K   ��61   �� *
ErpValue+6[   5+   #   5,         CellEditingTemplate 5.      �� 5/      .R�50   "   �� $��ErpValue��6*   ��6:   �� *
ErpValue+ �� UpdateSourceTrigger$ PropertyChanged=�51      53        54      -   5        colComValue��$ 	Com Value��54   B   $
 150* 6U    55      �� 56      .R�57   "   �� .+�58   &   �� $��11(�6K   ��61   �� *
ComValue+6[   59   #   5:       5<      �� 5=      .R�5>   "   �� $��ComValue��6*   ��6:   �� *
ComValue+$ PropertyChanged=�5?      5A        5B      -   5        colIsActive��$ Active��5B   B   $	 100 6R    . $ IsActive��6r   $ 	AscendingX�6�    5C      �� 5D      .R�5E   "   �� $��11(�6K    d� 	IsChecked 6,   �� *
IsActive+6[   5F       5H      �� 5I      .R�5J   "   �� $��chkIsActive��6+   $��11(�6�    6>   �� *
IsActive+$ PropertyChanged=�
 � Tag 6�   �� 6�   5K      5M        5N      -   5        colAdditionalErpValue��$ Additional Erp Value��5N   L   $
 150* 6j    . $ AdditionalErpValue��6�   $ 	AscendingX�6�    5O      �� 5P      .R�5Q   "   �� .+�5R   &   �� $��11(�6U   ��61   �� *AdditionalErpValue+6e   5S   #   5T       5V      �� 5W      .R�5X   "   �� $��AdditionalErpValue��6*   ��6D   �� *AdditionalErpValue+$ PropertyChanged=�5Y      5[        5\      -   5        
colOptions��$  ��5\   A   $	 120 6K    5]      �� 5^      .R�5_   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5`   &   ��  ��Name$ btnSave��6-   -	   5         J� IsManipulationEnabled . $��0,0,10,0q�5`   �    6T   �� ��5a   *   �� 5b   .    �� 
TargetType" ��6>   .R�5c   2   �� 5d   /   .��5f   *   ��  � Cursor$
 Hand��64    �� Padding$ 0,0,0,0q�6B   $��14(�6T    �� TextAlignment$ Center=�6b   $��Center=�6y   $��Center=�6�   $��24��6�   $��0q�6�   $��24��6�   ��5g   .   �� 5h   2    �� ImageSource$/ )..\Contents\ConnectorImages\save_icon.png��6=    j� Stretch$ Uniform=�6u    j� 
AlignmentX$ Right=�6�    j� 
AlignmentY$ Center=�6�   5j   +   5k   '   5l   #   5m      5o      5q      5r      5s      5t      