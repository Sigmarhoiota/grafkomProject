using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Console1
{
    static class Warna
    {
        public const string col = "../../../shader/";
    }
    internal class Window : GameWindow
    {
        Shader _shader;
        List<Asset_3D> _object = new List<Asset_3D>();
        Asset_3D[] _object3d = new Asset_3D[50];
        Camera _camera;
        bool _firstMove = true;
        Vector2 _lastposition;
        Vector3 _objectposition = new Vector3(0.0f, 0.0f, 0.0f);
        float _rotationSpeed = 0.2f;
        float _time;
        float degr = 0;
        float countSpb = 0;
        bool invertSpb = false;
        float walkSpb = 0;
        float counterP = 0;
        bool reverseP = false;
        float howmanyP = 0;
        int counterR = 0;
        bool pauseP = false;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            //Spongebob
            var kubus = new Asset_3D(new Vector3(0.8f, 0.8f, 0f));
            var badan = new Asset_3D(new Vector3(0.8f, 0.8f, 0f));
            var tangankanan = new Asset_3D(new Vector3(0.8f, 0.8f, 0f));
            var tangankiri = new Asset_3D(new Vector3(0.8f, 0.8f, 0f));
            var bahukanan = new Asset_3D(new Vector3(1,1,1));
            var bahukiri = new Asset_3D(new Vector3(1,1,1));
            var matakanan = new Asset_3D(new Vector3(1,1,1));
            var matakiri = new Asset_3D(new Vector3(1,1,1));
            var pupilkanan = new Asset_3D(new Vector3(0,0,0));
            var pupilkiri = new Asset_3D(new Vector3(0,0,0));
            var mulut = new Asset_3D(new Vector3(0.6f, 0, 0));
            var baju = new Asset_3D(new Vector3(1, 1, 1));
            var celana = new Asset_3D(new Vector3(0.4f, 0.4f, 0));
            var kakikanan = new Asset_3D(new Vector3(0.8f, 0.8f, 0f));
            var kakikiri = new Asset_3D(new Vector3(0.8f, 0.8f, 0f));
            var kaoskakikanan = new Asset_3D(new Vector3(1f, 1f, 1f));
            var kaoskakikiri = new Asset_3D(new Vector3(1f, 1f, 1f));
            var sepatukanan = new Asset_3D(new Vector3(0f, 0f, 0f));
            var sepatukiri = new Asset_3D(new Vector3(0f, 0f, 0f));
            var hidung = new Asset_3D(new Vector3(0.8f, 0.8f, 0));
            var outlinehidung = new Asset_3D(new Vector3(0, 0, 0));
            var outlinehidung2 = new Asset_3D(new Vector3(0, 0, 0));


            kubus.createCuboid(45, 0, 0, 1, 0.5f, 1.5f);
            badan.createCuboid(45, -1.1f, 0, 2f, 2.75f, 1.5f);
            tangankanan.createCuboid(-2f, -46.5f, 0, 2, 0.5f, 0.5f);
            tangankanan.rotate(badan._centerPosition, tangankanan._euler[2], 90f);
            tangankiri.createCuboid(-2f, -43.5f, 0, 2, 0.5f, 0.5f);
            tangankiri.rotate(badan._centerPosition, tangankiri._euler[2], 90f);
            bahukanan.createCone(-46.5f, 0.6f, 0, 0.5f, 0.5f, 0.5f, 72, 24);
            bahukanan.rotate(badan._centerPosition, bahukanan._euler[2], 180f);
            bahukiri.createCone(-43.5f, 0.6f, 0, 0.5f, 0.5f, 0.5f, 72, 24);
            bahukiri.rotate(badan._centerPosition, bahukiri._euler[2], 180f);
            matakanan.createEllipsoid(0.3f, 0.3f, 0.5f, 45.4f, -0.75f, -0.5f, 72, 24);
            matakanan.rotate(badan._centerPosition, matakanan._euler[0], 270f);
            matakiri.createEllipsoid(0.3f, 0.3f, 0.5f, 44.6f, -0.75f, -0.5f, 72, 24);
            matakiri.rotate(badan._centerPosition, matakiri._euler[0], 270f);
            pupilkanan.createEllipsoid(0.1f, 0.1f, 0.2f, 45.3f, -1f, -0.5f, 72, 24);
            pupilkanan.rotate(badan._centerPosition, pupilkanan._euler[0], 270f);
            pupilkiri.createEllipsoid(0.1f, 0.1f, 0.2f, 44.7f, -1f, -0.5f, 72, 24);
            pupilkiri.rotate(badan._centerPosition, pupilkiri._euler[0], 270f);
            mulut.createEllipsoid(0.6f, 0.3f, 0.4f, 45f, 0.8f, 1.4f, 72, 24);
            mulut.rotate(badan._centerPosition, mulut._euler[0], 90f);
            baju.createCuboid(45, -2.6f, 0, 2, 0.25f, 1.5f);
            celana.createCuboid(45, -2.975f, 0, 2, 0.5f, 1.5f);
            kakikanan.createCuboid(-3.5f, -45.5f, 0, 0.5f, 0.5f, 0.5f);
            kakikanan.rotate(badan._centerPosition, kakikanan._euler[2], 90f);
            kakikiri.createCuboid(-3.5f, -44.5f, 0, 0.5f, 0.5f, 0.5f);
            kakikiri.rotate(badan._centerPosition, kakikiri._euler[2], 90f);
            kaoskakikanan.createCuboid(-4.25f, -45.5f, 0, 1, 0.5f, 0.5f);
            kaoskakikanan.rotate(badan._centerPosition, kaoskakikanan._euler[2], 90f);
            kaoskakikiri.createCuboid(-4.25f, -44.5f, 0, 1, 0.5f, 0.5f);
            kaoskakikiri.rotate(badan._centerPosition, kaoskakikiri._euler[2], 90f);
            sepatukanan.createCuboid(-5f, -45.5f, 0.25f, 0.5f, 0.5f, 1f);
            sepatukanan.rotate(badan._centerPosition, sepatukanan._euler[2], 90f);
            sepatukiri.createCuboid(-5f, -44.5f, 0.25f, 0.5f, 0.5f, 1f);
            sepatukiri.rotate(badan._centerPosition, sepatukiri._euler[2], 90f);
            hidung.createEllipsoid(0.1f, 0.2f, 0.4f, 45, -0.7f, 1f, 72, 24);

            outlinehidung.AddCoords(45, -0.7f, 0.6f);
            outlinehidung.AddCoords(45, -0.7f, 1.4f);

            outlinehidung.Bezier(2);

            outlinehidung2.AddCoords(45, -0.88f, 0.8f);
            outlinehidung2.AddCoords(45, -0.9f, 0.9f);
            outlinehidung2.AddCoords(45, -0.9f, 1f);
            outlinehidung2.AddCoords(45, -0.9f, 1.1f);
            outlinehidung2.AddCoords(45, -0.9f, 1.2f);
            outlinehidung2.AddCoords(45, -0.88f, 1.3f);
            outlinehidung2.AddCoords(45, -0.84f, 1.4f);
            outlinehidung2.AddCoords(45, -0.76f, 1.4f);
            outlinehidung2.AddCoords(45, -0.72f, 1.4f);
            outlinehidung2.AddCoords(45, -0.7f, 1.4f);

            outlinehidung2.Bezier(10);  

            _object.Add(kubus);

            _object[0].child.Add(badan);        //0
            _object[0].child.Add(tangankanan);  //1
            _object[0].child.Add(tangankiri);   //2
            _object[0].child.Add(bahukanan);    //3
            _object[0].child.Add(bahukiri);     //4
            _object[0].child.Add(matakanan);    //5
            _object[0].child.Add(matakiri);     //6
            _object[0].child.Add(pupilkanan);   //7
            _object[0].child.Add(pupilkiri);    //8
            _object[0].child.Add(mulut);        //9
            _object[0].child.Add(baju);         //10
            _object[0].child.Add(celana);       //11
            _object[0].child.Add(kakikanan);    //12
            _object[0].child.Add(kakikiri);     //13
            _object[0].child.Add(kaoskakikanan);//14
            _object[0].child.Add(kaoskakikiri); //15
            _object[0].child.Add(sepatukanan);  //16
            _object[0].child.Add(sepatukiri);   //17
            _object[0].child.Add(hidung);       //18
            _object[0].child.Add(outlinehidung);//19
            _object[0].child.Add(outlinehidung2);//20




            //Patrick
            var coreP = new Asset_3D(new Vector3(1f, 0.5f, 0.54f));
            var leherP = new Asset_3D(new Vector3(0f, 0.5f, 0.54f));
            var badanP = new Asset_3D(new Vector3(1f, 0.5f, 0.54f));
            var tangankananP = new Asset_3D(new Vector3(1f, 0.5f, 0.54f));
            var tangankiriP = new Asset_3D(new Vector3(1f, 0.5f, 0.54f));
            var celanaP = new Asset_3D(new Vector3(0f, 1f, 0.54f));
            var kakikananP = new Asset_3D(new Vector3(1f, 0.5f, 0.54f));
            var kakikiriP = new Asset_3D(new Vector3(1f, 0.5f, 0.54f));

            var matakananP = new Asset_3D(new Vector3(0.968f, 0.482f, 0.901f));
            var isimatakananP = new Asset_3D(new Vector3(1f, 1f, 1f));
            var bolamatakananP = new Asset_3D(new Vector3(0f, 0f, 0f));

            var matakiriP = new Asset_3D(new Vector3(0.968f, 0.482f, 0.901f));
            var isimatakiriP = new Asset_3D(new Vector3(1f, 1f, 1f));
            var bolamatakiriP = new Asset_3D(new Vector3(0f, 0f, 0f));

            var mulutP = new Asset_3D(new Vector3(0f, 0f, 0f));

            var bellyP = new Asset_3D(new Vector3(0f, 0f, 0f));


            var alisP = new Asset_3D(new Vector3(0f, 0f, 0f));
            var alis2P = new Asset_3D(new Vector3(0, 0f, 0f));

            coreP.createCone(0, -3.5f, 0, 2f, 6f, 2f, 72, 24);
            coreP.rotate(badanP._centerPosition, tangankananP._euler[2], 180f);

            tangankananP.createCone(-1f, -4f, 0, 1f, 4, 1.5f, 72, 24);
            tangankananP.rotate(badanP._centerPosition, tangankananP._euler[2], 90f);

            tangankiriP.createCone(1f, -4f, 0, 1f, 4, 1.5f, 72, 24);
            tangankiriP.rotate(badanP._centerPosition, tangankiriP._euler[2], 270f);

            celanaP.createEllipsoid(2f, 2f, 2f, 0, -2.5f, 0, 72, 24);

            kakikananP.createCone(-1f, -6.5f, 0.5f, 1f, 4f, 1, 72, 24);
            kakikananP.rotate(badanP._centerPosition, kakikananP._euler[2], 30f);

            kakikiriP.createCone(1f, -6.5f, 0.5f, 1f, 4f, 1, 72, 24);
            kakikiriP.rotate(badanP._centerPosition, kakikiriP._euler[2], 330f);

            matakananP.createEllipsoid2(0.3f, 0.5f, 0.3f, 0.2f, 1.75f, 0.4f, 72, 24);
            isimatakananP.createEllipsoid(0.3f, 0.5f, 0.3f, 0.2f, 1.75f, 0.4f, 72, 24);
            bolamatakananP.createEllipsoid(0.1f, 0.3f, 0.1f, 0.2f, 1.75f, 0.62f, 72, 24);

            matakiriP.createEllipsoid2(0.3f, 0.5f, 0.3f, -0.2f, 1.75f, 0.4f, 72, 24);
            isimatakiriP.createEllipsoid(0.3f, 0.5f, 0.3f, -0.2f, 1.75f, 0.4f, 72, 24);
            bolamatakiriP.createEllipsoid(0.1f, 0.3f, 0.1f, -0.2f, 1.75f, 0.62f, 72, 24);

            mulutP.createEllipsoid(0.75f, 1.15f, 0.5f, 0f, 0.75f, 0.75f, 72, 24);
            mulutP.rotate(badanP._centerPosition, mulutP._euler[0], -30f);

            bellyP.createCuboid(0, -2f, 1.75f, 0.2f, 0.2f, 0.2f);

            alisP.AddCoords(-0.3f, 3f, 0.05f);
            alisP.AddCoords(-0.22f, 3f, 0.1f);
            alisP.AddCoords(-0.15f, 3f, 0.15f);
            alisP.AddCoords(-0.10f, 3f, 0.2f);
            alisP.AddCoords(-0.05f, 3f, 0.2f);

            alisP.Bezier(5);

            alis2P.AddCoords(0.3f, 3f, 0.05f);
            alis2P.AddCoords(0.22f, 3f, 0.1f);
            alis2P.AddCoords(0.15f, 3f, 0.15f);
            alis2P.AddCoords(0.10f, 3f, 0.2f);
            alis2P.AddCoords(0.05f, 3f, 0.2f);

            alis2P.Bezier(5);

            _object.Add(coreP);
            _object.Add(alisP);
            _object.Add(alis2P);

            _object[1].child.Add(leherP); //0
            _object[1].child.Add(badanP); //1
            _object[1].child.Add(tangankananP); //2
            _object[1].child.Add(tangankiriP); //3
            _object[1].child.Add(celanaP); //4
            _object[1].child.Add(kakikananP); //5
            _object[1].child.Add(kakikiriP); //6
            _object[1].child.Add(bolamatakananP); //7
            _object[1].child.Add(bolamatakiriP); //8
            _object[1].child.Add(isimatakananP); //9
            _object[1].child.Add(isimatakiriP); //10
            _object[1].child.Add(matakananP); //11
            _object[1].child.Add(matakiriP); //12
            _object[1].child.Add(mulutP); //13
            _object[1].child.Add(bellyP); //14
            _object[1].child.Add(alisP); //15
            _object[1].child.Add(alis2P); //16

            //Rumah Patrick
            _object3d[0] = new Asset_3D(new Vector3(0.4f, 0.26f, 0.13f));
            _object3d[0].createEllipsoid(15f, 10f, 10f, 0.0f, -10f, -20f, 72, 24); //rumah batu
            _object3d[0].rotate(_object3d[0]._centerPosition, _object3d[0]._euler[0], 180f);

            _object3d[1] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[1].createBoxVertices(0, .5f, -20f, 1f); //batang sign

            _object3d[2] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[2].createBoxVertices(0, 1.5f, -20f, 1f);//batang sign

            _object3d[3] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[3].createBoxVertices(0, 2.5f, -20f, 1f);//batang sign

            _object3d[4] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[4].createBoxVertices(0, 3.5f, -20f, 1f);//batang sign

            _object3d[5] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[5].createBoxVertices(0, 4.5f, -20f, 1f);//batang sign

            _object3d[6] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[6].createCone(-5f, 4.5f, -20f, 1f, 1.5f, 1.5f, 7, 300);//batang sign
            _object3d[6].rotate(_object3d[6]._centerPosition, _object3d[6]._euler[2], 270f);


            _object3d[7] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[7].createBoxVertices(-1f, 4.5f, -20f, 1f);//batang sign

            _object3d[8] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[8].createBoxVertices(-2f, 4.5f, -20f, 1f);//batang sign

            _object3d[9] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[9].createBoxVertices(-3f, 4.5f, -20f, 1f);//batang sign

            _object3d[10] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[10].createBoxVertices(1f, 4.5f, -20f, 1f);//batang sign

            _object3d[11] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[11].createBoxVertices(2f, 4.5f, -20f, 1f);//batang sign

            _object3d[12] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[12].createBoxVertices(3f, 4.5f, -20f, 1f);//batang sign

            _object3d[13] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[13].createBoxVertices(1f, 5.5f, -20f, 1f);//batang sign

            _object3d[14] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[14].createBoxVertices(1.5f, 5f, -20f, 1f);//batang sign

            _object3d[15] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[15].createBoxVertices(2.5f, 4f, -20f, 1f);//batang sign

            _object3d[16] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[16].createBoxVertices(3f, 3.5f, -20f, 1f);//batang sign

            _object3d[17] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[17].createBoxVertices(4f, 4.5f, -20f, 1f);//batang sign

            _object3d[18] = new Asset_3D(new Vector3(0.99f, 0.81f, 0.1f));
            _object3d[18].createBoxVertices(15f, -10.0f, -20f, 0.08f);//katup

            _object3d[19] = new Asset_3D(new Vector3(1f, 0f, 0f)); //X mark
            _object3d[19].AddCoords(0, -5f , -11.3f);
            _object3d[19].AddCoords(1, -4.75f, -11.3f);
            _object3d[19].AddCoords(2, -4.5f, -11.6f);
            _object3d[19].AddCoords(3, -4.25f, -11.7f);
            _object3d[19].AddCoords(4, -4f, -12.2f);

            _object3d[19].Bezier(5);

            _object3d[20] = new Asset_3D(new Vector3(1f, 0f, 0f)); //X mark
            _object3d[20].AddCoords(0, -4f, -12f);
            _object3d[20].AddCoords(1, -4.25f, -11.7f);
            _object3d[20].AddCoords(2, -4.5f, -11.6f);
            _object3d[20].AddCoords(3, -4.75f, -11.3f);
            _object3d[20].AddCoords(4, -5f, -11.3f);

            _object3d[20].Bezier(5);

            //_object3d[20] = new Asset_3D(new Vector3(1f, 0f, 0f)); //X mark


            //Gary
            _object3d[21] = new Asset_3D(new Vector3(1f, 0.5f, 0.54f));
            _object3d[21].createEllipsoid(2f, 3f, 2.5f, 28f, -5f, -.8f, 72, 24); //cangkang
            _object3d[21].rotate(_object3d[21]._centerPosition, _object3d[0]._euler[0], 180f);
            _object3d[22] = new Asset_3D(new Vector3(0.313f, 0.886f, 0.815f));
            _object3d[22].createEllipsoid2(2f, 4f, 1f, 28f, 0, 5.5f, 72, 24); //badan
            _object3d[22].rotate(_object3d[22]._centerPosition, _object3d[0]._euler[0], 90f);
            _object3d[23] = new Asset_3D(new Vector3(0.313f, 0.886f, 0.815f));
            _object3d[23].createEllipsoid2(0.2f, 1f, 0.2f, 27.5f, -4.5f, 3f, 72, 24); //batang mata kiri
            _object3d[24] = new Asset_3D(new Vector3(0.313f, 0.886f, 0.815f));
            _object3d[24].createEllipsoid2(0.2f, 1f, 0.2f, 28.5f, -4.5f, 3f, 72, 24); //batang mata kanan
            _object3d[25] = new Asset_3D(new Vector3(1f, 1f, 1f));
            _object3d[25].createEllipsoid2(0.5f, 0.5f, 0.5f, 28.5f, -3.5f, 3f, 72, 24); //mata kanan
            _object3d[26] = new Asset_3D(new Vector3(1f, 1f, 1f));
            _object3d[26].createEllipsoid2(0.5f, 0.5f, 0.5f, 27.5f, -3.5f, 3f, 72, 24); //mata kiri
            _object3d[27] = new Asset_3D(new Vector3(0f, 0f, 0f));
            _object3d[27].createEllipsoid2(0.2f, 0.2f, 0.2f, 28.5f, -3.5f, 3.4f, 72, 24); //dalam mata kanan
            _object3d[28] = new Asset_3D(new Vector3(0f, 0f, 0f));
            _object3d[28].createEllipsoid2(0.2f, 0.2f, 0.2f, 27.5f, -3.5f, 3.4f, 72, 24); //dalam mata kiri

            //Environment
            var env = new Asset_3D(new Vector3(0, 0, 0));
            var sand = new Asset_3D(new Vector3(0.761f, 0.698f, 0.502f));
            sand.createCuboid(25, -6.1f, 0, 100, 0.1f, 100);
            var seaL = new Asset_3D(new Vector3(0, 0.412f, 0.58f));
            seaL.createCuboid(20f, 0, 0, 0.1f, 25, 100);
            var seaB = new Asset_3D(new Vector3(0, 0.412f, 0.58f));
            seaB.createCuboid(25, 0, -35, 100, 25, 0.1f);
            var seaR = new Asset_3D(new Vector3(0, 0.412f, 0.58f));
            seaR.createCuboid(-20f, 0, 0, 0.1f, 25, 100);
            var seaL2 = new Asset_3D(new Vector3(0, 0.412f, 0.58f));
            seaL2.createCuboid(65, 0, 0, 0.1f, 25, 100);
            _object.Add(env);
            _object[4].child.Add(sand);
            _object[4].child.Add(seaL);
            _object[4].child.Add(seaB);
            _object[4].child.Add(seaR);
            _object[4].child.Add(seaL2);

            //Trial

            //var torus = new Asset_3D(new Vector3(0, 0, 0));
            //torus.createTorus(0, 0, 0, 5, 3, 72, 24);
            //_object.Add(torus);



            //for (var i = 0; i < _object.Count; i++)
            //{
            //    _object[i].resize2();
            //}




            //_object[0].resize2();
            //_object[1].resize2();
            //_object[2].resize2();
            //_object[3].resize2();
            foreach (var i in _object)
            {
                i.OnLoad(Warna.col + "shader.vert", Warna.col + "shader.frag", Size.X, Size.Y);
            }
            _object3d[0].OnLoad(Warna.col + "shader.vert", Warna.col + "shader.frag", Size.X, Size.Y);
            for (var i=1; i<29; i++)
            {
                _object3d[i].OnLoad(Warna.col + "shader1.vert", Warna.col + "shader.frag", Size.X, Size.Y);
            }


            _camera = new Camera(new Vector3(0, 0, 5), Size.X / Size.Y);
            CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _time = (float)args.Time;
            _object[0].child[19].OnRender(1, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            _object[0].child[20].OnRender(1, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            _object[0].OnRender(3, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            _object[1].OnRender(3, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            _object[2].OnRender(1, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            _object[3].OnRender(1, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            _object[4].OnRender(3, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            //trial
            //_object[5].OnRender(3, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());


            for (var i=0; i<29; i++)
            {
                if (i == 6)
                {
                    _object3d[i].OnRender(1, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                }
                if (i == 19 || i == 20)
                {
                    _object3d[i].OnRender(1, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                }
                else
                {
                    _object3d[i].OnRender(3, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                }
                _object3d[i].resetEuler();
            }

            float constanta = 200;
            float aniP = 3000;
            float speed = 0.001f;
            if (!pauseP)
            {
                if (howmanyP < aniP)
                {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }

                    _object[1].translate(0, 0, speed * 2);

                }
                else if (howmanyP < aniP * 2) {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }
                    if ((howmanyP >= aniP + 0.1 * aniP) && (howmanyP <= 2 *aniP - 0.1 * aniP)) {
                        _object[1].translate(-speed, 0, speed);
                    }
                    _object[1].rotate(_object[1]._centerPosition, _object[1]._euler[1], 90/aniP);

                }
                else if (howmanyP < aniP * 3)
                {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }

                    _object[1].translate(-speed, 0, 0.000f);

                }
                else if (howmanyP < aniP * 4)
                {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }
                    if ((howmanyP >= 3 * aniP + 0.1 * aniP) && (howmanyP <= 4 * aniP - 0.1 * aniP))
                    {
                        _object[1].translate(-speed, 0, -speed);
                    }
                    _object[1].rotate(_object[1]._centerPosition, _object[1]._euler[1], 90 / aniP);

                }
                else if (howmanyP < aniP * 5)
                {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }

                    _object[1].translate(0, 0, -speed);
                }
                else if (howmanyP < aniP * 6)
                {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }
                    if ((howmanyP >= 5 * aniP + 0.1 * aniP) && (howmanyP <= 6 * aniP - 0.1 * aniP))
                    {
                        _object[1].translate(speed, 0, -speed);
                    }
                    _object[1].rotate(_object[1]._centerPosition, _object[1]._euler[1], 90 / aniP);

                }
                else if (howmanyP < aniP * 7)
                {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }

                    _object[1].translate(speed, 0, 0.000f);

                }
                else if (howmanyP < aniP * 8)
                {
                    howmanyP += 1;

                    if (counterP < constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.075f);


                    }
                    if (counterP > constanta)
                    {
                        _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.03f);
                        _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.03f);

                        _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.075f);
                        _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.075f);

                    }
                    if (counterP == 2 * constanta)
                    {
                        reverseP = true;
                    }
                    if (counterP == 0)
                    {
                        reverseP = false;
                    }
                    if (!reverseP)
                    {
                        counterP += 1;
                    }
                    else
                    {
                        counterP -= 1;
                    }
                    if ((howmanyP >= 7 * aniP + 0.1 * aniP) && (howmanyP <= 8 * aniP - 0.1 * aniP))
                    {
                        _object[1].translate(speed, 0, speed);
                    }
                    _object[1].rotate(_object[1]._centerPosition, _object[1]._euler[1], 90 / aniP);

                }

                if (howmanyP == 8 * aniP)
                {
                    howmanyP = 0;
                }
            }
            //if (howmanyP >= 8000 && howmanyP < 9000)
            //{
            //    howmanyP += 1;

            //    _object[1].resize(1.0001f, 1.0001f, 1.0001f);
            //}
            //if (howmanyP >= 9000 && howmanyP < 10000)
            //{
            //    howmanyP += 1;

            //    _object[1].resize(0.9999f, 0.9999f, 0.9999f);
            //}

            if (counterR < 8000 && !pauseP)
            {
                
                _object3d[0].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[1].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[2].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[3].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[4].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[5].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[6].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[7].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[8].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[9].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[10].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[11].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[12].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[13].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[14].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[15].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[16].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[17].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[19].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);
                _object3d[20].rotate(new Vector3(-0.5f, 0.04f, 1f), _object3d[18]._euler[2], 0.01f);

                counterR++;
            };

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            Console.WriteLine("ini Resize");
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            var input = KeyboardState;
            if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
            float cameraSpeed = 5f;

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.T))
            {
                _object[0].child[1].rotate(_object[0].child[0]._centerPosition, _object[0].child[1]._euler[1], 1f);
                _object[0].child[3].rotate(_object[0].child[0]._centerPosition, _object[0].child[1]._euler[1], 1f);
                _object[0].child[2].rotate(_object[0].child[0]._centerPosition, _object[0].child[1]._euler[1], 1f);
                _object[0].child[4].rotate(_object[0].child[0]._centerPosition, _object[0].child[1]._euler[1], 1f);
            }
            
            if (KeyboardState.IsKeyDown(Keys.Y))
            {
                if (!invertSpb)
                {
                    _object[0].child[1].rotate(_object[0].child[0]._centerPosition, _object[0].child[1]._euler[1], 0.1f);
                    _object[0].child[3].rotate(_object[0].child[0]._centerPosition, _object[0].child[3]._euler[1], 0.003f);
                    _object[0].child[2].rotate(_object[0].child[0]._centerPosition, _object[0].child[2]._euler[1], -0.1f);
                    _object[0].child[4].rotate(_object[0].child[0]._centerPosition, _object[0].child[4]._euler[1], -0.003f);
                    _object[0].child[13].rotate(_object[0].child[0]._centerPosition, _object[0].child[13]._euler[1], 0.1f);
                    _object[0].child[15].rotate(_object[0].child[0]._centerPosition, _object[0].child[15]._euler[1], 0.1f);
                    _object[0].child[17].rotate(_object[0].child[0]._centerPosition, _object[0].child[17]._euler[1], 0.1f);
                    _object[0].child[12].rotate(_object[0].child[0]._centerPosition, _object[0].child[12]._euler[1], -0.1f);
                    _object[0].child[14].rotate(_object[0].child[0]._centerPosition, _object[0].child[14]._euler[1], -0.1f);
                    _object[0].child[16].rotate(_object[0].child[0]._centerPosition, _object[0].child[16]._euler[1], -0.1f);
                    countSpb++;
                }
                else
                {
                    _object[0].child[1].rotate(_object[0].child[0]._centerPosition, _object[0].child[1]._euler[1], -0.1f);
                    _object[0].child[3].rotate(_object[0].child[0]._centerPosition, _object[0].child[3]._euler[1], -0.003f);
                    _object[0].child[2].rotate(_object[0].child[0]._centerPosition, _object[0].child[2]._euler[1], 0.1f);
                    _object[0].child[4].rotate(_object[0].child[0]._centerPosition, _object[0].child[4]._euler[1], 0.003f);
                    _object[0].child[13].rotate(_object[0].child[0]._centerPosition, _object[0].child[13]._euler[1], -0.1f);
                    _object[0].child[15].rotate(_object[0].child[0]._centerPosition, _object[0].child[15]._euler[1], -0.1f);
                    _object[0].child[17].rotate(_object[0].child[0]._centerPosition, _object[0].child[17]._euler[1], -0.1f);
                    _object[0].child[12].rotate(_object[0].child[0]._centerPosition, _object[0].child[12]._euler[1], 0.1f);
                    _object[0].child[14].rotate(_object[0].child[0]._centerPosition, _object[0].child[14]._euler[1], 0.1f);
                    _object[0].child[16].rotate(_object[0].child[0]._centerPosition, _object[0].child[16]._euler[1], 0.1f);
                    countSpb--;
                }
                if (countSpb == 100)
                {
                    invertSpb = true;
                }
                else if (countSpb == -100)
                {
                    invertSpb = false;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.U))
            {
                _object[1].resize(1.0005f, 1.0005f, 1.0005f);
            }
            if (KeyboardState.IsKeyDown(Keys.I))
            {
                _object[1].resize(0.9995f, 0.9995f, 0.9995f);
            }

            if (KeyboardState.IsKeyPressed(Keys.P)) { 
                if (pauseP)
                {
                    pauseP = false;
                }
                else { pauseP = true; }
            }
            if (KeyboardState.IsKeyDown(Keys.N))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectposition;
                _camera.Position = Vector3.Transform(
                    _camera.Position,
                    generateArbRotationMatrix(axis, _rotationSpeed)
                    .ExtractRotation());
                _camera.Position += _objectposition;
                _camera._front = -Vector3.Normalize(_camera.Position
                    - _objectposition);
            }
            if (KeyboardState.IsKeyDown(Keys.Comma))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectposition;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, -_rotationSpeed)
                    .ExtractRotation());
                _camera.Position += _objectposition;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectposition);
            }
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectposition;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _rotationSpeed).ExtractRotation());
                _camera.Position += _objectposition;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectposition);
            }
            if (KeyboardState.IsKeyDown(Keys.M))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectposition;
                _camera.Pitch += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position, generateArbRotationMatrix(axis, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectposition;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectposition);
            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                _object[0].translate(0, 0, 0.01f);

                float constanta = 200;
                if (counterP < constanta)
                {
                    _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.02f);
                    _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.02f);

                    _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.1f);
                    _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.1f);
                }
                if (counterP > constanta)
                {
                    _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.02f);
                    _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.02f);

                    _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.1f);
                    _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.1f);
                }
                if (counterP == 2 * constanta)
                {
                    reverseP = true;
                }
                if (counterP == 0)
                {
                    reverseP = false;
                }
                if (!reverseP)
                {
                    counterP += 1;
                }
                else
                {
                    counterP -= 1;
                }
                for (int i = 21; i <= 28; i++)
                {
                    _object3d[i].translate(0, 0, 0.01f);
                }

                _object[1].translate(0, 0, 0.002f);
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                _object[0].translate(0, 0, -0.01f);

                float constanta = 200;
                if (counterP < constanta)
                {
                    _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], 0.02f);
                    _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], -0.02f);

                    _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], 0.1f);
                    _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], -0.1f);
                }
                if (counterP > constanta)
                {
                    _object[1].child[2].rotate(_object[1]._centerPosition, _object[1].child[2]._euler[0], -0.02f);
                    _object[1].child[3].rotate(_object[1]._centerPosition, _object[1].child[3]._euler[0], 0.02f);

                    _object[1].child[5].rotate(_object[1].child[4]._centerPosition, _object[1].child[5]._euler[0], -0.1f);
                    _object[1].child[6].rotate(_object[1].child[4]._centerPosition, _object[1].child[6]._euler[0], 0.1f);
                }
                if (counterP == 2 * constanta)
                {
                    reverseP = true;
                }
                if (counterP == 0)
                {
                    reverseP = false;
                }
                if (!reverseP)
                {
                    counterP += 1;
                }
                else
                {
                    counterP -= 1;
                }
                for (int i = 21; i <= 28; i++)
                {
                    _object3d[i].translate(0, 0, -0.01f);
                }

                _object[1].translate(0, 0, -0.002f);
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                _object[0].translate(-0.01f, 0, 0);

                for(int i = 21; i <= 28; i++)
                {
                    _object3d[i].translate(-0.01f, 0, 0);
                }
            }
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                _object[0].translate(0.01f, 0, 0);
                for (int i = 21; i <= 28; i++)
                {
                    _object3d[i].translate(0.01f, 0, 0);
                }
            }
            
            var mouse = MouseState;
            var sensitivity = 0.5f;

            if (_firstMove == true)
            {
                _lastposition = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastposition.X;
                var deltaY = mouse.Y - _lastposition.Y;
                _lastposition = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X / 2) / (Size.X / 2);
                float _y = -(MousePosition.Y - Size.Y / 2) / (Size.Y / 2);

                Console.WriteLine("X= " + _x + " y =" + _y);


            }
        }
        public Matrix4 generateArbRotationMatrix(Vector3 axis, float angle)
        {
            angle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4(
                (float)Math.Cos(angle) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(angle)), axis.X * axis.Y * (1 - (float)Math.Cos(angle)) - axis.Z * (float)Math.Sin(angle), axis.X * axis.Z * (1 - (float)Math.Cos(angle)) + axis.Y * (float)Math.Sin(angle), 0,
                axis.Y * axis.X * (1 - (float)Math.Cos(angle)) + axis.Z * (float)Math.Sin(angle), (float)Math.Cos(angle) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(angle)), axis.Y * axis.Z * (1 - (float)Math.Cos(angle)) - axis.X * (float)Math.Sin(angle), 0,
                axis.Z * axis.X * (1 - (float)Math.Cos(angle)) - axis.Y * (float)Math.Sin(angle), axis.Z * axis.Y * (1 - (float)Math.Cos(angle)) + axis.X * (float)Math.Sin(angle), (float)Math.Cos(angle) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(angle)), 0,
0, 0, 0, 1
                );

            return arbRotationMatrix;

        }
    }
}
