   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI?    9VSI.EDGEAXConnector.UI.UserControlls.XmlFileMappingCreate   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .��� -   $��
grdMapping��$��0q�  � ColumnDefinitions  �� $	��320 ��� $��* �.+��� $��0��$��0q����#ffffff._�� $��10q� � RowDefinitions �� $��85 ��� $��* ��� $��* ��� $��* �.+��� $��0��.+��� $��Source Entities :��$��5q�
���   ��� -   $��cboSrc�� �� 
IsEditable .  �� SelectedIndex$ -1�� �� DisplayMemberPath$
 Name��$��0,0,0,5q��� $��Source Properties :��$��5q�
���   ��� $��1��._��� -   $��lstSrcProperties������  �� Path$ ActualHeight� �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorLevel$ 1��	 �� AncestorType"	 ������ .R��� 
 �� Orientation$
 
Horizontal=�.+��� ���� *Name+�� 
�������$��5,0,0,0q����� *PropertyType+�� $��2��.+��� $
 
Horizontal=�.+��� -   $��btnConstantValueTag��$��Constant Value��$��0,5,5,5q�$��5q��� -   $��btnDefaultValueTag��$��Default Value��$��0,5,5,5q�$��5q��� -   $��btnExpressionValueTag��$��
Expression��$��0,5,5,5q�$��5q��� $
 
Horizontal=�.+��� -   $��btnShowNode��$��	Show Node��$��0,5,5,5q�$��5q��� -   $��	btnRepeat��$��Repeat Node��$��0,5,5,5q�$��5q��� -	   $��btnDataObject��$��Data Object��$��0,5,5,5q�$��5q��� $
 
Horizontal=�.+��� -
   $��
btnToLower��$��To Lower��$��0,5,5,5q�$��5q��� -   $��
btnToUpper��$��To Upper��$��0,5,5,5q�$��5q��� -   $$��btnAttributeExpressionValueTag��$��Attribute Expression��$��0,5,5,5q�$��5q��� $
 
Horizontal=�.+��� -   $��btnConfigurationHelperKey��$��ConfigurationHelper.Key��$��0,5,5,5q�$��5q��� -   $��btnCustomAttributeValue��$��Custom Attribute Value��$��0,5,5,5q�$��5q��� $��3��.+��� $%��Others Templates Files Source :��$��5q�
���   ��� ._��� -   $��lstTemplates������ $ ActualHeight� �� $ FindAncestor=�$ 1��"	 ������ .R��� $
 
Horizontal=�.+��� ���� *Name+�� $��1��$��0q����#ffffff._��� $��Create XML Mapping��$��10q�
���   �.��� $��0�� �� $��90 ��� $��* ��� $��60 �.+��� $��0��.+��� $	��Top=�$��Name : ��$��32��$��Center=��� -    ��Name$ txtName��$	��Top=�$��32��$��5q��� $	��Top=�$��XML : ��$��32��$	��Top=��� $��1��._��� -    ��Name$ txtXML��$��0q�$��5q����� $ ActualHeight� �� $ FindAncestor=�$ 1��"	 ���� $��2��.+��� -   $��btnSave��$	��100��$��32��$��Right=�$��Center=�$
��Save��   GreenButtonStyle#���   ���#ffffff�� -   $��txtReadInstruction��$��Read Instructions�� � Cursor$
 Hand��
����  �$��	Underline{�$��16(�