   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI<    6VSI.EDGEAXConnector.UI.UserControlls.CommerceLinkFiles   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     
�������.���   � RowDefinitions  �� $��40 ��� $��* �.+�� $��0�� � ColumnDefinitions �� $��* ��� $��* �.+��� $��0��$
��Left=� �� Orientation$ 
Horizontal=�.+��� $��Select Entity��
���   �$��12(�$��Center=�$
��Bold"�$��Center=�$��	10,0,10,0q��� -   $��	cmbEntity�� �� SelectedValuePath$ Content��$��5q�$��32��$	��120��
������ ���#4caab5$��2q�$��14(�
���   �.@��� $��3q� �� 
IsSelected . .��
SalesOrder�� $��1��$��Right=�$ 
Horizontal=�� $��1�� �� $��* ��� $	��120 ��� $��* �.+��� $��0��$��Failed��$��10q����	#FF123A70$��Medium"�.���� $��Disabled=�$��Visible=�.��&   System.Windows.Controls.DataGrid  -   $��dgFailed��
������    SelectionMode$ Extended=�   CanUserAddRows .  $	��Top=�$��Center=�   	RowHeight$ 30Q�$
��Left=�   RowDetailsVisibilityMode$ 	Collapsed=�	   AutoGenerateColumns	 .  
   CanUserSortColumns
 . $��0,5,0,0q�"   VerticalScrollBarVisibility$
 Auto=�$   HorizontalScrollBarVisibility$ Hidden=� � 	Resources =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value��%�   '        �� "  .���  � �� �#1E5C70��  � ��
 �������  � ��$
 Bold"���  � ��$ 32����  � �� �#A7D8E8��  � ��$ 0,0,1,0q���  � ��$ 10,0,0,0q�   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 200*    CanUserSort .    SortMemberPath$
 Name��   SortDirection$ 	AscendingX�   CellTemplate �� .R��� .+��� $��11(����� *Name+  $ DateTime��$
 120*  . $ DateTime��$ 	AscendingX� �� .R��� .+��� $��11(����� *
DateTime+�� $��1��$��Center=�.+��� -   $��btnMoveSelected��$��0��$��Move Selected��   BlueButtonStyle#���   ���#ffffff$��Right=�$	��120��$��32��$��0q��� -   $��
btnMoveAll��$��0��$��Move All >>��  RedButtonStyle#���  ���#ffffff$��Center=�$	��120��$��32��$��0,10,0,0q��� -   $��
btnRefresh��$��0��$��Refresh��  GreenButtonStyle#���  ���#ffffff$��Center=�$	��120��$��32��$��0,10,0,0q��� $��2��$��
Processing��$��10q����	#FF123A70$��Medium"�.���� $��Disabled=�$��Visible=�.��  -   $��dgProcessed��
������ $ Extended=� .  $	��Top=�$��Center=�$ 30Q�$
��Left=�$ 	Collapsed=�	 .  
 . $��0,5,0,0q�$
 Auto=�$ Hidden=� ��%�   '        �� "  .���  � �� �#1E5C70��  � ��
 �������  � ��$
 Bold"���  � ��$ 32����  � �� �#A7D8E8��  � ��$ 0,0,1,0q���  � ��$ 10,0,0,0q�   $
 Name��$
 200*  . $
 Name��$ 	AscendingX� �� .R��� .+��� $��11(����� *Name+  $ DateTime��$
 120*  . $ DateTime��$ 	AscendingX� �� .R��� .+��� $��11(����� *
DateTime+