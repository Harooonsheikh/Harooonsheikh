   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI/    )VSI.EDGEAXConnector.UI.ConnectorLogDetail   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Connector Log Detail��$	��800��$	��700�� � WindowStartupLocation$ CenterOwner=� � ShowInTaskbar .   � WindowStyle$
 None=�   ModalWindowStyle#���   .���� .+��� $��10q����#ffffff._��� $��Connector Log Detail��$��10q�
���   �.��� $��10q� � RowDefinitions �� $��50 ��� $��50 ��� $��50 ��� $	��210 ��� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 �.+���  �� Orientation$ 
Horizontal=�$��0��.+��� -    ��Name$ lblEventDateTime��$y�	Collapsed=��� $��Event Date Time: ��$��32��$	��140��$
��Bold"�$��Center=��� -    ��Name$ txtEventDateTime��$	��170��$��32��$��5q��� -   $ lblEventLevel��$y�	Collapsed=��� $��Event Level: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtLevel��$	��170��$��32��$��5q��� $ 
Horizontal=�$��1��.+��� -   $ lblUserName��$y�	Collapsed=��� $��User Name: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtUserName��$	��480��$��32��$��5q��� $ 
Horizontal=�$��2��.+��� -   $ lblMachineName��$y�	Collapsed=��� $��Machine Name: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtMachineName��$	��480��$��32��$��5q��� $ 
Horizontal=�$��3��.+��� -	   $ lblEventMessage��$y�	Collapsed=��� $��Event Message: ��$��32��$	��140��$
��Bold"�$��Center=��� -
   $ txtEventMessage��$	��480��$	��200��$��5q���. �� $ 
Horizontal=�$��4��.+��� -   $ lblErrorSource��$y�	Collapsed=��� $��Error Source: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtErrorSource��$	��170��$��32��$��5q��� -   $ lblErrorClass��$y�	Collapsed=��� $��Error Class: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtErrorClass��$	��170��$��32��$��5q��� $ 
Horizontal=�$��5��.+��� -   $ lblErrorMethod��$y�	Collapsed=��� $��Error Method: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtErrorMethod��$	��170��$��32��$��5q���. �� $ 
Horizontal=�$��6��.+��� -   $ lblErrorMessage��$y�	Collapsed=��� $��Error Message: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtErrorMessage��$	��480��$��32��$��5q���. �� $ 
Horizontal=�$��7��.+��� -   $ lblInnerErrorMessage��$y�	Collapsed=��� $��Inner Error Message: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtInnerErrorMessage��$	��480��$��32��$��5q���. �� $ 
Horizontal=�$��8��.+��� -   $ lblIdentityId��$y�	Collapsed=��� $��Identity Id: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtIdentityId��$	��170��$��32��$��5q��� -   $ lblErrorModule��$y�	Collapsed=��� $��Error Module: ��$��32��$	��140��$
��Bold"�$��Center=��� -   $ txtErrorModule��$	��170��$��32��$��5q��� -   $��btnGenerateTextFile��$��9��$	��173��$��32��$��Right=�$��Center=�$��Generate Text File��  GreenButtonStyle#���  ���#ffffff$��	0,9,338,9q��� -   $��btnClose��$��9��$	��100��$��32��$��Right=�$��Center=�$��Close��#���  ���#ffffff�� -    ��Name$ 	btnDelete��$��9��$	��100��$��32��$��Right=�$��Center=�$��Delete��#���  ���#ffffff$��	0,9,516,9q�