   M S B A M L   `   `   `  ���� H  DBoardControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:BoardControls;assembly=BoardControlsBoardControls   BoardClient   BoardClient.Client   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Dcntrl2clr-namespace:BoardControls;assembly=BoardControls       � Title$&   Student Board [Отключен]�� � Topmost .  $	��600��$	��400��$	��300��$	��200�� � WindowStyle$
 None=� � AllowsTransparency . 
������  J� Opacity$	 0.9Q� � 
ResizeMode$ CanResizeWithGrip=� � Icon$ Images/Client.jpg�� � 	Resources  �� 
TargetType	 �� Property
 �� Value��%3   '��       �� " ��.��� 	 � ��$
 0 0 3 40q� J� CommandBindings )  #System.Windows.Input.CommandBinding  -   #  System.Windows.SystemCommands   CloseWindowCommand   Command# Z   -      MaximizeWindowCommand# Z   -      RestoreWindowCommand# Z   -      MinimizeWindowCommand# Z .���� -   $��border�� �� CornerRadius$ 5��$��2q�$��0 0 3 40q�#��� ��._�]� ��. .+��� -   $��rsTop��$	��Top=�$	��1.5��
������ �� -   $��rsRigth��$��Right=�$	��1.5��
������ �� -   $��rsBottom��$��Bottom=�$	��1.5��
������ �� -	   $��rsLeft��$
��Left=�$	��1.5��
������ ]� -
   $��caption��$	��Top=�$��20����.  � Cursor$ SizeAll��.+��� $
��Left=�$��10 2 2 2q�$��16������ *Icon+ �� RelativeSource �� *FindAncestor+ �� AncestorType" ��� $
��Left=�$
��Bold"�$��14(�$��10 0 0 0q����� *Title+ �� *FindAncestor+" ��� $��RightToLeft=� �� Orientation$ 
Horizontal=� J� SnapsToDevicePixels . $��60��$��Right=�$ Arrow��
������ .+���  � ToolTip$ Закрыть окно��
������ $��1q�$ Arrow��#��Z .���� $��Images/Close.png���� -   $��btMaxRestore��$< 6Развернуть/Восстановить окно��
������ $��1q�$ Arrow��#��Z .���� -   $��imMaxRestore��$��Images/Maximize.png���� $ Свернуть окно��
������ $��1q�$ Arrow��#��Z .���� $��Images/Minimize.png��o� $	��Top=�$��25��.@�m� $��	_Файл��.@�m� -   $��	mnConnect��$��Подключиться�� m� IsCheckable . �� m� $��
Выход�� m� Command# Z m� -   $��
mnSettings��$��Настройки...��m� $��Справка��.@�m� -   $��mnAbout��$��О программе���� $��Bottom=�$��20�����	#FFF1EDED(� $��5q�.���� -   $��canvas�� �� EditingMode$
 None=�$	��800��$	��500�� �� 
Background ��  �� Color$ White�� �� Opacity$ 0Q� �� DefaultDrawingAttributes *  $System.Windows.Ink.DrawingAttributes     
FitToCurve .     IsHighlighter  . .H�    BoardControls.BoardTextBox  -   !  Name$! board��" �� 
IsReadOnly" . |�.  $��Courier New)�$��24(�$
��Bold"����� # �� Path$# ActualWidth� �� " ������ $# ActualHeight� �� " ��.������ .��� 	 � ��$
 0q�$ �� Triggers$ P� % P� Property% �@J�IsMouseOver& P� Value& . . ��� 	 � ��$
 0q�.��