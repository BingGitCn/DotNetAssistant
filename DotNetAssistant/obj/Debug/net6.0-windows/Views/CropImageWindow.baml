   M S B A M L   `   `   `  ����  DotNetAssistant+    %DotNetAssistant.Views.CropImageWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  CropImageWindow��5   	   $	��800��5   	   $	��450��5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	   
������  � 	Resources 5
         BlinkAnimation g� RepeatBehavior �� 
TargetName �� TargetProperty �� From	 �� To	 g� AutoReverse
 g� Duration t� From	 t� To��%v  &	        �� 5   
   $ Forever��6,   .��5      �� $ border��5      $2 ,(Border.BorderBrush).(SolidColorBrush.Color)�5      	 Black5       DeepSkyBlue5      $
	 True��5      $
 0:0:2D�5      t� 5      $ border��5      $ BorderThickness�5       15       35      $
	 True��5      $
 0:0:2D�5      5      5      .��5      � .+�5    
   �� -   5         ��Name$ border��5       
������ $��1q�5"      
���   � J� Effect 5$      3  -System.Windows.Media.Effects.DropShadowEffect  5%         
BlurRadius$ 10Q�6#      ShadowDepth$ 0Q�63      Color$	 Red��6C   ._�5'      �� -   5         ��Name$	 img��5'      $��1q�6!   5(      5)       � Triggers 5*      :� 5,   
    :� RoutedEvent$ Loaded��6   .[�5-      �� #��[  6   5.      50      