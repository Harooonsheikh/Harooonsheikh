   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.CommonkLclr-namespace:VSI.EDGEAXConnector.Common;assembly=VSI.EDGEAXConnector.CommonVSI.EDGEAXConnector.Common  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  VSI.EDGEAXConnector.UI>   8VSI.EDGEAXConnector.UI.UserControlls.ConfigurableObjects   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ^localLclr-namespace:VSI.EDGEAXConnector.Common;assembly=VSI.EDGEAXConnector.Common     6sys&clr-namespace:System;assembly=mscorlib     -   
�������.���   � RowDefinitions  �� $��* ��� $��40 �.+��� $��Configurable Objects��$��5,10,5,5q����	#FF123A70$��Medium"�.��&   System.Windows.Controls.DataGrid  -   $��dgConfigurableObjects��
������ $	��Top=�$��Center=�   	RowHeight$ 30Q�$
��Left=�   RowDetailsVisibilityMode$ 	Collapsed=�   AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns . $��0,5,0,0q� � 	Resources =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property	 �� Value��%�   '        �� "  .���  � ��	 �#1E5C70��  � ��
	 �������  � ��$
	 Bold"���  � ��$	 32����  � ��	 �#A7D8E8��  � ��$	 0,0,1,0q���  � ��$	 10,0,0,0q�
   Columns
 4  .System.Windows.Controls.DataGridTemplateColumn  -   	colId��,  &System.Windows.Controls.DataGridColumn   Header$ Id��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 50*    CanUserSort .    SortMemberPath$ Id��   SortDirection$ 	AscendingX�   CellTemplate �� .R��� $��1q�._��� .+��� $��11(����� *Id+  -   colComValue��$ Commerce Value��$
 150*  �� .R��� .+��� $��11(����� *
ComValue+   CellEditingTemplate �� .R��� $��ComValue������ *
ComValue+ �� UpdateSourceTrigger$ PropertyChanged=�  -   colErpValue��$ 	ERP Value��$
 150*  �� .R��� .+��� $��11(����� *
ErpValue+ �� .R��� $��ErpValue������ *
ErpValue+$ PropertyChanged=�  -   
colOptions��$  ��$	 120  �� .R���  �� Orientation$ 
Horizontal=�.+���  ��Name$ btnSave��-    J� IsManipulationEnabled . $��0,0,10,0q�
 � Tag �� ����  �� 
TargetType" ��.R��� .����  � Cursor$
 Hand�� �� Padding$ 0,0,0,0q�$��14(� �� TextAlignment$ Center=�$��Center=�$��Center=�$��24��$��0q�$��24������  �� ImageSource$/ )..\Contents\ConnectorImages\save_icon.png�� j� Stretch$ Uniform=� j� 
AlignmentX$ Right=� j� 
AlignmentY$ Center=�