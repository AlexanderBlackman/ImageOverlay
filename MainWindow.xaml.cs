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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ImageOverlay
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    [ComImport, System.Runtime.InteropServices.Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithWindow
    {
        void Initialize([In] IntPtr hwnd);
    }

    public sealed partial class MainWindow : WindowEx
    {

        //[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
        //public static extern IntPtr GetActiveWindow();

        public MainWindow()
        {
            this.InitializeComponent();
            
            SetImageSource();
        }

        public void SetImageSource()
        {
          //  string[] arguments = Environment.GetCommandLineArgs();
          ////selectedImage.Source = new BitmapImage(new Uri("b://847.jpg"));

          //  if (arguments[1] != null)
          //      selectedImage.Source = new BitmapImage(new Uri(arguments[1]));

        }

        private async void OpenFileWindow(object sender, RoutedEventArgs e)
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
  




            selectedImage.Source = source;

        }

        //private async SoftwareBitmapSource FileToSBS(StorageFile file)
        //{

           
                
        //    SoftwareBitmap softwareBitmap;
        //    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
        //    {
        //        //create decoder
        //        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
        //        //Get softwarebitmap from file.
        //        softwareBitmap = await decoder.GetSoftwareBitmapAsync();

        //    }

        //    if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
        //         softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
        //    {
        //        softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        //    }
        //    var source = new SoftwareBitmapSource();
        //    await source.SetBitmapAsync(softwareBitmap);
        //    return source;
        //}




    }
}
