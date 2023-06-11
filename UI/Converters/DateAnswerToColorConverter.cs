using System.Globalization;

namespace UI.Converters
{
    public class DateAnswerToColorConverter : IMultiValueConverter
    {


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? dateTime = null;
            string numberOut = null;
            bool isMailDone = false;

            if (values[0] != null && values[0] is DateTime time)
               dateTime = time;
            if (values[1] != null && values[1] is string number)
                numberOut = number;
            if (values[2] != null && values[2] is bool b)
                isMailDone = b;

            //если письмо выполнено то срок не выводим
            if (isMailDone) return default;
          
            if (dateTime is null) return default;

            if(numberOut is not null)
            {
                return Colors.Green;
            }

            if (dateTime <= DateTime.Now )
            {
                return Colors.Red;
            }
            else if (dateTime > DateTime.Now && dateTime < DateTime.Now.AddDays(7) )
            {
                return Colors.Orange;
            }
            else if (dateTime > DateTime.Now.AddDays(7) && dateTime < DateTime.Now.AddDays(14))
            {
                return Colors.Yellow;
            }

            return Colors.White;
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return default;
        }
    }
}
