using System;

namespace Data.Util
{
    public static class DataConvert
    {
        public static T To<T>(this IConvertible obj)
        {
            Type t = typeof(T);
            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
            {
                if (obj == null)
                    return default(T);

                return (T)Convert.ChangeType(obj, u);
            }
            else
            {
                return (T)Convert.ChangeType(obj, t);
            }
        }

        public static string ToStringNull(this object obj)
        {
            try
            {
                if (!Convert.IsDBNull(obj))
                    return Convert.ToString(obj);
                else
                    return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Retira os caracteres acentuados, substituindo também % e ¨ por espaço
        /// e _ por -
        /// </summary>
        /// <param name="frase">Frase que contém os caracteres associados</param>
        /// <returns></returns>
        public static string ToNoAscent(this object obj)
        {
            if (Convert.IsDBNull(obj))
                return null;

            try
            {
                string frase = Convert.ToString(obj);

                string listaUpper = "ÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÔÖÚÙÛÜÇÑ%¨_";
                string listaLower = "áàãâäéèêëíìîïóòõôöúùûüçñ%¨_";
                string corretoUpper = "AAAAAEEEEIIIIOOOOOUUUUCN  -";
                string corretoLower = "aaaaaeeeeiiiiooooouuuucn  -";

                for (int i = 0; i < listaUpper.Length; i++)
                {
                    frase = frase.Replace(listaUpper.ToCharArray()[i], corretoUpper.ToCharArray()[i]);
                    frase = frase.Replace(listaLower.ToCharArray()[i], corretoLower.ToCharArray()[i]);
                }

                return frase.Replace("~", "").Replace("'", "").Replace("´", "").Replace("`", "");
            }
            catch { return null; }
        }

        /// <summary>
        /// Retira os caracteres acentuados, substituindo também % e ¨ por espaço
        /// e _ por -
        /// </summary>
        /// <param name="frase">Frase que contém os caracteres associados</param>
        /// <returns></returns>
        public static string ToUpperNoAscent(this object obj)
        {
            if (Convert.IsDBNull(obj))
                return null;

            try
            {
                string frase = Convert.ToString(obj);
                frase = frase.ToUpper();
                string listaUpper = "ÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÔÖÚÙÛÜÇÑ%¨_";
                //string listaLower = "áàãâäéèêëíìîïóòõôöúùûüçñ%¨_";
                string corretoUpper = "AAAAAEEEEIIIIOOOOOUUUUCN  -";
                //string corretoLower = "aaaaaeeeeiiiiooooouuuucn  -";

                for (int i = 0; i < listaUpper.Length; i++)
                {
                    frase = frase.Replace(listaUpper.ToCharArray()[i], corretoUpper.ToCharArray()[i]);
                    //frase = frase.Replace(listaLower.ToCharArray()[i], corretoLower.ToCharArray()[i]);
                }

                return frase.Replace("~", "").Replace("'", "").Replace("´", "").Replace("`", "");
            }
            catch { return null; }
        }

        public static int ToInt(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                    return Convert.ToInt32(obj);
                else
                    return 0;
            }
            catch { return 0; }
        }

        public static int? ToIntNull(this object obj)
        {
            try
            {
                if (!Convert.IsDBNull(obj))
                    return Convert.ToInt32(obj);
                else
                    return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converte para DateTime
        /// </summary>
        public static DateTime ToDateTime(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                    return Convert.ToDateTime(obj);
                else
                    return DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }

        public static DateTime? ToDateTimeNull(this object obj)
        {
            try
            {
                if (!Convert.IsDBNull(obj))
                    return Convert.ToDateTime(obj);
                else
                    return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converte para Double
        /// </summary>
        public static double ToDouble(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                    return Convert.ToDouble(obj);
                else
                    return 0;
            }
            catch { return 0; }
        }

        public static double? ToDoubleNull(this object obj)
        {
            try
            {
                if (!Convert.IsDBNull(obj))
                    return Convert.ToDouble(obj);
                else
                    return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converte para Decimal
        /// </summary>
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                    return Convert.ToDecimal(obj);
                else
                    return 0;
            }
            catch { return 0; }
        }

        public static decimal? ToDecimalNull(this object obj)
        {
            try
            {
                if (!Convert.IsDBNull(obj))
                    return Convert.ToDecimal(obj);
                else
                    return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converte para Long
        /// </summary>
        public static long ToLong(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                    return Convert.ToInt64(obj);
                else
                    return 0;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Converte para Long padrao Java
        /// </summary>
        public static long ToLongJava(this DateTime obj)
        {
            try
            {
                if (obj != null)
                {
                    long javaTimestamp = obj.Ticks - new DateTime(1969, 12, 31).Ticks;
                    javaTimestamp /= TimeSpan.TicksPerSecond;
                    return (javaTimestamp * 1000);
                }
                else
                    return 0;
            }
            catch { return 0; }
        }

        public static long? ToLongNull(this object obj)
        {
            try
            {
                if (!Convert.IsDBNull(obj))
                    return Convert.ToInt64(obj);
                else
                    return null;
            }
            catch { return null; }
        }

        //public static string BlobToString(this object obj)
        //{
        //    string rt = string.Empty;            //OracleLob o = obj;
        //                                         //if (!o.IsNull)
        //                                         //{
        //                                         //    byte[] b = new byte[o.Length];
        //                                         //    o.Read(b, 0, (int)o.Length);
        //                                         //    System.Text.ASCIIEncoding conversor = new System.Text.ASCIIEncoding();
        //                                         //    rt = conversor.GetString(b);
        //                                         //}

        //    return rt;
        //}

        /// <summary>
        /// Converte para Bool
        /// </summary>
        public static bool ToBool(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                {
                    if (obj is bool)
                        return (bool)obj;
                    else
                        return obj.ToString() == "1";
                }
                else
                    return false;
            }
            catch { return false; }
        }

        public static bool? ToBoolNull(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                    return obj.ToBool();
                else
                    return null;
            }
            catch { return null; }
        }

        public static string ToXmlString(this object obj)
        {
            try
            {
                if (obj != null && obj != System.DBNull.Value)
                {
                    System.Xml.Serialization.XmlSerializer _ser = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    System.IO.StringWriter _xml = new System.IO.StringWriter();
                    _ser.Serialize(_xml, obj);
                    return _xml.ToString();
                }
                else
                    return string.Empty;
            }
            catch { return string.Empty; }
        }
    }
}