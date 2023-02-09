using Library.Entidades;
using Library;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;

namespace Engine.DataProcessors
{
    public class Core
    {
        public Core()
        {
            Order = new Orden();
            UserPreferences = new UserPreferences();
            
            Productos = DBController.LoadProductos();
            Clientes = DBController.LoadClientes();
            
        }

        public Orden Order { get; set; }
        public List<Producto> Productos;
        public List<Cliente> Clientes;
        public UserPreferences UserPreferences;

        public bool CreateOrder()
        {
            if (!Validator.Check(Order)) return false;
            
            ExcelWorkerRemaster.GenerateOrder(Order);

            if (!Order.Test)
            {
                SerializeClient();

                UpdateUserPreferences();

                new OrdenDoneBox(OrdenDone, Order.Numero);
            }
            else
            {
                Order.Test = false;
            }

            return true;
        }

        public bool AgregarProducto(int id, string x20, string x10, string x4, string x1,
                                    string preciox20, string preciox10, string preciox4, string preciox1)
        {
            // Verifico que las cantidades sean int.
            int[] quantities = Validator.ConvertToInt(new string[] { x20, x10, x4, x1 });
            // Verifico que los precios sean double.
            decimal[] prices = Validator.ConvertToDecimal(new string[] { preciox20, preciox10, preciox4, preciox1 });

            if (quantities == null || prices == null)
            {
                return false;
            }

            var nProducto = new Producto(FindProduct(id));

            if (nProducto != null)
            {
                nProducto.x20 = quantities[0];
                nProducto.x10 = quantities[1];
                nProducto.x4 = quantities[2];
                nProducto.x1 = quantities[3];

                nProducto.preciox20 = prices[0];
                nProducto.preciox10 = prices[1];
                nProducto.preciox4 = prices[2];
                nProducto.preciox1 = prices[3];

                // Verifico que tengan precio
                if (!Validator.Check(nProducto))
                    return false;

                Order.AddProduct(nProducto);
                UpdateClientProductPrice(nProducto);

                return true;
            }
            else return false;
        }
        
        public bool AgregarRegalias(int id, string x20, string x10, string x4, string x1)
        {
            int[] quantities = Validator.ConvertToInt(new string[] { x20, x10, x4, x1 });

            if (quantities == null) return false;

            var nRegalia = new Producto(FindProduct(id));

            if (nRegalia != null)
            {
                nRegalia.x20 = quantities[0];
                nRegalia.x10 = quantities[1];
                nRegalia.x4 = quantities[2];
                nRegalia.x1 = quantities[3];

                Order.AddRegalia(nRegalia);

                return true;
            }
            else return false;
        }

        private void SerializeClient()
        {
            if (!Order.Cliente.IsSerialized && !Order.Test)
            {
                Order.Cliente.Id = DBController.WriteClientAndGetID(Order.Cliente.Nombre);
            }

            string res = Order.Cliente.Serialize();

            if (res != "GOOD")
            {
               new InfoBox("Error en programa",
                           "No se pudo guardar el cliente", res);
            }
        }
       

        public Producto FindProduct(int id)
        {
            return Productos.FirstOrDefault(p => p.id == id); 
        }

        public Cliente FindCliente(Cliente cliente)
        {
            foreach (var cl in Clientes)
            {
                if (string.Equals(cl.Nombre, cliente.Nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return cl.Deserialize();
                }
            }
            return null;
        }

        public TextBox LoadClientsOnTxt(TextBox box)
        {
            var source = new AutoCompleteStringCollection();
            foreach (var cliente in Clientes)
            {
                source.Add(cliente.Nombre);
            }

            box.AutoCompleteCustomSource = source;
            return box;
        }

        private void UpdateUserPreferences()
        {
            if (UserPreferences.lastSeller != Order.Vendedor)
            {
                UserPreferences.lastSeller = Order.Vendedor;
                DBController.UpdateUserPreferences(UserPreferences);
            }
        }

        private void UpdateClientProductPrice(Producto prod)
        {
            Action<Precios> update = p =>
            {
                if (Order.Facturado)
                    if (Order.IvaIncluido)
                        Order.Cliente.PreciosIVA_INCLUIDO[(int)prod.id] = p;
                    else
                        Order.Cliente.Precios[(int)prod.id] = p;
                else
                    Order.Cliente.PreciosNegro[(int)prod.id] = p;
                return;
            };

            if (prod.categoria == "Latex")  
                update(new Precios(prod));

            var idList = DBController.GetIdsFromCategory(prod.categoria);
            if (idList != null) 
                update(new Precios(prod));
        }

        public void OrdenDone(int optionNumber)
        {
            switch (optionNumber)
            {
                case 1: // Nueva Orden
                    Order = null;
                    break;

                case 2: // Nueva Orden ( a partir de la anterior )
                    Order.Terminada = false;
                    Order.Test = true;
                    break;

                case 3: // Borrar
                    if (BorrarOrden())
                        Order.Test = true;
                    else
                        new OrdenDoneBox(OrdenDone, Order.Numero);
                    break;
            }
        }
        
        
        public bool BorrarOrden()
        {
            ExcelWorker.Delete(Order);
            return Order.Terminada ? false: true;
        }

       
    }
}