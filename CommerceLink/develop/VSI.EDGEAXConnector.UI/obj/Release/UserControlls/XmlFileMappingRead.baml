   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI=    7VSI.EDGEAXConnector.UI.UserControlls.XmlFileMappingRead   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .��� -   $��
grdMapping��$��0q�  � ColumnDefinitions  �� $	��320 ��� $��* �.+��� $��0��$��0q����#ffffff._�� $��10q� � RowDefinitions �� $��85 ��� $��* ��� $	��120 ��� $��* �.+��� $��0��.+��� $��Source Entities :��$��5q�
���   ��� -   $��cboSrc�� �� 
IsEditable .  �� SelectedIndex$ -1�� �� DisplayMemberPath$
 Name��$��0,0,0,5q��� $��Source Properties :��$��5q�
���   ��� $��1��._��� -   $��lstSrcProperties������  �� Path$ ActualHeight� �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorLevel$ 1��	 �� AncestorType"	 ������ .R��� 
 �� Orientation$
 
Horizontal=�.+��� ���� *Name+�� 
�������$��5,0,0,0q����� *PropertyType+�� $��2��.+��� $
 
Horizontal=�.+��� -   $��btnConstantValueTag��$��Constant Value��$��0,5,5,5q�$��5q��� -   $��btnDefaultValueTag��$��Default Value��$��0,5,5,5q�$��5q��� -   $��btnPropertiesTag��$��
Properties��$��0,5,5,5q�$��5q��� $
 
Horizontal=�.+��� -   $��btnAttributeNameTag��$��Attribute Name��$��0,5,5,5q�$��5q��� -   $��	btnRepeat��$��Repeat Node��$��0,5,5,5q�$��5q��� $
 
Horizontal=�.+��� -	   $��btnIsCustomAttributeTag��$��Is Custom Attribute��$��0,5,5,5q�$��5q��� -
   $��btnConcatenate��$��Concatenate��$��0,5,5,5q�$��5q��� $��1��$��0q����#ffffff._��� $��Read XML Mapping��$��10q�
���   �.��� $��0�� �� $��90 ��� $��* ��� $	��100 �.+��� $��0��.+��� $	��Top=�$��Name : ��$��32��$��Center=��� -    ��Name$ txtName��$	��Top=�$��32��$��5q��� $	��Top=�$��XML : ��$��32��$	��Top=��� $��1��._��� -    ��Name$ txtXML��$��0q�$��5q����� $ ActualHeight� �� $ FindAncestor=�$ 1��"	 ���� $��2��.+��� -   $��btnSave��$	��100��$��32��$��Right=�$��Center=�$
��Save��   GreenButtonStyle#���   ���#ffffff�� $��Bottom=�$O��INote : If use constant-value attribute then remove source-path attribute.��
����  �$��22��$��0q��� $��Bottom=�$E��?To read nested properties use Properties tag inside Target tag.��
����  �$��22��$��0q��� $��Bottom=�$5��/e.g. <Target><Properties></Properties></Target>��
����  �$��22��$��0q�