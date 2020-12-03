using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatbotServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lister: in ascolto quando si parla dei server
            // EndPoint: identifica una coppia IP/Porta

            //Creare il mio socketlistener
            //1) specifico che versione IP
            //2) tipo di socket. Stream.
            //3) protocollo a livello di trasporto
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                    ProtocolType.Tcp);
            // config: IP dove ascoltare. Possiamo usare l'opzione Any: ascolta da tutte le interfaccie all'interno del mio pc.
            IPAddress ipaddr = IPAddress.Any;

            // config: devo configurare l'EndPoint
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            // config: Bind -> collegamento
            // listenerSocket lo collego all'endpoint che ho appena configurato
            listenerSocket.Bind(ipep);

            // Mettere in ascolto il server.
            // parametro: il numero massimo di connessioni da mettere in coda.
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");
            Console.WriteLine("in attesa di connessione da parte del client...");
            // Istruzione bloccante
            // restituisce una variabile di tipo socket.
            try {
                Socket client = listenerSocket.Accept();

                Console.WriteLine("Client IP: " + client.RemoteEndPoint.ToString());


                
                byte[] recvBuff = new byte[128];
                byte[] sendBuff = new byte[128];
                int recvBytes = 0;
            
                string recvString="", sendString="";

                while (true) { 
                    recvBytes = client.Receive(recvBuff);
                    recvString = Encoding.ASCII.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine("Client: " + recvString);
                
                    
                    if (recvString.ToUpper().Trim() == "QUIT")
                    {
                        break;
                    }else if(recvString.ToUpper().Trim() == "CIAO"){
                        sendString="ciao";   
                    }else if(recvString.ToUpper().Trim() == "COME VA?"){
                        sendString="bene!";   
                    }else if(recvString.ToUpper().Trim() == "CHE FAI?"){
                        sendString="niente";   
                    }else{
                        sendString="Non ho capito.";
                    }

                    // lo converto in byte
                    sendBuff = Encoding.ASCII.GetBytes(sendString);

                    //invio al client il messaggio
                    client.Send(sendBuff);

                    Array.Clear(sendBuff, 0, sendBuff.Length);
                    Array.Clear(recvBuff, 0, recvBuff.Length);
                    sendString="";
                    recvString="";
                    recvBytes=0;
                }
         }catch(Exception ex){
                Console.WriteLine(ex.Message);
         }
            Console.WriteLine("Programma terminato. Premere Enter per uscire...");
            Console.ReadLine();


        }
    }
}
