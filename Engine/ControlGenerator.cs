using Library.Entidades;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Engine
{
    public static class ControlGenerator
    {
        public static (List<Button>, Dictionary<string, List<Button>>)
        Buttons_DashBoard(List<Producto> productList)
        {
            // Metodo que genera los BOTONES necesarios para el dashboard
            // Genera una lista de las "categorias" y un diccionario para los productos x categoria

            // Lista de Botones (Categorias):
            var buttonList = new List<Button>();

            // Diccionario de Botones (Producto): KEY categoria VALUE btnlist
            var buttonDictionary = new Dictionary<string, List<Button>>();

            var auxList = new List<Button>();
            string name;
            string text;
            string cat;
            string catAnt = "";

            foreach (var product in productList)
            {
                cat = product.categoria;
                if (cat != catAnt)
                {
                    if (auxList.Count > 0)
                    {
                        buttonDictionary[catAnt] = new List<Button>(auxList);
                        buttonList.Add(CreateButton(catAnt, catAnt));
                    }

                    catAnt = cat;
                    auxList = new List<Button>();
                }

                text = SetText(product);
                name = Regex.Replace(text, @"\s+", "") + "id" + product.id.ToString();
                auxList.Add(CreateButton(name, text));
            }

            if (auxList.Count > 0)
            {
                buttonDictionary[catAnt] = new List<Button>(auxList);
                buttonList.Add(CreateButton(catAnt, catAnt));
            }

            return (buttonList, buttonDictionary);
        }

        private static Button CreateButton(string name, string text)
        {
            var btn = new Button();

            btn.Name = name;
            btn.Text = text;
            btn.Enabled = false;
            btn.Visible = false;

            btn.Size = new Size(130, 50);
            btn.Font = new Font("Nirmala UI Semilight", 15, FontStyle.Regular);
            btn.FlatStyle = FlatStyle.Flat;
            SetButtonColor(btn);

            btn.TabStop = false;

            return btn;
        }
        private static string SetText(Producto product)
        {
            if (product.categoria == "Latex")
                return product.nombre;
            else
                return product.color;
        }

        
        public static void SetButtonColor(Button btn) 
        {
            btn.ForeColor = Color.ForestGreen;
            btn.BackColor = Color.WhiteSmoke;
            btn.FlatAppearance.BorderColor = Color.WhiteSmoke;
            btn.Font = new Font(btn.Font, FontStyle.Regular);
        }
        public static void SetButtonColorHighLight(Button btn) 
        {
            btn.ForeColor = Color.ForestGreen;
            btn.BackColor = Color.Gainsboro;
            btn.FlatAppearance.BorderColor = Color.Gainsboro;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
        }

        public static void SetButtonColor2(Button btn)
        {
            btn.ForeColor = Color.ForestGreen;
            btn.BackColor = Color.Gainsboro;
            btn.FlatAppearance.BorderColor = Color.Gainsboro;
            btn.Font = new Font(btn.Font, FontStyle.Regular);
        }
        public static void SetButtonColorHighLight2(Button btn)
        {
            btn.ForeColor = Color.ForestGreen;
            btn.BackColor = Color.Gainsboro;
            btn.FlatAppearance.BorderColor = Color.Gainsboro;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
        }
    }
}