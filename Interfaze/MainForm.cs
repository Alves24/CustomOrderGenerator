using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.DataProcessors;
using Library;
using Library.Entidades;


namespace Interface
{
    public partial class MainForm : Form
    {
        private ControlStatus status;
        private Core core;

        // Loads
        public MainForm()
        {
            InitializeComponent();
        }
        private void InitializeControls()
        {
            core = new Core();
            status = new ControlStatus();


            // Inizializo variables y posiciones.

            pRegalias.Location = new Point(4, 4);
            pQuantities.Location = new Point(-1, 34);

            // Cargo txtClientes
            this.txtCliente = core.LoadClientsOnTxt(txtCliente);

            // Cargo UserPreferences
            core.UserPreferences = DBController.LoadPreferences();
            this.txtVendedor.Text = core.UserPreferences.lastSeller;

            // Cargo ComboBox!
            var ProductNamesList = core.Productos.Select(x => x.nombre)
                                                      .Distinct()
                                                      .ToList();

            cbxProductNames.DataSource = ProductNamesList;
            cbxProductNames.SelectedIndex = 0;
            cbxProductsRegalias.DataSource = ProductNamesList;
            cbxProductsRegalias.SelectedIndex = 0;

            cbxFormaEntrega.SelectedIndex = 0;
            cbxCondicion.SelectedIndex = 0;

            BaseColorTable();
        }
        private void InitializeDataEntry()
        {
            txt1.Text = "";
            txt4.Text = "";
            txt10.Text = "";
            txt20.Text = "";
            txtPrecio1.Text = "";
            txtPrecio4.Text = "";
            txtPrecio10.Text = "";
            txtPrecio20.Text = "";

            //txtCliente.Text = "";
            txtCuit.Text = "";
            txtContacto.Text = "";
            txtDireccionEntrega.Text = "";
            txtDireccionLegal.Text = "";
            txtFormaPago.Text = "";
            txtObservaciones.Text = "";
            txtTelefono.Text = "";
            txtAgregados.Text = "";
            CreateOrderBtn.Enabled = false;

            tablaOrden.Rows.Clear();
            txtCliente.Focus();
        }
        private void MainInterface_Load(object sender, EventArgs e)
        {
            // Idioma - Cultura
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Size = new Size(1366, 768);

            InitializeEvents();
            InitializeControls();
            txtCliente.Focus();

            //Testing();
        }

        private void Testing()
        {
            txtCliente.Text = "Cliente Prueba";
            core.Order.Productos.Add(core.Productos[0]);
            //MouseClick(btnTestOrder,null);
        }
        private void Reset_QuatityPanel()
        {
            txt20.Text = "";
            txt10.Text = "";
            txt4.Text = "";
            txt1.Text = "";
            txtPrecio20.Text = "";
            txtPrecio10.Text = "";
            txtPrecio4.Text = "";
            txtPrecio1.Text = "";
        }
        private void LoadPrices_QuantityPanel()
        {
            if (pQuantities.Visible == false) return;
            
            bool LoadedPrices = false;
            decimal x20, x10, x4, x1;
            
            try // Si es que existen, cargo los precios del cliente
            {
                if (core.Order.Facturado)
                {
                    if (core.Order.IvaIncluido)
                    {
                        x20 = core.Order.Cliente.PreciosIVA_INCLUIDO[status.idProduct].x20;
                        x10 = core.Order.Cliente.PreciosIVA_INCLUIDO[status.idProduct].x10;
                        x4 = core.Order.Cliente.PreciosIVA_INCLUIDO[status.idProduct].x4;
                        x1 = core.Order.Cliente.PreciosIVA_INCLUIDO[status.idProduct].x1;
                        LoadedPrices = true;
                    }
                    else
                    { 
                        x20 = core.Order.Cliente.Precios[status.idProduct].x20;
                        x10 = core.Order.Cliente.Precios[status.idProduct].x10;
                        x4 = core.Order.Cliente.Precios[status.idProduct].x4;
                        x1 = core.Order.Cliente.Precios[status.idProduct].x1;
                        LoadedPrices = true;
                    }
                }
                else
                {
                    x20 = core.Order.Cliente.PreciosNegro[status.idProduct].x20;
                    x10 = core.Order.Cliente.PreciosNegro[status.idProduct].x10;
                    x4 = core.Order.Cliente.PreciosNegro[status.idProduct].x4;
                    x1 = core.Order.Cliente.PreciosNegro[status.idProduct].x1;
                    LoadedPrices = true;
                }

                txtPrecio20.Text = x20 == 0m ? "" : x20.ToString();
                txtPrecio10.Text = x10 == 0m ? "" : x10.ToString();
                txtPrecio4.Text = x4 == 0m ? "" : x4.ToString();
                txtPrecio1.Text = x1 == 0m ? "" : x1.ToString();
            }
            catch (Exception) { }

            if (!LoadedPrices)
            {
                txtPrecio20.Text = "";
                txtPrecio10.Text = "";
                txtPrecio4.Text = "";
                txtPrecio1.Text = "";
            }
        }
        private void LoadDataGrid_QuantityPanel()
        {
            // PRODUCTOS
            Producto prod = null;

            int id = core.Order.SeachPossibleProduct(status.idProduct);
            if (id >= 0) prod = core.Order.Productos[id];

            if (prod != null)
            {
                txt20.Text = prod.x20 > 0 ? prod.x20.ToString() : "";
                txt10.Text = prod.x10 > 0 ? prod.x10.ToString() : "";
                txt4.Text = prod.x4 > 0 ? prod.x4.ToString() : "";
                txt1.Text = prod.x1 > 0 ? prod.x1.ToString() : "";
            }


            // REGALIAS
            if (pRegalias.Visible == true)
            {
                prod = null;
                id = core.Order.SeachPossibleRegalia(status.idProductRegalia);
                if (id >= 0) prod = core.Order.Regalias[id];

                if (prod != null)
                {
                    txtR20.Text = prod.x20 > 0 ? prod.x20.ToString() : "";
                    txtR10.Text = prod.x10 > 0 ? prod.x10.ToString() : "";
                    txtR4.Text = prod.x4 > 0 ? prod.x4.ToString() : "";
                    txtR1.Text = prod.x1 > 0 ? prod.x1.ToString() : "";
                }
            }
        }

        #region Events
        // Custom multiuse Events
        private void InitializeEvents()
        {
            pCenter.MouseEnter += MouseEnter;
            addProductsBtn.MouseClick += MouseClick;
            CloseApplication.MouseEnter += MouseEnter;
            CloseApplication2.MouseClick += MouseClick;
            CloseApplication2.MouseLeave += MouseLeave;
            MinimizeApp.MouseEnter += MouseEnter;
            MinimizeApp2.MouseLeave += MouseLeave;
            MinimizeApp2.MouseClick += MouseClick;

            CreateOrderBtn.MouseClick += MouseClick;
            btnTestOrder.MouseClick += MouseClick;
          
            nuevaOrdenToolStripMenuItem.Click += MouseClick;

            TopMenu.MouseDown += MouseDown;

            txt20.MouseWheel += MouseWheel;
            txt10.MouseWheel += MouseWheel;
            txt4.MouseWheel += MouseWheel;
            txt1.MouseWheel += MouseWheel;
            txtR20.MouseWheel += MouseWheel;
            txtR10.MouseWheel += MouseWheel;
            txtR4.MouseWheel += MouseWheel;
            txtR1.MouseWheel += MouseWheel;

            txt20.KeyDown += KeyDown;
            txt10.KeyDown += KeyDown;
            txt4.KeyDown += KeyDown;
            txt1.KeyDown += KeyDown;
            txtPrecio20.KeyDown += KeyDown;
            txtPrecio10.KeyDown += KeyDown;
            txtPrecio4.KeyDown += KeyDown;
            txtPrecio1.KeyDown += KeyDown;
            txtCliente.KeyDown += KeyDown;
            txtDireccionLegal.KeyDown += KeyDown;
            txtTelefono.KeyDown += KeyDown;
            txtCuit.KeyDown += KeyDown;
            txtFechaEntrega.KeyDown += KeyDown;
            txtFormaPago.KeyDown += KeyDown;
            txtObservaciones.KeyDown += KeyDown;
            txtContacto.KeyDown += KeyDown;
            txtAgregados.KeyDown += KeyDown;
            txtDireccionEntrega.KeyDown += KeyDown;
            cbxFormaEntrega.KeyDown += KeyDown;

            txtCliente.Leave += TextFocusLeave;
            txtCuit.Leave += TextFocusLeave;
            txtTelefono.Leave += TextFocusLeave;
            txtContacto.Leave += TextFocusLeave;
            txtDireccionEntrega.Leave += TextFocusLeave;
            txtVendedor.Leave += TextFocusLeave;
            txtFormaPago.Leave += TextFocusLeave;
            txtObservaciones.Leave += TextFocusLeave;
            txtDireccionLegal.Leave += TextFocusLeave;
            txtAgregados.Leave += TextFocusLeave;
            txtFechaEntrega.Leave += TextFocusLeave;

            // ComboBox
            cbxColorsRegalias.SelectedValueChanged += ColorComboBox_Changed;
            cbxProductColors.SelectedValueChanged += ColorComboBox_Changed;
            cbxProductNames.SelectedValueChanged += ProductComboBox_Changed;
            cbxProductsRegalias.SelectedValueChanged += ProductComboBox_Changed;

            // Dash
            DashPic_Client.MouseEnter += DashBoard_MouseEnter;
            DashPic_Client_MouseOver.MouseLeave += DashBoard_MouseLeave;
            DashPic_Client_MouseOver.MouseClick += DashBoard_Click;
            DashPic_Products.MouseEnter += DashBoard_MouseEnter;
            DashPic_Products_MouseOver.MouseLeave += DashBoard_MouseLeave;
            DashPic_Products_MouseOver.MouseClick += DashBoard_Click;

            //Regalias
            addRegaliasBtn.MouseClick += MouseClick;
            btnActivarRegalias.MouseClick += MouseClick;
            btnCloseRegalias.MouseClick += MouseClick;
        }
        private void DashBoard_Click(object sender, EventArgs e)    
        {
            var pic = sender as PictureBox;

            if (pic.Name == DashPic_Client_MouseOver.Name)
            {
                DashPic_Client_Selected.Visible = true;
                DashPic_Client_MouseOver.Visible = false;

                DashPic_Products.Visible = true;
                DashPic_Products_MouseOver.Visible = false;
                DashPic_Products_Selected.Visible = false;

                pQuantities.Visible = false;
            }

            if (pic.Name == DashPic_Products_MouseOver.Name)
            {
                if (status.blockProducts)
                    new InfoBox("Ingrese el cliente primero!!");
                else
                {
                    DashPic_Products_Selected.Visible = true;
                    DashPic_Products_MouseOver.Visible = false;

                    DashPic_Client.Visible = true;
                    DashPic_Client_MouseOver.Visible = false;
                    DashPic_Client_Selected.Visible = false;

                    pRegalias.Visible = false;
                    pQuantities.Visible = true;
                    LoadPrices_QuantityPanel();
                }
            }
        }
        private void DashBoard_MouseEnter(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;

            if (pic.Name == DashPic_Client.Name)
            {
                DashPic_Client_MouseOver.Visible = true;
                DashPic_Client.Visible = false;
            }

            if (pic.Name == DashPic_Products.Name)
            {
                DashPic_Products_MouseOver.Visible = true;
                DashPic_Products.Visible = false;
            }
        }
        private void DashBoard_MouseLeave(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;

            if (pic.Name == DashPic_Client_MouseOver.Name)
            {
                DashPic_Client_MouseOver.Visible = false;
                DashPic_Client.Visible = true;
            }

            if (pic.Name == DashPic_Products_MouseOver.Name)
            {
                DashPic_Products_MouseOver.Visible = false;
                DashPic_Products.Visible = true;
            }
        }
        private void MouseClick(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                var x = sender as Control;

                switch (x.Name)
                {
                    case "addRegaliasBtn":
                        AddRegalias();
                        break;

                    case "addProductsBtn":
                        AddProducts();
                        break;

                    case "btnActivarRegalias":
                        pRegalias.Visible = true;
                        LoadDataGrid_QuantityPanel();
                        break;

                    case "btnCloseRegalias":
                        pRegalias.Visible = false;
                        break;

                    case "btnTestOrder":
                        core.Order.Test = true;
                        AsignDataToOrder();
                        break;

                    case "CloseApplication2":
                        Application.Exit();
                        break; 

                    case "btnNuevaOrden":
                        nuevaOrdenToolStripMenuItem_Click(null, null);
                        break;

                    case "MinimizeApp2":
                        this.WindowState = FormWindowState.Minimized;
                        break;

                    case "CreateOrderBtn":
                        core.Order.Test = false;
                        AsignDataToOrder();
                        break;


                    default:
                        break;
                }
            }
        }
        private void MouseEnter(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                var p = sender as PictureBox;

                switch (p.Name)
                {

                    case "CloseApplication":
                        CloseApplication2.Enabled = true;
                        CloseApplication2.Visible = true;
                        break;

                    case "MinimizeApp":
                        MinimizeApp2.Enabled = true;
                        MinimizeApp2.Visible = true;
                        break;

                    default:
                        break;
                }
            }
        }
        private void MouseLeave(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                var p = sender as PictureBox;

                switch (p.Name)
                {

                    case "CloseApplication2":
                        CloseApplication2.Enabled = false;
                        CloseApplication2.Visible = false;
                        break;

                    case "MinimizeApp2":
                        MinimizeApp2.Enabled = false;
                        MinimizeApp2.Visible = false;
                        break;

                    default:
                        break;
                }
            }
        }
        private void MouseWheel(object sender, MouseEventArgs e)
        {
            if (sender is TextBox)
            {
                var txt = sender as TextBox;
                short value;

                if (e.Delta > 0)
                {
                    value = 1;
                }
                else
                {
                    value = -1;
                }

                if (txt.Text.Trim() == "")
                    txt.Text = "1";
                else
                if (Validator.IsInt(txt.Text))
                {
                    txt.Text = (Convert.ToInt32(txt.Text) + value).ToString();
                    if (int.Parse(txt.Text) < 0) txt.Text = "0";
                }

            }

        }
        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is MenuStrip)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }
        private void KeyDown(object sender, KeyEventArgs e)
        {
            var t = sender as Control;

            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Right
                && e.KeyCode != Keys.Left && e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (pQuantities.Visible && pQuantities.Enabled && e.KeyCode == Keys.Enter)
            {
                MouseClick(addProductsBtn, null);
                return;
            }

            switch (t.Name)
            {
                case "txt20":
                    if (e.KeyCode == Keys.Down) txt10.Focus();
                    break;

                case "txt10":
                    if (e.KeyCode == Keys.Up) txt20.Focus();
                    if (e.KeyCode == Keys.Down) txt4.Focus();
                    break;

                case "txt4":
                    if (e.KeyCode == Keys.Up) txt10.Focus();
                    if (e.KeyCode == Keys.Down) txt1.Focus();
                    break;

                case "txt1":
                    if (e.KeyCode == Keys.Up) txt4.Focus();
                    break;

                case "txtPrecio20":
                    if (e.KeyCode == Keys.Down) txtPrecio10.Focus();

                    break;

                case "txtPrecio10":
                    if (e.KeyCode == Keys.Up) txtPrecio20.Focus();
                    if (e.KeyCode == Keys.Down) txtPrecio4.Focus();

                    break;

                case "txtPrecio4":
                    if (e.KeyCode == Keys.Up) txtPrecio10.Focus();
                    if (e.KeyCode == Keys.Down) txtPrecio1.Focus();

                    break;

                case "txtPrecio1":
                    if (e.KeyCode == Keys.Up) txtPrecio4.Focus();
                    break;


                //
                case "txtCliente":
                    if (e.KeyCode == Keys.Up) txtAgregados.Focus();
                    if (e.KeyCode == Keys.Down) txtCuit.Focus();
                    break;

                case "txtCuit":
                    if (e.KeyCode == Keys.Up) txtCliente.Focus();
                    if (e.KeyCode == Keys.Down) txtTelefono.Focus();
                    break;

                case "txtTelefono":
                    if (e.KeyCode == Keys.Up) txtCuit.Focus();
                    if (e.KeyCode == Keys.Down) txtContacto.Focus();
                    break;

                case "txtContacto":
                    if (e.KeyCode == Keys.Up) txtTelefono.Focus();
                    if (e.KeyCode == Keys.Down) txtDireccionEntrega.Focus();
                    break;


                //
                case "txtDireccionEntrega":
                    if (e.KeyCode == Keys.Up) txtContacto.Focus();
                    if (e.KeyCode == Keys.Down) txtDireccionLegal.Focus();
                    break;

                case "txtDireccionLegal":
                    if (e.KeyCode == Keys.Up) txtDireccionEntrega.Focus();
                    if (e.KeyCode == Keys.Down) txtFormaPago.Focus();
                    break;

                case "txtFormaPago":
                    if (e.KeyCode == Keys.Up) txtDireccionLegal.Focus();
                    if (e.KeyCode == Keys.Down) cbxFormaEntrega.Focus();
                    break;

                case "cbxFormaEntrega":
                    if (e.KeyCode == Keys.Up) txtFormaPago.Focus();
                    if (e.KeyCode == Keys.Down) txtFechaEntrega.Focus();
                    break;

                //
                case "txtFechaEntrega":
                    if (e.KeyCode == Keys.Up) cbxFormaEntrega.Focus();
                    if (e.KeyCode == Keys.Down) txtObservaciones.Focus();
                    break;

                case "txtObservaciones":
                    if (e.KeyCode == Keys.Up) txtFechaEntrega.Focus();
                    if (e.KeyCode == Keys.Down) txtAgregados.Focus();
                    break;

                case "txtAgregados":
                    if (e.KeyCode == Keys.Up) txtObservaciones.Focus();
                    if (e.KeyCode == Keys.Down) txtCliente.Focus();

                    break;

                default:
                    break;
            }
        }
        private void TextFocusLeave(object sender, EventArgs e)
        {
            // Funcion para formatear los TXTs. (Ponerle mayuscula a la primer letra)
            // Ademas verifica que cliente NO este vacio.
            if (sender is TextBox)
            {
                var box = sender as TextBox;
                string text = box.Text.Trim();

                if (text.Length == 0)
                {
                    if (box.Name == "txtCliente") box.Focus();
                    return;
                }
                else 
                    if (text.Length == 1)
                        box.Text = char.ToUpper(text[0]).ToString();
                    else
                        box.Text = char.ToUpper(text[0]) + text.Substring(1);
            }
        }

        // Default Events
        private void nuevaOrdenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core = new Core();
            status = new ControlStatus();

            InitializeDataEntry();

            txtCliente.Text = "";
            ChangeDudesColorTo("black");

            DashBoard_Click(DashPic_Client_MouseOver, null);

            cbxFormaEntrega.SelectedIndex = 0;

            cbxProductNames.SelectedIndex = 0;
            cbxProductsRegalias.SelectedIndex = 0;

            cbxCondicion_SelectedIndexChanged(null, null); //IDK
            ColorComboBox_Changed(cbxProductColors, null);
            ColorComboBox_Changed(cbxColorsRegalias, null);
        }
        private void FacturadoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void cbxCondicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            const string Format = "{0:C2}";

            if (cbxCondicion.SelectedIndex == 0) //Derecho
            {
                core.Order.Facturado = false;
                core.Order.IvaIncluido = false;
                lbl_IVANum.Text = "X";
                lbl_SubTotalNum.Text = "X";
                lbl_TotalNum.Text = String.Format(Format, core.Order.SubTotal);
                lbl_FacInfo_pQuantities.Text = "(DERECHO)";
                //lbl_FacInfo_pQuantities.ForeColor = Color.Crimson;
                //cbxCondicion.ForeColor = Color.Crimson;

                lbl_IVA.Enabled = false;
                lbl_IVANum.Enabled = false;
                lbl_SubTotal.Enabled = false;
                lbl_SubTotalNum.Enabled = false;
            }
            if (cbxCondicion.SelectedIndex == 2) //NETO + IVA
            {
                core.Order.Facturado = true;
                core.Order.IvaIncluido = false;
                lbl_IVANum.Text = String.Format(Format, core.Order.GetIVA());
                lbl_TotalNum.Text = String.Format(Format, core.Order.GetTotal());
                lbl_SubTotalNum.Text = String.Format(Format, core.Order.SubTotal);
                lbl_FacInfo_pQuantities.Text = "(+IVA)";
                //lbl_FacInfo_pQuantities.ForeColor = Color.ForestGreen;
                //cbxCondicion.ForeColor = Color.ForestGreen;

                lbl_IVA.Enabled = true;
                lbl_IVANum.Enabled = true;
                lbl_SubTotal.Enabled = true;
                lbl_SubTotalNum.Enabled = true;
            }
            if (cbxCondicion.SelectedIndex == 1) //IVA INCLUIDO
            {
                core.Order.Facturado = true;
                core.Order.IvaIncluido = true;
                lbl_IVANum.Text = String.Format(Format, core.Order.GetIVA());
                lbl_TotalNum.Text = String.Format(Format, core.Order.GetTotal());
                lbl_SubTotalNum.Text = "X";
                lbl_FacInfo_pQuantities.Text = "(IVA INCL.)";
                //lbl_FacInfo_pQuantities.ForeColor = Color.Blue;
                //cbxCondicion.ForeColor = Color.Blue;

                lbl_IVA.Enabled = true;
                lbl_IVANum.Enabled = true;
                lbl_SubTotal.Enabled = false;
                lbl_SubTotalNum.Enabled = false;
            }


            LoadPrices_QuantityPanel();
        }
        private void cbxFormaEntrega_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }
        private void ProductComboBox_Changed(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;

            // Me guardo los colores del producto
            var colores = from prod in core.Productos
                          where prod.nombre == ctrl.Text
                          select prod.color;

            if (ctrl.Name == cbxProductNames.Name)
            {
                // Cargo Colores
                cbxProductColors.DataSource = colores.ToList();

                // Cargo los precios
                var product = core.Productos.FirstOrDefault
                            (p => p.nombre == cbxProductNames.Text && p.color == cbxProductColors.Text);
                status.idProduct = (int)product.id;

                Reset_QuatityPanel();
                LoadPrices_QuantityPanel();
                LoadDataGrid_QuantityPanel();
            }
            else if (ctrl.Name == cbxProductsRegalias.Name)
            {
                // Cargo Colores
                cbxColorsRegalias.DataSource = colores.ToList();
            }
        }
        private void ColorComboBox_Changed(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            string ColorHexa = Validator.ConvertStringColorToHexa(ctrl.Text);

            if (ctrl.Name == cbxProductColors.Name)
            {
                txt20.Text = "";
                txt10.Text = "";
                txt4.Text = "";
                txt1.Text = "";

                var product = core.Productos.FirstOrDefault
                            (p => p.nombre == cbxProductNames.Text && p.color == cbxProductColors.Text);

                status.idProduct = (int)product.id;

                ImgColorProduct.FillColor = ColorTranslator.FromHtml(ColorHexa);

                LoadDataGrid_QuantityPanel();
            }
            else if (ctrl.Name == cbxColorsRegalias.Name)
            {
                txtR20.Text = "";
                txtR10.Text = "";
                txtR4.Text = "";
                txtR1.Text = "";

                var product = core.Productos.FirstOrDefault
                            (p => p.nombre == cbxProductsRegalias.Text && p.color == cbxColorsRegalias.Text);

                status.idProductRegalia = (int)product.id;

                ImgColorProductREGALIAS.FillColor = ColorTranslator.FromHtml(ColorHexa);

                LoadDataGrid_QuantityPanel();
            }
        }
        private void txtDireccionEntrega_Leave(object sender, EventArgs e)
        {
            var collection = new AutoCompleteStringCollection();
            collection.Add(txtDireccionEntrega.Text);
            txtDireccionLegal.AutoCompleteCustomSource = collection;
        }
        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            var box = sender as TextBox;
            string text = Regex.Replace(box.Text, @"\s+", " ");

            //ChangeDudesColorTo("black");
            //box.AutoCompleteMode = AutoCompleteMode.None; // For debuging!

            if (text.Length < 3)
            {
                box.Focus();
                ChangeDudesColorTo("red");
                status.blockProducts = true;
            }
            else
            {
                status.blockProducts = false;
                foreach (var cliente in core.Clientes)
                {
                    if (string.Equals(text, cliente.Nombre, StringComparison.OrdinalIgnoreCase))
                    {
                        var clientData = core.FindCliente(cliente);

                        if (clientData != null)
                        {
                            txtCliente.Text = clientData.Nombre;
                            txtContacto.Text = clientData.Contacto;
                            txtCuit.Text = clientData.Cuit;
                            txtTelefono.Text = clientData.Telefono;
                            txtDireccionEntrega.Text = clientData.DireccionEntrega;
                            txtDireccionLegal.Text = clientData.DireccionLegal;
                            cbxFormaEntrega.Text = clientData.FormaEntrega;
                            txtFormaPago.Text = clientData.FormaPago;

                            if (clientData.Vendedor != null) txtVendedor.Text = clientData.Vendedor;  

                            core.Order.Cliente = clientData;
                            core.Order.Cliente.IsSerialized = true;

                            ChangeDudesColorTo("green");
                            return;
                        }
                        else
                        {
                            core.Order.Cliente.IsSerialized = false;
                        }
                    }
                    else if (core.Order.Cliente.IsSerialized)
                    {
                        core.Order.Cliente.IsSerialized = false;
                       
                        InitializeDataEntry();
                        
                        core.Order.Cliente = new Cliente();
                    }                    
                }
            }
            return;
        }
        #endregion

        // Main Methods
        private void AddProducts()
        {
            bool wasGood = core.AgregarProducto(status.idProduct, txt20.Text, txt10.Text, txt4.Text, txt1.Text, txtPrecio20.Text, 
                                txtPrecio10.Text, txtPrecio4.Text, txtPrecio1.Text);

            if (wasGood) RefreshTable();
            
            LoadDataGrid_QuantityPanel();
        }
        private void AddRegalias()
        {
            bool wasGood = core.AgregarRegalias(status.idProductRegalia, txtR20.Text, txtR10.Text, txtR4.Text, txtR1.Text);

            if (wasGood)
            {
                RefreshTable();
                labelInfoPanelRegalias.Visible = false;
            }
            else
                labelInfoPanelRegalias.Visible = true;
        }
        private void AsignDataToOrder()
        {
            core.Order.Numero = PathManager.GetLastOrderNumber() + 1;
            core.Order.Cliente.Nombre = txtCliente.Text;
            core.Order.Cliente.Cuit = txtCuit.Text;
            core.Order.Cliente.Telefono = txtTelefono.Text;
            core.Order.Cliente.Contacto = txtContacto.Text;
            core.Order.Cliente.DireccionLegal = txtDireccionLegal.Text;
            
            core.Order.Cliente.Vendedor = txtVendedor.Text;
            core.Order.Vendedor = txtVendedor.Text;

            core.Order.Cliente.DireccionEntrega = txtDireccionEntrega.Text;
            core.Order.Cliente.FormaEntrega = cbxFormaEntrega.Text;
            core.Order.Cliente.FormaPago = txtFormaPago.Text;

            core.Order.FechaEntrega = txtFechaEntrega.Text;
            core.Order.Observaciones = txtObservaciones.Text;
            core.Order.Agregados = txtAgregados.Text;

            core.Order.Fecha = DateTime.Now;

            // Creacion de la ORDEN!
            core.CreateOrder();
            // Post Creacion de la ORDEN!

            CreateOrderBtn.Enabled = false;
            if (core.Order == null)
            {
                nuevaOrdenToolStripMenuItem_Click(null,null);
            }
            else if (!core.Order.Test)
            {
                CreateOrderBtn.Enabled = true;
            }
        }
        
        // Draw Table
        private void RefreshTable()
        {
            tablaOrden.Rows.Clear();
            int n;
            
            bool backColor = false;
            
            foreach (var prod in core.Order.Productos)
            {
                if (prod.x20 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, prod.nombre, prod.color, "x20", prod.preciox20, prod.x20, backColor);
                }
                if (prod.x10 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, prod.nombre, prod.color, "x10", prod.preciox10, prod.x10, backColor);
                }
                if (prod.x4 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, prod.nombre, prod.color, "x4", prod.preciox4, prod.x4, backColor);
                }
                if (prod.x1 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, prod.nombre, prod.color, "x1", prod.preciox1, prod.x1, backColor);
                }

                backColor = backColor ? false : true;
            }

            foreach (var rega in core.Order.Regalias)
            {
                if (rega.x20 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, rega.nombre, rega.color, "x20", 0m, rega.x20, backColor);
                }
                if (rega.x10 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, rega.nombre, rega.color, "x10", 0m, rega.x10, backColor);
                }
                if (rega.x4 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, rega.nombre, rega.color, "x4", 0m, rega.x4, backColor);
                }
                if (rega.x1 > 0)
                {
                    n = tablaOrden.Rows.Add();
                    WriteRow(n, rega.nombre, rega.color, "x1", 0m, rega.x1, backColor);
                }
            }

            cbxCondicion_SelectedIndexChanged(null, null); //IDK
        }
        private void WriteRow(int row, string nombre, string color, string peso, decimal precio, int cantidad, bool backColor)
        {
            tablaOrden.Rows[row].Cells[0].Value = cantidad;
            tablaOrden.Rows[row].Cells[1].Value = nombre;
            tablaOrden.Rows[row].Cells[2].Value = peso;
            tablaOrden.Rows[row].Cells[3].Value = color;
            if (precio > 0)
            {
                tablaOrden.Rows[row].Cells[5].Value = String.Format("{0:C2}", precio);
                tablaOrden.Rows[row].Cells[6].Value = String.Format("{0:C2}", (decimal)precio * cantidad);
                RowColor(row, color, backColor);
            }
            else
            {
                tablaOrden.Rows[row].Cells[5].Value = "REGALIA";
                tablaOrden.Rows[row].Cells[6].Value = "REGALIA";
                RowColor(row, "REGALIA" , backColor);
            }
            
        }
        private void RowColor(int row , string color, bool backcolor)
        {
            // Seteo backcolor de la columna 'color'
            Color backgroundColor = backcolor ? Color.White : Color.WhiteSmoke;
            string productColor = "default";
            switch (color.ToUpper())
            {
                case "BLANCO":
                    productColor = "default";
                    break;

                case "VERDE":
                    productColor = "#7fc980";
                    break;

                case "ROJO":
                    productColor = "#d17777";
                    break;

                case "BEIGE":
                    productColor = "#dbd0a7";
                    break;

                case "GRIS HIELO":
                    productColor = "#cedade";
                    break;

                case "REGALIA":
                    backgroundColor = System.Drawing.ColorTranslator.FromHtml("#bcffba");
                    break;
            }

                
            tablaOrden.Rows[row].DefaultCellStyle.BackColor = backgroundColor;
            tablaOrden.Rows[row].DefaultCellStyle.SelectionBackColor = backgroundColor;

            if (productColor != "default")
            {
                tablaOrden.Rows[row].Cells[4].Style.BackColor = System.Drawing.ColorTranslator.FromHtml(productColor);
                tablaOrden.Rows[row].Cells[4].Style.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml(productColor);
            }
        }

        // Others...

        private void ChangeDudesColorTo(string color)
        {
            var value = new bool[] { false, false, false };

            switch (color)
            {
                case "red": 
                    value[0] = true; break;
                case "green":
                    value[1] = true; break;
                default:
                    value[2] = true; break;
            }

            DudeIcon_Red.Visible = value[0];
            DudeIcon_Green.Visible = value[1];
            DudeIcon_Black.Visible = value[2];
        }
        private void DetectIfProductExistInTable()
        {
            int i = core.Order.SeachPossibleProduct(status.idProduct);
            if (i > -1)
            {
                // Ya existe el producto en la tabla
                // Lo cargo de vuelta
                txt20.Text = core.Order.Productos[i].x20 == 0 ? "" : core.Order.Productos[i].x20.ToString();
                txt10.Text = core.Order.Productos[i].x10 == 0 ? "" : core.Order.Productos[i].x10.ToString();
                txt4.Text  = core.Order.Productos[i].x4  == 0 ? "" : core.Order.Productos[i].x4.ToString();
                txt1.Text  = core.Order.Productos[i].x1  == 0 ? "" : core.Order.Productos[i].x1.ToString();

                txtPrecio20.Text = core.Order.Productos[i].preciox20 == 0 ? "" : core.Order.Productos[i].preciox20.ToString();
                txtPrecio10.Text = core.Order.Productos[i].preciox10 == 0 ? "" : core.Order.Productos[i].preciox10.ToString();
                txtPrecio4.Text  = core.Order.Productos[i].preciox4  == 0 ? "" : core.Order.Productos[i].preciox4.ToString();
                txtPrecio1.Text  = core.Order.Productos[i].preciox1  == 0 ? "" : core.Order.Productos[i].preciox1.ToString();
            }
        }
        private void BaseColorTable()
        {
            tablaOrden.Columns["cantidad"].DefaultCellStyle.ForeColor = Color.DeepSkyBlue;
            tablaOrden.Columns["cantidad"].DefaultCellStyle.SelectionForeColor = Color.DeepSkyBlue;
            tablaOrden.Columns["cantidad"].DefaultCellStyle.SelectionBackColor = Color.White;
            tablaOrden.Columns["cantidad"].DefaultCellStyle.Font = new Font("Nirmala UI", 14, FontStyle.Bold);

            tablaOrden.Columns["nombre"].DefaultCellStyle.SelectionForeColor = Color.Black;
            tablaOrden.Columns["nombre"].DefaultCellStyle.SelectionBackColor = Color.White;

            tablaOrden.Columns["kg"].DefaultCellStyle.SelectionForeColor = Color.Black;
            tablaOrden.Columns["kg"].DefaultCellStyle.SelectionBackColor = Color.White;

            tablaOrden.Columns["color"].DefaultCellStyle.SelectionForeColor = Color.Black;
            tablaOrden.Columns["color"].DefaultCellStyle.SelectionBackColor = Color.White;

            tablaOrden.Columns["precioUnitario"].DefaultCellStyle.ForeColor = Color.ForestGreen;
            tablaOrden.Columns["precioUnitario"].DefaultCellStyle.SelectionForeColor = Color.ForestGreen;
            tablaOrden.Columns["precioUnitario"].DefaultCellStyle.SelectionBackColor = Color.White;
            tablaOrden.Columns["precioUnitario"].DefaultCellStyle.Font = new Font("Nirmala UI", 14, FontStyle.Bold);

            tablaOrden.Columns["precioTotal"].DefaultCellStyle.ForeColor = Color.ForestGreen;
            tablaOrden.Columns["precioTotal"].DefaultCellStyle.SelectionForeColor = Color.ForestGreen;
            tablaOrden.Columns["precioTotal"].DefaultCellStyle.SelectionBackColor = Color.White;
            tablaOrden.Columns["precioTotal"].DefaultCellStyle.Font = new Font("Nirmala UI", 14, FontStyle.Bold);
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }

    public class ControlStatus
    {
        public ControlStatus()
        {
            blockProducts = true;
        }
        public int idProduct { get; set; }
        public int idProductRegalia { get; set; }
        public bool blockProducts { get; set; }

        public void ChangeToSelected(Button btn)
        {
            ControlGenerator.SetButtonColorHighLight(btn);
        }
        public void ChangeToNormal(Button btn)
        {
            ControlGenerator.SetButtonColor(btn);
        }
    }
}