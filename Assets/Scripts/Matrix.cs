using UnityEngine;

public static class Matrix
{
    private static Matrix4x4 matrix;
    public static void Matrix4x4_Transfrom(this Transform transform, Vector3 targetPos)
    {
        matrix = Matrix4x4.identity;
        Vector4 v = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1);

        matrix.m03 = targetPos.x;
        matrix.m13 = targetPos.y;
        matrix.m23 = targetPos.z;

        v = matrix * v;

        transform.Matrix4x4(matrix);
    }

    public static void Matrix4x4_Rotation(this Transform transform, Axle axle, float angle)
    {
        matrix = Matrix4x4.identity;

        if (axle == Axle.X)
        {
            matrix.m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
            matrix.m12 = -Mathf.Sin(angle * Mathf.Deg2Rad);
            matrix.m21 = Mathf.Sin(angle * Mathf.Deg2Rad);
            matrix.m22 = Mathf.Cos(angle * Mathf.Deg2Rad);

        }
        else if (axle == Axle.Y)
        {
            matrix.m00 = Mathf.Cos(angle * Mathf.Deg2Rad);
            matrix.m02 = Mathf.Sin(angle * Mathf.Deg2Rad);
            matrix.m20 = -Mathf.Sin(angle * Mathf.Deg2Rad);
            matrix.m22 = Mathf.Cos(angle * Mathf.Deg2Rad);
        }
        else
        {
            matrix.m00 = Mathf.Cos(angle * Mathf.Deg2Rad);
            matrix.m01 = -Mathf.Sin(angle * Mathf.Deg2Rad);
            matrix.m10 = Mathf.Sin(angle * Mathf.Deg2Rad);
            matrix.m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        transform.Matrix4x4(matrix);
    }

    public static void Matrix4x4_Scale(this Transform transform, Vector3 targetScale)
    {
        matrix = Matrix4x4.identity;
        Vector4 v = new Vector4(transform.localScale.x, transform.localScale.y, transform.localScale.z, 1);

        matrix.m00 = targetScale.x;
        matrix.m11 = targetScale.y;
        matrix.m22 = targetScale.z;
        v = matrix * v;
        transform.Matrix4x4(matrix);
    }
}
