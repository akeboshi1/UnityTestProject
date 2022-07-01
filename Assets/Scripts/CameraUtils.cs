using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtils : MonoBehaviour
{
    /// <summary>
    /// 相机当前是否可交互
    /// </summary>
    public bool IsInteracte = true;

    public float moveSpeed = 0.1f;// 相机移动速度

    // [Header("相机视野范围")]
    // public float minSize = 1;
    // public float maxSize = 3;

    [Header("相机活动范围(上下左右边界——做坐标值)")]
    /// <summary>
    /// 相机最小视野时 活动范围
    /// </summary>
    // public float[] minSizeBound = new float[4];
    /// <summary>
    /// 相机最大视野时 活动范围
    /// </summary>
    public float[] maxSizeBound = new float[4];

    private Camera cam;
    private Transform myCamera;


    private float topPar;
    private float bottomPar;
    private float leftPar;
    private float rightPar;


    private void Awake()
    {
        cam = GetComponent<Camera>();
        myCamera = cam.transform;
        setCameraMoveParam();

        SetCameraInitPos(Vector3.zero);

    }

    private void Update()
    {
        if (IsInteracte)
        {
            keyCodeMoveCamera();

            restrictCameraPosition();

        }
    }

    /// <summary>
    /// 设置相机位置
    /// </summary>
    public void SetCameraInitPos(Vector3 pos)
    {
        myCamera.position = new Vector3(pos.x, pos.y, myCamera.position.z);
    }

    /// <summary>
    /// 设置相机移动的系数
    /// </summary>
    private void setCameraMoveParam()
    {
        float sizeRange = 1;// maxSize - minSize;

        float shangRange = maxSizeBound[0];// - minSizeBound[0];
        float xiaRange = maxSizeBound[1];// - minSizeBound[1];
        float zuoRange = maxSizeBound[2];// - minSizeBound[2];
        float youRange = maxSizeBound[3];// - minSizeBound[3];

        topPar = shangRange / sizeRange;
        bottomPar = xiaRange / sizeRange;
        leftPar = zuoRange / sizeRange;
        rightPar = youRange / sizeRange;
    }

    // test code
    private void keyCodeMoveCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            myCamera.Translate(Vector2.up * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            myCamera.Translate(Vector2.down * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            myCamera.Translate(Vector2.left * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            myCamera.Translate(Vector2.right * moveSpeed);
        }
    }

    /// <summary>
    /// 限制相机位置
    /// </summary>
    private void restrictCameraPosition()
    {
        float cameraSize = cam.orthographicSize;// - minSize;

        float minY = cameraSize * topPar;// + minSizeBound[0];
        float maxY = cameraSize * bottomPar;// + minSizeBound[1];
        float minX = cameraSize * leftPar;// + minSizeBound[2];
        float maxX = cameraSize * rightPar;// + minSizeBound[3];

        if (myCamera.position.y > maxY)
        {
            myCamera.position = new Vector3(myCamera.position.x, maxY, myCamera.position.z);
        }
        if (myCamera.position.y < minY)
        {
            myCamera.position = new Vector3(myCamera.position.x, minY, myCamera.position.z);
        }
        if (myCamera.position.x < minX)
        {
            myCamera.position = new Vector3(minX, myCamera.position.y, myCamera.position.z);
        }
        if (myCamera.position.x > maxX)
        {
            myCamera.position = new Vector3(maxX, myCamera.position.y, myCamera.position.z);
        }
    }
}
