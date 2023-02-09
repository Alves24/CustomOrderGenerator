using Library.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.DataProcessors
{
    public static class Validator 
    {
        public static bool IsInt(string value)
        {
            if (int.TryParse(value, out _))
            {
                return true;
            }
            return false;
        }
        internal static bool Check(Orden order)
        {
            if (order.Cliente.Nombre.Trim()?.Length == 0)
            {
                new InfoBox("Falta el CLIENTE!", "Ingrese el cliente de la orden!");
                return false;
            }

            if (order.Vendedor.Length == 0)
            {
                new InfoBox("Falta el VENDEDOR!", "Ingrese el vendedor de la orden!");
                return false;
            }

            if (order.Productos.Count == 0)
            {
                new InfoBox("Faltan los PRODUCTOS!!", "Ingrese los productos a la orden!");
                return false;
            }
            return true;
        }
        internal static bool Check(Producto prod)
        {
            if (prod.x20 > 0 && prod.preciox20 <= 0m)
                return false;
            if (prod.x10 > 0 && prod.preciox10 <= 0m)
                return false;
            if (prod.x4 > 0 && prod.preciox4 <= 0m)
                return false;
            if (prod.x1 > 0 && prod.preciox1 <= 0m)
                return false;
            
            return true;
        }


        // Convertors
        public static int[] ConvertToInt(string[] values)
        {
            var intValues = new int[values.Length];

            for (int i = 0; i < values.Length; i++)
            {

                if (values[i].Trim() == "")
                {
                    intValues[i] = 0;
                }
                else
                    if (!int.TryParse(values[i].Trim(), out intValues[i]))
                {
                    return null;
                }

                if (intValues[i] < 0)
                {
                    intValues[i] = 0;
                }
            }

            return intValues;
        }

        public static decimal[] ConvertToDecimal(string[] values)
        {
            var decimalValues = new decimal[values.Length];

            for (int i = 0; i < values.Length; i++)
            {

                if (values[i].Trim() == "")
                {
                    decimalValues[i] = 0;
                }
                else
                    if (!decimal.TryParse(values[i].Trim(), out decimalValues[i]))
                    {
                        return null;
                    }

                if (decimalValues[i] < 0m)
                {
                    decimalValues[i] = 0m;
                }
            }

            return decimalValues;
        }

        public static string ConvertStringColorToHexa(string color)
        {
            switch (color.ToUpper())
            {
                case "BLANCO":
                    return "#ffffff";

                case "VERDE":
                    return "#7fc980";

                case "ROJO":
                    return "#d17777";

                case "BEIGE":
                    return "#dbd0a7";

                case "GRIS HIELO":
                    return "#cedade";
                   
                default:
                    return "#ffffff";
            }
        }
    }
}
