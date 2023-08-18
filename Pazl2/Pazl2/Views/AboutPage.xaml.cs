using SkiaSharp;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
 

namespace Pazl2.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }
        int _holder_IMG_Sousce;
        bool Win = false;
        bool stop = false;
        Razrez r = new Razrez();
        ImageSource[] im = null;
        int _gridRow = 0;
        int _gridColum = 0;
        bool StateClick = false;
        object _holder = null;
        int xy;
        Grid Grids = new Grid();
        int[] mass = null;
        Random random = new Random();
        void spawn()
        {
            Content = null;
            Grids = new Grid();
            xy = Convert.ToInt32(Holder.XY.Substring(0, Holder.XY.IndexOf('*')));
            Grids.ColumnSpacing = 2;
            Grids.RowSpacing = 2;
            for (int i = 0; i < xy; i++)
            {
                Grids.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                Grids.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < xy; i++)
            {
                for (int x = 0; x < xy; x++)
                {
                    ImageButton imageButton = new ImageButton() { BackgroundColor = Color.Aqua };
                    imageButton.Clicked += ImageButton_Clicked1;
                    imageButton.CornerRadius = 5;
                    imageButton.Aspect = Aspect.Fill;
                    Grids.Children.Add(imageButton,x,i);
                }
            }
            Content = Grids;
        }
        private   void ImageButton_Clicked1(object sender, EventArgs e)
        {
            if (stop == true)
                return;
            if (sender == null)
                return;
            if (im == null)
                return;
            if (StateClick == false)
            {
                int p = 0;
                foreach (ImageSource imageButton in im)
                {
                    if (imageButton == (sender as ImageButton).Source)
                    {
                        _holder_IMG_Sousce = p;
                    }
                    p++;
                }
                _gridRow = Grid.GetRow((sender as ImageButton));
                _gridColum = Grid.GetColumn((sender as ImageButton));
                _holder = (sender as ImageButton);
                (sender as ImageButton).Margin = new Thickness(10, 10, 10, 10);
                StateClick = true;
            }
            else
            {
                int localIMG = 0;
                int p = 0;
                foreach (ImageSource imageButton in im)
                {
                    if (imageButton == (sender as ImageButton).Source)
                    {
                        localIMG = p;
                    }
                    p++;
                }
                int zamen = mass[_holder_IMG_Sousce];
                int zamen2 = mass[localIMG];
                mass[_holder_IMG_Sousce] = zamen2;
                mass[localIMG] = zamen;
                int positRow = Grid.GetRow((sender as ImageButton));
                int positColu = Grid.GetColumn((sender as ImageButton));
                (_holder as ImageButton).Margin = new Thickness(0, 0, 0, 0);
                Grids.Children.Add((ImageButton)_holder, positColu, positRow);
                Grids.Children.Add((sender as ImageButton), _gridColum, _gridRow);
                StateClick = false;
                for (int i = 0; i < im.Length; i++)
                {
                    if (i != mass[i])
                    {
                        return;
                    }
                }
                if (Win == false)
                {
                    Holder.Peremesh = false;
                    foreach (ImageButton imageButtd in Grids.Children)
                    {
                        imageButtd.CornerRadius = 0;
                        Grids.ColumnSpacing = 0;
                        Grids.RowSpacing = 0;
                    }
                    Win = true;

                    ImageButton_Clicked1(sender, e);
                    ImageButton_Clicked1(sender, e);
                    Holder.Create = false;
                    DisplayAlert("Пазл собран!", "Спасибо за игру!", "ОK");
                    stop = true;

                }
            }
        }
        private void Clearss()
        {
            _gridRow = 0;
            _gridColum = 0;
            StateClick = false;
            _holder = null;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            stop = false ;
            Win = false ;
            if (Holder.Create == true)
            {
                bool result = await DisplayAlert("Подтвердить действие", "Вы хотите пересоздать пазл?", "Да", "Нет");
                if (!result)
                    return;
            }
            Holder.Create = true;
            Holder.Peremesh = false;
            if (Holder.img == "" && Holder.imageSource3 == null)
            {
                await DisplayAlert("Внимание!", "Выберите картинку в настройках.", "ОK");
                return;
            }
            _holder = null;
            StateClick = false;
            Grids.IsVisible = true;
            spawn();
            Stream f = null;
            if (Holder.imageSource3 != null)
                f = new MemoryStream(Holder.imageSource3);
            else
            {
                string someUrl = Holder.img;
                using (var webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(someUrl);
                    var stre = new MemoryStream(imageBytes);
                    f = stre;
                }
            }
            SKBitmap sKBitmap = SKBitmap.Decode(f);
            im = r.Imag(sKBitmap, xy);
            mass = new int[im.Length];
            for (int i = 0; i < im.Length; i++)
            {
                mass[i] = i;
            }
            for (int i = 0; i < xy * xy; i++)
            {
                (Grids.Children[i] as ImageButton).Source = im[i];
            }
            f.Dispose();
            Clearss();
        }
        private   void Refresh()
        {
            if (im == null)
                return;
            Grids.ColumnSpacing = 2;
            Grids.RowSpacing = 2;
            foreach (ImageButton imageButtd in Grids.Children)
            {
                imageButtd.CornerRadius = 5;
            }
            _holder = null;
            StateClick = false;      
            Holder.Peremesh = true;
            for (int i = 0; i < Grids.Children.Count; i++)
            {
                int otkuda = random.Next(0, Grids.Children.Count);
                int kuda = random.Next(0, Grids.Children.Count);
                if (StateClick == false)
                {
                    int p = 0;
                    foreach (ImageSource imageButton in im)
                    {
                        if (imageButton == (Grids.Children[otkuda] as ImageButton).Source)
                        {
                            _holder_IMG_Sousce = p;
                        }
                        p++;
                    }
                    _gridRow = Grid.GetRow((Grids.Children[otkuda] as ImageButton));
                    _gridColum = Grid.GetColumn((Grids.Children[otkuda] as ImageButton));
                    _holder = (Grids.Children[otkuda] as ImageButton);
                    StateClick = true;
                }
                else
                {
                    int localIMG = 0;
                    int p = 0;
                    foreach (ImageSource imageButton in im)
                    {
                        if (imageButton == (Grids.Children[kuda] as ImageButton).Source)
                        {
                            localIMG = p;
                        }
                        p++;
                    }
                    int zamen = mass[_holder_IMG_Sousce];
                    int zamen2 = mass[localIMG];
                    mass[_holder_IMG_Sousce] = zamen2;
                    mass[localIMG] = zamen;
                    int positRow = Grid.GetRow((Grids.Children[kuda] as ImageButton));
                    int positColu = Grid.GetColumn((Grids.Children[kuda] as ImageButton));
                    Grids.Children.Add((ImageButton)_holder, positColu, positRow);
                    Grids.Children.Add((Grids.Children[kuda] as ImageButton), _gridColum, _gridRow);
                    StateClick = false;
                }
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (Win == true)
                return;
            if (Holder.Peremesh == true)
            {
                bool result = await DisplayAlert("Подтвердить действие", "Вы хотите перемешать пазл?", "Да", "Нет");
                if (!result)
                    return;
            }
            if (Grids.Children.Count == 0)
            {
               await DisplayAlert("Внимание!", "Сначала создайте пазл.", "ОK");
                return;
            }
            foreach (ImageButton ims in Grids.Children)
            {
                ims.Margin = new Thickness(0, 0, 0, 0);
            }
            Refresh();
            Refresh();
            Refresh();
            Clearss();
        }
    }
}