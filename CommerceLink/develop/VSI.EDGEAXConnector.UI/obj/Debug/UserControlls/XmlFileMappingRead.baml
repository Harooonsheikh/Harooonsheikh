   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI=    7VSI.EDGEAXConnector.UI.UserControlls.XmlFileMappingRead   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      � -   5        $��
grdMapping��5      $��0q�6     � ColumnDefinitions  5	   
   �� 5
      $	��320 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0q�6!   ���#ffffff._�5      � $��10q�6    � RowDefinitions 5      �� 5      $��85 �6$   �� 5      $��* �6$   �� 5      $	��120 �6$   �� 5      $��* �6$   .+�5      �� $��0��6   .+�5      �� $��Source Entities :��6    $��5q�69   
���   �6D   �� 5      -   5        $��cboSrc��5       �� 
IsEditable .  �� SelectedIndex$ -1��6k    �� DisplayMemberPath$
 Name��6~   $��0,0,0,5q�6�   �� 5      $��Source Properties :��6    $��5q�6;   
���   �6F   5      �� 5      $��1��6   ._�5      �� -   5        $��lstSrcProperties��5      ��6k   ��  �� Path$ ActualHeight� �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorLevel$ 1��	 �� AncestorType"	 ����5      �� 5      .R�5   "   �� 
 �� Orientation$
 
Horizontal=�6-   .+�5   &   �� ��60   �� *Name+�� 5    &   
�������$��5,0,0,0q�6e   ��6G   �� *PropertyType+6e   5!   #   5"      5$      5%      �� 5&      $��2��6   .+�5'      �� $
 
Horizontal=�6!   .+�5(      �� -   5        $��btnConstantValueTag��5(   !   $��Constant Value��6<   $��0,5,5,5q�6w   $��5q�6�   �� 5)      -   5        $��btnDefaultValueTag��5)   !   $��Default Value��6;   $��0,5,5,5q�6t   $��5q�6�   �� 5*      -   5        $��btnPropertiesTag��5*   !   $��
Properties��69   $��0,5,5,5q�6m   $��5q�6~   5+      �� 5,      $
 
Horizontal=�6!   .+�5-      �� -   5        $��btnAttributeNameTag��5-   !   $��Attribute Name��6<   $��0,5,5,5q�6w   $��5q�6�   �� 5.      -   5        $��	btnRepeat��5.   !   $��Repeat Node��62   $��0,5,5,5q�6`   $��5q�6q   5/      �� 50      $
 
Horizontal=�6!   .+�51      �� -	   5        $��btnIsCustomAttributeTag��51   !   $��Is Custom Attribute��6@   $��0,5,5,5q�6�   $��5q�6�   �� 52      -
   5        $��btnConcatenate��52   !   $��Concatenate��67   $��0,5,5,5q�6j   $��5q�6{   53      54      5C      5D      �� 5E   
   $��1��6   $��0q�6!   ���#ffffff._�5F      �� $��Read XML Mapping��6   $��10q�61   
���   �.��5G      � $��0��6    5H      �� 5I      $��90 �6(   �� 5J      $��* �6(   �� 5K      $	��100 �6(   .+�5M      �� $��0��6!   .+�5N      �� $	��Top=�6    $��Name : ��68   $��32��6J   $��Center=�6V   �� 5O      -   5         ��Name$ txtName��5O   :   $	��Top=�6"   $��32��6K   $��5q�6W   �� 5P      $	��Top=�6    $��XML : ��68   $��32��6I   $	��Top=�6U   5Q      �� 5R      $��1��6   ._�5S      �� -   5         ��Name$ txtXML��5S   &   $��0q�6�   $��5q�6�   ��66   �� $ ActualHeight� �� $ FindAncestor=�$ 1��"	 ��6�   5T      �� 5U      $��2��6!   .+�5V      �� -   5        $��btnSave��5V   9   $	��100��6!   $��32��6-   $��Right=�6^   $��Center=�6z   $
��Save��6�      GreenButtonStyle#���   6�   ���#ffffff6�   �� 5W      $��Bottom=�6    $O��INote : If use constant-value attribute then remove source-path attribute.��6;   
����  �$��22��6�   $��0q�6�   �� 5X      $��Bottom=�6    $E��?To read nested properties use Properties tag inside Target tag.��6;   
����  �$��22��6�   $��0q�6�   �� 5Y      $��Bottom=�6    $5��/e.g. <Target><Properties></Properties></Target>��6;   
����  �$��22��6�   $��0q�6�   5Z      5[      5\      5]      5^      5_      