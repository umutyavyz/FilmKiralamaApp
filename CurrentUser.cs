using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmKiralama
{
    public static class CurrentUser
    {
        public static int UserId { get; set; }

        private static string _userName;
        public static string UserName
        {
            get => _userName;
            set
            {  // İlk harfi büyük yazdırmak için kod.
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _userName = char.ToUpper(value[0]) + value.Substring(1).ToLower();
                }
                else
                {
                    _userName = value;
                }
            }
        }
    }

}

