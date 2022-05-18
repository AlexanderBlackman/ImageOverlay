using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using System.Runtime.InteropServices;
using WinRT;
using WinUIEx;
using Windows.Storage;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageOverlay
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 


    public sealed partial class MainWindow : WindowEx
    {
        double ratio;
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }

        //WindowEx rootWindow = (WindowEx)Window.Current;
        public MainWindow()
        {
            this.InitializeComponent();
            // this.SizeChanged += MainWindow_SizeChanged; 
            string[] arguments = Environment.GetCommandLineArgs();
            this.IsTitleBarVisible = false;
            this.ExtendsContentIntoTitleBar = true;
            SetTitleBar(toolbarGrid);
            this.Title = "ImageOverlay";

            LoadImageOnLaunch();



            // OpenFileWindow();
        }





        private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            //selectedImage.Width = args.Size.Width;
            //selectedImage.Height = args.Size.Width * ratio;
        }

        private void LoadImageOnLaunch()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length > 1)
            {
                selectedImage.Source = new BitmapImage(new Uri(arguments[1]));
            }
            else
                OpenFileWindow();
        }


        public void SetImageSource()
        {
            //  
            ////selectedImage.Source = new BitmapImage(new Uri("b://847.jpg"));

            //  if (arguments[1] != null)
            //      selectedImage.Source = new BitmapImage(new Uri(arguments[1]));

        }

        private async void OpenFileWindow_Click(object sender, RoutedEventArgs e)
        {
            await OpenFileWindow();
        }

        private async Task OpenFileWindow()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SetOwnerWindow(this);
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();

            //var source = FileToSBS(file);
            if (file == null)
                return;
            SoftwareBitmap softwareBitmap;
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                //create decoder
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                //Get softwarebitmap from file.
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            }

            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
                 softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
            {
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }
            var source = new SoftwareBitmapSource();
            await source.SetBitmapAsync(softwareBitmap);



            setWindowToBitmapRatioSize(softwareBitmap);
            selectedImage.Source = source;
        }


        private double setWindowToBitmapRatioSize(SoftwareBitmap image)
        {

            ratio = image.PixelHeight / image.PixelWidth;

            //TODO get monitor size, and have the images resized if too large

            this.Height = image.PixelHeight;
            this.Width = image.PixelWidth;


            return ratio;
        }




        private void pinToTopToggle(object sender, RoutedEventArgs e)
        {
            this.IsAlwaysOnTop = (this.IsAlwaysOnTop) ? false : true;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var flyout = FlyoutBase.GetAttachedFlyout((FrameworkElement)sender);
            var options = new FlyoutShowOptions()
            {
                //position shows the flyout next to the pointer.
                //"Transient" ShowMode makes the flyout open in its collapsed state
                Position = e.GetPosition((FrameworkElement)sender),
                ShowMode = FlyoutShowMode.Transient
            };
            flyout?.ShowAt((FrameworkElement)sender, options);
        }

        private void closeApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }









        //IDEA  See what happens when you set the Drag event 














        //[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
        //public static extern IntPtr GetActiveWindow();

        //public MainWindow()
        //{
        //    this.InitializeComponent();

        //   Image image = new Image() { Source = new BitmapImage(new Uri("b://motivate3.jpg")) };
        //   // TapableImage image = new TapableImage();


        //    //TitleBar = image;

        //    this.ExtendsContentIntoTitleBar = true;
        //    this.IsAlwaysOnTop = true;
        //    SetTitleBar(imageGrid);
        // this.SizeChanged += MainWindow_SizeChanged; 
        //    string[] arguments = Environment.GetCommandLineArgs();
        //  LoadImageOnLaunch();



        // OpenFileWindow();



    }
}
