using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSA_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void ApplyButton_Click(object sender, RoutedEventArgs re)
        {
            //MessageBox.Show(this.pBox.Text);
            String tempP = this.pBox.Text;
            //String tempP = "275604541";
            //String tempQ = "413158511";
            //543848111
            String tempQ = this.qBox.Text;
            BigInteger p = BigInteger.Parse(tempP);
            BigInteger q = BigInteger.Parse(tempQ);

            BigInteger n = p * q;
            BigInteger fi = (p - 1) * (q - 1);
            BigInteger e = ((p + 1) / 2) * q;
            BigInteger a = 0, ee, nn, d, b;

            do
            {
                e -= 2;
                ee = e;
                nn = fi;
                while (nn != 0)
                {
                    a = ee % nn;
                    ee = nn;
                    nn = a;
                }
            } while (ee > 1);

            publicKeyBox.Text = "Klucz publiczny: (e, n) = " + e + ", " + n;

            d = El_odw(e, fi);
            Console.WriteLine("e = " + e + " d=e^-1 = " + d + " e*d = " + irc(e, d, fi));
            privateKeyBox.Text = "Klucz prywatny: (d, n) = " + d + ", " + n;

            //Wiadomosc do zakodowania
            String str_a = this.codeBox.Text;
            a = BigInteger.Parse(str_a);
            //Wiadomosc zakodowana
            b = power(a, e, n);
            this.EnMessage.Text = "Wiadomosc zakodowana: " + b;
            a = power(b, d, n);
            this.deMessage.Text = "Wiadomosc odkodowana: " + a;

        }


        public static BigInteger El_odw(BigInteger aa, BigInteger n)
        {
            BigInteger a = aa, q, c, b = n;
            BigInteger xa = 1, xb = 0;

            do
            {    // printf("   a=%lld   b=%lld   q=%lld   xa=%lld  xb=%lld\n",a,b,a/b,xa,xb);
                q = a / b; c = a; a = b; b = c % b;
                //            c=xa;  xa=xb; xb=(c+(n-  (q*xb)%n )%n)%n;     } while (b);
                c = xa; xa = xb; xb = (c + (n - irc(q, xb, n)) % n) % n;
            } while (b != 0);
            //printf(">> a=%lld   b=%lld   q=%lld   xa=%lld  xb=%lld\n",a,b,q,xa,xb);
            return xa;
        }

        public static BigInteger irc(BigInteger a, BigInteger b, BigInteger n)
        {
            BigInteger il = 0;
            char r;

            while (b != 0)
            {
                if (b % 2 == 1)
                {
                    il = (il + a) % n;
                }
                a = (a * 2) % n; b = b / 2;   /* printf(" r=%d  pow=%lld  aa=%lld  e=%lld \n",r,pow,aa,e); */
            };

            return il;
        }

        public static BigInteger power(BigInteger aa, BigInteger e, BigInteger n)
        {
            BigInteger pow = 1;

            //while (e){ r=e%2;  if(r){pow=   (pow*aa)%n; };  aa=   (aa*aa)%n;    e=e/2;  /* printf(" r=%d  pow=%lld  aa=%lld  e=%lld \n",r,pow,aa,e); */ };
            while (e != 0)
            {
                BigInteger rr = e % 2;
                if (rr != 0)
                {
                    pow = irc(pow, aa, n);
                }
                aa = irc(aa, aa, n);
                e = e / 2;  /* printf(" r=%d  pow=%lld  aa=%lld  e=%lld \n",r,pow,aa,e); */
            };

            return pow;
        }

    }
}
