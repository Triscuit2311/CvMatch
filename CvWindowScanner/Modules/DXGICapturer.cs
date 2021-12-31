using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Rectangle = System.Drawing.Rectangle;
using Resource = SharpDX.DXGI.Resource;


namespace CvWindowScanner
{
    public static class DXGICapturer
    {
        
        private static Factory1 _dxFactory1;
        private static Adapter1 _dxAdapter1;
        private static SharpDX.Direct3D11.Device _dxDevice;
        private static Output _dxOutput;
        private static Output1 _dxOutput1;

        private static int _dskWidth;
        private static int _dskHeight;

        private static Texture2DDescription _dxTexture2DDescription;
        private static Texture2D _uniScreenTex;

        
        private static bool _capture = false;
        private static bool _initialized = false;

        private static Thread _captureThread;

        private static System.Drawing.Rectangle _captureRect;
        private static Bitmap _lastCapturedFrame;
        
        /// <summary>
        /// Initializes D3D11 members and spawns capture thread. Must be called before other DXGICapturer methods. 
        /// </summary>
        public static void Init()
        {
            _dxFactory1 = new Factory1();
            _dxAdapter1 = _dxFactory1.GetAdapter1(0);
            _dxDevice = new SharpDX.Direct3D11.Device(_dxAdapter1);
            _dxOutput = _dxAdapter1.GetOutput(0);
            _dxOutput1 = _dxOutput.QueryInterface<Output1>();
            
            _dskWidth = _dxOutput.Description.DesktopBounds.Right;
            _dskHeight = _dxOutput.Description.DesktopBounds.Bottom;
            
            _dxTexture2DDescription = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = _dskWidth,
                Height = _dskHeight,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };

            _captureRect = new Rectangle(0, 0, _dskWidth, _dskHeight);
            _uniScreenTex = new Texture2D(_dxDevice, _dxTexture2DDescription);
            _initialized = true;
            _capture = true;

            _captureThread = new Thread(new ThreadStart(CaptureThreadOperation));
            _captureThread.Start();
            
            Thread.Sleep(1000);
        }

        
        /// <summary>
        /// Stops capture thread, but does not reset D3D11 members. 
        /// </summary>
        public static void Stop()
        {
            if (!_initialized)
                throw new Exception("DXGICapturer needs initialization.");
            _capture = false;
            _initialized = false;
            _captureThread.Join();
        }

        /// <summary>
        /// Gets last valid frame data as bitmap.
        /// </summary>
        /// <returns> Last frame, or pure red bitmap if no valid frame exists</returns>
        /// <exception cref="Exception">Throws if Class is not initialized using Init()</exception>
        public static Bitmap GetLast()
        {
            if (!_initialized)
                throw new Exception("DXGICapturer needs initialization.");
            if (!(_lastCapturedFrame is null)) return _lastCapturedFrame;
            Bitmap placeHolderBmp = new Bitmap(_dskWidth, _dskHeight);
            Graphics g = Graphics.FromImage(placeHolderBmp);
            g.Clear(System.Drawing.Color.Red);
            return placeHolderBmp;

        }
        
        /// <summary>
        /// Sets the crop zone for frame capture
        /// </summary>
        /// <param name="rect">Area to crop the captured frame to. Typically window rect.</param>
        /// <exception cref="Exception">Throws if Class is not initialized using Init()</exception>
        public static void SetCaptureRect(System.Drawing.Rectangle rect)
        {
            if (!_initialized)
                throw new Exception("DXGICapturer needs initialization.");
            _captureRect = rect;
        }

        /// <summary>
        /// Capture Thread Function
        /// </summary>
        private static void CaptureThreadOperation()
        {
            using ( var dxDuplication = _dxOutput1.DuplicateOutput(_dxDevice))
            {
                var i = 0;
                while(_initialized)
                {
                    if (!_capture)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    try
                    {
                        dxDuplication.AcquireNextFrame(5, out _, out var screenResource);
                        
                        using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
                            _dxDevice.ImmediateContext.CopyResource(screenTexture2D, _uniScreenTex);
                        
                        var mapSource = _dxDevice.ImmediateContext
                            .MapSubresource(_uniScreenTex, 0,
                                MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
                        
                        using (var bitmap = new Bitmap(_dskWidth, _dskHeight, PixelFormat.Format32bppArgb))
                        {
                            var boundsRect = new System.Drawing.Rectangle(0, 0, _dskWidth, _dskHeight);
                            
                            var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                            var sourcePtr = mapSource.DataPointer;
                            var destPtr = mapDest.Scan0;
                            for (int y = 0; y < _dskHeight; y++)
                            {
                                Utilities.CopyMemory(destPtr, sourcePtr, _dskWidth * 4);
                                sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                                destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                            }
                            
                            // Declare an array to hold the bytes of the bitmap.
                            int bytes = mapDest.Stride * bitmap.Height;
                            byte[] rgbValues = new byte[bytes];

                            // Copy the RGB values into the array.
                            destPtr = mapDest.Scan0;
                            System.Runtime.InteropServices.Marshal.Copy(destPtr, rgbValues, 0, bytes);

                            bool AllOneColor = true;
                            for (int index = 0; index < rgbValues.Length; index++)
                            {
                                //compare the current A or R or G or B with the A or R or G or B at position 0,0.
                                if (rgbValues[index] != rgbValues[index % 4])
                                {
                                    AllOneColor= false;
                                    break;
                                }
                            }

                            bitmap.UnlockBits(mapDest);
                            _dxDevice.ImmediateContext.UnmapSubresource(_uniScreenTex, 0);

                            i++;
                            // Save last frame
                            if (!bitmap.Size.IsEmpty && !AllOneColor)
                            {
                                _lastCapturedFrame = CropAtRect(bitmap, _captureRect);
                            }

                        }

                        screenResource.Dispose();
                        dxDuplication.ReleaseFrame();
                    }
                    catch (SharpDXException e)
                    {
                        if (e.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                        {
                            Trace.TraceError(e.Message);
                            Trace.TraceError(e.StackTrace);
                        }
                    }
                }
            }
        }
        
        // Utils
        
        /// <summary>
        /// Crops a bitmap to a new rect size
        /// </summary>
        /// <param name="bitmap">Input bitmap</param>
        /// <param name="rect">Rect area to crop bitmap to.</param>
        /// <returns>Cropped bitmap</returns>
        public static Bitmap CropAtRect(Bitmap bitmap,  System.Drawing.Rectangle rect)
        {
            try
            {
                return bitmap.Clone(rect, bitmap.PixelFormat);
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Rect out of bounds for bitmap!");
                Bitmap placeHolderBmp = new Bitmap(_dskWidth, _dskHeight);
                Graphics g = Graphics.FromImage(placeHolderBmp);
                g.Clear(System.Drawing.Color.Red);
                return placeHolderBmp;
            }
        }

    }
}