   M S B A M L   `   `   `  ����  
GalaxyTool    Galactic.SplashScreen   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Galactic - Welcome��5   	   $	��500��6$   $	��800��61    � WindowStartupLocation$ CenterScreen=�6=    � 
ResizeMode$ NoResize=�6b   ���#2D2D30.��5      � .+�5   
   �� $��Center=�6   $��Center=�60   .+�5      �� $��official_logo.png��6   $	��120��6/   $	��120��6;   $��0,0,0,20q�6H   �� 5      $��Welcome to Galactic!��6   $��24(�64   $
��Bold"�6B   
������� �� TextAlignment$ Center=�6g   �� 5	      $J��DPlease re-plug your Galaxy device into your computer to synchronize.��6   $��16(�6d   
�������$��	0,10,0,10q�6�   $ Center=�6�   �� 5
      $��Pre-Alpha Dev v.1.0��6   $��14(�63   
�������$��0,0,0,30q�6S   $ Center=�6e   �� 5      $��50��6   $��50��6    $��	0,10,0,10q�6,   .+�5      A� -   5         A�Name$ Spinner��5      $��40��6+   $��40��66   
������$��4��6V   z�5      �� 5       �� Angle$ 0Q�6*    �� CenterX$ 20Q�64    �� CenterY$ 20Q�6A   5      5      �� 5      -   5         ��Name$ 
SplashText��5      $(��"Copyright PSYBR Media Technologies��6,   $��12(�6V   
�������$ Center=�6v   $��0,30,0,0q�6�   5      5      	 � Triggers	 5      :� 5   
   
 :� RoutedEvent$
 Window.Loaded��6   .[�5      �� .��5      �� .��5      W�  �� 
TargetName$ Spinner��6&    �� TargetProperty$9 3(UIElement.RenderTransform).(RotateTransform.Angle)�5   &    W� From 05   &   	 W� To 3606/    g� Duration$ 0:0:2D�68    g� RepeatBehavior$ Forever��6I   5      5      5      5       