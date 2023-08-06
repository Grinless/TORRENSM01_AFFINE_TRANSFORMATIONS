using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MatrixLib;
using static UnityEditor.PlayerSettings;
using static MatrixLib.Matrix2X2;

public class UTest_Vec3D
{
    [Test]
    public void UTest_Vec3D_InnerProduct()
    {
        Vector3D a = new Vector3D(1, 2, 3);
        Vector3D b = new Vector3D(1, 2, 3);
        float c = a * b; 
        Assert.True(c == (1*1 + 2*2 + 3*3));
        Debug.Log(c);
        Debug.Log((1*1 + 2*2 + 3*3));
    }

}

public class UTest_2x2
{
    private const string prefix1 = "Matrix Translation (";
    private const string prefix2 = "Matrix Identity was set correctly.";
    private const string suffix1 = ") was set incorrectly.";

    [Test]
    public void UTest_2x2_Default()
    {
        Assert.True(true);
    }

    [Test]
    public void UTest_2x2_MatrixRowAccessor()
    {
        Matrix2X2 m = new Matrix2X2(
                new Vector3D(1, 4, 7),
                new Vector3D(2, 5, 8),
                new Vector3D(3, 6, 9));
        m.DebugMatrix("Matrix");
        m.R0.DebugVector(); 
        m.R1.DebugVector(); 
        m.R2.DebugVector(); 
        Assert.True((Vector3)m.R0 == new Vector3(1, 2, 3));
        Assert.True((Vector3)m.R1 == new Vector3(4, 5, 6));
        Assert.True((Vector3)m.R2 == new Vector3(7, 8, 9));
    }

    [Test]
    public void UTest_2x2_MatrixColumnAccessor()
    {
        Matrix2X2 m = new Matrix2X2(
                new Vector3D(1, 4, 7),
                new Vector3D(2, 5, 8),
                new Vector3D(3, 6, 9));
        m.DebugMatrix("Matrix");
        m.R0.DebugVector();
        m.R1.DebugVector();
        m.R2.DebugVector();
        Assert.True((Vector3)m.C0 == new Vector3(1, 4, 7));
        Assert.True((Vector3)m.C1 == new Vector3(2, 5, 8));
        Assert.True((Vector3)m.C2 == new Vector3(3, 6, 9));
    }

    [Test]
    public void UTest_2x2_ZeroProperty()
    {
        Matrix2X2 zeroMatrixCreated = new Matrix2X2();
        Matrix2X2 zeroMatrix = new Matrix2X2(
                Vector3D.Zero,
                Vector3D.Zero,
                Vector3D.Zero);

        Assert.IsTrue( zeroMatrixCreated == zeroMatrix,
            "Matrix is zero matrix: True"
            );
    }

    [Test]
    public void UTest_2x2_IdentityMatrix()
    {
        Matrix2X2 generatedIdentityM = Matrix2X2.Identity;
        Matrix2X2 compM1 = new Matrix2X2(
                new Vector3D(1, 0, 0),
                new Vector3D(0, 1, 0),
                new Vector3D(0, 0, 1));
        Matrix2X2 compM2 = new Matrix2X2(
                new Vector3D(1, 0, 1),
                new Vector3D(0, 2, 0),
                new Vector3D(1, 0, 1));

        Debug.Log(generatedIdentityM.ToString());
        Debug.Log(compM1.ToString());

        Assert.IsTrue(compM1 == generatedIdentityM, prefix2);
        Assert.IsFalse(compM2 == generatedIdentityM, prefix2);
    }

    [Test]
    public void UTest_2x2_TranslationMatrix()
    {
        Matrix2X2 translationM = Matrix2X2.CreateTranslationMatrix(1, 1);
        Matrix2X2 compM = new Matrix2X2(
                new Vector3D(1, 0, 0),
                new Vector3D(0, 1, 0),
                new Vector3D(1, 1, 1));

        translationM.DebugMatrixPair(compM);

        Assert.IsTrue(compM == translationM, prefix1 + "1, 1" + suffix1);

        translationM = Matrix2X2.CreateTranslationMatrix(2, 2);
        compM = new Matrix2X2(
        new Vector3D(1, 0, 0),
        new Vector3D(0, 1, 0),
        new Vector3D(2, 2, 1));

        translationM.DebugMatrixPair(compM);
        Assert.IsTrue(compM == translationM, prefix1 + "1, 1" + suffix1);

        translationM = Matrix2X2.CreateTranslationMatrix(3, 5);
        compM = new Matrix2X2(
        new Vector3D(1, 0, 0),
        new Vector3D(0, 1, 0),
        new Vector3D(3, 5, 1));

        translationM.DebugMatrixPair(compM);
        Assert.IsTrue(compM == translationM, prefix1 + "1, 1" + suffix1);

        translationM = Matrix2X2.CreateTranslationMatrix(2, 2);
        compM = new Matrix2X2(
        new Vector3D(1, 0, 0),
        new Vector3D(0, 1, 0),
        new Vector3D(0, 0, 1));

        translationM.DebugMatrixPair(compM);
        Assert.IsFalse(compM == translationM, prefix1 + "1, 1" + suffix1);
    }

    [Test]
    public void UTest_2x2_ScaleMatrix()
    {
        Matrix2X2 scaleM = Matrix2X2.CreateScaleMatrix(2, 2);
        Matrix2X2 compM = Matrix2X2.Identity;
        compM.xAxis.x = 2;
        compM.yAxis.y = 2;

        scaleM.DebugMatrixPair(compM);
        Assert.IsTrue(scaleM == compM);

        compM = Matrix2X2.Identity;
        compM.xAxis.x = 3;
        compM.yAxis.y = 5;
        scaleM.DebugMatrixPair(compM);
        Assert.IsFalse(scaleM == compM);
    }

    [Test]
    public void UTest_2x2_RotationMatrix()
    {
        Matrix2X2 scaleM = Matrix2X2.CreateRotationMatrix(90);
        Matrix2X2 compM = Matrix2X2.Identity;
        compM.xAxis.x = Mathf.Cos(90 * Mathf.Deg2Rad);
        compM.xAxis.y = Mathf.Sin(90 * Mathf.Deg2Rad);
        compM.yAxis.x = -Mathf.Sin(90 * Mathf.Deg2Rad);
        compM.yAxis.y = Mathf.Cos(90 * Mathf.Deg2Rad);

        scaleM.DebugMatrixPair(compM);
        Assert.IsTrue(scaleM == compM);
    }

    [Test]
    public void UTest_2x2_RotationApplication()
    {
        float angle = 90;
        Matrix2X2 rot = Matrix2X2.CreateRotationMatrix(90);
        Matrix2X2 compM = new Matrix2X2(
            new Vector3D(Mathf.Cos(angle), Mathf.Sin(angle), 0),
            new Vector3D(-Mathf.Sin(angle), Mathf.Cos(angle), 0),
            new Vector3D(0, 0, 1)
            );
        Vector3D vec = new Vector3D(-1, 1, 1);
        rot.DebugMatrixPair(compM);
        vec.DebugVector();

    }

    [Test]
    public void UTest_2x2_MatrixByMatrixMultiplication()
    {
        Matrix2X2 a = new Matrix2X2(
            new(1, 2, 3),
            new(4, 5, 6),
            new(7, 8, 9)
            );

        Matrix2X2 b = new Matrix2X2(
            new(-1, 2, -3),
            new(4, -5, 6),
            new(-7, 8, -9)
            );


        Matrix2X2 c = a * b; 
        a.DebugMatrix();
        b.DebugMatrix();
        c.DebugMatrix();

        Assert.IsTrue((Vector3)c.R0 == (Vector3)new Vector3D(-14, 26, -38));
        Assert.IsTrue((Vector3)c.R1 == (Vector3)new Vector3D(-16, 31, -46));
        Assert.IsTrue((Vector3)c.R2 == (Vector3)new Vector3D(-18, 36, -54));
        
    }

    [Test]
    public void UTest_2x2_TRSMultiplicationZero()
    {
        TranslationComponent tComp = new TranslationComponent(0, 0);
        ScaleComponent sComp = new ScaleComponent(0, 0);
        RotationComponent rComp = new RotationComponent(0);
        Matrix2X2 trs = (tComp.Matrix * rComp.Matrix) * sComp.Matrix;
        Vector2 Vec = (Vector2)(trs * new Vector3D(0, 0, 1));
        Assert.IsTrue(Vec == new Vector2(0, 0));
    }

    [Test]
    public void UTest_2x2_TRSMultiplication()
    {
        Matrix2X2 tr = 
            Matrix2X2.CreateTranslationMatrix(3, 1) * 
            Matrix2X2.CreateRotationMatrix(0);
        Matrix2X2 trs = tr * Matrix2X2.CreateScaleMatrix(2, 2);
        Vector2 Vec = (Vector2)(trs * new Vector3D(0, 0, 1));
        Debug.Log("Output Vector: " + Vec.ToString());
        tr.DebugMatrix("Tr M");
        trs.DebugMatrix("Trs M");
        Assert.IsTrue(Vec == new Vector2(3, 1));
    }
}
