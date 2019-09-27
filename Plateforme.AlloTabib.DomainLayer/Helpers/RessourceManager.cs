using System;
using System.Net;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public  class RessourceManager
    {
         // L’instance du singleton. Cette instance doit être privée et statique.
        private static RessourceManager instance = null; 
        // Pour éviter, lors de l’utilisation de multiple thread, que plusieurs singleton soit instanciés.
        private static readonly object myLock = new object();


        
        public string LocalIPAddress()
        {
            var nomMachine = Environment.MachineName;
            var IpMachine = Dns.GetHostAddresses(Environment.MachineName)[0].ToString();

            //IPHostEntry host;
            //string localIP = "";
            //host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        localIP = ip.ToString();
            //        break;
            //    }
            //}
            return string.Format("{0} : {1} et {2} = {3} ", "Nom machine", nomMachine, "IP", IpMachine);
        }

        // La ressource ? partager.  
      
        // Des accesseurs pour cette ressources. 
        public string IpAddress
        {
            get { return string.Format("{0}-{1}", LocalIPAddress(), Environment.MachineName); }
           
        }

        // Le constructeur d’un singleton est TOUJOURS privée. 
        private RessourceManager() { }

        // La méthode qui va nous permettre de récupérer l’unique instance de notre singleton.
        // La méthode doit être statique pour être appelé depuis le nom de la classe maClasse.getInstance();
        public static RessourceManager getInstance() 
        { 
            //lock permet de s’assurer qu’un thread n’entre pas dans une section critique du code pendant qu’un autre thread s’y trouve.  
            //Si un autre thread tente d’entrer dans un code verrouillé, il attendra, bloquera, jusqu’à ce que l’objet soit libéré.
            lock (myLock) 
            { 
                // Si on demande une instance qui n’existe pas, alors on crée notre RessourceManager.
                if (instance == null) instance = new RessourceManager();
                // Dans tous les cas on retourne l’unique instance de notre RessourceManager.
                return instance; 
            } 
        }

    } 
    }

