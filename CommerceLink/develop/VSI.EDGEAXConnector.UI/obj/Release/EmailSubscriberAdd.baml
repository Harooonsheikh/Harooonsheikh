   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI/    )VSI.EDGEAXConnector.UI.EmailSubscriberAdd   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Email Subscriber Add��$	��550��$	��500�� � WindowStartupLocation$ CenterOwner=� � ShowInTaskbar .   � WindowStyle$
 None=�   ModalWindowStyle#���    J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ Close��.���� .+��� $��10q����#ffffff._��� $��Add New Subscriber��$��10q�
���   �.��� $��10q� � RowDefinitions �� $��50 ��� $��50 ��� $��50 ��� $	��210 ��� $��35 �.+���  �� Orientation$ 
Horizontal=�$��0��.+��� -    ��Name$ lblId��$y�	Collapsed=��� $��Name : ��$��32��$	��100��$
��Bold"�$��Center=��� -   	 ��Name$	 txtName��$	��300��$��32��$��5q��� $ 
Horizontal=�$��1��.+��� $��Email : ��$��32��$	��100��$
��Bold"�$��Center=��� -   $	 txtEmail��$	��300��$��32��$��5q��� $ 
Horizontal=�$��2��.+��� $��	Active : ��$��32��$	��100��$
��Bold"�$��Center=��� -   
 ��Name$
 chkIsActive��$��5q��� $ 
Horizontal=�$��3��.+��� $��Notifications : ��$��32��$	��100��$
��Bold"�$��Center=��� $ Vertical=�.+��� -    ��Name$ lstcheckbox�� �� SelectionMode$ Multiple=�$	��300��$	��200��$��Visible=����� .R��� $	��260�� �� 
IsSelected �� *
IsSelected+.���� ���� *Name+ d� 	IsChecked ��  �� RelativeSource ��  �� AncestorType" �� �� Path$ 
IsSelected�
 � Tag �� �� -   $��btnSave��$��5��$	��100��$��32��$��Right=�$��Center=�$
��Save��  GreenButtonStyle#���  ���#ffffff