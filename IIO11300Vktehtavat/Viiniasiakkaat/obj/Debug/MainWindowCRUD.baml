   M S B A M L   `   `   `  ����I  EViiniasiakkaat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ViiniasiakkaatViiniasiakkaat  5   	   #    Viiniasiakkaat.MainWindowCRUD   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Viiniasiakkaat     5   	     � Title$  Viinikellarin Asiakkaat��5   	   $	��600��6)   $	��800��66    � 	Resources 5
         myTextBoxStyle �� 
TargetType �� Property �� Value��%y   &	        �� 5   
   " ��6)   .�5      ��  � ��$	 100��6)   �� 5       � ��
 ����6+   5      5      .��5      � .+�5   
   ]� .+�5      �� $	��Top=�6   .+�5      �� -   5         ��Name$ btnGetCustomers��5      $��Hae asiakkaat��62   6J   �� 5      -   5        $ btnNewCustomer��5      $��Tee uusi��61   6D   �� 5      -   5         ��Name$ 
spCustomer��5       �� Orientation$ 
Horizontal=�61   .+�5      �� 	Etunimi6)   �� 5      -   5         ��Name$ txtFirstName��5      #��[  6O   ��64   �� *	FirstName+6O   �� 5      
Sukunimi6*   �� 5      -   5        $ txtLastName��5      #��[  6M   ��63   �� *
LastName+6M   �� 5      Osoite6(   �� 5      -   5        $ 
txtAddress��5      #��[  6K   ��62   �� *	Address+6K   �� 5       
Postinro6*   �� 5!      -   5        $ txtZIP��5!      #��[  6C   $��50��6k   ��6.   �� *ZIP+6k   �� 5"      
Kaupunki6*   �� 5#      -   5        $ txtCity��5#      #��[  6E   ��6/   �� *City+6E   �� 5$      -	   5        $ btnSaveCustomer��5$      $��Tallenna��66   6I   5%      �� 5'      -
   5        $ btnDeleteCustomer��5'      $��Poista valittu��64   6M   5(      �� 5*      $��Bottom=�6   .@�5+      �� .��5,      �� ._�5-      �� -   5        $��
lblMessage��5-   $   $�� ��66   5.      5/      50      &   System.Windows.Controls.DataGrid  52      -   5        	  Name$	 dgCustomers��52      $��10q�6,   
   AutoGenerateColumns
 . ��68   �� 6k   54      56      57      