using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using WCFDemo;
using System.ServiceModel.Description;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost myHost = null;
            try
            {
                //URL Donde estara el servicio, yo inclui el puerto
                Uri baseAddres = new Uri("http://localhost:8002/MiServicio");
                //Aqui es donde das de alta el servicio, si te das cuenta, aqui uso la referencia a la clase en la dll
                myHost = new ServiceHost(typeof(MiServicio), baseAddres);
                //Cuando agregas el endpoint, es decir la url, usas referencia a la interface 
                myHost.AddServiceEndpoint(typeof(IMiServicio), new BasicHttpBinding(), "");

                //esta parte  espara definir el comportamiento... puedes copiar y pegar xD
                ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
                serviceBehavior.HttpGetEnabled = true;
                myHost.Description.Behaviors.Add(serviceBehavior);

                //Aqui pones a la escucha el servicio, es decir, aqui termina de darse de alta
                myHost.Open();
                Console.WriteLine("Servicio acticvo en: {0}", baseAddres);
                //Console.ReadKey(); 
            }
            catch (Exception ex)
            {
                myHost = null;
                Console.WriteLine("Error al hospedar el servicio" +ex.Message);
                Console.ReadKey();
            }

            try
            {
                //aqui estoy probando el servicio, no agregue el service reference, lo hice manual,
                //Pero  tambien se puede
                Console.WriteLine("Connectando");
                //Se crea el binding
                BasicHttpBinding binding = new BasicHttpBinding();
                //apuntas al servicio
                EndpointAddress address = new EndpointAddress("http://localhost:8002/MiServicio");
                //creas el canal de comunicacon
                ChannelFactory<IMiServicio> factory = new ChannelFactory<IMiServicio>(binding, address);
                IMiServicio channel = factory.CreateChannel();
                //mandas llamar las funciones que ocupes
                string returnMessage = channel.Test();
                Console.WriteLine("El servicio dice: {0}", returnMessage);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio" + ex.Message);
                Console.ReadKey();
            }

        }
    }
}
