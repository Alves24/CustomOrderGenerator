using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library.Entidades
{
    [Serializable]
    public class Cliente
    {
        public Cliente(int id, string nombre)
        {
            this.Id = id;
            this.Nombre = nombre;
            Initialize();
        }
        public Cliente()
        {
            Initialize();
        }
        private void Initialize()
        {
            this.Precios = new Dictionary<int, Precios>();
            this.PreciosNegro = new Dictionary<int, Precios>();
            this.PreciosIVA_INCLUIDO = new Dictionary<int, Precios>();
            IsSerialized = false;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        public string DireccionLegal { get; set; }
        public string DireccionEntrega { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public string FormaEntrega { get; set; }
        public string FormaPago { get; set; }
        public bool IsSerialized { get; set; }
        public string Vendedor { get; set; }
        public Dictionary<int, Precios> Precios { get; set; }
        public Dictionary<int, Precios> PreciosNegro { get; set; }
        public Dictionary<int, Precios> PreciosIVA_INCLUIDO { get; set; }

        public string Serialize()
        {
            string Ruta = $@"Files/Clientes/{Nombre}_{Id}.dat";
            try
            {
                BinaryFormatter Formateador = new BinaryFormatter();

                Stream File = new FileStream(Ruta, FileMode.Create, FileAccess.Write);

                Formateador.Serialize(File, this);

                File.Close();

                this.IsSerialized = true;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "GOOD";
        }

        public Cliente Deserialize()
        {
            string Ruta = $@"Files/Clientes/{Nombre}_{Id}.dat";
            var Formateador = new BinaryFormatter();
            Cliente EsteCliente = null;
            try
            {
                if (File.Exists(Ruta))
                {
                    Stream File = new FileStream(Ruta, FileMode.Open, FileAccess.Read);

                    EsteCliente = (Cliente)Formateador.Deserialize(File);

                    File.Close();
                }

                if (EsteCliente?.Nombre == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            if (EsteCliente.PreciosIVA_INCLUIDO == null)
            {
                EsteCliente.PreciosIVA_INCLUIDO = new Dictionary<int, Precios>();
            }

            return EsteCliente;
        }

        

        public override string ToString()
        {
            return $"{Nombre}  {Id}";
        }
    }
}