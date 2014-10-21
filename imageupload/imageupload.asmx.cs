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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

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
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string[] GetImages()
        {

            var s3client = new Amazon.S3.AmazonS3Client("AKIAJA3PK2CYTZEC5E6A", "vJIJRmV+kWU4J+Ex3veaFaohK7UU4aaYOy8ggEe9", Amazon.RegionEndpoint.USWest2);


            ListObjectsRequest listrequest = new ListObjectsRequest();

            listrequest.BucketName = "slofurnotest";
           // listrequest.Prefix = "First";


            ListObjectsResponse response = new ListObjectsResponse();


            response = ListAsync(listrequest, s3client).Result;


            List<String> filelist = new List<String>();


            string baseuri = "https://s3-us-west-2.amazonaws.com/slofurnotest/";

            foreach (S3Object obj in response.S3Objects)
            {
               

                filelist.Add((baseuri + obj.Key));


            }


            return filelist.ToArray();

            


            /*

            List<GetObjectRequest> requestlist = new List<GetObjectRequest>();


            foreach (S3Object obj in response.S3Objects)
            {
                Debug.WriteLine("Object - " + obj.Key);

                requestlist.Add(new GetObjectRequest{
                    BucketName = "slofurnotest",
                    Key = obj.Key
                });


            }

            Task<MemoryStream>[] tasks = requestlist.Select(r => GetObjectAsync(r, s3client)).ToArray();
            
            Task.WaitAll(tasks);

            List<MemoryStream> resultslist = tasks.Select(t => t.Result).ToList();

            */




        }


        


        public static Task<ListObjectsResponse> ListAsync(ListObjectsRequest request, AmazonS3Client client)
        {


            return Task<ListObjectsResponse>.Factory.FromAsync(client.BeginListObjects, client.EndListObjects, request, null);

        }

        public static Task<MemoryStream> GetObjectAsync(GetObjectRequest request, AmazonS3Client client)
        {


            Task<GetObjectResponse> response = Task<GetObjectResponse>.Factory.FromAsync(client.BeginGetObject, client.EndGetObject, request, null);

            Task<MemoryStream> translation = response.ContinueWith(t =>
            {
                using (Task<GetObjectResponse> resp = t)
                {
                    var ms = new MemoryStream();
                    t.Result.ResponseStream.CopyTo(ms);
                    return ms;
                }
            });

            return translation;


        }

        [WebMethod]
        public string UploadImage(byte[] data, string title)
        {


            Stopwatch stopWatch = new Stopwatch();


            var bitsperpixel = 4;

            int oldwidth = 10;
            int oldheight = 10;

            int heightmulti = 100;
            int widthmulti = 100;

            var oldstride = oldwidth * bitsperpixel;
            var newstride = oldstride * widthmulti;

            var newheight = oldheight * heightmulti;

            
       


           

            byte[] newdata = scalePixels(data, oldwidth, oldheight, widthmulti, heightmulti);



            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            

            stopWatch.Reset();
            stopWatch.Start();

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

            MemoryStream ms = new MemoryStream();

            encoder.Save(ms);

            

            var s3client = new Amazon.S3.AmazonS3Client("AKIAJA3PK2CYTZEC5E6A", "vJIJRmV+kWU4J+Ex3veaFaohK7UU4aaYOy8ggEe9", Amazon.RegionEndpoint.USWest2);
            IAsyncResult asyncResult;



            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = "slofurnotest",
                Key = fulltitle,
            };



            var requestlist = new List<PutObjectRequest>();


            for (int i = 0; i < 50; i++)
            {

                PutObjectRequest temprequest = new PutObjectRequest
                {
                    BucketName = "slofurnotest",
                    Key = (title + i.ToString() + ".gif"),
                };

                MemoryStream tempms = new MemoryStream();
                ms.Position = 0;
                ms.CopyTo(tempms);



                temprequest.InputStream = tempms;

                requestlist.Add(temprequest);


            }


            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts3 = stopWatch.Elapsed;


            stopWatch.Reset();
            stopWatch.Start();



            StartUploadAsync(s3client, requestlist);


           

            /*
            MemoryStream ms = new MemoryStream();
            
                 encoder.Save(ms);




                
            
                

                 request.InputStream = ms;

                 // Put object
                 //PutObjectResponse response = s3client.PutObject(request);

                 asyncResult = s3client.BeginPutObject(request, CallbackWithState, new ClientState { Client = s3client, Start = DateTime.Now });
            */

            

            



            stopWatch.Stop();
            TimeSpan ts2 = stopWatch.Elapsed;


            String responsestring = ("Scale runtime " + ts.TotalMilliseconds + "Memory copy runtine : " + ts3.TotalMilliseconds + " Upload runtime " + ts2.TotalMilliseconds);



            // Get the elapsed time as a TimeSpan value.
            
            


            return responsestring;
        }


        static Task UploadFileAsync(AmazonS3Client s3client, PutObjectRequest request)
        {
            


            Task result = Task.Factory.StartNew(() => s3client.BeginPutObject(request, CallbackWithState, new ClientState { Client = s3client, Start = DateTime.Now }));


            return result;
            /*
            Task<PutObjectResponse> response =
                Task.Factory.FromAsync<PutObjectRequest, PutObjectResponse>(
                    s3client.BeginPutObject, s3client.EndPutObject, request, new ClientState { Client = s3client, Start = DateTime.Now }
                );



            return response;
            */
        }


        static Task[] UploadAsync(AmazonS3Client s3, IEnumerable<PutObjectRequest> requests)
        {

            return requests.Select(r => UploadFileAsync(s3, r)).ToArray();

            //return requests.Select(r => UploadFileAsync(s3, r)).ToArray();
        }

        static void StartUploadAsync(AmazonS3Client s3, IEnumerable<PutObjectRequest> requests)
        {

           
            Task[] tasks = UploadAsync(s3, requests);
            Task.WaitAll(tasks);


            //return tasks.Select(t => t.result).ToList();
        }



        public static void SimpleCallback(IAsyncResult asyncResult)
        {
            Console.WriteLine("Finished PutObject operation with simple callback");
            Console.Write("\n\n");
        }


        public static void CallbackWithClient(IAsyncResult asyncResult)
        {
            try
            {
                AmazonS3Client s3Client = (AmazonS3Client)asyncResult.AsyncState;
                PutObjectResponse response = s3Client.EndPutObject(asyncResult);
                Console.WriteLine("Finished PutObject operation with client callback");
            }
            catch (AmazonS3Exception s3Exception)
            {

            }
        }


        public static void CallbackWithState(IAsyncResult asyncResult)
        {
            try
            {
                ClientState state = asyncResult.AsyncState as ClientState;
                AmazonS3Client s3Client = (AmazonS3Client)state.Client;
                PutObjectResponse response = state.Client.EndPutObject(asyncResult);
                Debug.WriteLine("Finished PutObject. Elapsed time: {0}",
                  (DateTime.Now - state.Start).ToString());
            }
            catch (AmazonS3Exception s3Exception)
            {
            
            }
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

                             

                                target[x * newstride + y * bytesperpixel] = source[i * oldstride + j * bytesperpixel];
                                


                            }


                        }



                    }



                }


            }
            return newdata;





        }



    }

    class ClientState
    {
        AmazonS3Client client;
        DateTime startTime;

        public AmazonS3Client Client
        {
            get { return client; }
            set { client = value; }
        }

        public DateTime Start
        {
            get { return startTime; }
            set { startTime = value; }
        }
    }
}