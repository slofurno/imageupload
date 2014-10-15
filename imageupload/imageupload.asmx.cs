using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
//using System.Drawing;
//using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Media;



namespace imageupload
{
    /// <summary>
    /// Summary description for fileupload
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class imageupload : System.Web.Services.WebService
    {

        [WebMethod]
        public string UploadImage(byte[] data)
        {



            PixelFormat pf = PixelFormats.Bgra32;

            int rawStride = (10 * pf.BitsPerPixel + 7) / 8;

            BitmapPalette myPalette = BitmapPalettes.Halftone256;

            BitmapSource bitmap = BitmapSource.Create(10, 10,
                96, 96, pf, myPalette,
                data, rawStride);




            FileStream stream = new FileStream(@"D:\test.gif", FileMode.Create);
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            


            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);

            stream.Flush();

            stream.Close();



            /*

            Bitmap bmp = new Bitmap(10, 10, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            bmp.Save(@"TestImage.gif", ImageFormat.Gif);

            */
            //temp.Save(@"test.bmp");


            /*

            Bitmap bmp = new Bitmap(10, 10, PixelFormat.Format32bppArgb);

           
           

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;

            IntPtr ptr = bmpData.Scan0;


            System.Runtime.InteropServices.Marshal.Copy(data, 0, ptr, bytes);

            bmp.UnlockBits(bmpData);
            */



            return "Hello World";
        }


    }
}