   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI*    $VSI.EDGEAXConnector.UI.StoreSelector   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$*  $EdgeAX CommerceLink Service Selector��5   	   $	��320��66   $	��400��6C    � 
ResizeMode$ NoResize=�6O      ModalWindowStyle#���   5   	    � WindowStartupLocation$ CenterScreen=�5   	    � ShowInTaskbar .  J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           Command$ Close��5
      5   	   .��5      � -   5        $��grdMain��5       � ColumnDefinitions 5   
   �� 5      $
��150* �6   .+�5   
   � -   5         �Name$ grdSelectStore��5      $��10q�6'   .+�5      �� $��Center=�6   .+�5      ��  �� TextAlignment$ Center=�6   $
��Bold"�63   $��24(�6E   
�������$!��EdgeAX CommerceLink Service��6j   �� 5      -   5        $��cmbStore��5      	 �� SelectedValuePath$
	 Name��6+   $��	0,10,0,10q�6D   $
��Bold"�6W   $��15(�6i   
�������  ComboBoxFlatStyle#��[ 6�   $	��300��6�   5      �� 5      
 �� Orientation$
 
Horizontal=�6   $��Center=�66   .+�5      �� -   5        $��
btnProceed��5   o   $��Proceed��6     GreenButtonStyle#���  6/   ���#ffffff$��Center=�6�   $	��100��6�   $��32��6�   $��12(�6�   $��10q�6�   $��Bottom=�6�   5      �� 5      $
 
Horizontal=�6   $��Center=�66   .+�5       �� -   5        $��btnAddStore��5    t   $��Add New Store��6     BlueButtonStyle#���  65   ���#ffffff$��Center=�6�   $	��100��6�   $��32��6�   $��12(�6�   $��10q�6�   $��Bottom=�6�   $y�Hidden=�6  �� 5!      -   5        $��btnEditStore��5!   p   $��
Edit Store��6     RedButtonStyle#���  62   ���#ffffff$��Center=�6�   $	��100��6�   $��32��6�   $��12(�6�   $��10q�6�   $��Bottom=�6�   $y�Hidden=�6	  5"      5#      5$      � 5%   
   -	   5        $ grdAddStore��5%      $y�	Collapsed=�6$   $��10q�6;   .+�5&      �� $��Center=�6   .+�5'      �� -
   5        $��	lblIsEdit��5'      $y�Hidden=�6-   $��false��6A   �� 5(      $ Center=�6   $
��Bold"�63   $��14(�6E   
�������$��
Store Name��6j   �� 5)      -   5        $��txtStoreName��5)      $
��Bold"�6.   $��14(�6@   
���   �$��3q�6a   �� 5*      $
 Left=�6   $
��Bold"�61   $��10(�6C   
�������$)��#Special characters are not allowed.��6d   �� 5+      $ Center=�6   $
��Bold"�63   $��14(�6E   
�������$��Database Server��6j   �� 5,      -   5        $��txtServerName��5,      $
��Bold"�6/   $��14(�6A   
���   �$��3q�6b   �� 5-      $ Center=�6   $
��Bold"�63   $��14(�6E   
�������$'��!EdgeAX CommerceLink Database Name��6j   �� 5.      -   5        $��txtCatalogName��5.      $
��Bold"�60   $��14(�6B   
���   �$��3q�6c   �� 5/      $
 
Horizontal=�6   $��Center=�66   .+�50      �� -   5        $��btnSave��50   l   $
��Save��6   #���  6,   ���#ffffff$��Center=�6�   $	��100��6�   $��32��6�   $��12(�6�   $��10q�6�   $��Bottom=�6�   �� 51      -   5        $��	btnCancel��51   m   $��Cancel��6   #���  6.   ���#ffffff$��Center=�6�   $	��100��6�   $��32��6�   $��12(�6�   $��10q�6�   $��Bottom=�6�   �� 52      -   5        $��	btnDelete��52   l   $��Delete��6   #���  6.   ���#ffffff$��Center=�6�   $	��100��6�   $��32��6�   $��12(�6�   $��10q�6�   $��Bottom=�6�   53      54      55      56      57      