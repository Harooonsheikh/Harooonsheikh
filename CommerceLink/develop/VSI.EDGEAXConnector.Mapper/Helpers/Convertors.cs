using System;
using AutoMapper;

namespace VSI.EDGEAXConnector.Mapper.Helpers
{

    #region AutoMapTypeConverters
    // Automap type converter definitions for 
    // int, int?, decimal, decimal?, bool, bool?, Int64, Int64?, DateTime
    // Mapper string to int?
    public class NullIntTypeConverter : TypeConverter<string, int?>
    {
        protected override int? ConvertCore(string source)
        {
            if (source == null)
                return null;
            else
            {
                int result;
                return Int32.TryParse(source, out result) ? (int?)result : null;
            }
        }
    }
    // Mapper string to int
    public class IntTypeConverter : TypeConverter<string, int>
    {
        protected override int ConvertCore(string source)
        {
            if (String.IsNullOrEmpty(source))
                return 0;
            else
                return Int32.Parse(source);
        }
    }
  

     
    // Mapper string to long
    public class LongTypeConverter : TypeConverter<string, long>
    {
        protected override long ConvertCore(string source)
        {
            if (String.IsNullOrEmpty(source))
                return 0;
            else
                return long.Parse(source);
        }
    }
    // Mapper long to string
    public class StringTypeConverter : TypeConverter<long, string>
    {
        protected override string ConvertCore(long source)
        {
            return source.ToString();
        }
    }


    // Mapper string to decimal?
    public class NullDecimalTypeConverter : TypeConverter<string, decimal?>
    {
        protected override decimal? ConvertCore(string source)
        {
            if (String.IsNullOrEmpty(source))
                return 0;
            else
            {
                decimal result;
                return Decimal.TryParse(source, out result) ? (decimal?)result : null;
            }
        }
    }
    // Mapper string to decimal
    public class DecimalTypeConverter : TypeConverter<string, decimal>
    {
        protected override decimal ConvertCore(string source)
        {
            if (String.IsNullOrEmpty(source))
                return 0;
            else
                return Decimal.Parse(source);
        }
    }
    // Mapper string to bool?
    public class NullBooleanTypeConverter : TypeConverter<string, bool?>
    {
        protected override bool? ConvertCore(string source)
        {
            if (source == null)
                return null;
            else
            {
                bool result;
                return Boolean.TryParse(source, out result) ? (bool?)result : null;
            }
        }
    }
    // Mapper string to bool
    public class BooleanTypeConverter : TypeConverter<string, bool>
    {
        protected override bool ConvertCore(string source)
        {
            if (source == null)
                throw new MappingException("null string value cannot convert to non-nullable return type.");
            else
                return Boolean.Parse(source);
        }
    }
    // Mapper string to Int64?
    public class NullInt64TypeConverter : TypeConverter<string, Int64?>
    {
        protected override Int64? ConvertCore(string source)
        {
            if (String.IsNullOrEmpty(source))
                return 0;
            else
            {
                Int64 result;
                return Int64.TryParse(source, out result) ? (Int64?)result : null;
            }
        }
    }
    // Mapper string to Int64
    public class Int64TypeConverter : TypeConverter<string, Int64>
    {
        protected override Int64 ConvertCore(string source)
        {
            if (String.IsNullOrEmpty(source))
                return 0;
            else
                return Int64.Parse(source);
        }
    }
    // Mapper string to DateTime?
    public class NullDateTimeTypeConverter : TypeConverter<string, DateTime?>
    {
        protected override DateTime? ConvertCore(string source)
        {
            if (source == null)
                return null;
            else
            {
                DateTime result;
                return DateTime.TryParse(source, out result) ? (DateTime?)result : null;
            }
        }
    }
    // Mapper string to DateTime
    public class DateTimeTypeConverter : TypeConverter<string, DateTime>
    {
        protected override DateTime ConvertCore(string source)
        {
            if (source == null)
                throw new MappingException("null string value cannot convert to non-nullable return type.");
            else
                return DateTime.Parse(source);
        }
    }
    #endregion
}
