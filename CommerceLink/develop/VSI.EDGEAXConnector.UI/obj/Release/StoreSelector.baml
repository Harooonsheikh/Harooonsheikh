   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI*    $VSI.EDGEAXConnector.UI.StoreSelector   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$*  $EdgeAX CommerceLink Service Selector��$	��320��$	��400�� � 
ResizeMode$ NoResize=�   ModalWindowStyle#���    � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .  J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ Close��.��� -   $��grdMain�� � ColumnDefinitions �� $
��150* �.+�� -    �Name$ grdSelectStore��$��10q�.+��� $��Center=�.+���  �� TextAlignment$ Center=�$
��Bold"�$��24(�
�������$!��EdgeAX CommerceLink Service���� -   $��cmbStore��	 �� SelectedValuePath$
	 Name��$��	0,10,0,10q�$
��Bold"�$��15(�
�������  ComboBoxFlatStyle#��[ $	��300���� 
 �� Orientation$
 
Horizontal=�$��Center=�.+��� -   $��
btnProceed��$��Proceed��  GreenButtonStyle#���  ���#ffffff$��Center=�$	��100��$��32��$��12(�$��10q�$��Bottom=��� $
 
Horizontal=�$��Center=�.+��� -   $��btnAddStore��$��Add New Store��  BlueButtonStyle#���  ���#ffffff$��Center=�$	��100��$��32��$��12(�$��10q�$��Bottom=�$y�Hidden=��� -   $��btnEditStore��$��
Edit Store��  RedButtonStyle#���  ���#ffffff$��Center=�$	��100��$��32��$��12(�$��10q�$��Bottom=�$y�Hidden=�� -	   $ grdAddStore��$y�	Collapsed=�$��10q�.+��� $��Center=�.+��� -
   $��	lblIsEdit��$y�Hidden=�$��false���� $ Center=�$
��Bold"�$��14(�
�������$��
Store Name���� -   $��txtStoreName��$
��Bold"�$��14(�
���   �$��3q��� $
 Left=�$
��Bold"�$��10(�
�������$)��#Special characters are not allowed.���� $ Center=�$
��Bold"�$��14(�
�������$��Database Server���� -   $��txtServerName��$
��Bold"�$��14(�
���   �$��3q��� $ Center=�$
��Bold"�$��14(�
�������$'��!EdgeAX CommerceLink Database Name���� -   $��txtCatalogName��$
��Bold"�$��14(�
���   �$��3q��� $
 
Horizontal=�$��Center=�.+��� -   $��btnSave��$
��Save��#���  ���#ffffff$��Center=�$	��100��$��32��$��12(�$��10q�$��Bottom=��� -   $��	btnCancel��$��Cancel��#���  ���#ffffff$��Center=�$	��100��$��32��$��12(�$��10q�$��Bottom=��� -   $��	btnDelete��$��Delete��#���  ���#ffffff$��Center=�$	��100��$��32��$��12(�$��10q�$��Bottom=�