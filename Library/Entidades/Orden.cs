using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Entidades
{
    public class Orden
    {
        public Orden()
        {
            Productos = new List<Producto>();
            Regalias = new List<Producto>();
            Cliente = new Cliente();
            SubTotal = 0m;

            Facturado = true;
            Terminada = false;
            Test = false;
        }

        public int Numero { get; set; }
        public List<Producto> Productos { get; set; }
        public List<Producto> Regalias { get; set; }
        public Cliente Cliente { get; set; }
        public string Vendedor { get; set; }
        public decimal SubTotal { get; set; }
        public bool Facturado { get; set; }
        public bool IvaIncluido { get; set; }

        public DateTime Fecha { get; set; }
        public string FechaEntrega { get; set; }
        public string Observaciones { get; set; }
        public string Agregados { get; set; }

        public bool ResetValues { get; set; }
        public bool Terminada { get; set; }
        public bool Test { get; set; }

        public void AddProduct(Producto prod)
        {
            int i = SeachPossibleProduct((int)prod.id);
            
            // El producto existe en la lista.
            if (i > -1)
            {
                if (prod.CantidadTotal() <= 0)
                {
                    // Lo remuevo. 
                    Productos.RemoveAt(i);
                }
                else
                {
                    // Lo remplazo.
                    Productos[i] = prod;
                }
            }
            // El producto no esta en la lista.
            else if (prod.CantidadTotal() > 0)
            {
                // Lo agrego.
                Productos.Add(prod);
            }

            Productos = Productos.OrderBy(q => q.id).ToList();
            CalculateSubTotal();
        }

        public void AddRegalia(Producto prod)
        {
            int i = SeachPossibleRegalia((int)prod.id);

            // El producto existe en la lista.
            if (i > -1)
            {
                if (prod.CantidadTotal() <= 0)
                {
                    // Lo remuevo. 
                    Regalias.RemoveAt(i);
                }
                else
                {
                    // Lo remplazo.
                    Regalias[i] = prod;
                }
            }
            // El producto no esta en la lista.
            else if (prod.CantidadTotal() > 0)
            {
                // Lo agrego.
                Regalias.Add(prod);
            }

            Regalias = Regalias.OrderBy(q => q.id).ToList();
        }

        public int SeachPossibleProduct(int id)
        {
            for (int i = 0; i < Productos.Count; i++)
            {
                if (Productos[i].id == id)
                    return i;
            }
            return -1;
        }

        public int SeachPossibleRegalia(int id)
        {
            for (int i = 0; i < Regalias.Count; i++)
            {
                if (Regalias[i].id == id)
                    return i;
            }
            return -1;
        }

        public decimal GetTotal()
        {
            if (IvaIncluido)
                return SubTotal;
            return SubTotal + (SubTotal * 0.21m); 
        }
        public decimal GetIVA()
        {
            if (IvaIncluido)
                return SubTotal - (SubTotal / 1.21m);
            return SubTotal * 0.21m;   
        }
        private void CalculateSubTotal()
        {
            SubTotal = 0;
            foreach (var item in Productos)
            {
                SubTotal += (item.preciox20 * item.x20) +
                            (item.preciox10 * item.x10) +
                            (item.preciox4 * item.x4) +
                            (item.preciox1 * item.x1);
            }
        }

        public List<Producto> GetTotalProductOrder()
        {
            // Lista auxiliar para juntar REGALIAS y PRODUCTOS
            bool founded;
            var list = new List<Producto>();
            foreach (var item in Productos)
            {
                list.Add(new Producto(item));
            }

            //  1 - Por cada REGALIA busco en PRODUCTOS si existe asi lo sumo
            //  2 - Y si no existe la Agrego!
            for (int i = 0; i < Regalias.Count; i++)
            {
                founded = false;

                for (int j = 0; j < list.Count; j++)
                {
                    if (Regalias[i].id == list[j].id)
                    {
                        // Sumo a list (1)
                        list[j].x20 += Regalias[i].x20;
                        list[j].x10 += Regalias[i].x10;
                        list[j].x4 += Regalias[i].x4;
                        list[j].x1 += Regalias[i].x1;
                        founded = true;
                    }
                }

                if (!founded)
                {
                    // Agrego a list (2)
                    list.Add(Regalias[i]);
                }
            }


            return list;
        }

    }
}