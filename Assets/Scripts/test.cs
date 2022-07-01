using System.Collections.Generic;
using DragonBones;
using System;
using UnityEngine;

public class test : MonoBehaviour
{
    public LayerMask clickMask;
    public float speed = 2.0f;

    private Camera camera;
    private Vector3 moveDirection;
    private Vector3 moveTowardPosition;

    private Vector3 hitPosition;
    private bool moving = false;

    private Grid grid = new Grid();

    private float angle = 0;

    private bool gameObjectDown = false;

    // Start is called before the first frame update
    void Start()
    {
        this.camera = Camera.main;
        Debug.Log("camera size:" + camera.pixelWidth);
        //this.reset();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // moveTowardPosition = this.camera.ScreenToWorldPoint(Input.mousePosition);
            // moveTowardPosition.z = 0;
            // this.moving = true;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.Log(Input.mousePosition);
            var hit = Physics2D.RaycastAll(ray.origin, ray.direction);
            var len = hit.Length;
            for (var i = 0; i < len; i++)
            {
                var spriteRenders = hit[i].collider.gameObject.GetComponentsInChildren<SpriteRenderer>();
                var renderLen = spriteRenders.Length;
                var hitPosition = hit[i].point;
                for (var j = 0; j < renderLen; j++)
                {
                    var spriteRenderer = spriteRenders[j];
                    var texture = spriteRenderer.sprite.texture;
                    var clickBoo = this.checkPixelPos(spriteRenderer, hitPosition);
                    if (clickBoo)
                    {
                        Debug.Log("pop up click gameObject:" + gameObject);
                        // gameObjectDown = true;
                        // gameObject.transform.localScale = new Vector3(1, 1, 1);
                        return;
                    }
                }
                // Debug.Log("collider: " + spriteRenders);
            }
        }

        // if (Input.GetMouseButtonUp(0))
        // {
        //     gameObjectDown = false;
        // }

        // if (gameObjectDown)
        // {
        //     gameObject.transform.localScale = new Vector3(2, 2, 1);
        // }
        // else
        // {
        //     gameObject.transform.localScale = new Vector3(1, 1, 1);
        // }

        // if (this.moving)
        // {
        //     this.doMove();
        // }
    }

    // Matrix4x4 GetYAxisMatrix(float angle)
    // {
    //     Matrix4x4 matrix = Matrix4x4.identity;
    //     var angleRad = angle * Mathf.Deg2Rad;
    //     matrix.m00 = Mathf.Cos(angleRad);
    //     matrix.m02 = Mathf.Sin(angleRad);
    //     matrix.m20 = -Mathf.Sin(angleRad);
    //     matrix.m22 = Mathf.Cos(angleRad);
    //     return matrix;
    // }

    private bool checkPixelPos(SpriteRenderer spriteRenderer, Vector3 clickPoint)
    {
        // 世界坐标系中的计算
        var texture = spriteRenderer.sprite.texture;
        // unity单位像素
        var pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        var textureOriginPosition = spriteRenderer.transform.position;
        // rect.width*pivotX,rect.height*pivotY
        var pivot = spriteRenderer.sprite.pivot;
        var rect = spriteRenderer.sprite.rect;
        var tmpPosition = new Vector2((clickPoint.x - textureOriginPosition.x) * pixelsPerUnit + pivot.x, (clickPoint.y - textureOriginPosition.y) * pixelsPerUnit + pivot.y);
        // if (rect.Contains(tmpPosition) == false)
        // {
        //     Debug.Log("no contains");
        //     return false;
        // }
        var outPosition = new Vector3();
        // （点击坐标-贴图的世界坐标）* 单位像素 + 注册点偏移(向量) + texture.bounds的位置(合图bounds的xy不为0)
        // (clickPoint.x - textureOriginPosition.x) * pixelsPerUnit + pivot.x + rect.x
        outPosition.x = tmpPosition.x + rect.x;
        outPosition.y = tmpPosition.y + rect.y;
        outPosition.z = clickPoint.z;
        var clickColor = texture.GetPixel((int)outPosition.x, (int)outPosition.y);
        if (clickColor.a <= 0)
        {
            Debug.Log("click alpha is 0: " + gameObject);
            return false;
        }
        return true;
    }

    private void doMove()
    {
        Vector3 currentPosition = transform.position;
        //if (Input.GetMouseButtonDown(0))
        //{


        moveDirection = moveTowardPosition - currentPosition;
        moveDirection.z = 0;
        moveDirection.Normalize();

        var distance = Vector3.Distance(currentPosition, moveTowardPosition);
        // Debug.Log(string.Format("curenPosition:{0}, moveTowardPosition{1},distance:{2},speed:{3}", curenPosition, moveTowardPosition, distance, speed * Time.deltaTime));
        if (distance < 0.01f)
        {
            transform.position = moveTowardPosition;
            this.moving = false;
        }
        else
        {
            Vector3 target = moveDirection * speed * Time.deltaTime + currentPosition;
            target.z = 0;
            transform.position = target;

        }
    }

}
