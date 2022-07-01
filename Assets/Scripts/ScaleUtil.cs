using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ScaleUtil : MonoBehaviour
{
    public int startSize = 1;
    public int minSize = 1;
    public int maxSize = 3;

    public float speed = 50.0f;

    private Camera camera;

    private BoxCollider2D cameraBox;

    private Bounds bounds;

    private Vector3 targetScale;
    private Vector3 baseScale;
    private int currScale;

    private float leftPivot;
    private float rightPivot;
    private float topPivot;
    private float botPivot;

    void Start()
    {
        this.camera = Camera.main;

        baseScale = transform.localScale;
        transform.localScale = baseScale * startSize;
        currScale = startSize;
        targetScale = baseScale * startSize;
        // this.AspectRatioBoxChange();
        // this.CalculateCameraPivot();
    }


    void AspectRatioBoxChange()
    {
        float height = 2f * this.camera.orthographicSize;
        float width = height * this.camera.aspect;
        cameraBox.size = new Vector2(width, height);
    }

    void CalculateCameraPivot()
    {
        botPivot = bounds.min.y + cameraBox.size.y / 2;
        topPivot = bounds.max.y - cameraBox.size.y / 2;
        leftPivot = bounds.min.x + cameraBox.size.x / 2;
        rightPivot = bounds.max.x - cameraBox.size.x / 2;

    }

    // void FollowPlayer()
    // {
    //     transform.position = new Vector3(Mathf.Clamp(target.position.x, leftPivot, rightPivot),
    //                                      Mathf.Clamp(target.position.y, botPivot, topPivot),
    //                                      transform.position.z);
    // }

    void Update()
    {

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);
        // if (transform.localScale == targetScale)
        // {
        //     ChangeSize(false);
        //     return;
        // }
        if (Input.GetMouseButtonDown(0))
        {
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
                        ChangeSize(true);
                        return;
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            ChangeSize(false);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            Destroy(coll.gameObject);
            ChangeSize(true);
        }
    }

    public void ChangeSize(bool bigger)
    {

        if (bigger)
            currScale++;
        else
            currScale--;

        currScale = Mathf.Clamp(currScale, minSize, maxSize);

        targetScale = baseScale * currScale;
    }

    private bool checkPixelPos(SpriteRenderer spriteRenderer, Vector3 clickPoint)
    {
        // 世界坐标系中的计算
        var texture = spriteRenderer.sprite.texture;
        var parentScale = spriteRenderer.transform.lossyScale;
        // unity单位像素
        var pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        var textureOriginPosition = spriteRenderer.transform.position;
        // rect.width*pivotX,rect.height*pivotY
        var pivot = spriteRenderer.sprite.pivot;
        var rect = spriteRenderer.sprite.rect;
        var tmpPosition = new Vector2((clickPoint.x - textureOriginPosition.x) * pixelsPerUnit * parentScale.x + pivot.x, (clickPoint.y - textureOriginPosition.y) * pixelsPerUnit * parentScale.y + pivot.y);
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

}