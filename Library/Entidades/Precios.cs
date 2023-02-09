using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entidades
{
    [Serializable]
    public class Precios
    {
        public Precios(int x20 , int x10, int x4, int x1)
        {
            this.x20 = x20;
            this.x10 = x10;
            this.x4 = x4;
            this.x1 = x1;
        }

        public Precios(Producto producto)
        {
            x20 = producto.preciox20;
            x10 = producto.preciox10;
            x4 = producto.preciox4;
            x1 = producto.preciox1;
        }


        public decimal x20 { get; set; }
        public decimal x10 { get; set; }
        public decimal x4  { get; set; }
        public decimal x1 { get; set; }

        public void SetPrices(Producto producto)
        {
            x20 = producto.preciox20;
            x10 = producto.preciox10;
            x4  = producto.preciox4;
            x1  = producto.preciox1;
        }
    }
}
