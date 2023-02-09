namespace Library.Entidades
{
    public class Producto
    {
        public long id { get; }
        public string nombre { get; }
        public string color { get; }
        public string categoria { get; }

        public int x20 { get; set; }
        public int x10 { get; set; }
        public int x4 { get; set; }
        public int x1 { get; set; }

        public decimal preciox20 { get; set; }
        public decimal preciox10 { get; set; }
        public decimal preciox4 { get; set; }
        public decimal preciox1 { get; set; }

        public Producto(long id, string nombre, string color, string categoria)
        {
            this.id = id;
            this.nombre = nombre;
            this.color = color;
            this.categoria = categoria;

            Inicializar();
        }

        public Producto(Producto p)
        {
            this.id = p.id;
            this.nombre = p.nombre;
            this.color = p.color;
            this.categoria = p.categoria;

            this.x20 = p.x20;
            this.x10 = p.x10;
            this.x4 = p.x4;
            this.x1 = p.x1;

            this.preciox20 = p.preciox20;
            this.preciox10 = p.preciox10;
            this.preciox4 = p.preciox4;
            this.preciox1 = p.preciox1;
        }

        

        public string nombreCompleto()
        {
            return nombre + color;
        }
        private void Inicializar()
        {
            preciox20 = 0;
            preciox10 = 0;
            preciox4 = 0;
            preciox1 = 0;

            x20 = 0;
            x10 = 0;
            x4 = 0;
            x1 = 0;
        }

        public override string ToString()
        {
            return $"{this.nombre} {this.color}";
        }

        public int CantidadTotal()
        {
            return x20 + x10 + x4 + x1;
        }
    }
}