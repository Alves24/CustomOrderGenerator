
using Library;
using Library.Entidades;
using System;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using System.Collections.Generic;

namespace Engine.DataProcessors
{
    public static class ExcelWorker
    {
        // Writing Methods
        private static readonly int ITOP = 20; // Comienzo escritura productos PAGINA ARRIBA
        private static readonly int IBOT = 84; // Comienzo escritura productos PAGINA ABAJO

        public static void GenerateOrder(Orden order)
        {
            bool good;
            string path;

            (good , path) = CopyBaseDoc();
            if (!good) return;

            FileInfo file = new FileInfo(path);
            using (ExcelPackage excelFile = new ExcelPackage(file))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet sheet = excelFile.Workbook.Worksheets[0];

                WriteInformation(sheet, order);

                WriteAllProducts(sheet, order);

                WriteTotals(sheet, order);

                CompleteRowsHeight(sheet, order);

                excelFile.Save();
            }

            if (order.Test)
            {
                //SendToDebugFolder(path, order);
                System.Diagnostics.Process.Start(path); // for testing...
            }
            else
            {
                SendToDropbox(path, order); // Upload to Dropbox.
            }
        }


        private static void WriteInformation(ExcelWorksheet sheet, Orden order)
        {
            string CurrentDate = Fechas.Formato_Dia_Mes_Anio_Numeros(order.Fecha);
            // Parte valuada (TOP)
            sheet.Cells["I7"].Value = order.Numero;
            sheet.Cells["I9"].Value = order.Vendedor;

            sheet.Cells["A6"].Value = CurrentDate;
            sheet.Cells["B8"].Value = order.Cliente.Nombre;
            sheet.Cells["B9"].Value = order.Facturado ? order.Cliente.Cuit : "";
            sheet.Cells["B10"].Value = order.Cliente.DireccionLegal;
            sheet.Cells["B11"].Value = order.Cliente.Contacto;
            sheet.Cells["B12"].Value = order.Cliente.Telefono;

            sheet.Cells["B52"].Value = order.Agregados;
            sheet.Cells["B54"].Value = order.Cliente.FormaPago;
            sheet.Cells["B56"].Value = order.FechaEntrega;
            sheet.Cells["B57"].Value = order.Cliente.FormaEntrega;
            sheet.Cells["B58"].Value = order.Cliente.DireccionEntrega;
            sheet.Cells["B60"].Value = order.Observaciones;

            sheet.Cells["I68"].Value = order.Numero;
            sheet.Cells["I70"].Value = order.Vendedor;

            sheet.Cells["A66"].Value = CurrentDate;
            sheet.Cells["B68"].Value = order.Cliente.Nombre;
            sheet.Cells["B69"].Value = order.Facturado ? order.Cliente.Cuit :"";
            sheet.Cells["B70"].Value = order.Cliente.DireccionEntrega;
            sheet.Cells["B71"].Value = order.Cliente.Telefono;

            sheet.Cells["B73"].Value = order.Cliente.FormaEntrega;
            sheet.Cells["B75"].Value = order.Observaciones;
            sheet.Cells["B116"].Value = order.Agregados;
            sheet.Cells["B118"].Value = order.FechaEntrega;
        }

        private static void WriteTotals(ExcelWorksheet sheet, Orden order)
        {
            // Escribo los totales y el color del titulo y nro de orden.
            // Si esta facturado = Fondo Azul.
            // No esta facturado = Fondo Amarillo.
            
            string titleColor;
            string totalColor = "#e6e6f2"; // Azul claro (Enfasis 2)

            if (order.Facturado)
            {
                // FACTURADO
                sheet.Cells["C15"].Value = "PEDIDO DE FACTURA";
                sheet.Cells["H56"].Value = "Sub-Total";
                sheet.Cells["H57"].Value = "21% IVA";
                sheet.Cells["H58"].Value = "TOTAL";

                sheet.Cells["I56"].Value = order.SubTotal;
                sheet.Cells["I57"].Value = order.GetIVA();
                sheet.Cells["I58"].Value = order.GetTotal();

                Paint(sheet, "I56", totalColor);
                Paint(sheet, "I57", totalColor);
               
                titleColor = "#9ac1f5";
            }
            else
            {
                //SIN FACTURAR
                sheet.Cells["C15"].Value = "PEDIDO DE REMITO";
                sheet.Cells["H58"].Value = "TOTAL";
                sheet.Cells["I58"].Value = order.SubTotal;
                titleColor = "#ebf283";
            }

            // Total (fondo)
            //Paint(sheet, "F50", totalColor);
            //Paint(sheet, "H50", totalColor);

            // Titulos (Orden, Numero)
            Paint(sheet, "A3" , titleColor);
            Paint(sheet, "C15", titleColor);
            Paint(sheet, "I9" , titleColor);
            //Paint(sheet, "C80", titleColor);
            //Paint(sheet, "I70", titleColor);
        }

        private static void WriteAllProducts(ExcelWorksheet sheet, Orden order)
        {
            // Escribo los productos de la orden

            // Modificacion 06/10/2021
            // En la hoja de arriba van a estar resaltadas las REGALIAS en la orden
            // En la de abajo las regalias van a estar ya incluidas.

            int row; // Auxiliar
            int type; // Indico el tipo de List ( 1,2,3 )
           
            // 1 - Productos (sin regalias) Hoja superior
            row = ITOP;
            type = 1;
            row = WriteListOfProducts(sheet, order.Productos, row, type);

            // 2 - Regalias Hoja superior
            type = 2;
            WriteListOfProducts(sheet, order.Regalias, row, type);

            // 3 - TODO Hoja Inferior
            row = IBOT;
            type = 3;
            WriteListOfProducts(sheet, order.GetTotalProductOrder(), row, type); 
        }
       
        private static int WriteListOfProducts(ExcelWorksheet sheet, List<Producto> productos, int row, int type)
        {
            string nombre, color;
         

            foreach (var prod in productos)
            {
                nombre = prod.nombre == "M.E.P." ? "Membrana en Pasta" : prod.nombre;
                color = prod.color == "Transparente" ? "BLANCO" : prod.color.ToUpper();

                if (prod.x20 > 0)
                {
                    WriteProduct(sheet, type, nombre, color, row, prod.x20, prod.preciox20, "x 20");
                    row++;
                }
                if (prod.x10 > 0)
                {
                    WriteProduct(sheet, type, nombre, color, row, prod.x10, prod.preciox10, "x 10");
                    row++;
                }
                if (prod.x4 > 0)
                {
                    WriteProduct(sheet, type, nombre, color, row, prod.x4, prod.preciox4, "x 4");
                    row++;
                }
                if (prod.x1 > 0)
                {
                    WriteProduct(sheet, type, nombre, color, row, prod.x1, prod.preciox1, "x 1");
                    row++;
                }

                sheet.Row(row).Height = 8;
                row++;
            }

            return row;
        }
       
        private static void WriteProduct(ExcelWorksheet sheet, int type, string nombre, string color, int Row,
                                           int cantidad, decimal precio, string peso)
        {
            string colorEnfasis , colorEnfasis2;

            if (type == 1) // Productos (Sin regalias) Hoja Superior
            {
                sheet.Cells["H" + Row.ToString()].Value = precio;
                sheet.Cells["I" + Row.ToString()].Value = precio * cantidad;

                colorEnfasis = "#e6e6f2"; // Azul clarito (Enfasis 1)
                Paint(sheet, "A" + Row.ToString(), colorEnfasis);
                Paint(sheet, "H" + Row.ToString(), colorEnfasis);
                Paint(sheet, "I" + Row.ToString(), colorEnfasis);
            }

            if (type == 2) // Regalias Hoja Superior)
            {
                sheet.Cells["H" + Row.ToString()].Value = "REGALIA";
                sheet.Cells["I" + Row.ToString()].Value = "REGALIA";

                colorEnfasis = "#c2ffc2"; // Verde Claro
                colorEnfasis2 = "#cfffcf"; //Verde muy clarito
                Paint(sheet, "A" + Row.ToString(), colorEnfasis);
                
                Paint(sheet, "B" + Row.ToString(), colorEnfasis2);
                Paint(sheet, "C" + Row.ToString(), colorEnfasis2);
                Paint(sheet, "D" + Row.ToString(), colorEnfasis2);
                Paint(sheet, "E" + Row.ToString(), colorEnfasis2);
                Paint(sheet, "F" + Row.ToString(), colorEnfasis2);
                Paint(sheet, "G" + Row.ToString(), colorEnfasis2);
                
                Paint(sheet, "H" + Row.ToString(), colorEnfasis);
                Paint(sheet, "I" + Row.ToString(), colorEnfasis);
            }

            if (type == 3) // Todo ( Productos junto a Regalias) Hoja Inferior
            {
                colorEnfasis = Validator.ConvertStringColorToHexa(color);
                Paint(sheet, "G" + Row.ToString(), colorEnfasis);
            }
            
            //EN TODOS !
            sheet.Cells["B" + Row.ToString()].Value = nombre;
            sheet.Cells["F" + Row.ToString()].Value = color;
            sheet.Cells["A" + Row.ToString()].Value = cantidad;
            sheet.Cells["D" + Row.ToString()].Value = peso;
        }

        private static void CompleteRowsHeight(ExcelWorksheet sheet, Orden order)
        {
            // Metodo para compenzar la reduccion de altura de algunas celdas 
            // probocado por los metodos que escriben los productos...
            //
            // Hoja Superior
            int Count = order.Productos.Count + order.Regalias.Count;
            
            if (Count > 1)
            {
                sheet.Row(ITOP + 26).Height = (Count * 8) + 8 + 16;
            }
            else
            {
                sheet.Row(ITOP + 26).Height = 32;
                sheet.Row(IBOT + 31).Height = 32;
                return;
            }
            //
            // Hoja Inferior
            Count = order.GetTotalProductOrder().Count;
            if (Count > 1)
            {
                sheet.Row(IBOT + 31).Height = (Count * 8) + 8;
            }
            else
            {
                sheet.Row(IBOT + 31).Height = 24;
            }
            return;
        }
        private static void Paint(ExcelWorksheet sheet, string cell, string color)
        {
            sheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
        }

        // Paths & Others
        private static (bool,string) CopyBaseDoc()
        {
            (string basePath, string copyPath) = GetEmptyBaseExcel();

            try
            {
                if (File.Exists(copyPath))
                {
                    File.SetAttributes(copyPath, FileAttributes.Normal);
                    File.Delete(copyPath);
                }

                File.Copy(basePath, copyPath);

                return (true, copyPath);
            }
            catch (Exception e)
            {
                new InfoBox("ERROR", 
                            "Cierre los excel que tenga abiertos",
                            e.Message);

                return (false, e.Message);
            }
        }
        public static (string, string) GetEmptyBaseExcel()
        {
            const string EmptyExcelFile = @"Files\BaseFile.xlsx";
            const string EmptyExcelFileCopy = @"Files\EDITED_BaseFile.xlsx";
            return (EmptyExcelFile, EmptyExcelFileCopy);
        }

        private static string GenerateDropboxPath(Orden order)
        {
            string facturado = order.Facturado ? "MEP FACTURADO  " : "MEP CONDICION 2";

            string fileName =  $"{ order.Numero } - " +
                               $"{ facturado } - " +
                               $"{ order.Cliente.Nombre } - " +
                               $"{ Fechas.Formato_Dia_Mes_Anio_Numeros(order.Fecha)}.xlsx";

            return PathManager.GetDropboxPath_MEP() + @"\" + fileName;
        }
        private static void SendToDebugFolder(string path, Orden order)
        {

            // Muevo el excel generado en "path" a la ruta de dropbox.
            string fileName;
            string FinalPath;

            string facturado = order.Facturado ? "MEP FACTURADO" : "MEP CONDICION 2";
            fileName = $"{ order.Numero } - { facturado } - { order.Cliente.Nombre }.xlsx";
            FinalPath = @"C:\Users\julia\Dropbox\DebugFolder\" + fileName;

            try
            {
                File.Copy(path, FinalPath);

                System.Diagnostics.Process.Start(FinalPath);
                order.Terminada = true;
            }
            catch (Exception e)
            {
                string Error = "No se pudo enviar a DebugFolder " +
                               "(method SendToDebug, ExcelWorker)\n" + DateTime.Now.ToString();
                new InfoBox(Error);
                Logger.Error(Error + e.Message);
            }
        }
        private static void SendToDropbox(string path, Orden order)
        {
            // Muevo el excel generado en "path" a la ruta de dropbox.
            //
            string dropboxFinalPath = GenerateDropboxPath(order);
            //
            //
            try
            {
                File.Copy(path, dropboxFinalPath);
                System.Diagnostics.Process.Start(dropboxFinalPath);
                order.Terminada = true;
            }
            catch (Exception e)
            {
                string Error = "No se pudo enviar a Dropbox " +
                               "(method SendToDropbox, ExcelWorker)\n" 
                               + DateTime.Now.ToString();
                new InfoBox(Error);
                Logger.Error(Error + e.Message);
            }
        }
        internal static void Delete(Orden order)
        {
            string dropboxFinalPath = GenerateDropboxPath(order);

            try
            {
                if (File.Exists(dropboxFinalPath))
                {
                    File.SetAttributes(dropboxFinalPath, FileAttributes.Normal);
                    File.Delete(dropboxFinalPath);
                    new InfoBox($"Orden {order.Numero} Eliminada");
                    order.Terminada = false;
                }
                else
                {
                    new InfoBox("El Archivo ya no existe!");
                }
            }
            catch (Exception e)
            {
                string Error = "No se pudo borrar orden en dropbox " +
                               "(method Delete, ExcelWorker)\n" + DateTime.Now.ToString();
                new InfoBox(Error,"Cierre las planillas excel e intente de nuevo", e.Message);
                Logger.Error(Error + e.Message);
            }
        }
    }
}