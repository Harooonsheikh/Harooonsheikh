   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI(    "VSI.EDGEAXConnector.UI.ChildWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Complex Property Mapping��$
��Auto�� � WindowStyle$ SingleBorderWindow=�$	��700��.���  � RowDefinitions �� $	��58* ��� $
��363* ��� $	��48* � � ColumnDefinitions �� $	��300 ��� $
��350* �.+��� $��25��$��1��$��0��$��1��$��	5,16,0,16q����� *SelectedDestEntity.Name+�� $��25��$	��200��$
��Left=� �� SelectedIndex$ 0�� �� DisplayMemberPath$
 Name��$��0��$��1��$��1��$��5,0,0,0q����� *SrcEntities+ �� SelectedItem �� *SelectedSrcEntity+&   System.Windows.Controls.DataGrid  $��1��$��Stretch=�   AutoGenerateColumns .  $��2��$��1������ *Transformer.Properties+   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn	   Header$	 Target��
   Width5  /System.Windows.Controls.DataGridLengthConverter$	
 300    CellTemplate �� .R��� .+��� ���� *DestinationProperty.Name+�� 
�������$��Italic$����� *" DestinationProperty.PropertyType+  $	 Source��$
 *  �� .R��� .+��� ���� *SourceProperty.Name+�� 
�������$��Italic$����� *SourceProperty.PropertyType+   CellEditingTemplate �� .R��� $��cboSourceProp��-   ����  �� RelativeSource ��  �� AncestorType" � �� Path$" DataContext.SourceProperties����� .R���  �� Orientation$ 
Horizontal=�.+��� ���� *Name+�� 
�������$��5,0,0,0q����� *PropertyType+�� -   $��Generate��$��Right=�$��5,0,5,0q�$��2��$��2��$	��100��$��25��.��
Generate�� $��2������ *	ViewModel+