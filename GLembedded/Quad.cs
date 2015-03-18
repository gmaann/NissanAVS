using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GLembedded
{
    public class Quad
    {
        Vector2 position = new Vector2(0, 0);
        
        int i = 0;
        int j = 0;
        const int nx = 100;
        const int ny = 100;
        meshstruct[,] mesh = new meshstruct[nx+1, ny+1];
        public Quad()
        {
            Alpha = 1f;
            DrawWireframe = false;
            Color = new Vector4(1f, 1f, 1f, Alpha);
       
           
            for (i = 0; i < nx+1 ; i++)
            {
                for (j = 0; j < ny+1 ; j++)

                {

                    mesh[i, j].u = ((1f / nx) * i) * 0.75f +0.1f;
                    mesh[i, j].v = ((1f / ny) * j) * 0.75f +0.1f;

                    mesh[i, j].x = ((-1f + ((2f / nx) * i)) * 0.01f * (ny - j));
                        
                    mesh[i, j].y = (1f - ((2f / ny) * j )*0.5f);

                    mesh[i, j].i = 1;
                }
            }
        
        }
    
        public Vector4 Color { get; set; }

        public float Alpha { get; set; }

        public bool DrawWireframe { get; set; }

        public Vector2 Position
        {
            set
            {
                position = value;
            }
            get
            {
                return position;
            }
        }

        public float X
        {
            set
            {
                position = new Vector2(value, position.Y);
            }
            get
            {
                return position.X;
            }
        }

        public float Y
        {
            set
            {
                position = new Vector2(position.X, value);
            }
            get
            {
                return position.Y;
            }
        }


        struct meshstruct
        {
          
        
            public float u;
            public float v;

            public float x;
            public float y;

            public float i;
        }


        public void Draw(int location)
        {
         
            GL.PushMatrix();
            switch (location)
            {
                case 1:


                    break;
                case 2:
                    GL.Rotate(90, 0, 0, -1);
                    
                    break;
                case 3:
                    GL.Rotate(180, 0, 0, -1);
                    break;
                case 4:
                    GL.Rotate(270, 0, 0, -1);
                    break;

            }
            GL.Begin(BeginMode.Quads);

       
       
         
                    
                   
                  
                    for (i =0; i < nx-0 ; i++)
                    {
                        for (j = 0; j < ny-20 ; j++)
                        {
                            if (mesh[i, j].i < 0 || mesh[i + 1, j].i < 0 || mesh[i + 1, j + 1].i < 0 || mesh[i, j + 1].i < 0)
                                continue;

                        //    GL.Color3(mesh[i, j].i, mesh[i, j].i, mesh[i, j].i);
                            GL.TexCoord2(mesh[i, j].u, mesh[i, j].v);
                            GL.Vertex3(mesh[i, j].x, mesh[i, j].y, 0.0);

                        //    GL.Color3(mesh[i + 1, j].i, mesh[i + 1, j].i, mesh[i + 1, j].i);
                            GL.TexCoord2(mesh[i + 1, j].u, mesh[i + 1, j].v);
                            GL.Vertex3(mesh[i + 1, j].x, mesh[i + 1, j].y, 0.0);

                           // GL.Color3(mesh[i + 1, j + 1].i, mesh[i + 1, j + 1].i, mesh[i + 1, j + 1].i);
                            GL.TexCoord2(mesh[i + 1, j + 1].u, mesh[i + 1, j + 1].v);
                            GL.Vertex3(mesh[i + 1, j + 1].x, mesh[i + 1, j + 1].y, 0.0);

                          // GL.Color3(mesh[i, j + 1].i, mesh[i, j + 1].i, mesh[i, j + 1].i);
                            GL.TexCoord2(mesh[i, j + 1].u, mesh[i, j + 1].v);
                            GL.Vertex3(mesh[i, j + 1].x, mesh[i, j + 1].y, 0.0);

                        }
                    }
           


              
                    
            
            GL.End();
          
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);


        }

    
    }
}

