using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Files
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Mep3._0
{
    [Serializable]
    public class OrdenDePedido
    {
        public Productos Products;
        private string   Cliente;
        private string   Vendedor;
        private ulong    Numero; //ID
        public DateTime Fecha;

        //  Constructores
        private void ConstBase ()
        {
            Products = new Productos();
            Fecha = DateTime.Now;
        }
        public OrdenDePedido()
        {
            ConstBase();
        }
        public OrdenDePedido(string Cliente , string Vendedor , ulong Numero)
        {
            ConstBase();
            this.Cliente = Cliente;
            this.Vendedor = Vendedor;
            this.Numero = Numero;
        }
        public OrdenDePedido(OrdenDePedido Orden)
        {
            Cliente = Orden.GetCliente();
            Vendedor = Orden.GetVendedor();
            Numero = Orden.GetNumero();
            Products = new Productos(Orden.Products);
            Fecha = Orden.Fecha;
        }
        
        //  Setters
        public void SetNumero (ulong Numero) 
        { this.Numero = Numero;  }
        public void SetCliente(string Cliente)
        { this.Cliente = Cliente; }
        public void SetVendedor(string Vendedor)
        { this.Vendedor = Vendedor; }
        public void SetData ( ulong Numero , string Cliente , string Vendedor)
        {
            this.Numero = Numero;
            this.Cliente = Cliente;
            this.Vendedor = Vendedor;
        }

        //  Getters
        public ulong GetNumero ()
        { return Numero; }
        public string GetCliente()
        { return Cliente; } 
        public string GetVendedor()
        { return Vendedor; }
        



        // Funciones
        public string Serializar()
        {
            string Ruta = LibreriaPersonal.Rutas.Ordenes();
            try
            {
                BinaryFormatter Formateador = new BinaryFormatter();

                Stream File = new FileStream( Ruta , FileMode.Append, FileAccess.Write);

                Formateador.Serialize(File, this);

                File.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "GOOD";
        }
        
        public string mostrardatos()
        {
            string cad;
            cad = "Orden Nro: " + this.Numero + " - ";
            //cad += "Cliente: " + this.Cliente + " - ";
            //cad += " Vendedor:" + this.Vendedor + " - ";
            cad += this.Fecha;

            foreach (Balde aux in Products.baldes)
            {
                cad += aux.MostrarSinVacios();
            }

            return cad;
        }

        public string fecha__Dia_Mes()
        {
            string res;
            int dia = this.Fecha.Day;
            int mes = this.Fecha.Month;
            int ano = this.Fecha.Year % 100;

            res  = dia > 9 ? dia.ToString() : "0" + dia.ToString();
            
            res += "/";

            res += mes > 9 ? mes.ToString() : "0" + mes.ToString();

            res += "/" + ano.ToString();


            return res;
            
        }

    }
}
