using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Globalization;

namespace Dometrain.EFCore.API.Data.ValueConverter
{
    public class DateTimeToChar8Converter : ValueConverter<DateTime, string> //model/database oldali adattagok
    {
        public DateTimeToChar8Converter() : base(
            dateTime => dateTime.ToString("yyyyMdd", CultureInfo.InvariantCulture), 
            StringValueGenerator => DateTime.ParseExact(StringValueGenerator, "yyyyMMdd", CultureInfo.InvariantCulture))
        { }



    }
}
