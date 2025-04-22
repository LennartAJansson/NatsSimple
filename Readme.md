## Vad �r detta?  
Detta �r exempel p� hur man kan implementera Nats JetStreams i sin enklaste form i en applikation som s�nder och en applikation som lyssnar.  
  
Applikationerna inneh�ller varsin folder som heter Nats och som inneh�ller en extensionmetod f�r att registrera DI f�r Nats, en service f�r att s�nda och en service f�r att lyssna.  
  
Titta i Program.cs hur extensionmetoderna anv�nds. Datatypen som �r registrerad f�r lyssnaren �r en vanlig string men kan i princip vara vilket objekt som helst om man antingen skickar objektet i string eller om man g�r en egen INatsSerializer<T> f�r just den datatypen.  
  
Applikationerna anv�nder en massa "Magic Strings", byt ut dem mot konfigurationsv�rden i st�llet.  
  
Anv�nd hellre denna f�renklade version �n den som finns i repot CQRSSample (den �r �verdrivet komplex genom sin implementation av NatsChannels).  
  
Anv�nd allt detta "AS-IS", det finns s�kert tonvis av buggar, brister och en massa f�rb�ttringspotential.  
