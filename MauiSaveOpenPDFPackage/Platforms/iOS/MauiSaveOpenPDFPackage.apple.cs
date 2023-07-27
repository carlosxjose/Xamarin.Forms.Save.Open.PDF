﻿using UIKit;
using QuickLook;
using Foundation;

namespace Plugin.MauiSaveOpenPDFPackage
{
    /// <summary>
    /// Interface for MauiSaveOpenPDFPackage
    /// </summary>
    public class MauiSaveOpenPDFPackageImplementation : IMauiSaveOpenPDFPackage
    {
        public async Task SaveAndView(string filename, string contentType, MemoryStream stream, PDFOpenContext context)
        {
            //Get the root path in iOS device.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, filename);

            //Create a file and write the stream into it.
            FileStream fileStream = File.Open(filePath, FileMode.Create);
            stream.Position = 0;
            await stream.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            fileStream.Close();

            //Invoke the saved document for viewing
            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (currentController.PresentedViewController != null)
                currentController = currentController.PresentedViewController;
            UIView currentView = currentController.View;

            QLPreviewController qlPreview = new QLPreviewController();
            QLPreviewItem item = new QLPreviewItemBundle(filename, filePath);
            qlPreview.DataSource = new PreviewControllerDS(item);

            currentController.PresentViewController(qlPreview, true, null);
        }
    }

    public class PreviewControllerDS : QLPreviewControllerDataSource
    {
        //Document cache
        private QLPreviewItem _item;

        //Setting the document
        public PreviewControllerDS(QLPreviewItem item)
        {
            _item = item;
        }

        //Setting document count to 1
        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return 1;
        }

        //Return the document
        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return _item;
        }
    }

    public class QLPreviewItemFileSystem : QLPreviewItem
    {

        string _fileName, _filePath;

        //Setting file name and path
        public QLPreviewItemFileSystem(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        //Return file name
        public override string PreviewItemTitle
        {
            get
            {
                return _fileName;
            }
        }

        //Retun file path as NSUrl
        public override NSUrl PreviewItemUrl
        {
            get
            {
                return NSUrl.FromFilename(_filePath);
            }
        }
    }

    public class QLPreviewItemBundle : QLPreviewItem
    {
        string _fileName, _filePath;

        //Setting file name and path
        public QLPreviewItemBundle(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        //Return file name
        public override string PreviewItemTitle
        {
            get
            {
                return _fileName;
            }
        }

        //Retun file path as NSUrl
        public override NSUrl PreviewItemUrl
        {
            get
            {
                var documents = NSBundle.MainBundle.BundlePath;
                var lib = Path.Combine(documents, _filePath);
                var url = NSUrl.FromFilename(lib);
                return url;
            }
        }
    }
}
