   M S B A M L   `   `   `  ����  VSI.EDGEAXConnector.UI/    )VSI.EDGEAXConnector.UI.EmailSubscriberAdd   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Email Subscriber Add��5   	   $	��550��6&   $	��500��63    � WindowStartupLocation$ CenterOwner=�6?    � ShowInTaskbar .   � WindowStyle$
 None=�6y      ModalWindowStyle#���   6�    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ Close��5      5   	   .��5	      �� .+�5
   
   �� $��10q�6   ���#ffffff._�5      �� $��Add New Subscriber��6   $��10q�63   
���   �.��5      � $��10q�6    � RowDefinitions 5      �� 5      $��50 �6(   �� 5      $��50 �6(   �� 5      $��50 �6(   �� 5      $	��210 �6(   �� 5      $��35 �6(   .+�5      ��  �� Orientation$ 
Horizontal=�6!   $��0��6:   .+�5      �� -   5         ��Name$ lblId��5       $y�	Collapsed=�6/   �� 5      $��Name : ��6    $��32��62   $	��100��6>   $
��Bold"�6J   $��Center=�6\   �� 5      -   5        	 ��Name$	 txtName��5   "   $	��300��63   $��32��6?   $��5q�6K   5      �� 5      $ 
Horizontal=�6!   $��1��6:   .+�5      �� $��Email : ��6    $��32��63   $	��100��6?   $
��Bold"�6K   $��Center=�6]   �� 5      -   5        $	 txtEmail��5   "   $	��300��64   $��32��6@   $��5q�6L   5      �� 5      $ 
Horizontal=�6!   $��2��6:   .+�5      �� $��	Active : ��6    $��32��64   $	��100��6@   $
��Bold"�6L   $��Center=�6^   �� 5      -   5        
 ��Name$
 chkIsActive��5   #   $��5q�68   5       �� 5!      $ 
Horizontal=�6!   $��3��6:   .+�5"      �� $��Notifications : ��6    $��32��6;   $	��100��6G   $
��Bold"�6S   $��Center=�6e   �� 5#      $ Vertical=�6%   .+�5$      �� -   5         ��Name$ lstcheckbox��5$   (    �� SelectionMode$ Multiple=�6=   $	��300��6V   $	��200��6b   $��Visible=�6o   ��5%   "   �� 5&   &   .R�5'   *   �� $	��260��6X    �� 
IsSelected 66   �� *
IsSelected+.��5(   .   �� ��67   �� *Name+ d� 	IsChecked 6P   ��  �� RelativeSource ��  �� AncestorType" �� �� Path$ 
IsSelected�
 � Tag 6�   �� 5)   +   5*   '   57      58      59      �� 5:      -   5        $��btnSave��5:   B   $��5��6   $	��100��6*   $��32��66   $��Right=�6g   $��Center=�6�   $
��Save��6�     GreenButtonStyle#���  6�   ���#ffffff6�   5;      5<      5=      5>      5?      