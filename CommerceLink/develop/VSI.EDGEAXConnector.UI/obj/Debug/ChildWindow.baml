   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI(    "VSI.EDGEAXConnector.UI.ChildWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Complex Property Mapping��5   	   $
��Auto��6*    � WindowStyle$ SingleBorderWindow=�68   $	��700��6Y   .��5      �  � RowDefinitions 5   
   �� 5      $	��58* �6   �� 5      $
��363* �6   �� 5	      $	��48* �6    � ColumnDefinitions 5   
   �� 5      $	��300 �6   �� 5      $
��350* �6   .+�5   
   �� $��25��6   $��1��6J   $��0��6_   $��1��6l   $��	5,16,0,16q�6}   ��6!   �� *SelectedDestEntity.Name+6�   �� 5   
   $��25��6   $	��200��6   $
��Left=�6+    �� SelectedIndex$ 0��6k    �� DisplayMemberPath$
 Name��6�   $��0��6�   $��1��6�   $��1��6�   $��5,0,0,0q�6�   ��6G   �� *SrcEntities+ �� SelectedItem 6}   �� *SelectedSrcEntity+6�   &   System.Windows.Controls.DataGrid  5   
   $��1��6   $��Stretch=�6       AutoGenerateColumns .  $��2��6Z   $��1��6�   ��6n   �� *Transformer.Properties+   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 Target��6)   
   Width5  /System.Windows.Controls.DataGridLengthConverter$	
 300 69      CellTemplate 5      �� 5      .R�5      �� .+�5   "   �� ��6-   �� *DestinationProperty.Name+6Y   �� 5   "   
�������$��Italic$�6C   ��6V   �� *" DestinationProperty.PropertyType+6�   5      5      5        5      $	 Source��6)   $
 * 69    5      �� 5       .R�5!      �� .+�5"   "   �� ��6,   �� *SourceProperty.Name+6S   �� 5#   "   
�������$��Italic$�6C   ��6V   �� *SourceProperty.PropertyType+6�   5$      5%         CellEditingTemplate 5'      �� 5(      .R�5)      �� $��cboSourceProp��6'   -   5        ��5*   "   ��  �� RelativeSource ��  �� AncestorType" � �� Path$" DataContext.SourceProperties���5,   "   �� 5-   &   .R�5.   *   ��  �� Orientation$ 
Horizontal=�65   .+�5/   .   �� ��68   �� *Name+�� 50   .   
�������$��5,0,0,0q�6m   ��6O   �� *PropertyType+6m   51   +   52   '   54      56      58      5D      �� 5E   
   -   5        $��Generate��5E      $��Right=�6!   $��5,0,5,0q�6=   $��2��6N   $��2��6[   $	��100��6k   $��25��6w   .��6�   
Generate6�   �� 5F   
   $��2��6/   ��6   �� *	ViewModel+6/   5G      5H      