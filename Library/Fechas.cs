using System;


namespace Library
{
    public static class Fechas
    {
        public static string Formato_Dia_Mes_Anio_Numeros(DateTime Fecha , bool yHora = false)
        {
            string res;
            int dia = Fecha.Day;
            int mes = Fecha.Month;
            int ano = Fecha.Year % 100;


            res =  dia > 9 ? dia.ToString() : "0" + dia.ToString();
            res += "-";
            res += mes > 9 ? mes.ToString() : "0" + mes.ToString();
            res += "-" + ano.ToString();


            if ( yHora )
            {
                res += Fecha.TimeOfDay;
            }

            return res;

        }

        public static string Formato_Dia_Mes(DateTime Fecha)
        {
            string res;
            int dia = Fecha.Day;
            int mes = Fecha.Month;


            
            
            res  = dia > 9 ? dia.ToString() : "0" + dia.ToString();
            res += "/";
            res += mes > 9 ? mes.ToString() : "0" + mes.ToString();
            return res;
        }

        public static string Formato_Dia_Mes_Anio_Letras(DateTime Fecha)
        {
            string res;

            res = $"   {DiadeLaSemanaPalabra(Fecha.DayOfWeek)} " + Fecha.Day.ToString() +
                  $" de {MesPalabra(Fecha.Month)} de {Fecha.Year}";

            return res;
        }

        public static string Formato_Mes_Dia(DateTime Fecha)
        {
            string res;
            int dia = Fecha.Day;
            int mes = Fecha.Month;


            res = mes > 9 ? mes.ToString() : "0" + mes.ToString();
            res += "/";
            res += dia > 9 ? dia.ToString() : "0" + dia.ToString();

            return res;
        }

        public static string Formato_Mes_Anio(DateTime Fecha)
        {
            string res;
            int anio = Fecha.Year;
            int mes = Fecha.Month;

            res = mes > 9 ? mes.ToString() : "0" + mes.ToString();
            res += "-";
            res += anio.ToString(); 

            return res;
        }

        public static string DiadeLaSemanaPalabra (DayOfWeek dia)
        {
            switch (dia){

                case DayOfWeek.Monday:       return "Lunes";
                case DayOfWeek.Tuesday:      return "Martes";
                case DayOfWeek.Wednesday:    return "Miércoles";
                case DayOfWeek.Thursday:     return "Jueves";
                case DayOfWeek.Friday:       return "Viernes";
                case DayOfWeek.Saturday:     return "Sabado";
                case DayOfWeek.Sunday:       return "Domingo";

                default: return dia.ToString();
            }     
        }

        public static string MesPalabra (int mes)
        {
            switch (mes)
            {
                case 1:  return  "Enero";
                case 2:  return  "Febrero";
                case 3:  return  "Marzo";
                case 4:  return  "Abril";
                case 5:  return  "Mayo";
                case 6:  return  "Junio";
                case 7:  return  "Julio";
                case 8:  return  "Agosto";
                case 9:  return  "Septiembre";
                case 10: return  "Octubre";
                case 11: return  "Noviembre";
                case 12: return  "Diciembre";

                default: return mes.ToString();
            }

        }
    }
}
