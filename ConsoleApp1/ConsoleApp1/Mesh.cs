using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using LearnOpenTK.Common;

namespace Quadratic2
{
    class Mesh
    {
        protected List<Vector3> vertices = new List<Vector3>();
        protected List<Vector3> textureVertices = new List<Vector3>();
        protected List<Vector3> normals = new List<Vector3>();
        protected List<uint> vertexIndices = new List<uint>();
        protected int _vertexBufferObject;
        protected int _elementBufferObject;
        protected int _vertexArrayObject;
        protected Shader _shader;
        protected Matrix4 transform;


        public Mesh()
        {

        }
        public void LoadObjFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open file");
            }

            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
                    words.RemoveAll(s => s == string.Empty);

                    if (words.Count == 0)
                        continue;

                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {
                        //vertex
                        case "v":
                            vertices.Add(new Vector3(float.Parse(words[0]) / 10,
                                float.Parse(words[1]) / 10,
                                float.Parse(words[2]) / 10));
                            break;
                        case "vt":
                            textureVertices.Add(new Vector3(float.Parse(words[0]),
                                float.Parse(words[1]), words.Count < 3 ? 0 :
                                float.Parse(words[2])));
                            break;
                        case "vn":
                            normals.Add(new Vector3(float.Parse(words[0]),
                                float.Parse(words[1]),
                                float.Parse(words[2])));
                            break;
                        //face
                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                    continue;

                                string[] comps = w.Split('/');
                                vertexIndices.Add(uint.Parse(comps[0]) - 1);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        public void SetupObject()
        {
            transform = Matrix4.Identity;

            //VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer,
                vertices.Count * Vector3.SizeInBytes,
                vertices.ToArray(),
                BufferUsageHint.StaticDraw);

            //VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //EBO
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                vertexIndices.Count * sizeof(uint),
                vertexIndices.ToArray(),
                BufferUsageHint.StaticDraw);

            _shader = new Shader("C:/Users/vince/source/repos/Grafkom1/Quadratic2/Shaders/Shader.vert",
               "C:/Users/vince/source/repos/Grafkom1/Quadratic2/Shaders/Shader.frag");
            _shader.Use();
        }
        public void CreateBoxVertices()
        {
            float posX = 0.0f;
            float posY = 0.0f;
            float posZ = 0.0f;

            float boxLength = 0.5f;

            Vector3 temp_vector;
            //titik 1
            temp_vector.X = posX - boxLength / 2.0f;
            temp_vector.Y = posY + boxLength / 2.0f;
            temp_vector.Z = posZ - boxLength / 2.0f;

            vertices.Add(temp_vector);
            //titik 2
            temp_vector.X = posX + boxLength / 2.0f;
            temp_vector.Y = posY + boxLength / 2.0f;
            temp_vector.Z = posZ - boxLength / 2.0f;

            vertices.Add(temp_vector);
            //titik 3
            temp_vector.X = posX - boxLength / 2.0f;
            temp_vector.Y = posY - boxLength / 2.0f;
            temp_vector.Z = posZ - boxLength / 2.0f;

            vertices.Add(temp_vector);
            //titik 4
            temp_vector.X = posX + boxLength / 2.0f;
            temp_vector.Y = posY - boxLength / 2.0f;
            temp_vector.Z = posZ - boxLength / 2.0f;

            vertices.Add(temp_vector);
            //titik 5
            temp_vector.X = posX - boxLength / 2.0f;
            temp_vector.Y = posY + boxLength / 2.0f;
            temp_vector.Z = posZ + boxLength / 2.0f;

            vertices.Add(temp_vector);
            //titik 6
            temp_vector.X = posX + boxLength / 2.0f;
            temp_vector.Y = posY + boxLength / 2.0f;
            temp_vector.Z = posZ + boxLength / 2.0f;

            vertices.Add(temp_vector);
            //titik 7
            temp_vector.X = posX - boxLength / 2.0f;
            temp_vector.Y = posY - boxLength / 2.0f;
            temp_vector.Z = posZ + boxLength / 2.0f;

            vertices.Add(temp_vector);
            //titik 8
            temp_vector.X = posX + boxLength / 2.0f;
            temp_vector.Y = posY - boxLength / 2.0f;
            temp_vector.Z = posZ + boxLength / 2.0f;

            vertices.Add(temp_vector);

            vertexIndices = new List<uint>()
            {
                0,1,2, //depan
                1,2,3, //depan
                0,4,5, //atas
                0,1,5, //atas
                1,3,5, //kanan
                3,5,7, //kanan
                0,2,4, //kiri
                2,4,6, //kiri
                4,5,6, //belakang
                5,6,7, //belakang
                2,6,7, //bawah
                2,3,7  //bawah
            };
        }
        public void CreateHyperboloid2SheetsVertices_A()
        {
            float _positionX = -0.5f;
            float _positionY = 0.0f;
            float _positionZ = 0.0f;

            float _radiusX = 0.3f;
            float _radiusY = 0.3f;
            float _radiusZ = 0.3f;
            float _pi = 3.14159f;

            //sisi A
            for(float u = -_pi/2; u < _pi/2; u += _pi/30)
            {
                for( float v = -_pi/2; v < _pi/2; v += _pi/30)
                {
                    Vector3 vec;
                    vec.X = (float)Math.Tan(v) * (float)Math.Cos(u) * _radiusX + _positionX;
                    vec.Y = (float)Math.Tan(v) * (float)Math.Sin(u) * _radiusY + _positionY;
                    vec.Z = (1.0f / (float)Math.Cos(v) ) * _radiusZ + _positionZ;
                    vertices.Add(vec);
                }
            }
            
        }
        public void CreateHyperboloid2SheetsVertices_B()
        {
            float _positionX = -0.5f;
            float _positionY = 0.0f;
            float _positionZ = 0.0f;

            float _radiusX = 0.3f;
            float _radiusY = 0.3f;
            float _radiusZ = 0.3f;
            float _pi = 3.14159f;

            //sisi B
            for (float u = -_pi / 2; u < _pi / 2; u += _pi / 30)
            {
                for (float v = _pi / 2; v < 3 * _pi / 2; v += _pi / 30)
                {
                    Vector3 vec;
                    vec.X = (float)Math.Tan(v) * (float)Math.Cos(u) * _radiusX + _positionX;
                    vec.Y = (float)Math.Tan(v) * (float)Math.Sin(u) * _radiusY + _positionY;
                    vec.Z = (1.0f / (float)Math.Cos(v)) * _radiusZ + _positionZ;
                    vertices.Add(vec);
                }
            }

           
        }
        public void CreateEllipticVertices()
        {
            float _positionX = 0.0f;
            float _positionY = 0.0f;
            float _positionZ = 0.0f;

            float _radiusX = 0.3f;
            float _radiusY = 0.3f;
            float _radiusZ = 0.3f;
            float _pi = 3.14159f;

            for (float u = -_pi; u < _pi; u += _pi / 30)
            {
                for (float v = -_pi / 2; v < _pi / 2; v += _pi / 30)
                {
                    Vector3 vec;
                    vec.X = (float)Math.Cos(v) * (float)Math.Cos(u) * _radiusX + _positionX;
                    vec.Y = (float)Math.Cos(v) * (float)Math.Sin(u) * _radiusY + _positionY;
                    vec.Z = (float)Math.Sin(v) * _radiusZ + _positionZ;
                    vertices.Add(vec);
                }
            }
        }
        public void CreateHyperboloidParaboloidVertices()
        {
            float _positionX = -0.5f;
            float _positionY = 0.0f;
            float _positionZ = 0.0f;

            float _radiusX = 0.3f;
            float _radiusY = 0.3f;
            float _radiusZ = 0.3f;
            float _pi = 3.14159f;

            for (float u = -_pi; u < _pi; u += _pi / 30)
            {
                for (float v = 0; v < _pi; v += _pi / 90)
                {
                    Vector3 vec;
                    vec.X = (float)Math.Tan(u) * v * _radiusX + _positionX;
                    vec.Y = (1.0f / (float)Math.Cos(u)) * v * _radiusY + _positionY;
                    vec.Z = v * v + _positionZ;
                    vertices.Add(vec);
                }
            }

            
        }
        public void Rotate()
        {
            //Sumbu X
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0.05f));
            //Sumbu Y
            transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(0.05f));
            //Sumbu Z
            transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0.05f));
        }
        public void Scale()
        {
            transform = transform * Matrix4.CreateScale(0.3f);
        }
        public void Translate()
        {
            transform = transform * Matrix4.CreateTranslation(new Vector3(0.001f, 0.001f, 0f));
        }
        public void Render()
        {
            _shader.Use();
            _shader.SetMatrix4("transform", transform);
            GL.BindVertexArray(_vertexArrayObject);
            //GL.DrawElements(PrimitiveType.LineStrip,
            //    vertexIndices.Count,
            //    DrawElementsType.UnsignedInt, 0);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);


        }


    }
}

//using LearnOpenTK.Common;
//using OpenTK.Mathematics;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using OpenTK.Graphics.OpenGL4;

//namespace Quadratic2
//{
//    class Mesh
//    {
//        //Vector 3 pastikan menggunakan OpenTK.Mathematics
//        //tanpa protected otomatis komputer menganggap sebagai private
//        List<Vector3> vertices = new List<Vector3>();
//        List<Vector3> textureVertices = new List<Vector3>();
//        List<Vector3> normals = new List<Vector3>();
//        List<uint> vertexIndices = new List<uint>();
//        int _vertexBufferObject;
//        int _elementBufferObject;
//        int _vertexArrayObject;
//        Shader _shader;
//        Matrix4 transform;
//        int counter = 0;
//        public List<Mesh> child = new List<Mesh>();
//        public Mesh()
//        {
//        }

//        public void setupObject()
//        {

//            //inisialisasi Transformasi
//            transform = Matrix4.Identity;
//            //inisialisasi buffer
//            _vertexBufferObject = GL.GenBuffer();
//            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
//            //parameter 2 yg kita panggil vertices.Count == array.length
//            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
//                vertices.Count * Vector3.SizeInBytes,
//                vertices.ToArray(),
//                BufferUsageHint.StaticDraw);
//            //inisialisasi array
//            _vertexArrayObject = GL.GenVertexArray();
//            GL.BindVertexArray(_vertexArrayObject);
//            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
//            GL.EnableVertexAttribArray(0);
//            //inisialisasi index vertex
//            _elementBufferObject = GL.GenBuffer();
//            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
//            //parameter 2 dan 3 perlu dirubah
//            GL.BufferData(BufferTarget.ElementArrayBuffer,
//                vertexIndices.Count * sizeof(uint),
//                vertexIndices.ToArray(), BufferUsageHint.StaticDraw);
//            //inisialisasi shader
//            _shader = new Shader("E:/Asisten Grafkom/Pertemuan1/Pertemuan1/Shaders/shader.vert", "E:/Asisten Grafkom/Pertemuan1/Pertemuan1/Shaders/shader.frag");
//            _shader.Use();
//            scale();
//        }
//        public void render()
//        {
//            //render itu akan selalu terpanggil setiap frame
//            _shader.Use();

//            ////kalian menginginkan perpindahan object setiap frame sebanyak 0.1 X,0.1 Y,0 
//            //if(counter == 100)
//            //{
//            //    rotate();
//            //    counter = 0;
//            //}
//            //else
//            //{
//            //    counter++;
//            //}

//            _shader.SetMatrix4("transform", transform);
//            GL.BindVertexArray(_vertexArrayObject);
//            //perlu diganti di parameter 2
//            GL.DrawElements(PrimitiveType.Triangles,
//                vertexIndices.Count,
//                DrawElementsType.UnsignedInt, 0);

//            foreach (var meshobj in child)
//            {
//                meshobj.render();
//            }
//        }
//        public List<Vector3> getVertices()
//        {
//            return vertices;
//        }
//        public List<uint> getVertexIndices()
//        {
//            return vertexIndices;
//        }

//        public void setVertexIndices(List<uint> temp)
//        {
//            vertexIndices = temp;
//        }
//        public int getVertexBufferObject()
//        {
//            return _vertexBufferObject;
//        }

//        public int getElementBufferObject()
//        {
//            return _elementBufferObject;
//        }

//        public int getVertexArrayObject()
//        {
//            return _vertexArrayObject;
//        }

//        public Shader getShader()
//        {
//            return _shader;
//        }

//        public Matrix4 getTransform()
//        {
//            return transform;
//        }

//        public void rotate()
//        {
//            //rotate parentnya
//            //sumbu Z
//            transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(20f));
//            //rotate childnya
//            foreach (var meshobj in child)
//            {
//                meshobj.rotate();
//            }
//        }
//        public void scale()
//        {
//            transform = transform * Matrix4.CreateScale(1.9f);
//        }
//        //translasi perpindahan suatu objek ke posisi lain
//        public void translate()
//        {
//            //perpindahan sebanyak 0.1 ke x, 0.1 ke y, dan 0 ke z
//            transform = transform * Matrix4.CreateTranslation(0.1f, 0.1f, 0.0f);
//        }

//        public void LoadObjFile(string path)
//        {
//            //komputer ngecek, apakah file bisa diopen atau tidak
//            if (!File.Exists(path))
//            {
//                //mengakhiri program dan kita kasih peringatan
//                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
//            }
//            //lanjut ke sini
//            using (StreamReader streamReader = new StreamReader(path))
//            {
//                while (!streamReader.EndOfStream)
//                {
//                    //aku ngambil 1 baris tersebut -> dimasukkan ke dalam List string -> dengan di split pakai spasi
//                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
//                    //removeAll(kondisi dimana penghapusan terjadi)
//                    words.RemoveAll(s => s == string.Empty);
//                    //Melakukan pengecekkan apakah dalam satu list -> ada isinya atau tidak list nya tersebut
//                    //kalau ada continue, perintah-perintah yang ada dibawahnya tidak akan dijalankan 
//                    //dan dia bakal kembali keatas lagi / melanjutkannya whilenya
//                    if (words.Count == 0)
//                        continue;

//                    //System.Console.WriteLine("New While");
//                    //foreach (string x in words)
//                    //               {
//                    //	System.Console.WriteLine("tes");
//                    //	System.Console.WriteLine(x);
//                    //               }

//                    string type = words[0];
//                    //remove at -> menghapus data dalam suatu indexs dan otomatis data pada indeks
//                    //berikutnya itu otomatis mundur kebelakang 1
//                    words.RemoveAt(0);


//                    switch (type)
//                    {
//                        // vertex
//                        //parse merubah dari string ke tipe variabel yang diinginkan
//                        //ada /10 karena saaat ini belum masuk materi camera
//                        case "v":
//                            vertices.Add(new Vector3(float.Parse(words[0]) / 10, float.Parse(words[1]) / 10, float.Parse(words[2]) / 10));
//                            break;

//                        case "vt":
//                            textureVertices.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]),
//                                                            words.Count < 3 ? 0 : float.Parse(words[2])));
//                            break;

//                        case "vn":
//                            normals.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2])));
//                            break;
//                        // face
//                        case "f":
//                            foreach (string w in words)
//                            {
//                                if (w.Length == 0)
//                                    continue;

//                                string[] comps = w.Split('/');

//                                vertexIndices.Add(uint.Parse(comps[0]) - 1);

//                            }
//                            break;

//                        default:
//                            break;
//                    }
//                }
//            }
//        }

//        public void createBoxVertices(float x, float y, float z)
//        {
//            //biar lebih fleksibel jangan inisialiasi posisi dan 
//            //panjang kotak didalam tapi ditaruh ke parameter
//            float _positionX = x;
//            float _positionY = y;
//            float _positionZ = z;

//            float _boxLength = 0.05f;

//            //Buat temporary vector
//            Vector3 temp_vector;
//            //1. Inisialisasi vertex
//            // Titik 1
//            temp_vector.X = _positionX - _boxLength / 2.0f; // x 
//            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z

//            vertices.Add(temp_vector);

//            // Titik 2
//            temp_vector.X = _positionX + _boxLength / 2.0f; // x
//            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z

//            vertices.Add(temp_vector);
//            // Titik 3
//            temp_vector.X = _positionX - _boxLength / 2.0f; // x
//            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z
//            vertices.Add(temp_vector);

//            // Titik 4
//            temp_vector.X = _positionX + _boxLength / 2.0f; // x
//            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z

//            vertices.Add(temp_vector);

//            // Titik 5
//            temp_vector.X = _positionX - _boxLength / 2.0f; // x
//            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

//            vertices.Add(temp_vector);

//            // Titik 6
//            temp_vector.X = _positionX + _boxLength / 2.0f; // x
//            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

//            vertices.Add(temp_vector);

//            // Titik 7
//            temp_vector.X = _positionX - _boxLength / 2.0f; // x
//            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

//            vertices.Add(temp_vector);

//            // Titik 8
//            temp_vector.X = _positionX + _boxLength / 2.0f; // x
//            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
//            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

//            vertices.Add(temp_vector);
//            //2. Inisialisasi index vertex
//            vertexIndices = new List<uint> {
//                // Segitiga Depan 1
//                0, 1, 2,
//                // Segitiga Depan 2
//                1, 2, 3,
//                // Segitiga Atas 1
//                0, 4, 5,
//                // Segitiga Atas 2
//                0, 1, 5,
//                // Segitiga Kanan 1
//                1, 3, 5,
//                // Segitiga Kanan 2
//                3, 5, 7,
//                // Segitiga Kiri 1
//                0, 2, 4,
//                // Segitiga Kiri 2
//                2, 4, 6,
//                // Segitiga Belakang 1
//                4, 5, 6,
//                // Segitiga Belakang 2
//                5, 6, 7,
//                // Segitiga Bawah 1
//                2, 3, 6,
//                // Segitiga Bawah 2
//                3, 6, 7
//            };

//        }
//    }
//}

