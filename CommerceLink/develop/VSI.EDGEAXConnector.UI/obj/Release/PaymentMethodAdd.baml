   M S B A M L   `   `   `  ����   VSI.EDGEAXConnector.UI-    'VSI.EDGEAXConnector.UI.PaymentMethodAdd   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Payment Method Add��$��687.744��$	��550�� � WindowStartupLocation$ CenterOwner=� � ShowInTaskbar .   � WindowStyle$
 None=�   ModalWindowStyle#���    J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ Close��.���� .+��� $��10q����#ffffff._��� $��Add New Payment Method��$��10,40,10,10q�
���   �.��� $��10q� � RowDefinitions �� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 �.+���  �� Orientation$ 
Horizontal=�$��0��.+��� -    ��Name$ lblId��$y�	Collapsed=��� $!��Parent Payment Method Id : ��$��32��$	��170��$
��Bold"�$��Center=��� -   	 ��Name$	 cmbParentPaymentMethod��
 J� 	AllowDrop
 . $	��300��$��32��
������  �� 
IsEditable . �� $ 
Horizontal=�$��1��.+��� $��ECommerce Value : ��$��32��$	��170��$
��Bold"�$��Center=��� -    ��Name$ txtECommerceValue��$	��300��$��32��$��5q��� $ 
Horizontal=�$��2��.+��� $��Erp Value : ��$��32��$	��170��$
��Bold"�$��Center=��� -   $ txtErpValue��$	��300��$��32��$��5q��� $ 
Horizontal=�$��4��.+��� $��Erp Code : ��$��32��$	��170��$
��Bold"�$��Center=��� -   $ 
txtErpCode��$	��300��$��32��$��5q��� $ 
Horizontal=�$��3��.+��� $��Has SubMethod : ��$��32��$	��170��$
��Bold"�$��Center=��� -    ��Name$ chkHasSubMethod��$��5q��� $ 
Horizontal=�$��5��.+��� $��Is Prepayment : ��$��32��$	��170��$
��Bold"�$��Center=��� -   $ chkIsPrepayment��$��5q��� $ 
Horizontal=�$��6��.+��� $��Is Credit Card : ��$��32��$	��170��$
��Bold"�$��Center=��� -	   $ chkIsCreditCard��$��5q��� $ 
Horizontal=�$��7��.+��� $��Use Payment Connector : ��$��32��$	��170��$
��Bold"�$��Center=��� -
   $ chkUsePaymentConnector��$��5q��� $ 
Horizontal=�$��8��.+��� $��Service Account Id : ��$��32��$	��170��$
��Bold"�$��Center=��� -   $ txtServiceAccountId��$	��300��$��32��$��5q��� -   $��btnSave��$��9��$	��100��$��32��$��Right=�$��Center=�$
��Save��  GreenButtonStyle#���  ���#ffffff$��
0,88,0,-69q�