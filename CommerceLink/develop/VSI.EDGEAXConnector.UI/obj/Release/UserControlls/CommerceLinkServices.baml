   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI?    9VSI.EDGEAXConnector.UI.UserControlls.CommerceLinkServices   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -   
�������.���   � RowDefinitions  �� $	��50* ��� $	��50* �.+�� $��0��  �� $��* �.+��� $��0��$��Services Details��$��	10,5,10,5q����	#FF123A70$��Medium"�.���� .+�&   System.Windows.Controls.DataGrid  -     Name$ gdServiceFields��   AutoGenerateColumns .     GridLinesVisibility$
 None=�   CanUserAddRows .  
������    HeadersVisibility$
 None=�$��0q���Z�    RowStyle ��  �� 
TargetType)  #System.Windows.Controls.DataGridRow"   �� 	Resources 	 �� Color��%  (��       * SystemColors.HighlightBrushKey+)(��      *SystemColors.ControlBrushKey+)(�� .     *$"SystemColors.HighlightTextBrushKey+)(�� ?     *" SystemColors.ControlTextBrushKey+)�� $	 Transparent���� $	 Transparent���� $	 Black���� $	 Black��
   	CellStyle
 �� *  $System.Windows.Controls.DataGridCell"  .���  �� Property � �� �� Value$ 0q���  �@�FocusVisualStyle Z�    Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$	 30*    CanUserSort .     CellTemplate �� .R��� $��Center=�.+��� $��11(�$
��Bold"����	#FF123A70���� *Name+  $	 70*  .   �� .R��� $��Center=�.+��� $��5q�$��3q�
���   �$��Center=����� *Value+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ 	LostFocus=��� -   $��btnSave��$��1��$��1��$
��Save��   BlueButtonStyle#���   ���#ffffff$��Right=�$	��100��$��32��$��	10,10,5,0q��� $��1��$��Services Control��$��10q����	#FF123A70$��Medium"�.���� .+��� -   $��
btnRefresh��$��0��$��Refresh��#���   ���#ffffff$��Right=�$	��100��$��32��$��0q��� $��Visible=�$��Visible=�.��  -   $��	dgService��
������  .  $	��Top=�$��Center=�   	RowHeight$ 30Q�$
��Left=�   RowDetailsVisibilityMode$ 	Collapsed=� .     CanUserSortColumns . $��0,5,0,0q�"   VerticalScrollBarVisibility$
 Auto=�$   HorizontalScrollBarVisibility$
 Auto=� � 	Resources =  7System.Windows.Controls.Primitives.DataGridColumnHeader��%�   '        �� "  .���  � �� �#1E5C70��  � ��
 �������  � ��$
 Bold"���  � ��$ 32����  � �� �#A7D8E8��  � ��$ 0,0,1,0q���  � ��$ 10,0,0,0q�      Header$
 Name��$	 300  �� .R��� .+��� $��11(����� *Name+  $ Status��$	 120  �� .R��� .+��� $��11(����� *Status+  $  ��$	 250  �� .R���  �� Orientation$ 
Horizontal=�.+���  ��Name$ btnStart��-   $��24��$��Start�� J� IsManipulationEnabled . $��10,0,0,0q�$��	10,0,10,0q�
 � Tag �� {��� *	IsStart+�� $ btnStop��-   $��24��$
��Stop�� . $��10,0,0,0q�$��	10,0,10,0q� �� {��� *IsStop+�� $ 
btnRestart��-   $��24��$��Restart�� . $��10,0,0,0q�$��	10,0,10,0q� �� {��� *	IsRestart+