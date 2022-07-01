using UnityEngine;
public class InstantiationExample : MonoBehaviour
{
    // 玩家摄像头
    public Camera PlayerCamera;
    // 引用预制件。在 Inspector 中，将预制件拖动到该字段中。
    public GameObject myPrefab;
    // 子弹最大数量
    public int BulletCount = 3;
    // 子弹速度
    public int BulletSpeed = 80;

    // 开火速度
    public float FireRate = 2.0f;

    // 下一次可开火时间
    private float _nextFire = 0;
    // 该脚本将在游戏开始时简单地实例化预制件。
    void Start()
    {

    }

    void Update()
    {
        _nextFire = _nextFire + Time.fixedDeltaTime;
        if (Input.GetMouseButton(0) && FireRate < _nextFire)
        {
             _nextFire = 0;
            //在鼠标左键点击的地方创建一个物体  
            GameObject obj = Instantiate(myPrefab) as GameObject;
            obj.transform.localPosition = new Vector3(0, 0.5f, 0);//在指定坐标生成子弹
            obj.transform.rotation = PlayerCamera.transform.rotation;
            // 本地化 2D 坐标系
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
            Vector3 mousePositionOnScreen = Input.mousePosition;
            mousePositionOnScreen.z = screenPosition.z;
            Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            obj.GetComponent<Rigidbody2D>().AddForce((mousePositionInWorld - transform.position).normalized * BulletSpeed);//给予子弹一个向前的推进力
            Destroy(obj, 5.0f);//销毁子弹物体
        }
    }
}