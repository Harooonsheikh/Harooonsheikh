   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI-    'VSI.EDGEAXConnector.UI.EmailTemplateAdd   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Email Subscriber Add��$	��500�� � WindowStartupLocation$ CenterOwner=� � ShowInTaskbar .   � WindowStyle$
 None=�   ModalWindowStyle#���    J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ Close��.���� .+��� $��10q����#ffffff._��� $��Add New Template��$��10q�
���   �.��� $��10q� � RowDefinitions �� $��50 ��� $��50 ��� $	��110 ��� $	��110 ��� $��50 ��� $��35 �.+���  �� Orientation$ 
Horizontal=�$��0��.+��� -    ��Name$ lblId��$y�	Collapsed=��� $��Name : ��$��32��$	��100��$
��Bold"�$��Center=��� -   	 ��Name$	 txtName��$	��300��$��32��$��5q��� $ 
Horizontal=�$��1��.+��� $��
Subject : ��$��32��$	��100��$
��Bold"�$��Center=��� -   $	 
txtSubject��$	��300��$��32��$��5q��� $ 
Horizontal=�$��2��.+��� $��Body : ��$��32��$	��100��$
��Bold"�$��Center=��� -   $	 txtBody��
 �� TextWrapping$

 Wrap=�$	��300��$	��100��$��5q��� $ 
Horizontal=�$��3��.+��� $��	Footer : ��$��32��$	��100��$
��Bold"�$��Center=��� -   $	 	txtFooter��$

 Wrap=�$	��300��$	��100��$��5q��� $ 
Horizontal=�$��4��.+��� $��	Active : ��$��32��$	��100��$
��Bold"�$��Center=��� -    ��Name$ chkIsActive��$��5q��� -   $��btnSave��$��5��$	��100��$��32��$��Right=�$��Center=�$
��Save��  GreenButtonStyle#���  ���#ffffff