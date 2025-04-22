## Vad är detta?  
Detta är exempel på hur man kan implementera Nats JetStreams i sin enklaste form i en applikation som sänder och en applikation som lyssnar.  
  
Applikationerna innehåller varsin folder som heter Nats och som innehåller en extensionmetod för att registrera DI för Nats, en service för att sända och en service för att lyssna.  
  
Titta i Program.cs hur extensionmetoderna används. Datatypen som är registrerad för lyssnaren är en vanlig string men kan i princip vara vilket objekt som helst om man antingen skickar objektet i string eller om man gör en egen INatsSerializer<T> för just den datatypen.  
  
Applikationerna använder en massa "Magic Strings", byt ut dem mot konfigurationsvärden i stället.  
  
Använd hellre denna förenklade version än den som finns i repot CQRSSample (den är överdrivet komplex genom sin implementation av NatsChannels).  
  
Använd allt detta "AS-IS", det finns säkert tonvis av buggar, brister och en massa förbättringspotential.  
