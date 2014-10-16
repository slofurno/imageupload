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
using Amazon.S3;
using Amazon.S3.Model;


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
        public string UploadImage(byte[] data, string title)
        {

           
            var bitsperpixel = 4;

            int oldwidth = 10;
            int oldheight = 10;

            int heightmulti = 100;
            int widthmulti = 100;

            var oldstride = oldwidth * bitsperpixel;
            var newstride = oldstride * widthmulti;

            var newheight = oldheight * heightmulti;

            
       


           

            byte[] newdata = scalePixels(data, oldwidth, oldheight, widthmulti, heightmulti);






            string fulltitle = title + ".gif";


            PixelFormat pf = PixelFormats.Bgra32;

            int rawStride = (oldwidth*widthmulti * pf.BitsPerPixel + 7) / 8;

            BitmapPalette myPalette = BitmapPalettes.Halftone256;

            BitmapSource bitmap = BitmapSource.Create(oldwidth*widthmulti, oldheight*heightmulti,
                96, 96, pf, myPalette,
                newdata, rawStride);





            GifBitmapEncoder encoder = new GifBitmapEncoder();

           

            //encoder.Frames.Add(BitmapFrame.Create(bitmap));

            encoder.Frames.Add(BitmapFrame.Create(bitmap));





            var s3client = new Amazon.S3.AmazonS3Client("AKIAJA3PK2CYTZEC5E6A", "vJIJRmV+kWU4J+Ex3veaFaohK7UU4aaYOy8ggEe9", Amazon.RegionEndpoint.USWest2);




            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = "slofurnotest",
                Key = fulltitle,
            };


            using (MemoryStream ms = new MemoryStream())
            {
                 encoder.Save(ms);

                





                

                 request.InputStream = ms;

                 // Put object
                 PutObjectResponse response = s3client.PutObject(request);
                



            }



            /*
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);

                request.InputStream = ms;
               



            }
 

            */


            //stream.Flush();

            //stream.Close();



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


        static byte[] scalePixelsOld(byte[] data)
        {

            var bytesperpixel = 4;

            int oldwidth = 10;
            int oldheight = 10;

            int heightmulti = 100;
            int widthmulti = 100;

            var oldstride = oldwidth * bytesperpixel;
            var newstride = oldstride * widthmulti;

            var newheight = oldheight * heightmulti;

            byte[] newdata = new byte[newstride * oldheight * heightmulti];


            for (int i = 0; i < (oldheight); i++)
            {
                for (int j = 0; j < (oldwidth); j++)
                {


                    for (int x = i * heightmulti; x < (i * heightmulti + heightmulti); x++)
                    {

                        for (int y = j * widthmulti; y < (j * widthmulti + widthmulti); y++)
                        {

                            var ok = x * oldwidth * widthmulti * 4 + y;
                            var sure = i * oldwidth * 4 + j;

                            newdata[x * newstride + y * bytesperpixel] = data[i * oldstride + j * bytesperpixel];
                            newdata[x * newstride + y * bytesperpixel + 1] = data[i * oldstride + j * bytesperpixel + 1];
                            newdata[x * newstride + y * bytesperpixel + 2] = data[i * oldstride + j * bytesperpixel + 2];
                            newdata[x * newstride + y * bytesperpixel + 3] = data[i * oldstride + j * bytesperpixel + 3];


                        }


                    }



                }



            }

            return newdata;

        }


        static unsafe byte[] scalePixels(byte[] data, int oldwidth, int oldheight, int widthmulti, int heightmulti)
        {



            var bytesperpixel = 4;

 

            var oldstride = oldwidth * bytesperpixel;
            var newstride = oldstride * widthmulti;

            var newheight = oldheight * heightmulti;




            byte[] newdata = new byte[newstride * oldheight * heightmulti];


            bytesperpixel = 1;
            oldstride = oldwidth * bytesperpixel;
            newstride = oldstride * widthmulti;


            fixed (byte* sourcepointer = data, targetpointer = newdata)
            {

                UInt32* source = (UInt32*)sourcepointer;
                UInt32* target = (UInt32*)targetpointer;


                for (int i = 0; i < (oldheight); i++)
                {
                    for (int j = 0; j < (oldwidth); j++)
                    {


                        for (int x = i * heightmulti; x < (i * heightmulti + heightmulti); x++)
                        {

                            for (int y = j * widthmulti; y < (j * widthmulti + widthmulti); y++)
                            {

                                var ok = x * oldwidth * widthmulti * bytesperpixel + y;
                                var sure = i * oldwidth * bytesperpixel + j;

                                target[x * newstride + y * bytesperpixel] = source[i * oldstride + j * bytesperpixel];
                                //target[x * newstride + y * bytesperpixel + 1] = source[i * oldstride + j * bytesperpixel + 1];
                                //target[x * newstride + y * bytesperpixel + 2] = source[i * oldstride + j * bytesperpixel + 2];
                                //target[x * newstride + y * bytesperpixel + 3] = source[i * oldstride + j * bytesperpixel + 3];


                            }


                        }



                    }



                }


            }
            return newdata;





        }



    }
}