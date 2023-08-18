using Pazl2.Models;
using Pazl2.ViewModels;
using Pazl2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Threading;
using Xamarin.Essentials;
using SkiaSharp;
using System.Net;
using System.Reflection;
using Android.Graphics;
using Android.Provider;

namespace Pazl2.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            blockss = true;
            if (Device.Android == Device.RuntimePlatform)
            {
                HolderImage.IsVisible = false;
            }


        }

        

       
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        byte[] mass;

        async void Button_Clicked(object sender, EventArgs e)
        {
            
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
            });
            if (result == null)
            {
                return;

            }

            var input = await result.OpenReadAsync();


            using (BinaryReader br = new BinaryReader(input))
            {
                mass = br.ReadBytes((int)input.Length);
            }
            input.Dispose();
            var ds = new MemoryStream(mass);
            ds = new MemoryStream(mass);
            SKBitmap sK = null;
            if (mass != null)
                sK = SKBitmap.Decode(mass);
           
            if (Device.UWP == Device.RuntimePlatform)
            {
                if (sK.Width < sK.Height)
                {
                    await DisplayAlert("Настройки", $"Фото не подходит для UPW проекта", "ОK");
                    mass = null;
                IMG.Source = null;
                    return;
                }
                

            }
            else
            {
                if (sK.Width > sK.Height)
                {
                    await DisplayAlert("Настройки", $"Фото не подходит для Android проекта", "ОK");
                    mass = null;
                IMG.Source = null;
                    return;
                }
                

            }
            IMG.Source = ImageSource.FromStream(() => ds);
            blockss = false;



        }

        private void Galer_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            BTN_IMG_Gel.IsVisible = true;
            Url.IsVisible = false;
            IMG.Source = null;
            Holder.img = "";
            blockss = true;
            IMG.Source = null;

        }

        private void Intern_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            BTN_IMG_Gel.IsVisible = false;
            Url.IsVisible = true;
            Holder.imageSource3 = null;
            IMG.Source = null;
            mass = null;
            blockss = true;
            IMG.Source = null;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {



        }
        bool blockss = false;
        private void TextUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(TextUrl.Text);
                SKBitmap sK = null;
                
                    
                       
                        var stre = new MemoryStream(imageBytes);
                        sK = SKBitmap.Decode(stre);
                  
                 
                if (Device.UWP == Device.RuntimePlatform)
                {
                    if (sK.Width < sK.Height)
                    {
                        DisplayAlert("Настройки", $"Фото не подходит для UPW проекта", "ОK");
                        TextUrl.Text = null;
                IMG.Source = null;
                        return;
                    }


                }
                else
                {
                    if (sK.Width > sK.Height)
                    {
                        DisplayAlert("Настройки", $"Фото не подходит для Android проекта", "ОK");
                        TextUrl.Text = null;
                IMG.Source = null;
                        return;
                    }


                }
                blockss = false;
                IMG.Source = TextUrl.Text;

            }
            catch
            {
                blockss = true;
            }


        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (blockss == true)
            {
                DisplayAlert("Ошибка!!", $"Не применены! Выберите изображение!", "ОK");
                return;
            }

            if (BTN_IMG_Gel.IsVisible == true && mass != null)
                Holder.imageSource3 = mass;
            else if (mass == null && TextUrl.Text != "") 
            {
                try
                {
                    WebClient wc = new WebClient();
                    string HTMLSource = wc.DownloadString(TextUrl.Text);
                    IMG.Source = TextUrl.Text;
                    Holder.img = TextUrl.Text;

                }
                catch
                {

                }

            }

            if (Pickers.SelectedItem != null)
                Holder.XY = Pickers.SelectedItem.ToString();
             
            string soobh = "";
            if (Pickers.SelectedItem != null)
            {
                soobh = $", выбран размер {Pickers.SelectedItem.ToString()}";
            }
            if (Holder.img != "" || Holder.imageSource3 != null)
            {
                DisplayAlert("Настройки", $"Настройки применены! Изображение выбрано{soobh}.", "ОK");
                Holder.Create = false;
                Holder.Peremesh = false;

            }

        }

        private void HolderImage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(Device.Android == Device.RuntimePlatform)
            {
                byte[] buffer;
                var assembly = this.GetType().GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("Zveri.jpg") )
                {
                    long length = stream.Length;
                    buffer = new byte[length];
                    stream.Read(buffer, 0, (int)length);
                }
            }
            else
            {
                var bytr = File.ReadAllBytes("Zveri.jpg");
                Holder.imageSource3 = bytr;
            }
                
                IMG.Source = "Zveri.jpg";
            blockss = false;
            Holder.img = "";
            BTN_IMG_Gel.IsVisible = false;
            Url.IsVisible = false;


        }
    }
}