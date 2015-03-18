using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using TexLib;

namespace GLembedded
{







    public partial class Form1 : Form
    {

        bool loaded = false;
        Bitmap videoFrame1 = null;
        Bitmap videoFrame2 = null;
        Bitmap videoFrame3 = null;
        Bitmap videoFrame4 = null;
   
        VideoCaptureDevice videoSource1;
        VideoCaptureDevice videoSource2;
        VideoCaptureDevice videoSource3;
        VideoCaptureDevice videoSource4;
        FilterInfoCollection videoDevices;
        int videoTexture1 = -1;
        int videoTexture2 = -1;
        int videoTexture3 = -1;
        int videoTexture4 = -1;
        int videoTexture5 = -1;
        int selectedCam = 0;

        Quad fullscreenQuad = new Quad();




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.DoubleBuffered = true;
            serialPort1.Open();
        }
        private void SetupViewport()
        {

            int w = glControl1.Width;
            int h = glControl1.Height;
          //  GL.MatrixMode(MatrixMode.Projection);
        
          //  GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
        //    GL.Viewport(0, 0, w, h); // Use all of the glControl painting area

          
            
      
         
        }
        private void glControl1_Load(object sender, EventArgs e)
        {

            glControl1.MakeCurrent();
           
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Enable(EnableCap.DepthTest);
            TexUtil.InitTexturing();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.DepthFunc(DepthFunction.Lequal);

            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.Enable(EnableCap.ColorMaterial);
            
            GL.Clear(ClearBufferMask.None);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // render per default onto screen, not some FBO
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0); // use the visible framebuffer
            GL.ClearColor(Color.Black);
           
            GL.Viewport(glControl1.Location.X - 10, glControl1.Location.Y - 10, glControl1.Width, glControl1.Height);
            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, -1 * Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1.1);

            OpenVideoStream();
            loaded = true;
        }
        

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!loaded)
                return;

        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();


          
            if (!loaded) // Play nice
                return;


            //if(videoFrame1!=null & videoFrame2!=null & videoFrame3!=null & videoFrame4!=null )
            {


                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Enable(EnableCap.Texture2D);
                if (videoFrame1 != null)
              
                    lock (videoFrame1)
                    {
                        glControl1.MakeCurrent();
                        
                        if (videoTexture1 != -1)
                            GL.DeleteTextures(1, ref videoTexture1);

                        videoTexture1 = TexUtil.CreateTextureFromBitmap(videoFrame1);

                        GL.BindTexture(TextureTarget.Texture2D, videoTexture1);
             
                        //  videoFrame1 = null;
                        fullscreenQuad.Draw(1);
                        if (selectedCam == 1) 
                        {
                            drawSide(videoFrame1);
                        glControl1.MakeCurrent();

                    }
                        videoFrame1.Dispose();
                        GC.Collect();
                    }
           
                if (videoFrame2 != null)
                 
                    lock (videoFrame2)
                    {
                        glControl1.MakeCurrent();
                        if (videoTexture2 != -1)
                            GL.DeleteTextures(1, ref videoTexture2);
                        videoTexture2 = TexUtil.CreateTextureFromBitmap(videoFrame2);

                        GL.BindTexture(TextureTarget.Texture2D, videoTexture2);
                        if (selectedCam == 2)
                        {
                            drawSide(videoFrame2);
                            glControl1.MakeCurrent();

                        }
                          videoFrame2.Dispose();
                        // videoFrame2 = null;
                        GC.Collect();
                        fullscreenQuad.Draw(2);
                   

                    }

              
                if (videoFrame3 != null)
                 
                    lock (videoFrame3)
                    {
                        glControl1.MakeCurrent();
                        if (videoTexture3 != -1)
                            GL.DeleteTextures(1, ref videoTexture3);
                        videoTexture3 = TexUtil.CreateTextureFromBitmap(videoFrame3);
                        
                        GL.BindTexture(TextureTarget.Texture2D, videoTexture3);
                        if (selectedCam == 3)
                        {
                            drawSide(videoFrame3);
                            glControl1.MakeCurrent();

                        }
                        videoFrame3.Dispose();
                          //  videoFrame3 = null;

                        GC.Collect();
                      fullscreenQuad.Draw(3);
                    }

              
                if (videoFrame4 != null)
               
                    lock (videoFrame4)
                    {
                        if (videoTexture4 != -1)
                            GL.DeleteTextures(1, ref videoTexture4);
                        videoTexture4 = TexUtil.CreateTextureFromBitmap(videoFrame4);
                      
                        GL.BindTexture(TextureTarget.Texture2D, videoTexture4);
                        if (selectedCam == 4)
                        {
                            drawSide(videoFrame4);
                            glControl1.MakeCurrent();

                        }
                           videoFrame4.Dispose();
                      //   videoFrame4 = null;
                        GC.Collect();
                      fullscreenQuad.Draw(4);
                  
                    }

            }
            glControl2.SwapBuffers();
           // GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Quads);
            GL.Color4(0f, 0f, 0f,1f);

             GL.Vertex2(1, .98);
             GL.Vertex2(.98, 1);
             GL.Vertex2(0, 0.02);
             GL.Vertex2(0, -0.02);

             GL.Vertex2(-.98, 1);
             GL.Vertex2(-1, .98);
             GL.Vertex2(0, -0.02);
             GL.Vertex2(0, 0.02);

             GL.Vertex2(-1, -.98);
             GL.Vertex2(-.98, -1);
             GL.Vertex2(0, -0.02);
             GL.Vertex2(0, 0.02);

             GL.Vertex2(1, -.98);
             GL.Vertex2(.98, -1);
             GL.Vertex2(0, -0.02);
             GL.Vertex2(0, 0.02);




             GL.Vertex2(-1, 1);
             GL.Vertex2(-0.98, 1);
             GL.Vertex2(-0.98, 0.98);
             GL.Vertex2(-1, 0.98);

             GL.Vertex2(1, 1);
             GL.Vertex2(0.98, 1);
             GL.Vertex2(0.98, 0.98);
             GL.Vertex2(1, 0.98);

             GL.Vertex2(-1, -1);
             GL.Vertex2(-0.98, -1);
             GL.Vertex2(-0.98, -0.98);
             GL.Vertex2(-1, -0.98);

             GL.Vertex2(1, -1);
             GL.Vertex2(0.98, -1);
             GL.Vertex2(0.98, -0.98);
             GL.Vertex2(1, -0.98);


            GL.End();
            GL.Color4(1f, 1f, 1f, 1f);
            glControl1.SwapBuffers();
         
             
           
          
        }

        private void OpenVideoStream()
        {


            //     Console.WriteLine("Connecting to {0}", url);
            videoSource1 = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource2 = new VideoCaptureDevice(videoDevices[1].MonikerString);
            videoSource3 = new VideoCaptureDevice(videoDevices[2].MonikerString);
            videoSource4 = new VideoCaptureDevice(videoDevices[3].MonikerString);


            videoSource1.NewFrame += (Object sender, NewFrameEventArgs eventArgs) =>
            {
                System.Threading.Thread.Sleep(10);
                if (videoFrame1 != null)
                    lock (videoFrame1)
                    {
                   
                        videoFrame1 = new Bitmap(eventArgs.Frame);
                        
                      
                    }
                else
                {
                    videoFrame1 = new Bitmap(eventArgs.Frame);
                }
               
               

                
            };

            videoSource2.NewFrame += (Object sender, NewFrameEventArgs eventArgs) =>
            {
                System.Threading.Thread.Sleep(10);
                if (videoFrame2 != null)
                    lock (videoFrame2)
                    {
                        videoFrame2 =  new Bitmap(eventArgs.Frame);
                   
                    }
                else
                {
                    videoFrame2 = new Bitmap(eventArgs.Frame);
                }
            };

            videoSource3.NewFrame += (Object sender, NewFrameEventArgs eventArgs) =>
            {
                System.Threading.Thread.Sleep(10);
                if (videoFrame3 != null)
                    lock (videoFrame3)
                    {
                       
                        videoFrame3 =  new Bitmap(eventArgs.Frame);
                     
                    }
                else
                {
                    videoFrame3 = new Bitmap(eventArgs.Frame);
                }
            };

            videoSource4.NewFrame += (Object sender, NewFrameEventArgs eventArgs) =>
            {
                System.Threading.Thread.Sleep(10);
                if (videoFrame4 != null)
                    lock (videoFrame4)
                    {
                        videoFrame4 = new Bitmap(eventArgs.Frame);
                  
                    }
                else
                {

                    videoFrame4 = new Bitmap(eventArgs.Frame);
                }
            };

            videoSource1.Start();
            videoSource2.Start();
            videoSource3.Start();
            videoSource4.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            glControl1.Refresh();
          
        }

        private void glControl2_Paint(object sender, PaintEventArgs e)
        {
           


        //   glControl2.SwapBuffers();
       
        }

        private void glControl2_Load(object sender, EventArgs e)
        {
            glControl2.MakeCurrent();

            int w = glControl2.Width;
            int h = glControl2.Height;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1.1);
            GL.Viewport(0,0, glControl2.Width, glControl2.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            TexUtil.InitTexturing();
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Clear(ClearBufferMask.None);
           GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // render per default onto screen, not some FBO
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0); // use the visible framebuffer
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1.1);
     
        }

        private void drawSide(Bitmap cam)
        {
          
                glControl2.MakeCurrent();

                GL.MatrixMode(MatrixMode.Modelview);
                GL.Color3(1f, 1f, 1f);
                GL.PushMatrix();
                GL.Enable(EnableCap.Texture2D);
                videoTexture5 = TexUtil.CreateTextureFromBitmap(cam);
                GL.BindTexture(TextureTarget.Texture2D, videoTexture5);
                GL.Begin(BeginMode.Quads);

                GL.TexCoord2(0, 0);
                GL.Vertex2(-1, 1);

                GL.TexCoord2(1f, 0);
                GL.Vertex2(1f, 1f);

                GL.TexCoord2(1f, 1f);
                GL.Vertex2(1f, -1f);

                GL.TexCoord2(0, 1f);
                GL.Vertex2(-1f, -1f);

                GL.End();

                GL.Disable(EnableCap.Texture2D);
                GL.Begin(BeginMode.Quads);
                GL.Color4(1f, 0.5f, 0f, 1f);

                GL.Vertex2(-0.4f, -0.6f);
                GL.Vertex2(-0.38f, -0.6f);
                GL.Vertex2(-0.28f, 0.2f);
                GL.Vertex2(-0.3f, 0.2f);

                GL.Vertex2(-0.3f, 0.2f);
                GL.Vertex2(-0.3f, 0.18f);
               
                GL.Vertex2(0.3f, 0.18f);
                GL.Vertex2(0.3f, 0.2f);
                GL.Vertex2(0.4f, -0.6f);
                GL.Vertex2(0.38f, -0.6f);
                GL.Vertex2(0.28f, 0.2f);
                GL.Vertex2(0.3f, 0.2f);

                GL.End();

                GL.PopMatrix();

                glControl2.Refresh();
           

            



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCam = comboBox1.SelectedIndex;
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bytein = serialPort1.ReadByte();
            if (bytein < 10)
            {
                selectedCam = 1;
            }
            else if (bytein < 15)
            {
                selectedCam = 2;
            }
            else if (bytein < 20)
            {
                selectedCam = 3;
            }
            else if (bytein < 25)
            {
                selectedCam = 4;
            }

        }

        }
    }

  


