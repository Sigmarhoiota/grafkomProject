using LearnOpenTK.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Text;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Console1
{
    class Asset_3D
    {
        List<Vector3> _vertices = new List<Vector3>();
        List<uint> _indices = new List<uint>();


        int _vertexBufferObject;
        int _vertexArrayObject;
        int _elementBufferObject;
        Shader _shader;

        Matrix4 _model = Matrix4.Identity;
        //Matrix4 _view;
        //Matrix4 _projection;
        public List<Asset_3D> child = new List<Asset_3D>();
        public Vector3 _centerPosition = new Vector3(0, 0, 0);
        public List<Vector3> _euler = new List<Vector3>();
        Vector3 color;
        List<Vector3> _bezier_vertice = new List<Vector3>();


        public Asset_3D(Vector3 color)
        {
            //List<Vector3> vertices, List< uint > indices
            //this._vertices = vertices;
            //this._indices = indices;
            this.color = color;

            _euler.Add(new Vector3(1, 0, 0));
            //sumbu y
            _euler.Add(new Vector3(0, 1, 0));
            //sumbu z
            _euler.Add(new Vector3(0, 0, 1));
        }

        public Asset_3D()
        {
            _vertices = new List<Vector3>();
            //sumbu X
            _euler.Add(new Vector3(1, 0, 0));
            //sumbu y
            _euler.Add(new Vector3(0, 1, 0));
            //sumbu z
            _euler.Add(new Vector3(0, 0, 1));

        }

        public void OnLoad(string shadervert, string shaderfrag, float Size_x, float Size_y)
        {
            //VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes,
                _vertices.ToArray(), BufferUsageHint.StaticDraw);

            //VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);


            GL.VertexAttribPointer(0,       //tempat penyimpanan shader
                3,       //jumlah vertices
                VertexAttribPointerType.Float,
                false,                      //tidak pakai normalisasi
                3 * sizeof(float),          //ukuran byte dalam 1 vertex
                0                           //baca mulai dari index 0
                );
            GL.EnableVertexAttribArray(0); //refrence dari yang atas

            if (_indices.Count != 0)
            {
                //EBO
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);

            }

            //GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
            //Console.WriteLine($"Maximum number of vertex attribute supported:{maxAttributeCount}");




            _shader = new Shader(shadervert, shaderfrag);
            _shader.Use();

            //_view= Matrix4.CreateTranslation(0.0f,0.0f, -3.0f);
            //_projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size_x / (float)Size_y, 0.1f, 100.0f);
            foreach (var i in child)
            {
                i.OnLoad(shadervert, shaderfrag, Size_x, Size_y);
            }
        }

        public void OnRender(int _lines, Matrix4 camera_view, Matrix4 camera_projection)
        {
            //Console.WriteLine("Gambar : " + color);
            _shader.Use();

            //get uniform from shader


            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, new Vector4(color, 1.0f));
            _shader.SetVector3("objColor", color);

            //Console.WriteLine(_vertices[0]);

            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetMatrix4("model", _model);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);

            if (_indices.Count != 0)
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
            else
            {
                if (_lines == 0)
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, (int)(_vertices.Count));

                }
                else if (_lines == 1)
                {

                    //for (int i = 0; i < (int)(_vertices.Length / 3); i++)
                    //{
                    //    Console.WriteLine(_vertices[i * 3] + " " + _vertices[i * 3 + 1] + " " + _vertices[i * 3 + 2]);
                    //}

                    GL.DrawArrays(PrimitiveType.LineStrip, 0, (int)(_vertices.Count));
                }
                else if (_lines == 2)
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, (int)(_vertices.Count));
                }

            }
            foreach (var i in child)
            {
                i.OnRender(_lines, camera_view, camera_projection);
            }

        }

        public void createCuboid(float x_, float y_, float z_, float lengthx, float lengthy, float lengthz)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - lengthx / 2.0f;
            temp_vector.Y = y_ + lengthy / 2.0f;
            temp_vector.Z = z_ - lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + lengthx / 2.0f;
            temp_vector.Y = y_ + lengthy / 2.0f;
            temp_vector.Z = z_ - lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - lengthx / 2.0f;
            temp_vector.Y = y_ - lengthy / 2.0f;
            temp_vector.Z = z_ - lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + lengthx / 2.0f;
            temp_vector.Y = y_ - lengthy / 2.0f;
            temp_vector.Z = z_ - lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - lengthx / 2.0f;
            temp_vector.Y = y_ + lengthy / 2.0f;
            temp_vector.Z = z_ + lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + lengthx / 2.0f;
            temp_vector.Y = y_ + lengthy / 2.0f;
            temp_vector.Z = z_ + lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - lengthx / 2.0f;
            temp_vector.Y = y_ - lengthy / 2.0f;
            temp_vector.Z = z_ + lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + lengthx / 2.0f;
            temp_vector.Y = y_ - lengthy / 2.0f;
            temp_vector.Z = z_ + lengthz / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };
            _vertices = tempVertices;
            _indices = tempIndices;
        }


        public void createBoxVertices(float x, float y, float z, float length)
        {
            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);

            _indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void createEllipsoid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * (float)Math.PI / sectorCount;
            float stackStep = (float)Math.PI / stackCount;
            float sectorAngle, StackAngle, x, y, z;
            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;
            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x * (float)Math.Cos(sectorAngle) + _x;
                    temp_vector.Y = y * (float)Math.Sin(sectorAngle) + _y;
                    if (temp_vector.Y > _y)
                    {
                        temp_vector.Y = _y;
                    }
                    temp_vector.Z = z + _z;
                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }
        public void createEllipsoid2(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * (float)Math.PI / sectorCount;
            float stackStep = (float)Math.PI / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x * (float)Math.Cos(sectorAngle) + _x;
                    temp_vector.Y = y * (float)Math.Sin(sectorAngle) + _y;
                    temp_vector.Z = z + _z;
                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void resize2()
        {
            _model *= Matrix4.CreateScale(2f, 2f, 2f);


            foreach (var i in child)
            {
                i.resize2();
            }
        }

        public void createCone(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            _centerPosition = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Cos(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }
        public void resize(float x, float y, float z)
        {
            _model *= Matrix4.CreateScale(x, y, z);


            foreach (var i in child)
            {
                i.resize(x, y, z);
            }
        }

        public void AddCoords(float x, float y, float z)
        {
            _bezier_vertice.Add(new Vector3(x, y, z));

        }

        public Vector3 getSegment(float time, int numberofcoords)
        {
            time = Math.Clamp(time, 0, 1);
            float times = 1 - time;
            //Vector3 result =
            //    ((float)Math.Pow(times, 3) * _bezier_vertice[0] +
            //    (3 * time * times * times * _bezier_vertice[1]) +
            //    (3 * time * time * times * _bezier_vertice[2]) +
            //    (time * time * time * _bezier_vertice[3])
            //    );
            Vector3 result = (float)Math.Pow(times, numberofcoords - 1) * (float)Math.Pow(time, 0) * _bezier_vertice[0];
            for (int i = 1, n = numberofcoords; i < n; i++)
            {
                result += nCr(numberofcoords - 1, i) * (float)Math.Pow(times, numberofcoords - i - 1) * (float)Math.Pow(time, i) * _bezier_vertice[i];
            }

            return result;
        }

        public void Bezier(int numberofcoords)
        {
            List<Vector3> points = new List<Vector3>();


            for (float i = 0; i < 1f; i += 0.01f)
            {
                points.Add(getSegment(i, numberofcoords));

            }
            _vertices = points;
        }

        public static long nCr(int n, int r)
        {
            // naive: return Factorial(n) / (Factorial(r) * Factorial(n - r));
            return nPr(n, r) / Factorial(r);
        }

        public static long nPr(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivision(n, n - r);
        }

        private static long FactorialDivision(int topFactorial, int divisorFactorial)
        {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private static long Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }

        public void createSemiEllipsoid2(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount)
        {
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = (float)Math.PI / sectorCount;
            float stackStep = (float)Math.PI / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = z;
                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createLeftCone(float radX, float radY, float radZ, float x, float y, float z, float sectorCount, float stackCount)
        {
            _centerPosition = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Cos(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX;
                    temp_vector.Y = y + tempY * (float)Math.Cos(sectorAngle);
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    _vertices.Add(temp_vector);
                }
            }
        }

        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            var radAngle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
                Vector4.UnitW
                );

            _model *= Matrix4.CreateTranslation(-pivot);
            _model *= arbRotationMatrix;
            _model *= Matrix4.CreateTranslation(pivot);

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
            }

            _centerPosition = getRotationResult(pivot, vector, radAngle, _centerPosition);


            foreach (var i in child)
            {
                i.rotate(pivot, vector, angle);
            }
        }

        public void translate(float x, float y, float z)
        {
            _model *= Matrix4.CreateTranslation(x, y, z);
            _centerPosition.X += x;
            _centerPosition.Y += y;
            _centerPosition.Z += z;

            foreach (var i in child)
            {
                i.translate(x, y, z);
            }
        }

        //public void translate(Vector3 pivot, Vector3 vector, float angle)
        //{
        //    var radAngle = MathHelper.DegreesToRadians(angle);

        //    var arbRotationMatrix = new Matrix4
        //        (
        //        new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
        //        new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
        //        new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
        //        Vector4.UnitW
        //        );

        //    _model *= Matrix4.CreateTranslation(-pivot);
        //    _model *= arbRotationMatrix;
        //    _model *= Matrix4.CreateTranslation(pivot);

        //    for (int i = 0; i < 3; i++)
        //    {
        //        _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
        //    }

        //    _centerPosition = getRotationResult(pivot, vector, radAngle, _centerPosition);


        //    foreach (var i in child)
        //    {
        //        i.rotate(pivot, vector, angle);
        //    }
        //}


        Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;
            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                (float)temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                (float)temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));
            newPosition.Y =
                (float)temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                (float)temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));
            newPosition.Z =
                (float)temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                (float)temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                (float)temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }
    }
}